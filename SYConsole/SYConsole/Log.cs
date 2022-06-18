using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SYConsole
{
    public class Log
    {
        public static Log Instance = new Log(); //singleton

        ILog monitoringLogger;
        ILog debugLogger;

        private Log()
        {
            monitoringLogger = LogManager.GetLogger("MonitoringLogger");
            debugLogger = LogManager.GetLogger("DebugLogger");
        }

        public static void Debug(string message, System.Exception exception = null)
        {
            Instance.debugLogger.Debug(message, exception);
        }

        public static void Info(string message, System.Exception exception = null)
        {
            Instance.monitoringLogger.Info(message, exception);
        }

        public static void Warn(string message, System.Exception exception = null)
        {
            Instance.monitoringLogger.Warn(message, exception);
        }

        public static void Error(string message, System.Exception exception = null)
        {
            Instance.monitoringLogger.Error(message, exception);
        }

        public static void Fatal(string message, System.Exception exception = null)
        {
            Instance.monitoringLogger.Fatal(message, exception);
        }
    }
}