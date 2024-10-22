using System;
using UnityEngine;

namespace Heyzap
{
	public class HeyzapAdsAndroid : MonoBehaviour
	{
		public static void Start(string publisher_id, int options = 0)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("start", new object[]
				{
					publisher_id,
					options
				});
			}
		}

		public static bool IsNetworkInitialized(string network)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<bool>("isNetworkInitialized", new object[]
				{
					network
				});
			}
			return result;
		}

		public static bool OnBackPressed()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<bool>("onBackPressed", new object[0]);
			}
			return result;
		}

		public static void ShowMediationTestSuite()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("showNetworkActivity", new object[0]);
			}
		}

		public static string GetRemoteData()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return "{}";
			}
			AndroidJNIHelper.debug = false;
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<string>("getCustomPublisherData", new object[0]);
			}
			return result;
		}

		public static void ShowDebugLogs()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("showDebugLogs", new object[0]);
			}
		}

		public static void HideDebugLogs()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("hideDebugLogs", new object[0]);
			}
		}
	}
}
