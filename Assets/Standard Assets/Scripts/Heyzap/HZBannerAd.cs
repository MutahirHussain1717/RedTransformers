using System;
using UnityEngine;

namespace Heyzap
{
	public class HZBannerAd : MonoBehaviour
	{
		public static void ShowWithOptions(HZBannerShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZBannerShowOptions();
			}
			HZBannerAdAndroid.ShowWithOptions(showOptions);
		}

		public static bool GetCurrentBannerDimensions(out Rect banner)
		{
			return HZBannerAdAndroid.GetCurrentBannerDimensions(out banner);
		}

		public static void Hide()
		{
			HZBannerAdAndroid.Hide();
		}

		public static void Destroy()
		{
			HZBannerAdAndroid.Destroy();
		}

		public static void SetDisplayListener(HZBannerAd.AdDisplayListener listener)
		{
			HZBannerAd.adDisplayListener = listener;
		}

		public static void InitReceiver()
		{
			if (HZBannerAd._instance == null)
			{
				GameObject gameObject = new GameObject("HZBannerAd");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				HZBannerAd._instance = gameObject.AddComponent<HZBannerAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(new char[]
			{
				','
			});
			HZBannerAd.SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (HZBannerAd.adDisplayListener != null)
			{
				HZBannerAd.adDisplayListener(state, tag);
			}
		}

		[Obsolete("Use ShowWithOptions() to show ads instead of this deprecated method.")]
		public static void showWithTag(string position, string tag)
		{
			HZBannerAd.ShowWithOptions(new HZBannerShowOptions
			{
				Position = position,
				Tag = tag
			});
		}

		[Obsolete("Use ShowWithOptions() to show ads instead of this deprecated method.")]
		public static void show(string position)
		{
			HZBannerAd.ShowWithOptions(new HZBannerShowOptions
			{
				Position = position
			});
		}

		[Obsolete("Use the GetCurrentBannerDimensions(out Rect) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static bool getCurrentBannerDimensions(out Rect banner)
		{
			return HZBannerAd.GetCurrentBannerDimensions(out banner);
		}

		[Obsolete("Use the Hide() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void hide()
		{
			HZBannerAd.Hide();
		}

		[Obsolete("Use the Destroy() method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void destroy()
		{
			HZBannerAd.Destroy();
		}

		[Obsolete("Use the SetDisplayListener(AdDisplayListener) method instead - it uses the proper PascalCase for C#. Older versions of our SDK used incorrect casing.")]
		public static void setDisplayListener(HZBannerAd.AdDisplayListener listener)
		{
			HZBannerAd.SetDisplayListener(listener);
		}

		private static HZBannerAd.AdDisplayListener adDisplayListener;

		private static HZBannerAd _instance;

		[Obsolete("This constant has been relocated to HZBannerShowOptions")]
		public const string POSITION_TOP = "top";

		[Obsolete("This constant has been relocated to HZBannerShowOptions")]
		public const string POSITION_BOTTOM = "bottom";

		public delegate void AdDisplayListener(string state, string tag);
	}
}
