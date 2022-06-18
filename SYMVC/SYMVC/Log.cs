using log4net;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace SYMVC
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

		public static void Debug(string message, HttpSessionStateBase ss, System.Exception exception = null)
		{
			Instance.debugLogger.Debug((ss["UserName"] ?? "") + ": " + message, exception);
		}

		public static void Info(string message, HttpSessionStateBase ss, System.Exception exception = null)
		{
			Instance.monitoringLogger.Info((ss["UserName"] ?? "") + ": " + message, exception);
		}

		public static void Warn(string message, HttpSessionStateBase ss, System.Exception exception = null)
		{
			Instance.monitoringLogger.Warn((ss["UserName"] ?? "") + ": " + message, exception);
		}

		public static void Error(string message, HttpSessionStateBase ss, System.Exception exception = null)
		{
			if (ss == null)
				Instance.monitoringLogger.Error("No Session: " + message, exception);
			else
				Instance.monitoringLogger.Error((ss["UserName"] ?? "") + ": " + message, exception);
		}

		public static void Fatal(string message, HttpSessionStateBase ss, System.Exception exception = null)
		{
			Instance.monitoringLogger.Fatal((ss["UserName"] ?? "") + ": " + message, exception);
		}

		public string GetLogFileName()
		{
			log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)monitoringLogger.Logger.Repository.GetCurrentLoggers()[0];
			return ((log4net.Appender.RollingFileAppender)logger.Parent.Appenders[0]).File;
		}
	}
}