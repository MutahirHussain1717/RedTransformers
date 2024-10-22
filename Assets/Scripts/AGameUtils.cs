using System;
using Analytics;
using UnityEngine;

public class AGameUtils : MonoBehaviour
{
	public static void initAnalytics()
	{
		Analytics.MonoSingleton<Flurry>.Instance.StartSession(AGameUtils.FLURRY_ID, AGameUtils.FLURRY_ID);
	}

	public static void SendFeedbackMail()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.oas.emailcompose.EmailActivity");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		androidJavaObject.Call("sendEmail", new object[]
		{
			AGameUtils.PRODUCT_NAME,
			AGameUtils.PACKAGE_NAME,
			AGameUtils.EMAIL_VERSION,
			AGameUtils.EMAIL_ID
		});
	}

	public static void rateUsLink()
	{
		string url = "market://details?id=" + AGameUtils.PACKAGE_NAME + string.Empty;
		Application.OpenURL(url);
	}

	public static void moreAppsLink()
	{
		Application.OpenURL("market://search?q=pub:" + AGameUtils.MORE_APPS_DN);
	}

	public static void LogAnalyticEvent(string eventMessage)
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Analytics.MonoSingleton<Flurry>.Instance.LogEvent(eventMessage);
		}
	}

	public static string PRODUCT_NAME = string.Empty;

	public static string PACKAGE_NAME = string.Empty + Application.identifier;

	public static string EMAIL_VERSION = "1.0";

	public static string Cipher_Passwords = "Xikasdfadwrt4g69";

	public static string EMAIL_ID = string.Empty;

	public static string MORE_APPS_DN = string.Empty;

	public static string INAPP_ID = string.Empty;

	public static string FLURRY_ID = string.Empty;
}
