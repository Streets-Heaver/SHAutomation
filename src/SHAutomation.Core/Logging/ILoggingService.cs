using SHAutomation.Core.Enums;
using System;

namespace SHAutomation.Core.Logging
{
    public interface ILoggingService
    {
        void Debug(string message, LoggingLevel loggingLevel = LoggingLevel.Low);
        void Error(Exception ex);
        void Error(string errorMessage);
        void Fatal(Exception ex);
        void Fatal(string errorMessage);
        void Flush();
        void Info(string message, LoggingLevel loggingLevel = LoggingLevel.Low);
        void Warn(string message, LoggingLevel loggingLevel = LoggingLevel.Low);
    }
}