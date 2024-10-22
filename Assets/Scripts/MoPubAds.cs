using System;
using System.Collections.Generic;
using UnityEngine;

public static class MoPubAds
{
	public static void initMoPubPlugin()
	{
		MoPub.loadBannerPluginsForAdUnits(MoPubAds.allBannerAdUnits.ToArray());
		MoPub.loadInterstitialPluginsForAdUnits(MoPubAds.allInterstitialAdUnits.ToArray());
		MoPub.loadRewardedVideoPluginsForAdUnits(MoPubAds.allRewardedVideoAdUnits.ToArray());
		MoPub.initializeRewardedVideo();
	}

	public static void initBanner(string adUnitId, MoPubAdPosition position)
	{
		if (PlayerPrefs.GetInt(MoPubAds._RemoveAdPrefs, 0) != 1 && !MoPubAds.bannerInit)
		{
			MoPubAds.bannerInit = true;
			MoPub.createBanner(adUnitId, position);
		}
	}

	public static void hideBanner(string adUnitId)
	{
		UnityEngine.Debug.Log("hideBanner called");
		MoPub.showBanner(adUnitId, false);
		MoPubAds.bannerShouldHide = true;
	}

	public static void showBanner(string adUnitId)
	{
		if (PlayerPrefs.GetInt(MoPubAds._RemoveAdPrefs, 0) != 1)
		{
			MoPub.showBanner(adUnitId, true);
			MoPubAds.bannerShouldHide = false;
		}
	}

	public static void destroyBanner(string adUnitId)
	{
		MoPub.destroyBanner(adUnitId);
	}

	public static void destroyBanner()
	{
		MoPub.destroyBanner(MoPubAds._bannerAdUnitId);
	}

	public static void loadAd(string adUnitId)
	{
		if (PlayerPrefs.GetInt(MoPubAds._RemoveAdPrefs, 0) != 1)
		{
			MoPub.requestInterstitialAd(adUnitId, string.Empty);
		}
	}

	public static void showAd(string adUnitId)
	{
		if (PlayerPrefs.GetInt(MoPubAds._RemoveAdPrefs, 0) != 1)
		{
			MoPub.showInterstitialAd(adUnitId);
			MoPubAds.loadAd(adUnitId);
		}
	}

	public static void initializeRewardVideo(string AdUnitID)
	{
		MoPub.initializeRewardedVideo();
	}

	public static void requestRewardVideo(string AdUnitID)
	{
		MoPub.requestRewardedVideo(AdUnitID, null, null, 99999.0, 99999.0, null);
	}

	public static void showRewardVideo(string AdUnitID)
	{
		MoPub.showRewardedVideo(AdUnitID);
		MoPubAds.requestRewardVideo(AdUnitID);
	}

	public static void reportApplicationOpen()
	{
		MoPub.reportApplicationOpen(null);
	}

	public static void addBannerAdUnit(string adUnit)
	{
		if (!MoPubAds.allBannerAdUnits.Contains(adUnit))
		{
			MoPubAds.allBannerAdUnits.Add(adUnit);
		}
	}

	public static void addInterstitialAdUnit(string adUnit)
	{
		if (!MoPubAds.allInterstitialAdUnits.Contains(adUnit))
		{
			MoPubAds.allInterstitialAdUnits.Add(adUnit);
		}
	}

	public static void addRewardedAdUnit(string adUnit)
	{
		if (!MoPubAds.allRewardedVideoAdUnits.Contains(adUnit))
		{
			MoPubAds.allRewardedVideoAdUnits.Add(adUnit);
		}
	}

	public static void request_addIDs()
	{
		MoPubAds.addBannerAdUnit(MoPubAds._bannerAdUnitId);
		MoPubAds.addInterstitialAdUnit(MoPubAds._interstitialOnSelectionId);
		MoPubAds.addInterstitialAdUnit(MoPubAds._interstitialOnGpEndId);
		MoPubAds.addInterstitialAdUnit(MoPubAds._interstitialOnExit);
		MoPubAds.addRewardedAdUnit(MoPubAds._interstitialOnVideo);
		MoPubAds.initMoPubPlugin();
	}

	public static string _bannerAdUnitId = string.Empty;

	public static string _nativeAdId = string.Empty;

	public static string _application_Id = string.Empty;

	public static string _interstitialOnStartId = string.Empty;

	public static string _interstitialOnSelectionId = string.Empty;

	public static string _interstitialOnGpEndId = string.Empty;

	public static string _interstitialOnExit = string.Empty;

	public static string _interstitialOnVideo = string.Empty;

	public static string _rewardedOnSkipLevel = string.Empty;

	public static List<string> allBannerAdUnits = new List<string>();

	public static List<string> allInterstitialAdUnits = new List<string>();

	public static List<string> allRewardedVideoAdUnits = new List<string>();

	public static string _RemoveAdPrefs = "removeAds";

	private static bool bannerInit = false;

	public static bool bannerShouldHide = false;

	private static bool nativeInit = false;
}
