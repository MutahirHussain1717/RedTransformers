using System;
using UnityEngine;

namespace Heyzap
{
	public class HZIncentivizedAdAndroid : MonoBehaviour
	{
		public static void ShowWithOptions(HZIncentivizedShowOptions showOptions)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("showIncentivized", new object[]
				{
					showOptions.Tag,
					showOptions.IncentivizedInfo
				});
			}
		}

		public static void Fetch(string tag)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				androidJavaClass.CallStatic("fetchIncentivized", new object[]
				{
					tag
				});
			}
		}

		public static bool IsAvailable(string tag)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				result = androidJavaClass.CallStatic<bool>("isIncentivizedAvailable", new object[]
				{
					tag
				});
			}
			return result;
		}
	}
}
