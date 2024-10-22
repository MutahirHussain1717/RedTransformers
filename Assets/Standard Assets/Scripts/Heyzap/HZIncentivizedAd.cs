using System;
using UnityEngine;

namespace Heyzap
{
	public class HZIncentivizedAd : MonoBehaviour
	{
		public static void Fetch()
		{
			HZIncentivizedAd.Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZIncentivizedAdAndroid.Fetch(tag);
		}

		public static void Show()
		{
			HZIncentivizedAd.ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZIncentivizedShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZIncentivizedShowOptions();
			}
			HZIncentivizedAdAndroid.ShowWithOptions(showOptions);
		}

		public static bool IsAvailable()
		{
			return HZIncentivizedAd.IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZIncentivizedAdAndroid.IsAvailable(tag);
		}

		public static void SetDisplayListener(HZIncentivizedAd.AdDisplayListener listener)
		{
			HZIncentivizedAd.adDisplayListener = listener;
		}

		public static void InitReceiver()
		{
			if (HZIncentivizedAd._instance == null)
			{
				GameObject gameObject = new GameObject("HZIncentivizedAd");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				HZIncentivizedAd._instance = gameObject.AddComponent<HZIncentivizedAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(new char[]
			{
				','
			});
			HZIncentivizedAd.SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (HZIncentivizedAd.adDisplayListener != null)
			{
				HZIncentivizedAd.adDisplayListener(state, tag);
			}
		}

		[Obsolete("Use the Fetch() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch()
		{
			HZIncentivizedAd.Fetch();
		}

		[Obsolete("Use the Fetch(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch(string tag)
		{
			HZIncentivizedAd.Fetch(tag);
		}

		[Obsolete("Use the Show() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void show()
		{
			HZIncentivizedAd.Show();
		}

		[Obsolete("Use ShowWithOptions() to show ads instead of this deprecated method.")]
		public static void show(string tag)
		{
			HZIncentivizedAd.ShowWithOptions(new HZIncentivizedShowOptions
			{
				Tag = tag
			});
		}

		[Obsolete("Use the IsAvailable() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable()
		{
			return HZIncentivizedAd.IsAvailable();
		}

		[Obsolete("Use the IsAvailable(tag) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable(string tag)
		{
			return HZIncentivizedAd.IsAvailable(tag);
		}

		[Obsolete("Use the SetDisplayListener(AdDisplayListener) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void setDisplayListener(HZIncentivizedAd.AdDisplayListener listener)
		{
			HZIncentivizedAd.SetDisplayListener(listener);
		}

		private static HZIncentivizedAd.AdDisplayListener adDisplayListener;

		private static HZIncentivizedAd _instance;

		public delegate void AdDisplayListener(string state, string tag);
	}
}
