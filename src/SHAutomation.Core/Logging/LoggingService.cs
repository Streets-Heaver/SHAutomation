using log4net;
using SHAutomation.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SHAutomation.Core.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly ILog _logger;
        private readonly string _logName;
        private readonly bool _logToFile;
        private readonly LoggingLevel _loggingLevel;
        private readonly bool _disabled;

        public LoggingService()
        {
            _disabled = true;
        }

        public LoggingService(string name, bool logToFile, LoggingLevel loggingLevel, string configLocation)
        {
            if (!string.IsNullOrEmpty(configLocation))
            {
                var assembly = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;

                _logger = LogManager.GetLogger(assembly, name);
                _logName = name;
                _logToFile = logToFile;
                _loggingLevel = loggingLevel;

                XmlDocument log4netConfig = new XmlDocument();
                using (var config = File.OpenRead(configLocation))
                {
                    log4netConfig.Load(config);
                }

                log4net.Config.XmlConfigurator.Configure(LogManager.CreateRepository(assembly, typeof(log4net.Repository.Hierarchy.Hierarchy)), log4netConfig["log4net"]);

            }
            else
                _disabled = true;

        }

        private string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        private void LogMessageToFile(string msg)
        {
            if (_logToFile)
            {
                var path = GetTempPath();
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    path + _logName + ".txt");
                try
                {
                    string logLine = string.Format(
                        "{0:G}: {1}.", System.DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        public void Flush()
        {
            if (!_disabled)
                LogManager.Flush(3000);
        }

        public void Error(Exception ex)
        {
            if (!_disabled)
                Error(ex.ToString());

        }

        public void Fatal(Exception ex)
        {
            if (!_disabled)
                Fatal(ex.ToString());
        }

        public void Fatal(string errorMessage)
        {
            if (!_disabled)
            {

                if (_logger.IsFatalEnabled)
                    _logger.Fatal(errorMessage);

                LogMessageToFile(errorMessage);
            }

        }

        public void Error(string errorMessage)
        {
            if (!_disabled)
            {
                if (_logger.IsErrorEnabled)
                    _logger.Error(errorMessage);

                LogMessageToFile(errorMessage);
            }


        }

        public void Warn(string message, LoggingLevel loggingLevel = LoggingLevel.Low)
        {

            if (!_disabled && _loggingLevel >= loggingLevel)
            {
                if (_logger.IsWarnEnabled)
                    _logger.Warn(message);

                LogMessageToFile(message);
            }


        }

        public void Info(string message, LoggingLevel loggingLevel = LoggingLevel.Low)
        {
            if (!_disabled && _loggingLevel >= loggingLevel)
            {
                if (_logger.IsInfoEnabled)
                    _logger.Info(message);

                LogMessageToFile(message);
            }

        }

        public void Debug(string message, LoggingLevel loggingLevel = LoggingLevel.Low)
        {
            if (!_disabled && _loggingLevel >= loggingLevel)
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug(message);

                LogMessageToFile(message);
            }
        }

    }
}
