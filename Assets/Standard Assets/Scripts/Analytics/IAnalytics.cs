using System;
using System.Collections.Generic;

namespace Analytics
{
	public interface IAnalytics
	{
		void StartSession(string apiKeyIOS, string apiKeyAndroid);

		void LogAppVersion(string version);

		void SetLogLevel(LogLevel level);

		EventRecordStatus LogEvent(string eventName);

		EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters);

		EventRecordStatus BeginLogEvent(string eventName);

		EventRecordStatus BeginLogEvent(string eventName, Dictionary<string, string> parameters);

		void EndLogEvent(string eventName);

		void EndLogEvent(string eventName, Dictionary<string, string> parameters);

		void LogError(string errorID, string message, object target);

		void LogUserID(string userID);

		void LogUserAge(int age);

		void LogUserGender(UserGender gender);
	}
}
