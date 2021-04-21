using StackExchange.Redis;
using System;
using System.Text.RegularExpressions;
using SHAutomation.Core.StaticClasses;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using SHAutomation.Core.Classes;
using SHAutomation.Core.Logging;

namespace SHAutomation.Core.Caching
{
    public class CacheService : ICacheService
    {
        private bool _usingRedis;
        private string _branchNameRegex;
        private IDatabase _database;
        private readonly ILoggingService _loggingService;


        public CacheService(ILoggingService loggingService)
        {
            Init();
            _loggingService = loggingService;
        }

        public CacheService(string pathToConfigFile, ILoggingService loggingService)
        {
            Init(pathToConfigFile);
            _loggingService = loggingService;

        }

        private void Init()
        {
            Init(null);
        }

        private void Init(string pathToConfigFile)
        {
            if (!string.IsNullOrEmpty(pathToConfigFile))
            {
                var config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(pathToConfigFile));

                if (!string.IsNullOrEmpty(config.RedisEndpoint))
                    _usingRedis = true;

                RedisManager.EndPoint = config.RedisEndpoint;
                RedisManager.Password = config.RedisPassword;
                RedisManager.Port = config.RedisPort;
                RedisManager.UseSSL = config.RedisUseSSL;
                RedisManager.KeyExpiry = config.KeyExpiry;
                RedisManager.UpdateExpiryIfTTLLessThan = config.UpdateExpiryIfTTLLessThan;
                _branchNameRegex = @"\d\.\d\d"; //config.BranchMatchRegex.Replace(@"\\", @"\");

            }
        }

        public void SetCacheValue(string key, string value)
        {
            if (_usingRedis)
            {
                if (_database == null)
                {
                    try
                    {
                        _database = RedisManager.Connection.GetDatabase();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (ex is ObjectDisposedException || ex is RedisTimeoutException)
                            {
                                _loggingService.Error(ex);
                                RedisManager.ForceReconnect();
                                _database = RedisManager.Connection.GetDatabase();
                            }
                        }
                        catch
                        {
                            _loggingService.Error("Retry attempt to get Redis DB failed, setting Redis DB to null to prevent usage");
                            _database = null;
                        }

                    }
                }

                if (_database != null)
                {
                    try
                    {
                        _database.StringSet(key, value, expiry: RedisManager.KeyExpiry.HasValue ? RedisManager.KeyExpiry.Value : (TimeSpan?)null);
                    }
                    catch (Exception ex)
                    {
                        if (ex is RedisTimeoutException || ex is RedisConnectionException)
                        {
                            _loggingService.Warn("Encountered issue performing Redis StringSet so retrying");
                            _loggingService.Warn(ex.Message);
                            _database.StringSet(key, value, expiry: RedisManager.KeyExpiry.HasValue ? RedisManager.KeyExpiry.Value : (TimeSpan?)null);

                        }
                        else
                            _loggingService.Error(ex.Message);
                    }


                }
            }
            else
            {
                string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SHAutomation");

                if (!Directory.Exists(appDataFolder))
                    Directory.CreateDirectory(appDataFolder);

                var path = Path.Combine(appDataFolder, key + ".json");

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }

                File.WriteAllText(path, value);

            }
        }

        public string GetCacheValue(string key, string testName)
        {
            if (_usingRedis)
            {
                ThreadPool.SetMinThreads(300, 300);

                try
                {
                    _database = RedisManager.Connection.GetDatabase();
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (ex is ObjectDisposedException || ex is RedisTimeoutException)
                        {
                            _loggingService.Error(ex);
                            RedisManager.ForceReconnect();
                            _database = RedisManager.Connection.GetDatabase();
                        }
                    }
                    catch
                    {
                        _loggingService.Error("Retry attempt to get Redis DB failed, setting Redis DB to null to prevent usage");

                        _database = null;
                    }

                }

                var cacheValue = _database.StringGetWithExpiry(key);
                if (string.IsNullOrEmpty(cacheValue.Value) && key != testName)
                {
                    try
                    {
                        cacheValue = _database.StringGetWithExpiry(testName);
                    }
                    catch (Exception ex)
                    {
                        if (ex is RedisTimeoutException || ex is RedisConnectionException)
                        {
                            _loggingService.Warn("Encountered issue performing Redis StringGet so retrying");
                            _loggingService.Warn(ex.Message);

                            cacheValue = _database.StringGetWithExpiry(testName);
                        }
                        else
                            _loggingService.Error(ex.Message);


                        return null;
                    }

                }

                if (cacheValue.Expiry.HasValue && RedisManager.KeyExpiry.HasValue && RedisManager.UpdateExpiryIfTTLLessThan.HasValue
                    && cacheValue.Expiry.Value < RedisManager.UpdateExpiryIfTTLLessThan.Value)
                    _database.KeyExpire(testName, RedisManager.KeyExpiry);

                return cacheValue.Value;
            }
            else
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SHAutomation", key + ".json");
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                else
                    return string.Empty;
            }
        }

        public string GenerateCacheKey(string testName)
        {
            var branchName = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var regex = !string.IsNullOrEmpty(_branchNameRegex) ? _branchNameRegex : null; //@"\d\.\d\d"

            if (string.IsNullOrEmpty(branchName))
                return testName;
            else
            {
                if (!string.IsNullOrEmpty(regex) && Regex.IsMatch(branchName, regex))
                    return testName + "_" + branchName;
                else
                    return testName;
            }
        }
    }
}
