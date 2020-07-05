using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Logging;
using SHAutomation.Core.Tools;

namespace SHAutomation.Core
{
    /// <summary>
    /// Wrapper for an application which should be automated.
    /// </summary>
    public class Application : IDisposable
    {
        /// <summary>
        /// The process of this application.
        /// </summary>
        private readonly Process _process;

        /// <summary>
        /// Flag to indicate if Dispose has already been called.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The proces Id of the application.
        /// </summary>
        public int ProcessId => _process.Id;

        /// <summary>
        /// The name of the application's process.
        /// </summary>
        public string Name => _process.ProcessName;

        /// <summary>
        /// The current handle (Win32) of the application's main window.
        /// Can be IntPtr.Zero if no main window is currently available.
        /// </summary>
        public IntPtr MainWindowHandle => _process.MainWindowHandle;

        /// <summary>
        /// Gets a value indicating whether the associated process has been terminated.
        /// </summary>
        public bool HasExited => _process.HasExited;

        /// <summary>
        /// Gets the value that the associated process specified when it terminated.
        /// </summary>
        public int ExitCode => _process.ExitCode;

        /// <summary>
        /// Creates an application object with the given process id.
        /// </summary>
        /// <param name="processId">The process id.</param>
        /// <param name="isStoreApp">Flag to define if it's a store app or not.</param>
        public Application(int processId)
            : this(FindProcess(processId))
        {
        }

