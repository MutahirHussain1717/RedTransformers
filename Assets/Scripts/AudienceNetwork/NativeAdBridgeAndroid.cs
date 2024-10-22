using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using UnityEngine;

namespace AudienceNetwork
{
	internal class NativeAdBridgeAndroid : NativeAdBridge
	{
		private AndroidJavaObject nativeAdForNativeAdId(int uniqueId)
		{
			NativeAdContainer nativeAdContainer = null;
			bool flag = NativeAdBridgeAndroid.nativeAds.TryGetValue(uniqueId, out nativeAdContainer);
			if (flag)
			{
				return nativeAdContainer.bridgedNativeAd;
			}
			return null;
		}

		private string getStringForNativeAdId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<string>(method, new object[0]);
			}
			return null;
		}

		private string getImageURLForNativeAdId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
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

		public override int Create(string placementId, NativeAd nativeAd)
		{
			AdUtility.prepare();
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.facebook.ads.NativeAd", new object[]
			{
				androidJavaObject,
				placementId
			});
			NativeAdBridgeListenerProxy nativeAdBridgeListenerProxy = new NativeAdBridgeListenerProxy(nativeAd, androidJavaObject2);
			androidJavaObject2.Call("setAdListener", new object[]
			{
				nativeAdBridgeListenerProxy
			});
			NativeAdContainer nativeAdContainer = new NativeAdContainer(nativeAd);
			nativeAdContainer.bridgedNativeAd = androidJavaObject2;
			nativeAdContainer.listenerProxy = nativeAdBridgeListenerProxy;
			int num = NativeAdBridgeAndroid.lastKey;
			NativeAdBridgeAndroid.nativeAds.Add(num, nativeAdContainer);
			NativeAdBridgeAndroid.lastKey++;
			return num;
		}

		public override int Load(int uniqueId)
		{
			AdUtility.prepare();
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("registerExternalLogReceiver", new object[]
				{
					NativeAdBridge.source
				});
				androidJavaObject.Call("loadAd", new object[0]);
			}
			return uniqueId;
		}

		public override bool IsValid(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			return androidJavaObject != null && androidJavaObject.Call<bool>("isAdLoaded", new object[0]);
		}

		public override string GetTitle(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdTitle");
		}

		public override string GetSubtitle(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdSubtitle");
		}

		public override string GetBody(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdBody");
		}

		public override string GetCallToAction(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdCallToAction");
		}

		public override string GetSocialContext(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdSocialContext");
		}

		public override string GetIconImageURL(int uniqueId)
		{
			return this.getImageURLForNativeAdId(uniqueId, "getAdIcon");
		}

		public override string GetCoverImageURL(int uniqueId)
		{
			return this.getImageURLForNativeAdId(uniqueId, "getAdCoverImage");
		}

		public override string GetAdChoicesImageURL(int uniqueId)
		{
			return this.getImageURLForNativeAdId(uniqueId, "getAdChoicesIcon");
		}

		public override string GetAdChoicesText(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdChoicesText");
		}

		public override string GetAdChoicesLinkURL(int uniqueId)
		{
			return this.getStringForNativeAdId(uniqueId, "getAdChoicesLinkUrl");
		}

		public override int GetMinViewabilityPercentage(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<int>("getMinViewabilityPercentage", new object[0]);
			}
			return 1;
		}

		private string getId(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<string>("getId", new object[0]);
			}
			return null;
		}

		public override void ManualLogImpression(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				this.sendIntentToBroadcastManager(uniqueId, "com.facebook.ads.native.impression");
			}
		}

		public override void ManualLogClick(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				this.sendIntentToBroadcastManager(uniqueId, "com.facebook.ads.native.click");
			}
		}

		public override void ExternalLogImpression(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("logExternalImpression", new object[0]);
			}
		}

		public override void ExternalLogClick(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.nativeAdForNativeAdId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("logExternalClick", new object[]
				{
					NativeAdBridge.source
				});
			}
		}

		private bool sendIntentToBroadcastManager(int uniqueId, string intent)
		{
			if (intent != null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[]
				{
					intent + ":" + this.getId(uniqueId)
				});
				AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.support.v4.content.LocalBroadcastManager");
				AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("getInstance", new object[]
				{
					@static
				});
				return androidJavaObject2.Call<bool>("sendBroadcast", new object[]
				{
					androidJavaObject
				});
			}
			return false;
		}

		public override void Release(int uniqueId)
		{
			NativeAdBridgeAndroid.nativeAds.Remove(uniqueId);
		}

		public override void OnLoad(int uniqueId, FBNativeAdBridgeCallback callback)
		{
		}

		public override void OnImpression(int uniqueId, FBNativeAdBridgeCallback callback)
		{
		}

		public override void OnClick(int uniqueId, FBNativeAdBridgeCallback callback)
		{
		}

		public override void OnError(int uniqueId, FBNativeAdBridgeErrorCallback callback)
		{
		}

		public override void OnFinishedClick(int uniqueId, FBNativeAdBridgeCallback callback)
		{
		}

		private static Dictionary<int, NativeAdContainer> nativeAds = new Dictionary<int, NativeAdContainer>();

		private static int lastKey = 0;
	}
}
