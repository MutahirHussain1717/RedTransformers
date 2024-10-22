using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class RewardedVideoAdContainer
	{
		internal RewardedVideoAdContainer(RewardedVideoAd rewardedVideoAd)
		{
			this.rewardedVideoAd = rewardedVideoAd;
		}

		internal RewardedVideoAd rewardedVideoAd { get; set; }

		internal FBRewardedVideoAdBridgeCallback onLoad { get; set; }

		internal FBRewardedVideoAdBridgeCallback onImpression { get; set; }

		internal FBRewardedVideoAdBridgeCallback onClick { get; set; }

		internal FBRewardedVideoAdBridgeErrorCallback onError { get; set; }

		internal FBRewardedVideoAdBridgeCallback onDidClose { get; set; }

		internal FBRewardedVideoAdBridgeCallback onWillClose { get; set; }

		internal FBRewardedVideoAdBridgeCallback onComplete { get; set; }

		internal FBRewardedVideoAdBridgeCallback onDidSucceed { get; set; }

		internal FBRewardedVideoAdBridgeCallback onDidFail { get; set; }

		public override string ToString()
		{
			return string.Format("[RewardedVideoAdContainer: rewardedVideoAd={0}, onLoad={1}]", this.rewardedVideoAd, this.onLoad);
		}

		public static implicit operator bool(RewardedVideoAdContainer obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		internal AndroidJavaProxy listenerProxy;

		internal AndroidJavaObject bridgedRewardedVideoAd;
	}
}