        /// <summary>
        /// Creates an application object with the given process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="isStoreApp">Flag to define if it's a store app or not.</param>
        public Application(Process process)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));

        }

        /// <summary>
        /// Closes the application. Force-closes it after a small timeout.
        /// </summary>
        /// <returns>Returns true if the application was closed normally and false if it was force-closed.</returns>
        public bool Close()
        {
            if (_disposed || _process.HasExited)
            {
                return true;
            }
            _process.CloseMainWindow();
            _process.WaitForExit(5000);
            if (!_process.HasExited)
            {
                _process.Kill();
                _process.WaitForExit(5000);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kills the applications and waits until it is closed.
        /// </summary>
        public void Kill()
        {
            try
            {
                if (_process.HasExited)
                {
                    return;
                }
                _process.Kill();
                _process.WaitForExit();
            }
            catch
            {
                // NOOP
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Close();
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _process?.Dispose();
            }
            _disposed = true;
        }

        public static Application Attach(int processId)
        {
            return Attach(FindProcess(processId));
        }

        public static Application Attach(Process process)
        {
            return new Application(process);
        }

        public static Application Attach(string executable, int index = 0)
        {
            var processes = FindProcess(executable);
            if (processes.Length > index)
            {
                return Attach(processes[index]);
            }
            throw new Exception("Unable to find process with name: " + executable);
        }

        public static Application AttachOrLaunch(ProcessStartInfo processStartInfo)
        {
            var processes = FindProcess(processStartInfo.FileName);
            return processes.Length == 0 ? Launch(processStartInfo) : Attach(processes[0]);
        }

        public static Application Launch(string executable)
        {
            var processStartInfo = new ProcessStartInfo(executable);
            return Launch(processStartInfo);
        }

        public static Application Launch(ProcessStartInfo processStartInfo)
        {
            if (string.IsNullOrEmpty(processStartInfo.WorkingDirectory))
            {
                processStartInfo.WorkingDirectory = ".";
            }



            Process process;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Win32Exception ex)
            {
                throw ex;
            }

            return new Application(process);
        }

        public static Application LaunchStoreApp(string appUserModelId, string arguments = null)
        {
            var process = WindowsStoreAppLauncher.Launch(appUserModelId, arguments);
            return new Application(process);
        }

        /// <summary>
        /// Waits as long as the application is busy.
        /// </summary>
        /// <param name="waitTimeout">An optional timeout. If null is passed, the timeout is infinite.</param>
        /// <returns>True if the application is idle, false otherwise.</returns>
        public bool WaitWhileBusy(TimeSpan? waitTimeout = null)
        {
            var waitTime = (waitTimeout ?? TimeSpan.FromMilliseconds(-1)).TotalMilliseconds;
            return _process.WaitForInputIdle((int)waitTime);
        }

        /// <summary>
        /// Waits until the main handle is set.
        /// </summary>
        /// <param name="waitTimeout">An optional timeout. If null is passed, the timeout is infinite.</param>
        /// <returns>True a main window handle was found, false otherwise.</returns>
        public bool WaitWhileMainHandleIsMissing(TimeSpan? waitTimeout = null)
        {
            var waitTime = waitTimeout ?? TimeSpan.FromMilliseconds(-1);
            return Retry.WhileTrue(() =>
            {
                _process.Refresh();
                return _process.MainWindowHandle == IntPtr.Zero;
            }, waitTime, TimeSpan.FromMilliseconds(50)).Result;
        }

        /// <summary>
        /// Gets the main window of the application's process.
        /// </summary>
        /// <param name="automation">The automation object to use.</param>
        /// <param name="waitTimeout">An optional timeout. If null is passed, the timeout is infinite.</param>
        /// <param name="pathToConfigFile">Path to SHAutomation config file for configuring Redis</param>
        /// <param name="pathToLogConfig">Path to Log4Net config file for logging to an elastic search instance</param>
        /// <returns>The main window object as <see cref="Window" /> or null if no main window was found within the timeout.</returns>
        public Window GetMainWindow(AutomationBase automation, TimeSpan? waitTimeout = null, string pathToConfigFile = null)
        {
            return GetMainWindow(automation, waitTimeout, pathToConfigFile, null, null);
        }

        /// <summary>
        /// Gets the main window of the application's process.
        /// </summary>
        /// <param name="automation">The automation object to use.</param>
        /// <param name="waitTimeout">An optional timeout. If null is passed, the timeout is infinite.</param>
        /// <param name="pathToConfigFile">Path to SHAutomation config file for configuring Redis</param>
        /// <param name="pathToLogConfig">Path to Log4Net config file for logging to an elastic search instance</param>
        /// <returns>The main window object as <see cref="Window" /> or null if no main window was found within the timeout.</returns>
        public Window GetMainWindow(AutomationBase automation, string pathToLog4NetConfig, TimeSpan? waitTimeout = null, string pathToConfigFile = null)
        {
            return GetMainWindow(automation, waitTimeout, pathToConfigFile, pathToLog4NetConfig, null);
        }

        /// <summary>
        /// Gets the main window of the application's process.
        /// </summary>
        /// <param name="automation">The automation object to use.</param>
        /// <param name="waitTimeout">An optional timeout. If null is passed, the timeout is infinite.</param>
        /// <param name="pathToConfigFile">Path to SHAutomation config file for configuring Redis</param>
        /// <param name="loggingService">A custom implementation of ILogging service</param>
        /// <returns>The main window object as <see cref="Window" /> or null if no main window was found within the timeout.</returns>
        public Window GetMainWindow(AutomationBase automation, ILoggingService loggingService, TimeSpan? waitTimeout = null, string pathToConfigFile = null)
        {
            return GetMainWindow(automation, waitTimeout, pathToConfigFile, null, loggingService);
        }


        private Window GetMainWindow(AutomationBase automation, TimeSpan? waitTimeout, string pathToConfigFile, string pathToLogConfig, ILoggingService loggingService)
        {
            WaitWhileMainHandleIsMissing(waitTimeout);
            var mainWindowHandle = MainWindowHandle;
            if (mainWindowHandle == IntPtr.Zero)
            {
                return null;
            }

            var mainWindow = automation.FromHandle(mainWindowHandle)
                .AsWindow(loggingService != null ? loggingService : (!string.IsNullOrEmpty(pathToConfigFile) ?
                new LoggingService(Name + " tests", false, Enums.LoggingLevel.High, pathToLogConfig) :
                new LoggingService()), pathToConfigFile);
           
            if (mainWindow != null)
            {
                mainWindow.IsMainWindow = true;
            }
            return mainWindow;
        }

        /// <summary>
        /// Gets all top level windows from the application.
        /// </summary>
        public Window[] GetAllTopLevelWindows(AutomationBase automation)
        {
            var desktop = automation.GetDesktop();
            var foundElements = desktop.FindAllChildren(cf => cf.ByControlType(ControlType.Window).And(cf.ByProcessId(ProcessId)));
            return foundElements.Select(x => x.AsWindow()).ToArray();
        }

        private static Process FindProcess(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                return process;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find process with id: " + processId, ex);
            }
        }

        private static Process[] FindProcess(string executable)
        {
            return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(executable));
        }
    }
}
