using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using UnityEngine;

namespace AudienceNetwork
{
	internal class InterstitialAdBridgeAndroid : InterstitialAdBridge
	{
		private AndroidJavaObject interstitialAdForuniqueId(int uniqueId)
		{
			InterstitialAdContainer interstitialAdContainer = null;
			bool flag = InterstitialAdBridgeAndroid.interstitialAds.TryGetValue(uniqueId, out interstitialAdContainer);
			if (flag)
			{
				return interstitialAdContainer.bridgedInterstitialAd;
			}
			return null;
		}

		private string getStringForuniqueId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<string>(method, new object[0]);
			}
			return null;
		}

		private string getImageURLForuniqueId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
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

		public override int Create(string placementId, InterstitialAd interstitialAd)
		{
			AdUtility.prepare();
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.facebook.ads.InterstitialAd", new object[]
			{
				androidJavaObject,
				placementId
			});
			InterstitialAdBridgeListenerProxy interstitialAdBridgeListenerProxy = new InterstitialAdBridgeListenerProxy(interstitialAd, androidJavaObject2);
			androidJavaObject2.Call("setAdListener", new object[]
			{
				interstitialAdBridgeListenerProxy
			});
			InterstitialAdContainer interstitialAdContainer = new InterstitialAdContainer(interstitialAd);
			interstitialAdContainer.bridgedInterstitialAd = androidJavaObject2;
			interstitialAdContainer.listenerProxy = interstitialAdBridgeListenerProxy;
			int num = InterstitialAdBridgeAndroid.lastKey;
			InterstitialAdBridgeAndroid.interstitialAds.Add(num, interstitialAdContainer);
			InterstitialAdBridgeAndroid.lastKey++;
			return num;
		}

		public override int Load(int uniqueId)
		{
			AdUtility.prepare();
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("loadAd", new object[0]);
			}
			return uniqueId;
		}

		public override bool IsValid(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
			return androidJavaObject != null && androidJavaObject.Call<bool>("isAdLoaded", new object[0]);
		}

		public override bool Show(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
			return androidJavaObject != null && androidJavaObject.Call<bool>("show", new object[0]);
		}

		public override void Release(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.interstitialAdForuniqueId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("destroy", new object[0]);
			}
			InterstitialAdBridgeAndroid.interstitialAds.Remove(uniqueId);
		}

		public override void OnLoad(int uniqueId, FBInterstitialAdBridgeCallback callback)
		{
		}

		public override void OnImpression(int uniqueId, FBInterstitialAdBridgeCallback callback)
		{
		}

		public override void OnClick(int uniqueId, FBInterstitialAdBridgeCallback callback)
		{
		}

		public override void OnError(int uniqueId, FBInterstitialAdBridgeErrorCallback callback)
		{
		}

		public override void OnWillClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
		{
		}

		public override void OnDidClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
		{
		}

		private static Dictionary<int, InterstitialAdContainer> interstitialAds = new Dictionary<int, InterstitialAdContainer>();

		private static int lastKey = 0;
	}
}
