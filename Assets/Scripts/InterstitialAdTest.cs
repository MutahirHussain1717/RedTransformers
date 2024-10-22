using System;
using AudienceNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterstitialAdTest : MonoBehaviour
{
	public void LoadInterstitial()
	{
		this.statusLabel.text = "Loading interstitial ad...";
		InterstitialAd interstitialAd = new InterstitialAd("671547753055630_671548149722257");
		this.interstitialAd = interstitialAd;
		this.interstitialAd.Register(base.gameObject);
		this.interstitialAd.InterstitialAdDidLoad = delegate()
		{
			UnityEngine.Debug.Log("Interstitial ad loaded.");
			this.isLoaded = true;
			this.statusLabel.text = "Ad loaded. Click show to present!";
		};
		interstitialAd.InterstitialAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("Interstitial ad failed to load with error: " + error);
			this.statusLabel.text = "Interstitial ad failed to load. Check console for details.";
		};
		interstitialAd.InterstitialAdWillLogImpression = delegate()
		{
			UnityEngine.Debug.Log("Interstitial ad logged impression.");
		};
		interstitialAd.InterstitialAdDidClick = delegate()
		{
			UnityEngine.Debug.Log("Interstitial ad clicked.");
		};
		this.interstitialAd.LoadAd();
	}

	public void ShowInterstitial()
	{
		if (this.isLoaded)
		{
			this.interstitialAd.Show();
			this.isLoaded = false;
			this.statusLabel.text = string.Empty;
		}
		else
		{
			this.statusLabel.text = "Ad not loaded. Click load to request an ad.";
		}
	}

	private void OnDestroy()
	{
		if (this.interstitialAd != null)
		{
			this.interstitialAd.Dispose();
		}
		UnityEngine.Debug.Log("InterstitialAdTest was destroyed!");
	}

	public void NextScene()
	{
		SceneManager.LoadScene("AdViewScene");
	}

	private InterstitialAd interstitialAd;

	private bool isLoaded;

	public Text statusLabel;
}
