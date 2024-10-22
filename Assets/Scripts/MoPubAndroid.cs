using System;
using UnityEngine;

public class MoPubAndroid
{
	public static void addFacebookTestDeviceId(string hashedDeviceId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		MoPubAndroid._pluginClass.CallStatic("addFacebookTestDeviceId", new object[]
		{
			hashedDeviceId
		});
	}

	public static void setLocationAwareness(MoPubLocationAwareness locationAwareness)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		MoPubAndroid._pluginClass.CallStatic("setLocationAwareness", new object[]
		{
			locationAwareness.ToString()
		});
	}

	public static void reportApplicationOpen()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		MoPubAndroid._pluginClass.CallStatic("reportApplicationOpen", new object[0]);
	}

	private static readonly AndroidJavaClass _pluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");
}
