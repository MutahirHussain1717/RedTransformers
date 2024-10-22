using System;
using UnityEngine;

public class MoPubAndroidInterstitial
{
	public MoPubAndroidInterstitial(string adUnitId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public void requestInterstitialAd(string keywords = "")
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._interstitialPlugin.Call("requestInterstitialAd", new object[]
		{
			keywords
		});
	}

	public void showInterstitialAd()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._interstitialPlugin.Call("showInterstitialAd", new object[0]);
	}

	private readonly AndroidJavaObject _interstitialPlugin;
}
