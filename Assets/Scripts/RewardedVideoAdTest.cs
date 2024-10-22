using System;
using AudienceNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedVideoAdTest : MonoBehaviour
{
	public void LoadRewardedVideo()
	{
		this.statusLabel.text = "Loading rewardedVideo ad...";
		RewardedVideoAd rewardedVideoAd = new RewardedVideoAd("YOUR_PLACEMENT_ID");
		this.rewardedVideoAd = rewardedVideoAd;
		this.rewardedVideoAd.Register(base.gameObject);
		this.rewardedVideoAd.RewardedVideoAdDidLoad = delegate()
		{
			UnityEngine.Debug.Log("RewardedVideo ad loaded.");
			this.isLoaded = true;
			this.statusLabel.text = "Ad loaded. Click show to present!";
		};
		rewardedVideoAd.RewardedVideoAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("RewardedVideo ad failed to load with error: " + error);
			this.statusLabel.text = "RewardedVideo ad failed to load. Check console for details.";
		};
		rewardedVideoAd.RewardedVideoAdWillLogImpression = delegate()
		{
			UnityEngine.Debug.Log("RewardedVideo ad logged impression.");
		};
		rewardedVideoAd.RewardedVideoAdDidClick = delegate()
		{
			UnityEngine.Debug.Log("RewardedVideo ad clicked.");
		};
		this.rewardedVideoAd.LoadAd();
	}

	public void ShowRewardedVideo()
	{
		if (this.isLoaded)
		{
			this.rewardedVideoAd.Show();
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
		if (this.rewardedVideoAd != null)
		{
			this.rewardedVideoAd.Dispose();
		}
		UnityEngine.Debug.Log("RewardedVideoAdTest was destroyed!");
	}

	public void NextScene()
	{
		SceneManager.LoadScene("InterstitialAdScene");
	}

	private RewardedVideoAd rewardedVideoAd;

	private bool isLoaded;

	public Text statusLabel;
}
