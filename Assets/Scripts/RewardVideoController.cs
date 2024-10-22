using System;
using UnityEngine;

public class RewardVideoController : MonoBehaviour
{
	private void Start()
	{
		MoPubManager.onRewardedVideoLoadedEvent += this.onRewardedVideoLoadedEvent;
		MoPubManager.onRewardedVideoFailedEvent += this.onRewardedVideoFailedEvent;
		MoPubManager.onRewardedVideoExpiredEvent += this.onRewardedVideoExpiredEvent;
		MoPubManager.onRewardedVideoShownEvent += this.onRewardedVideoShownEvent;
		MoPubManager.onRewardedVideoClickedEvent += this.onRewardedVideoClickedEvent;
		MoPubManager.onRewardedVideoFailedToPlayEvent += this.onRewardedVideoFailedToPlayEvent;
		MoPubManager.onRewardedVideoReceivedRewardEvent += this.onRewardedVideoReceivedRewardEvent;
		MoPubManager.onRewardedVideoClosedEvent += this.onRewardedVideoClosedEvent;
		MoPubManager.onRewardedVideoLeavingApplicationEvent += this.onRewardedVideoLeavingApplicationEvent;
	}

	public void showmopubrewardedvideo()
	{
		MoPubAds.showRewardVideo(MoPubAds._interstitialOnVideo);
	}

	private void OnDisable()
	{
		MoPubManager.onRewardedVideoLoadedEvent -= this.onRewardedVideoLoadedEvent;
		MoPubManager.onRewardedVideoFailedEvent -= this.onRewardedVideoFailedEvent;
		MoPubManager.onRewardedVideoExpiredEvent -= this.onRewardedVideoExpiredEvent;
		MoPubManager.onRewardedVideoShownEvent -= this.onRewardedVideoShownEvent;
		MoPubManager.onRewardedVideoFailedToPlayEvent -= this.onRewardedVideoFailedToPlayEvent;
		MoPubManager.onRewardedVideoReceivedRewardEvent -= this.onRewardedVideoReceivedRewardEvent;
		MoPubManager.onRewardedVideoClosedEvent -= this.onRewardedVideoClosedEvent;
		MoPubManager.onRewardedVideoLeavingApplicationEvent -= this.onRewardedVideoLeavingApplicationEvent;
	}

	private void onRewardedVideoLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("onRewardedVideoLoadedEvent: " + adUnitId);
	}

	private void onRewardedVideoFailedEvent(string errorMsg)
	{
		this.videocomplete = false;
		UnityEngine.Debug.Log("onRewardedVideoFailedEvent: " + errorMsg);
	}

	private void onRewardedVideoExpiredEvent(string adUnitId)
	{
		this.videocomplete = false;
		UnityEngine.Debug.Log("onRewardedVideoExpiredEvent: " + adUnitId);
	}

	private void onRewardedVideoShownEvent(string adUnitId)
	{
		this.videocomplete = false;
		UnityEngine.Debug.Log("onRewardedVideoShownEvent: " + adUnitId);
	}

	private void onRewardedVideoClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("onRewardedVideoClickedEvent: " + adUnitId);
	}

	private void onRewardedVideoFailedToPlayEvent(string errorMsg)
	{
		this.videocomplete = false;
		UnityEngine.Debug.Log("onRewardedVideoFailedToPlayEvent: " + errorMsg);
	}

	private void onRewardedVideoReceivedRewardEvent(MoPubManager.RewardedVideoData rewardedVideoData)
	{
		UnityEngine.Debug.Log("onRewardedVideoReceivedRewardEvent: " + rewardedVideoData);
	}

	private void onRewardedVideoClosedEvent(string adUnitId)
	{
		this.videocomplete = true;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"onRewardedVideoClosedEvent: ",
			adUnitId,
			"-",
			Time.timeSinceLevelLoad
		}));
		this.checkvideocomplete();
	}

	private void onRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("onRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	public void checkvideocomplete()
	{
		if (this.videocomplete)
		{
			UnityEngine.Debug.Log("videocomplete give credit");
			this.videocomplete = false;
		}
	}

	private bool videocomplete;
}
