using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPrefab : MonoBehaviour
{
	private void Awake()
	{
		if (MainMenuPrefab.mainmenufirst && Application.platform == RuntimePlatform.Android)
		{
			MoPubAds.request_addIDs();
			MoPubAds.initBanner(MoPubAds._bannerAdUnitId, MoPubAdPosition.BottomCenter);
			MoPubAds.loadAd(MoPubAds._interstitialOnGpEndId);
			MoPubAds.loadAd(MoPubAds._interstitialOnExit);
			MoPubAds.loadAd(MoPubAds._interstitialOnSelectionId);
			MoPubAds.initializeRewardVideo(MoPubAds._interstitialOnVideo);
			MoPubAds.requestRewardVideo(MoPubAds._interstitialOnVideo);
		}
	}

	private void Start()
	{
		Time.timeScale = 1f;
		this.gamename.text = string.Empty + PrivacyTextGetter.privacy_Content;
		if (!MainMenuPrefab.mainmenufirst && Application.platform == RuntimePlatform.Android)
		{
			AdsManagerMainMenu.instance.hidebanner();
			MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
		}
		MoPubAds.showBanner(MoPubAds._bannerAdUnitId);
		MainMenuPrefab.mainmenufirst = false;
	}

	public static bool mainmenufirst = true;

	public Text gamename;
}
