using System;
using UnityEngine;

public class MoPubAndroidBanner
{
	public MoPubAndroidBanner(string adUnitId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._bannerPlugin = new AndroidJavaObject("com.mopub.unity.MoPubBannerUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public void createBanner(MoPubAdPosition position)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._bannerPlugin.Call("createBanner", new object[]
		{
			(int)position
		});
	}

	public void destroyBanner()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._bannerPlugin.Call("destroyBanner", new object[0]);
	}

	public void showBanner(bool shouldShow)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._bannerPlugin.Call("hideBanner", new object[]
		{
			!shouldShow
		});
	}

	public void setBannerKeywords(string keywords)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._bannerPlugin.Call("setBannerKeywords", new object[]
		{
			keywords
		});
	}

	private readonly AndroidJavaObject _bannerPlugin;
}
