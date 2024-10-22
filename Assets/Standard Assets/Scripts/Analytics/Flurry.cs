using System;
using System.Collections.Generic;
using UnityEngine;

namespace Analytics
{
	public sealed class Flurry : MonoSingleton<Flurry>, IAnalytics
	{
		private void Awake()
		{
			Application.logMessageReceived += this.ErrorHandler;
		}

		protected override void OnDestroy()
		{
			FlurryAndroid.Dispose();
			base.OnDestroy();
		}

		private void ErrorHandler(string condition, string stackTrace, LogType type)
		{
			if (type != LogType.Error)
			{
				return;
			}
			this.LogError("Uncaught Unity Exception", condition, this);
		}

		public void StartSession(string apiKeyIOS, string apiKeyAndroid)
		{
			FlurryAndroid.Init(apiKeyAndroid);
			FlurryAndroid.OnStartSession();
		}

		public void LogAppVersion(string version)
		{
			FlurryAndroid.SetVersionName(version);
		}

		public void SetLogLevel(LogLevel level)
		{
			FlurryAndroid.SetLogLevel(level);
		}

		public EventRecordStatus LogEvent(string eventName)
		{
			return FlurryAndroid.LogEvent(eventName);
		}

		public EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters)
		{
			return FlurryAndroid.LogEvent(eventName, parameters);
		}

		public EventRecordStatus LogEvent(string eventName, bool timed)
		{
			return FlurryAndroid.LogEvent(eventName, timed);
		}

		public EventRecordStatus BeginLogEvent(string eventName)
		{
			return FlurryAndroid.LogEvent(eventName, true);
		}

		public EventRecordStatus BeginLogEvent(string eventName, Dictionary<string, string> parameters)
		{
			return FlurryAndroid.LogEvent(eventName, parameters, true);
		}

		public void EndLogEvent(string eventName)
		{
			FlurryAndroid.EndTimedEvent(eventName);
		}

		public void EndLogEvent(string eventName, Dictionary<string, string> parameters)
		{
			FlurryAndroid.EndTimedEvent(eventName, parameters);
		}

		public void LogError(string errorID, string message, object target)
		{
			FlurryAndroid.OnError(errorID, message, target.GetType().Name);
		}

		public void LogUserID(string userID)
		{
			FlurryAndroid.SetUserId(userID);
		}

		public void LogUserAge(int age)
		{
			FlurryAndroid.SetAge(age);
		}

		public void LogUserGender(UserGender gender)
		{
			FlurryAndroid.SetGender((byte)((gender != UserGender.Male) ? ((gender != UserGender.Female) ? -1 : 0) : 1));
		}
	}
}
