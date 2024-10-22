using System;
using UnityEngine;

namespace GssAdSdk
{
	public class UtilsGssSdk
	{
		public static void Log(string msg)
		{
			if (UtilsGssSdk.isDebugOn)
			{
				UnityEngine.Debug.Log(msg);
			}
		}

		public static bool getBooleanValue(string name)
		{
			int @int = PlayerPrefs.GetInt(name);
			return @int == 1;
		}

		public static void setBooleanValue(string name, bool value)
		{
			PlayerPrefs.SetInt(name, (!value) ? 0 : 1);
		}

		public static string getStringValue(string name)
		{
			return PlayerPrefs.GetString(name, null);
		}

		public static void setStringValue(string name, string value)
		{
			PlayerPrefs.SetString(name, value);
		}

		public static int getIntValue(string name)
		{
			return PlayerPrefs.GetInt(name, 0);
		}

		public static void setIntValue(string name, int value)
		{
			PlayerPrefs.SetInt(name, value);
		}

		public static void setAdID(string adName, string adID)
		{
			PlayerPrefs.SetString(adName, adID);
		}

		public static string getAdID(string adName)
		{
			return PlayerPrefs.GetString(adName, null);
		}

		public static bool isRated()
		{
			int @int = PlayerPrefs.GetInt(UtilsGssSdk.appRateKey);
			return @int == 1;
		}

		public static void setRating()
		{
			PlayerPrefs.SetInt(UtilsGssSdk.appRateKey, 1);
		}

		public static bool isInternetConnected()
		{
			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
		}

		public static bool isDebugOn;

		private static string appRateKey = "GSSAppRatedOrNot";
	}
}
