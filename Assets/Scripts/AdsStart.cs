using System;
using GssAdSdk;
using UnityEngine;
using UnityEngine.UI;

public class AdsStart : MonoBehaviour
{
	private void Awake()
	{
		if (AdsStart.onetime)
		{
			MoPubAds.request_addIDs();
			MoPubAds.initBanner(MoPubAds._bannerAdUnitId, MoPubAdPosition.BottomCenter);
			MoPubAds.loadAd(MoPubAds._interstitialOnGpEndId);
			MoPubAds.loadAd(MoPubAds._interstitialOnExit);
			MoPubAds.loadAd(MoPubAds._interstitialOnSelectionId);
			MoPubAds.requestRewardVideo(MoPubAds._interstitialOnVideo);
			AdsStart.onetime = false;
		}
	}

	private void Start()
	{
		AdsManagerMainMenu.instance.hidebanner();
		MoPubAds.showBanner(MoPubAds._bannerAdUnitId);
		MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
		this.maintext.text = string.Empty + AGameUtils.PRODUCT_NAME;
	}

	public void showbanner()
	{
		this.maintext.text = string.Empty + MoPubAds._bannerAdUnitId;
		MoPubAds.showBanner(MoPubAds._bannerAdUnitId);
	}

	public void hidebanner()
	{
		MoPubAds.hideBanner(MoPubAds._bannerAdUnitId);
	}

	public void showmovie()
	{
		Handheld.PlayFullScreenMovie("loading.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
	}

	public void show_interestial()
	{
		this.maintext.text = string.Empty + MoPubAds._interstitialOnSelectionId;
		MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
	}

	public void levelload()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void printpackages()
	{
		string text = TenlogixAds.getaddsurlpackage();
		UnityEngine.Debug.Log("print" + text);
		this.maintext.text = string.Empty + text;
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + this.GetProductName(text));
	}

	public void get_bundleindetifier()
	{
		this.maintext.text = string.Empty + Application.identifier;
	}

	public void get_domainname()
	{
		this.maintext.text = string.Empty + AGameUtils.MORE_APPS_DN;
		Application.OpenURL("https://play.google.com/store/apps/developer?id=" + AGameUtils.MORE_APPS_DN);
	}

	public void get_siteurl()
	{
		this.maintext.text = string.Empty + TenlogixAds.UR;
	}

	public void get_gpend()
	{
		this.maintext.text = string.Empty + MoPubAds._interstitialOnGpEndId;
		MoPubAds.showAd(MoPubAds._interstitialOnGpEndId);
	}

	public void get_exit()
	{
		this.maintext.text = string.Empty + MoPubAds._interstitialOnExit;
		MoPubAds.showAd(MoPubAds._interstitialOnExit);
	}

	public void get_rewardvideo()
	{
		this.maintext.text = string.Empty + MoPubAds._interstitialOnVideo;
		MoPubAds.showRewardVideo(MoPubAds._interstitialOnVideo);
	}

	private string GetProductName(string getstringname)
	{
		string text = getstringname;
		if (text.Contains("/") && text.Contains("."))
		{
			int num = text.LastIndexOf("/", StringComparison.Ordinal) + 1;
			getstringname = text.Substring(num, text.Length - num);
			num = getstringname.LastIndexOf(".", StringComparison.Ordinal) + 1;
			MonoBehaviour.print(string.Concat(new object[]
			{
				"lastpart-",
				num,
				"getstringlenth",
				getstringname.Length
			}));
			return getstringname.Substring(0, getstringname.Length - (getstringname.Length - num + 1));
		}
		return text;
	}

	public static bool onetime = true;

	public Text maintext;
}
