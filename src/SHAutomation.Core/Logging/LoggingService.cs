﻿using ElasticLogging;
using ElasticLogging.Classes;
using SHAutomation.Core.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace SHAutomation.Core.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly Logger _elasticLogging;
        private readonly bool _disabled;

        public LoggingService()
        {
            _disabled = true;
        }

        public LoggingService(LoggingSettings loggingSettings, string name, bool logToFile)
        {
            _elasticLogging = new Logger(loggingSettings, name, logToFile);
        }

        public LoggingService(Logger elasticLogging)
        {
            if (elasticLogging != null)
            {
                _elasticLogging = elasticLogging;
            }
            else
                _disabled = true;

        }

        public void Dispose()
        {
            _elasticLogging.Dispose();
        }

        public void Error(Exception ex)
        {
            if (!_disabled)
                _elasticLogging.Error(ex);

        }

        public void Error(string errorMessage)
        {
            if (!_disabled)
                _elasticLogging.Error(errorMessage);


        }

        public void Fatal(Exception ex)
        {
            if (!_disabled)
                _elasticLogging.Fatal(ex);

        }

        public void Fatal(string errorMessage)
        {
            if (!_disabled)
                _elasticLogging.Fatal(errorMessage);


        }

        

        public void Warn(string message)
        {
            if (!_disabled)
                _elasticLogging.Warn(message);

        }

        public void Info(string message)
        {
            if (!_disabled)
                _elasticLogging.Info(message);

        }

        public void Debug(string message)
        {
            if (!_disabled)
                _elasticLogging.Debug(message);
        }

    }
}
