using System;
using System.Collections.Generic;
using UnityEngine;

public static class MoPub
{
	public static void loadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
	{
		foreach (string text in bannerAdUnitIds)
		{
			MoPub._bannerPluginsDict.Add(text, new MoPubAndroidBanner(text));
		}
		UnityEngine.Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
	}

	public static void loadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
	{
		foreach (string text in interstitialAdUnitIds)
		{
			MoPub._interstitialPluginsDict.Add(text, new MoPubAndroidInterstitial(text));
		}
		UnityEngine.Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
	}

	public static void loadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			MoPub._rewardedVideoPluginsDict.Add(text, new MoPubAndroidRewardedVideo(text));
		}
		UnityEngine.Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
	}

	public static void enableLocationSupport(bool shouldUseLocation)
	{
		MoPubAndroid.setLocationAwareness(MoPubLocationAwareness.NORMAL);
	}

	public static void reportApplicationOpen(string iTunesAppId = null)
	{
		MoPubAndroid.reportApplicationOpen();
	}

	public static void createBanner(string adUnitId, MoPubAdPosition position)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPub._bannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.createBanner(position);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void destroyBanner(string adUnitId)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPub._bannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.destroyBanner();
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showBanner(string adUnitId, bool shouldShow)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPub._bannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.showBanner(shouldShow);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void requestInterstitialAd(string adUnitId, string keywords = "")
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPub._interstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			moPubAndroidInterstitial.requestInterstitialAd(keywords);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showInterstitialAd(string adUnitId)
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPub._interstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			moPubAndroidInterstitial.showInterstitialAd();
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void initializeRewardedVideo()
	{
		MoPubAndroidRewardedVideo.initializeRewardedVideo();
	}

	public static void initializeRewardedVideo(MoPubRewardedNetwork[] networks)
	{
		MoPubAndroidRewardedVideo.initializeRewardedVideoWithNetworks(networks);
	}

	public static void requestRewardedVideo(string adUnitId, List<MoPubMediationSetting> mediationSettings = null, string keywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPub._rewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.requestRewardedVideo(mediationSettings, keywords, latitude, longitude, customerId);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showRewardedVideo(string adUnitId)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPub._rewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.showRewardedVideo();
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static bool hasRewardedVideo(string adUnitId)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPub._rewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			return moPubAndroidRewardedVideo.hasRewardedVideo();
		}
		UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		return false;
	}

	public static List<MoPubManager.MoPubReward> getAVailableRewards(string adUnitId)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPub._rewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			return moPubAndroidRewardedVideo.getAVailableRewards();
		}
		UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		return null;
	}

	public static void selectReward(string adUnitId, MoPubManager.MoPubReward selectedReward)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPub._rewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.selectReward(selectedReward);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public const double LAT_LONG_SENTINEL = 99999.0;

	public const string ADUNIT_NOT_FOUND_MSG = "AdUnit {0} not found: no plugin was initialized";

	private static Dictionary<string, MoPubAndroidBanner> _bannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();

	private static Dictionary<string, MoPubAndroidInterstitial> _interstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();

	private static Dictionary<string, MoPubAndroidRewardedVideo> _rewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();
}
