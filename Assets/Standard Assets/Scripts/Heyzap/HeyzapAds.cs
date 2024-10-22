using System;
using UnityEngine;

namespace Heyzap
{
	public class HeyzapAds : MonoBehaviour
	{
		public static void Start(string publisher_id, int options)
		{
			HeyzapAdsAndroid.Start(publisher_id, options);
			HeyzapAds.InitReceiver();
			HZInterstitialAd.InitReceiver();
			HZVideoAd.InitReceiver();
			HZIncentivizedAd.InitReceiver();
			HZBannerAd.InitReceiver();
		}

		public static string GetRemoteData()
		{
			return HeyzapAdsAndroid.GetRemoteData();
		}

		public static void ShowMediationTestSuite()
		{
			HeyzapAdsAndroid.ShowMediationTestSuite();
		}

		public static bool OnBackPressed()
		{
			return HeyzapAdsAndroid.OnBackPressed();
		}

		public static bool IsNetworkInitialized(string network)
		{
			return HeyzapAdsAndroid.IsNetworkInitialized(network);
		}

		public static void SetNetworkCallbackListener(HeyzapAds.NetworkCallbackListener listener)
		{
			HeyzapAds.networkCallbackListener = listener;
		}

		public static void PauseExpensiveWork()
		{
		}

		public static void ResumeExpensiveWork()
		{
		}

		public static void ShowDebugLogs()
		{
			HeyzapAdsAndroid.ShowDebugLogs();
		}

		public static void HideDebugLogs()
		{
			HeyzapAdsAndroid.HideDebugLogs();
		}

		public static void ShowThirdPartyDebugLogs()
		{
		}

		public static void HideThirdPartyDebugLogs()
		{
		}

		public void SetNetworkCallbackMessage(string message)
		{
			string[] array = message.Split(new char[]
			{
				','
			});
			HeyzapAds.SetNetworkCallback(array[0], array[1]);
		}

		protected static void SetNetworkCallback(string network, string callback)
		{
			if (HeyzapAds.networkCallbackListener != null)
			{
				HeyzapAds.networkCallbackListener(network, callback);
			}
		}

		public static void InitReceiver()
		{
			if (HeyzapAds._instance == null)
			{
				GameObject gameObject = new GameObject("HeyzapAds");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				HeyzapAds._instance = gameObject.AddComponent<HeyzapAds>();
			}
		}

		public static string TagForString(string tag)
		{
			if (tag == null)
			{
				tag = "default";
			}
			return tag;
		}

		[Obsolete("Use the Start() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void start(string publisher_id, int options)
		{
			HeyzapAds.Start(publisher_id, options);
		}

		[Obsolete("Use the GetRemoteData() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static string getRemoteData()
		{
			return HeyzapAds.GetRemoteData();
		}

		[Obsolete("Use the ShowMediationTestSuite() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void showMediationTestSuite()
		{
			HeyzapAds.ShowMediationTestSuite();
		}

		[Obsolete("Use the IsNetworkInitialized(String) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isNetworkInitialized(string network)
		{
			return HeyzapAds.IsNetworkInitialized(network);
		}

		[Obsolete("Use the SetNetworkCallbackListener(NetworkCallbackListener) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void setNetworkCallbackListener(HeyzapAds.NetworkCallbackListener listener)
		{
			HeyzapAds.SetNetworkCallbackListener(listener);
		}

		[Obsolete("Use the PauseExpensiveWork() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void pauseExpensiveWork()
		{
			HeyzapAds.PauseExpensiveWork();
		}

		[Obsolete("Use the ResumeExpensiveWork() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void resumeExpensiveWork()
		{
			HeyzapAds.ResumeExpensiveWork();
		}

		[Obsolete("Use the ShowDebugLogs() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void showDebugLogs()
		{
			HeyzapAds.ShowDebugLogs();
		}

		[Obsolete("Use the HideDebugLogs() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void hideDebugLogs()
		{
			HeyzapAds.HideDebugLogs();
		}

		[Obsolete("Use the OnBackPressed() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool onBackPressed()
		{
			return HeyzapAds.OnBackPressed();
		}

		private static HeyzapAds.NetworkCallbackListener networkCallbackListener;

		private static HeyzapAds _instance;

		public const int FLAG_NO_OPTIONS = 0;

		public const int FLAG_DISABLE_AUTOMATIC_FETCHING = 1;

		public const int FLAG_INSTALL_TRACKING_ONLY = 2;

		public const int FLAG_AMAZON = 4;

		public const int FLAG_DISABLE_MEDIATION = 8;

		public const int FLAG_DISABLE_AUTOMATIC_IAP_RECORDING = 16;

		public const int FLAG_NATIVE_ADS_ONLY = 32;

		public const int FLAG_CHILD_DIRECTED_ADS = 64;

		[Obsolete("Use FLAG_AMAZON instead - we refactored the flags to be consistently named.")]
		public const int AMAZON = 4;

		[Obsolete("Use FLAG_DISABLE_MEDIATION instead - we refactored the flags to be consistently named.")]
		public const int DISABLE_MEDIATION = 8;

		public const string DEFAULT_TAG = "default";

		public delegate void NetworkCallbackListener(string network, string callback);

		public static class NetworkCallback
		{
			public const string INITIALIZED = "initialized";

			public const string SHOW = "show";

			public const string AVAILABLE = "available";

			public const string HIDE = "hide";

			public const string FETCH_FAILED = "fetch_failed";

			public const string CLICK = "click";

			public const string DISMISS = "dismiss";

			public const string INCENTIVIZED_RESULT_COMPLETE = "incentivized_result_complete";

			public const string INCENTIVIZED_RESULT_INCOMPLETE = "incentivized_result_incomplete";

			public const string AUDIO_STARTING = "audio_starting";

			public const string AUDIO_FINISHED = "audio_finished";

			public const string BANNER_LOADED = "banner-loaded";

			public const string BANNER_CLICK = "banner-click";

			public const string BANNER_HIDE = "banner-hide";

			public const string BANNER_DISMISS = "banner-dismiss";

			public const string BANNER_FETCH_FAILED = "banner-fetch_failed";

			public const string LEAVE_APPLICATION = "leave_application";

			public const string FACEBOOK_LOGGING_IMPRESSION = "logging_impression";

			public const string CHARTBOOST_MOREAPPS_FETCH_FAILED = "moreapps-fetch_failed";

			public const string CHARTBOOST_MOREAPPS_HIDE = "moreapps-hide";

			public const string CHARTBOOST_MOREAPPS_DISMISS = "moreapps-dismiss";

			public const string CHARTBOOST_MOREAPPS_CLICK = "moreapps-click";

			public const string CHARTBOOST_MOREAPPS_SHOW = "moreapps-show";

			public const string CHARTBOOST_MOREAPPS_AVAILABLE = "moreapps-available";

			public const string CHARTBOOST_MOREAPPS_CLICK_FAILED = "moreapps-click_failed";
		}

		public static class Network
		{
			public const string HEYZAP = "heyzap";

			public const string HEYZAP_CROSS_PROMO = "heyzap_cross_promo";

			public const string HEYZAP_EXCHANGE = "heyzap_exchange";

			public const string FACEBOOK = "facebook";

			public const string UNITYADS = "unityads";

			public const string APPLOVIN = "applovin";

			public const string VUNGLE = "vungle";

			public const string CHARTBOOST = "chartboost";

			public const string ADCOLONY = "adcolony";

			public const string ADMOB = "admob";

			public const string IAD = "iad";

			public const string LEADBOLT = "leadbolt";

			public const string INMOBI = "inmobi";
		}
	}
}
