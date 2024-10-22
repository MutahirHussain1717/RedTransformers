using System;
using GoogleMobileAds.Api;
using GssAdSdk;
using Heyzap;
using UnityEngine;

public class AdsManagerMainMenu : MonoBehaviour
{
	private void Awake()
	{
		AdsManagerMainMenu.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(AdsManagerMainMenu.instance);
	}

	private void Start()
	{
	}

	public void showsmartbanner()
	{
		if (TenlogixAds.AdmobFill && AdsManagerMainMenu.checksmartbanner)
		{
			AdsManagerMainMenu.bannerView.Show();
		}
	}

	public void hidebanner()
	{
		if (TenlogixAds.AdmobFill && AdsManagerMainMenu.checksmartbanner)
		{
			AdsManagerMainMenu.bannerView.Hide();
		}
	}

	public void request_smart_banner()
	{
		if (TenlogixAds.AdmobFill)
		{
			if (!AdsManagerMainMenu.checksmartbanner)
			{
				string admobBannerID = AdIDs.AdmobBannerID;
				AdsManagerMainMenu.bannerView = new BannerView(admobBannerID, AdSize.SmartBanner, AdPosition.Bottom);
				AdsManagerMainMenu.bannerView.OnAdLoaded += this.HandleAdLoaded;
				AdsManagerMainMenu.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
				AdsManagerMainMenu.bannerView.OnAdLoaded += this.HandleAdOpened;
				AdsManagerMainMenu.bannerView.OnAdClosed += this.HandleAdClosed;
				AdsManagerMainMenu.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;
				AdRequest request = new AdRequest.Builder().Build();
				AdsManagerMainMenu.bannerView.LoadAd(request);
			}
			else
			{
				this.showsmartbanner();
			}
		}
	}

	public void request_nativead()
	{
		if (TenlogixAds.AdmobFill)
		{
			string admobNative = AdIDs.AdmobNative;
			AdsManagerMainMenu.nativeExpressAdView = new NativeExpressAdView(admobNative, new AdSize(300, 250), AdPosition.Bottom);
			AdsManagerMainMenu.nativeExpressAdView.LoadAd(new AdRequest.Builder().Build());
		}
	}

	public void request_nativead_small(AdPosition adpos)
	{
		if (TenlogixAds.AdmobFill)
		{
			string admobNative = AdIDs.AdmobNative;
			AdsManagerMainMenu.nativeExpressAdView = new NativeExpressAdView(admobNative, new AdSize(320, 150), adpos);
			AdsManagerMainMenu.nativeExpressAdView.LoadAd(new AdRequest.Builder().Build());
		}
	}

	public void request_nativead_large()
	{
		if (TenlogixAds.AdmobFill)
		{
			string admobNative = AdIDs.AdmobNative;
			AdsManagerMainMenu.nativeExpressAdView = new NativeExpressAdView(admobNative, new AdSize(320, 320), AdPosition.Bottom);
			AdsManagerMainMenu.nativeExpressAdView.LoadAd(new AdRequest.Builder().Build());
		}
	}

	public void destroy_nativead()
	{
		if (TenlogixAds.AdmobFill)
		{
			AdsManagerMainMenu.nativeExpressAdView.Destroy();
		}
	}

	public bool getisrewardvideoavailable()
	{
		return AdsManagerMainMenu.rewardBasedVideo.IsLoaded();
	}

	public void request_rewardvideo()
	{
		if (TenlogixAds.AdmobFill)
		{
			string empty = string.Empty;
			AdsManagerMainMenu.rewardBasedVideo = RewardBasedVideoAd.Instance;
			AdRequest request = new AdRequest.Builder().Build();
			AdsManagerMainMenu.rewardBasedVideo.LoadAd(request, empty);
		}
	}

	public void showrewardvideo()
	{
		if (TenlogixAds.AdmobFill)
		{
			if (AdsManagerMainMenu.rewardBasedVideo.IsLoaded())
			{
				AdsManagerMainMenu.rewardBasedVideo.Show();
			}
			else
			{
				this.request_rewardvideo();
			}
		}
	}

	public void HeyzapVdo()
	{
		if (HZInterstitialAd.IsAvailable())
		{
			HZInterstitialAd.Show();
		}
		else
		{
			HZInterstitialAd.Fetch();
		}
	}

	public void watchvideomopub()
	{
		MoPubAds.showRewardVideo(MoPubAds._interstitialOnVideo);
	}

	private void HandleAdLoaded(object sender, EventArgs args)
	{
		AdsManagerMainMenu.checksmartbanner = true;
		UnityEngine.Debug.Log("HandleAdLoaded event received.");
	}

	private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		UnityEngine.Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	private void HandleAdOpened(object sender, EventArgs args)
	{
		UnityEngine.Debug.Log("HandleAdOpened event received");
	}

	private void HandleAdClosing(object sender, EventArgs args)
	{
		UnityEngine.Debug.Log("HandleAdClosing event received");
	}

	private void HandleAdClosed(object sender, EventArgs args)
	{
		UnityEngine.Debug.Log("HandleAdClosed event received");
	}

	private void HandleAdLeftApplication(object sender, EventArgs args)
	{
		UnityEngine.Debug.Log("HandleAdLeftApplication event received");
	}

	public static AdsManagerMainMenu instance;

	private int adscount;

	public static BannerView bannerView;

	public static NativeExpressAdView nativeExpressAdView;

	public static RewardBasedVideoAd rewardBasedVideo;

	public static bool checksmartbanner;
}
