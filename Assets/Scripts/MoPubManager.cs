using System;
using System.Collections.Generic;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
	static MoPubManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		try
		{
			MonoBehaviour x = UnityEngine.Object.FindObjectOfType(typeFromHandle) as MonoBehaviour;
			if (!(x != null))
			{
				GameObject gameObject = new GameObject(typeFromHandle.ToString());
				gameObject.AddComponent(typeFromHandle);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}
		catch (UnityException)
		{
			UnityEngine.Debug.LogWarning("It looks like you have the " + typeFromHandle + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}

	public static event Action<float> onAdLoadedEvent;

	public static event Action<string> onAdFailedEvent;

	public static event Action<string> onAdClickedEvent;

	public static event Action<string> onAdExpandedEvent;

	public static event Action<string> onAdCollapsedEvent;

	public static event Action<string> onInterstitialLoadedEvent;

	public static event Action<string> onInterstitialFailedEvent;

	public static event Action<string> onInterstitialDismissedEvent;

	public static event Action<string> onInterstitialExpiredEvent;

	public static event Action<string> onInterstitialShownEvent;

	public static event Action<string> onInterstitialClickedEvent;

	public static event Action<string> onRewardedVideoLoadedEvent;

	public static event Action<string> onRewardedVideoFailedEvent;

	public static event Action<string> onRewardedVideoExpiredEvent;

	public static event Action<string> onRewardedVideoShownEvent;

	public static event Action<string> onRewardedVideoClickedEvent;

	public static event Action<string> onRewardedVideoFailedToPlayEvent;

	public static event Action<MoPubManager.RewardedVideoData> onRewardedVideoReceivedRewardEvent;

	public static event Action<string> onRewardedVideoClosedEvent;

	public static event Action<string> onRewardedVideoLeavingApplicationEvent;

	private void onAdLoaded(string height)
	{
		if (MoPubManager.onAdLoadedEvent != null)
		{
			MoPubManager.onAdLoadedEvent(float.Parse(height));
		}
	}

	private void onAdFailed(string errorMsg)
	{
		if (MoPubManager.onAdFailedEvent != null)
		{
			MoPubManager.onAdFailedEvent(errorMsg);
		}
	}

	private void onAdClicked(string adUnitId)
	{
		if (MoPubManager.onAdClickedEvent != null)
		{
			MoPubManager.onAdClickedEvent(adUnitId);
		}
	}

	private void onAdExpanded(string adUnitId)
	{
		if (MoPubManager.onAdExpandedEvent != null)
		{
			MoPubManager.onAdExpandedEvent(adUnitId);
		}
	}

	private void onAdCollapsed(string adUnitId)
	{
		if (MoPubManager.onAdCollapsedEvent != null)
		{
			MoPubManager.onAdCollapsedEvent(adUnitId);
		}
	}

	private void onInterstitialLoaded(string adUnitId)
	{
		if (MoPubManager.onInterstitialLoadedEvent != null)
		{
			MoPubManager.onInterstitialLoadedEvent(adUnitId);
		}
	}

	private void onInterstitialFailed(string errorMsg)
	{
		if (MoPubManager.onInterstitialFailedEvent != null)
		{
			MoPubManager.onInterstitialFailedEvent(errorMsg);
		}
	}

	private void onInterstitialDismissed(string adUnitId)
	{
		if (MoPubManager.onInterstitialDismissedEvent != null)
		{
			MoPubManager.onInterstitialDismissedEvent(adUnitId);
		}
	}

	private void interstitialDidExpire(string adUnitId)
	{
		if (MoPubManager.onInterstitialExpiredEvent != null)
		{
			MoPubManager.onInterstitialExpiredEvent(adUnitId);
		}
	}

	private void onInterstitialShown(string adUnitId)
	{
		if (MoPubManager.onInterstitialShownEvent != null)
		{
			MoPubManager.onInterstitialShownEvent(adUnitId);
		}
	}

	private void onInterstitialClicked(string adUnitId)
	{
		if (MoPubManager.onInterstitialClickedEvent != null)
		{
			MoPubManager.onInterstitialClickedEvent(adUnitId);
		}
	}

	private void onRewardedVideoLoaded(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoLoadedEvent != null)
		{
			MoPubManager.onRewardedVideoLoadedEvent(adUnitId);
		}
	}

	private void onRewardedVideoFailed(string errorMsg)
	{
		if (MoPubManager.onRewardedVideoFailedEvent != null)
		{
			MoPubManager.onRewardedVideoFailedEvent(errorMsg);
		}
	}

	private void onRewardedVideoExpired(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoExpiredEvent != null)
		{
			MoPubManager.onRewardedVideoExpiredEvent(adUnitId);
		}
	}

	private void onRewardedVideoShown(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoShownEvent != null)
		{
			MoPubManager.onRewardedVideoShownEvent(adUnitId);
		}
	}

	private void onRewardedVideoClicked(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoClickedEvent != null)
		{
			MoPubManager.onRewardedVideoClickedEvent(adUnitId);
		}
	}

	private void onRewardedVideoFailedToPlay(string errorMsg)
	{
		if (MoPubManager.onRewardedVideoFailedToPlayEvent != null)
		{
			MoPubManager.onRewardedVideoFailedToPlayEvent(errorMsg);
		}
	}

	private void onRewardedVideoReceivedReward(string json)
	{
		if (MoPubManager.onRewardedVideoReceivedRewardEvent != null)
		{
			MoPubManager.onRewardedVideoReceivedRewardEvent(new MoPubManager.RewardedVideoData(json));
		}
	}

	private void onRewardedVideoClosed(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoClosedEvent != null)
		{
			MoPubManager.onRewardedVideoClosedEvent(adUnitId);
		}
	}

	private void onRewardedVideoLeavingApplication(string adUnitId)
	{
		if (MoPubManager.onRewardedVideoLeavingApplicationEvent != null)
		{
			MoPubManager.onRewardedVideoLeavingApplicationEvent(adUnitId);
		}
	}

	public class RewardedVideoData
	{
		public RewardedVideoData(string json)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return;
			}
			if (dictionary.ContainsKey("adUnitId"))
			{
				this.adUnitId = dictionary["adUnitId"].ToString();
			}
			if (dictionary.ContainsKey("currencyType"))
			{
				this.currencyType = dictionary["currencyType"].ToString();
			}
			if (dictionary.ContainsKey("amount"))
			{
				this.amount = float.Parse(dictionary["amount"].ToString());
			}
		}

		public string ToString()
		{
			return string.Format("adUnitId: {0}, currencyType: {1}, amount: {2}", this.adUnitId, this.currencyType, this.amount);
		}

		public string adUnitId;

		public string currencyType;

		public float amount;
	}

	public class MoPubReward
	{
		public MoPubReward(string label, int amount)
		{
			this._label = label;
			this._amount = amount;
		}

		public string Label
		{
			get
			{
				return this._label;
			}
		}

		public int Amount
		{
			get
			{
				return this._amount;
			}
		}

		public string ToString()
		{
			return string.Format("\"{0} {1}\"", this.Amount, this.Label);
		}

		private readonly string _label;

		private readonly int _amount;
	}
}
