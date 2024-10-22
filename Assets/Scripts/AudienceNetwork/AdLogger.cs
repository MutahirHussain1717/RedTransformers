using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdLogger
	{
		internal static void Log(string message)
		{
			AdLogger.AdLogLevel adLogLevel = AdLogger.AdLogLevel.Log;
			if (AdLogger.logLevel >= adLogLevel)
			{
				UnityEngine.Debug.Log(AdLogger.logPrefix + AdLogger.levelAsString(adLogLevel) + message);
			}
		}

		internal static void LogWarning(string message)
		{
			AdLogger.AdLogLevel adLogLevel = AdLogger.AdLogLevel.Warning;
			if (AdLogger.logLevel >= adLogLevel)
			{
				UnityEngine.Debug.LogWarning(AdLogger.logPrefix + AdLogger.levelAsString(adLogLevel) + message);
			}
		}

		internal static void LogError(string message)
		{
			AdLogger.AdLogLevel adLogLevel = AdLogger.AdLogLevel.Error;
			if (AdLogger.logLevel >= adLogLevel)
			{
				UnityEngine.Debug.LogError(AdLogger.logPrefix + AdLogger.levelAsString(adLogLevel) + message);
			}
		}

		private static string levelAsString(AdLogger.AdLogLevel logLevel)
		{
			switch (logLevel)
			{
			case AdLogger.AdLogLevel.Notification:
				return string.Empty;
			case AdLogger.AdLogLevel.Error:
				return "<error>: ";
			case AdLogger.AdLogLevel.Warning:
				return "<warn>: ";
			case AdLogger.AdLogLevel.Log:
				return "<log>: ";
			case AdLogger.AdLogLevel.Debug:
				return "<debug>: ";
			case AdLogger.AdLogLevel.Verbose:
				return "<verbose>: ";
			default:
				return string.Empty;
			}
		}

		private static AdLogger.AdLogLevel logLevel = AdLogger.AdLogLevel.Log;

		private static readonly string logPrefix = "Audience Network Unity ";

		private enum AdLogLevel
		{
			None,
			Notification,
			Error,
			Warning,
			Log,
			Debug,
			Verbose
		}
	}
}
