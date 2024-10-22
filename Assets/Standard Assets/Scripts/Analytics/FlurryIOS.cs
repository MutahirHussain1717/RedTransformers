using System;
using System.Collections.Generic;

namespace Analytics
{
	public static class FlurryIOS
	{
		public static void StartSession(string apiKey)
		{
		}

		public static bool ActiveSessionExists()
		{
			return false;
		}

		public static void PauseBackgroundSession()
		{
		}

		public static void AddOrigin(string originName, string originVersion)
		{
		}

		public static void AddOrigin(string originName, string originVersion, Dictionary<string, string> parameters)
		{
		}

		public static void SetAppVersion(string version)
		{
		}

		public static string GetFlurryAgentVersion()
		{
			return string.Empty;
		}

		public static void SetShowErrorInLogEnabled(bool value)
		{
		}

		public static void SetDebugLogEnabled(bool value)
		{
		}

		public static void SetLogLevel(LogLevel level)
		{
		}

		public static void SetSessionContinueSeconds(int seconds)
		{
		}

		public static void SetCrashReportingEnabled(bool value)
		{
		}

		public static EventRecordStatus LogEvent(string eventName)
		{
			return EventRecordStatus.Failed;
		}

		public static EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters)
		{
			return EventRecordStatus.Failed;
		}

		public static void LogError(string errorID, string message, Exception exception)
		{
		}

		public static EventRecordStatus LogEvent(string eventName, bool timed)
		{
			return EventRecordStatus.Failed;
		}

		public static EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters, bool timed)
		{
			return EventRecordStatus.Failed;
		}

		public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters)
		{
		}

		public static void LogAllPageViewsForTarget(IntPtr target)
		{
			throw new NotSupportedException();
		}

		public static void StopLogPageViewsForTarget(IntPtr target)
		{
			throw new NotSupportedException();
		}

		public static void LogPageView()
		{
		}

		public static void SetUserId(string userID)
		{
		}

		public static void SetAge(int age)
		{
		}

		public static void SetGender(string gender)
		{
		}

		public static void SetLatitude(double latitude, double longitude, float horizontalAccuracy, float verticalAccuracy)
		{
		}

		public static void SetSessionReportsOnCloseEnabled(bool sendSessionReportsOnClose)
		{
		}

		public static void SetSessionReportsOnPauseEnabled(bool setSessionReportsOnPauseEnabled)
		{
		}

		public static void SetBackgroundSessionEnabled(bool setBackgroundSessionEnabled)
		{
		}

		public static void SetEventLoggingEnabled(bool value)
		{
		}

		private static void ToKeyValue(Dictionary<string, string> dictionary, out string keys, out string values)
		{
			keys = string.Empty;
			values = string.Empty;
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				keys = string.Format("{0}\n{1}", keys, keyValuePair.Key);
				values = string.Format("{0}\n{1}", values, keyValuePair.Value);
			}
		}
	}
}
