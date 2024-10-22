using System;
using UnityEngine;

namespace Heyzap
{
	public class HZInterstitialAd : MonoBehaviour
	{
		public static void Show()
		{
			HZInterstitialAd.ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZShowOptions();
			}
			HZInterstitialAdAndroid.ShowWithOptions(showOptions);
		}

		public static void Fetch()
		{
			HZInterstitialAd.Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZInterstitialAdAndroid.Fetch(tag);
		}

		public static bool IsAvailable()
		{
			return HZInterstitialAd.IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZInterstitialAdAndroid.IsAvailable(tag);
		}

		public static void SetDisplayListener(HZInterstitialAd.AdDisplayListener listener)
		{
			HZInterstitialAd.adDisplayListener = listener;
		}

		public static void ChartboostFetchForLocation(string location)
		{
			HZInterstitialAdAndroid.chartboostFetchForLocation(location);
		}

		public static bool ChartboostIsAvailableForLocation(string location)
		{
			return HZInterstitialAdAndroid.chartboostIsAvailableForLocation(location);
		}

		public static void ChartboostShowForLocation(string location)
		{
			HZInterstitialAdAndroid.chartboostShowForLocation(location);
		}

		public static void InitReceiver()
		{
			if (HZInterstitialAd._instance == null)
			{
				GameObject gameObject = new GameObject("HZInterstitialAd");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				HZInterstitialAd._instance = gameObject.AddComponent<HZInterstitialAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(new char[]
			{
				','
			});
			HZInterstitialAd.SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (HZInterstitialAd.adDisplayListener != null)
			{
				HZInterstitialAd.adDisplayListener(state, tag);
			}
		}

		[Obsolete("Use the Fetch() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch()
		{
			HZInterstitialAd.Fetch();
		}

		[Obsolete("Use the Fetch(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch(string tag)
		{
			HZInterstitialAd.Fetch(tag);
		}

		[Obsolete("Use the Show() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void show()
		{
			HZInterstitialAd.Show();
		}

		[Obsolete("Use ShowWithOptions() to show ads instead of this deprecated method.")]
		public static void show(string tag)
		{
			HZInterstitialAd.ShowWithOptions(new HZIncentivizedShowOptions
			{
				Tag = tag
			});
		}

		[Obsolete("Use the IsAvailable() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable()
		{
			return HZInterstitialAd.IsAvailable();
		}

		[Obsolete("Use the IsAvailable(tag) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable(string tag)
		{
			return HZInterstitialAd.IsAvailable(tag);
		}

		[Obsolete("Use the SetDisplayListener(AdDisplayListener) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void setDisplayListener(HZInterstitialAd.AdDisplayListener listener)
		{
			HZInterstitialAd.SetDisplayListener(listener);
		}

		[Obsolete("Use the ChartboostFetchForLocation(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void chartboostFetchForLocation(string location)
		{
			HZInterstitialAd.ChartboostFetchForLocation(location);
		}

		[Obsolete("Use the ChartboostIsAvailableForLocation(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool chartboostIsAvailableForLocation(string location)
		{
			return HZInterstitialAd.ChartboostIsAvailableForLocation(location);
		}

		[Obsolete("Use the ChartboostShowForLocation(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void chartboostShowForLocation(string location)
		{
			HZInterstitialAd.ChartboostShowForLocation(location);
		}

		private static HZInterstitialAd.AdDisplayListener adDisplayListener;

		private static HZInterstitialAd _instance;

		public delegate void AdDisplayListener(string state, string tag);
	}
}
