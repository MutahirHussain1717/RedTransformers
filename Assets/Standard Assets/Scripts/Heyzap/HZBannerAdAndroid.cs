using System;
using UnityEngine;

namespace Heyzap
{
	public class HZBannerAdAndroid : MonoBehaviour
	{
		public static bool GetCurrentBannerDimensions(out Rect banner)
		{
			banner = new Rect(0f, 0f, 0f, 0f);
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				string text = androidJavaClass.CallStatic<string>("getBannerDimensions", new object[0]);
				if (text == null || text.Length == 0)
				{
					result = false;
				}
				else
				{
					string[] array = text.Split(new char[]
					{
						' '
					});
					if (array.Length != 4)
					{
						result = false;
					}
					else
					{
						banner = new Rect((float)int.Parse(array[0]), (float)int.Parse(array[1]), (float)int.Parse(array[2]), (float)int.Parse(array[3]));
						result = true;
					}
				}
			}
			return result;
		}

		public static void ShowWithOptions(HZBannerShowOptions showOptions)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("showBanner", new object[]
				{
					showOptions.Tag,
					showOptions.Position
				});
			}
		}

		public static void Hide()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("hideBanner", new object[0]);
			}
		}

		public static void Destroy()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("destroyBanner", new object[0]);
			}
		}
	}
}
