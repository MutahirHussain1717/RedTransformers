using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using UnityEngine;

namespace AudienceNetwork
{
	internal class RewardedVideoAdBridgeAndroid : RewardedVideoAdBridge
	{
		private AndroidJavaObject rewardedVideoAdForUniqueId(int uniqueId)
		{
			RewardedVideoAdContainer rewardedVideoAdContainer = null;
			bool flag = RewardedVideoAdBridgeAndroid.rewardedVideoAds.TryGetValue(uniqueId, out rewardedVideoAdContainer);
			if (flag)
			{
				return rewardedVideoAdContainer.bridgedRewardedVideoAd;
			}
			return null;
		}

		private RewardedVideoAdContainer rewardedVideoAdContainerForUniqueId(int uniqueId)
		{
			RewardedVideoAdContainer result = null;
			bool flag = RewardedVideoAdBridgeAndroid.rewardedVideoAds.TryGetValue(uniqueId, out result);
			if (flag)
			{
				return result;
			}
			return null;
		}

		private string getStringForuniqueId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.rewardedVideoAdForUniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<string>(method, new object[0]);
			}
			return null;
		}

		private string getImageURLForuniqueId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.rewardedVideoAdForUniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>(method, new object[0]);
				if (androidJavaObject2 != null)
				{
					return androidJavaObject2.Call<string>("getUrl", new object[0]);
				}
			}
			return null;
		}

		public override int Create(string placementId, RewardedVideoAd rewardedVideoAd)
		{
			AdUtility.prepare();
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.facebook.ads.RewardedVideoAd", new object[]
			{
				androidJavaObject,
				placementId
			});
			RewardedVideoAdBridgeListenerProxy rewardedVideoAdBridgeListenerProxy = new RewardedVideoAdBridgeListenerProxy(rewardedVideoAd, androidJavaObject2);
			androidJavaObject2.Call("setAdListener", new object[]
			{
				rewardedVideoAdBridgeListenerProxy
			});
			RewardedVideoAdContainer rewardedVideoAdContainer = new RewardedVideoAdContainer(rewardedVideoAd);
			rewardedVideoAdContainer.bridgedRewardedVideoAd = androidJavaObject2;
			rewardedVideoAdContainer.listenerProxy = rewardedVideoAdBridgeListenerProxy;
			int num = RewardedVideoAdBridgeAndroid.lastKey;
			RewardedVideoAdBridgeAndroid.rewardedVideoAds.Add(num, rewardedVideoAdContainer);
			RewardedVideoAdBridgeAndroid.lastKey++;
			return num;
		}

		public override int Load(int uniqueId)
		{
			AdUtility.prepare();
			AndroidJavaObject androidJavaObject = this.rewardedVideoAdForUniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("loadAd", new object[0]);
			}
			return uniqueId;
		}

		public override bool IsValid(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.rewardedVideoAdForUniqueId(uniqueId);
			return androidJavaObject != null && androidJavaObject.Call<bool>("isAdLoaded", new object[0]);
		}

		public override bool Show(int uniqueId)
		{
			RewardedVideoAdContainer rewardedVideoAdContainer = this.rewardedVideoAdContainerForUniqueId(uniqueId);
			AndroidJavaObject rewardedVideoAd = this.rewardedVideoAdForUniqueId(uniqueId);
			rewardedVideoAdContainer.rewardedVideoAd.executeOnMainThread(delegate
			{
				if (rewardedVideoAd != null)
				{
					rewardedVideoAd.Call<bool>("show", new object[0]);
				}
			});
			return true;
		}

		public override void Release(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.rewardedVideoAdForUniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("destroy", new object[0]);
			}
			RewardedVideoAdBridgeAndroid.rewardedVideoAds.Remove(uniqueId);
		}

		public override void OnLoad(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
		{
		}

		public override void OnImpression(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
		{
		}

		public override void OnClick(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
		{
		}

		public override void OnError(int uniqueId, FBRewardedVideoAdBridgeErrorCallback callback)
		{
		}

		public override void OnWillClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
		{
		}

		public override void OnDidClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
		{
		}

		private static Dictionary<int, RewardedVideoAdContainer> rewardedVideoAds = new Dictionary<int, RewardedVideoAdContainer>();

		private static int lastKey = 0;
	}
}
