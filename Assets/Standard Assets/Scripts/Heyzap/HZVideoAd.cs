using System;
using UnityEngine;

namespace Heyzap
{
	public class HZVideoAd : MonoBehaviour
	{
		public static void Show()
		{
			HZVideoAd.ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZShowOptions();
			}
			HZVideoAdAndroid.ShowWithOptions(showOptions);
		}

		public static void Fetch()
		{
			HZVideoAd.Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZVideoAdAndroid.Fetch(tag);
		}

		public static bool IsAvailable()
		{
			return HZVideoAd.IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZVideoAdAndroid.IsAvailable(tag);
		}

		public static void SetDisplayListener(HZVideoAd.AdDisplayListener listener)
		{
			HZVideoAd.adDisplayListener = listener;
		}

		public static void InitReceiver()
		{
			if (HZVideoAd._instance == null)
			{
				GameObject gameObject = new GameObject("HZVideoAd");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				HZVideoAd._instance = gameObject.AddComponent<HZVideoAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(new char[]
			{
				','
			});
			HZVideoAd.SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (HZVideoAd.adDisplayListener != null)
			{
				HZVideoAd.adDisplayListener(state, tag);
			}
		}

		[Obsolete("Use the Fetch() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch()
		{
			HZVideoAd.Fetch();
		}

		[Obsolete("Use the Fetch(string) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void fetch(string tag)
		{
			HZVideoAd.Fetch(tag);
		}

		[Obsolete("Use the Show() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void show()
		{
			HZVideoAd.Show();
		}

		[Obsolete("Use ShowWithOptions() to show ads instead of this deprecated method.")]
		public static void show(string tag)
		{
			HZVideoAd.ShowWithOptions(new HZIncentivizedShowOptions
			{
				Tag = tag
			});
		}

		[Obsolete("Use the IsAvailable() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable()
		{
			return HZVideoAd.IsAvailable();
		}

		[Obsolete("Use the IsAvailable(tag) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool isAvailable(string tag)
		{
			return HZVideoAd.IsAvailable(tag);
		}

		[Obsolete("Use the SetDisplayListener(AdDisplayListener) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void setDisplayListener(HZVideoAd.AdDisplayListener listener)
		{
			HZVideoAd.SetDisplayListener(listener);
		}

		private static HZVideoAd.AdDisplayListener adDisplayListener;

		private static HZVideoAd _instance;

		public delegate void AdDisplayListener(string state, string tag);
	}
}
