using System;
using UnityEngine;

namespace Heyzap
{
	public class HZInterstitialAdAndroid
	{
		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("showInterstitial", new object[]
				{
					showOptions.Tag
				});
			}
		}

		public static void Fetch(string tag = "default")
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("fetchInterstitial", new object[]
				{
					tag
				});
			}
		}

		public static bool IsAvailable(string tag = "default")
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<bool>("isInterstitialAvailable", new object[]
				{
					tag
				});
			}
			return result;
		}

		public static void chartboostShowForLocation(string location)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("chartboostLocationShow", new object[]
				{
					location
				});
			}
		}

		public static bool chartboostIsAvailableForLocation(string location)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<bool>("chartboostLocationIsAvailable", new object[]
				{
					location
				});
			}
			return result;
		}

		public static void chartboostFetchForLocation(string location)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("chartboostLocationFetch", new object[]
				{
					location
				});
			}
		}
	}
}
