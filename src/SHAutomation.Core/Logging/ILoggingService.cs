using SHAutomation.Core.Enums;
using System;

namespace SHAutomation.Core.Logging
{
    public interface ILoggingService : IDisposable
    {
        void Debug(string message);
        void Error(Exception ex);
        void Error(string errorMessage);
        void Fatal(Exception ex);
        void Fatal(string errorMessage);
        void Info(string message);
        void Warn(string message);
    }
}