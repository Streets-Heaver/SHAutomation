using StackExchange.Redis;
using System;
using System.Text.RegularExpressions;
using SHAutomation.Core.StaticClasses;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using SHAutomation.Core.Classes;

namespace SHAutomation.Core.Caching
{
    public class CacheService : ICacheService
    {
        private bool _usingRedis;
        private string _branchNameRegex;

        public CacheService()
        {
            Init();
        }

        public CacheService(string pathToConfigFile)
        {
            Init(pathToConfigFile);
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
                _branchNameRegex = @"\d\.\d\d"; //config.BranchMatchRegex.Replace(@"\\", @"\");

            }
        }

        public void SetCacheValue(string key, string value)
        {
            if (_usingRedis)
            {
                IDatabase db;
                try
                {
                    db = RedisManager.Connection.GetDatabase();
                }
                catch (ObjectDisposedException)
                {
                    RedisManager.ForceReconnect();
                    db = RedisManager.Connection.GetDatabase();

                }
                db.StringSet(key, value);
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

                IDatabase db;
                try
                {
                    db = RedisManager.Connection.GetDatabase();
                }
                catch (ObjectDisposedException)
                {
                    RedisManager.ForceReconnect();
                    db = RedisManager.Connection.GetDatabase();

                }

                var cacheValue = db.StringGet(key);
                if (string.IsNullOrEmpty(cacheValue) && key != testName)
                {
                    return db.StringGet(testName);
                }
                else
                    return cacheValue;
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
