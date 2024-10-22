using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class RewardedVideoAdBridgeListenerProxy : AndroidJavaProxy
	{
		public RewardedVideoAdBridgeListenerProxy(RewardedVideoAd rewardedVideoAd, AndroidJavaObject bridgedRewardedVideoAd) : base("com.facebook.ads.S2SRewardedVideoAdListener")
		{
			this.rewardedVideoAd = rewardedVideoAd;
			this.bridgedRewardedVideoAd = bridgedRewardedVideoAd;
		}

		private void onError(AndroidJavaObject ad, AndroidJavaObject error)
		{
			string errorMessage = error.Call<string>("getErrorMessage", new object[0]);
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidFailWithError != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidFailWithError(errorMessage);
				}
			});
		}

		private void onAdLoaded(AndroidJavaObject ad)
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidLoad != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidLoad();
				}
			});
		}

		private void onAdClicked(AndroidJavaObject ad)
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidClick != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidClick();
				}
			});
		}

		private void onRewardedVideoDisplayed(AndroidJavaObject ad)
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
				{
					this.rewardedVideoAd.RewardedVideoAdWillLogImpression();
				}
			});
		}

		private void onRewardedVideoClosed()
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidClose != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidClose();
				}
			});
		}

		private void onRewardedVideoCompleted()
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdComplete != null)
				{
					this.rewardedVideoAd.RewardedVideoAdComplete();
				}
			});
		}

		private void onRewardServerSuccess()
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidSucceed != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidSucceed();
				}
			});
		}

		private void onRewardServerFailed()
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdDidFail != null)
				{
					this.rewardedVideoAd.RewardedVideoAdDidFail();
				}
			});
		}

		private void onLoggingImpression(AndroidJavaObject ad)
		{
			this.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (this.rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
				{
					this.rewardedVideoAd.RewardedVideoAdWillLogImpression();
				}
			});
		}

		private RewardedVideoAd rewardedVideoAd;

		private AndroidJavaObject bridgedRewardedVideoAd;
	}
}
