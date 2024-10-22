using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdViewBridgeAndroid : AdViewBridge
	{
		private AndroidJavaObject adViewForAdViewId(int uniqueId)
		{
			AdViewContainer adViewContainer = null;
			bool flag = AdViewBridgeAndroid.adViews.TryGetValue(uniqueId, out adViewContainer);
			if (flag)
			{
				return adViewContainer.bridgedAdView;
			}
			return null;
		}

		private string getStringForAdViewId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.adViewForAdViewId(uniqueId);
			if (androidJavaObject != null)
			{
				return androidJavaObject.Call<string>(method, new object[0]);
			}
			return null;
		}

		private string getImageURLForAdViewId(int uniqueId, string method)
		{
			AndroidJavaObject androidJavaObject = this.adViewForAdViewId(uniqueId);
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

		private AndroidJavaObject javaAdSizeFromAdSize(AdSize size)
		{
			AndroidJavaObject result = null;
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.facebook.ads.AdSize");
			if (size != AdSize.BANNER_HEIGHT_50)
			{
				if (size != AdSize.BANNER_HEIGHT_90)
				{
					if (size == AdSize.RECTANGLE_HEIGHT_250)
					{
						result = androidJavaClass.GetStatic<AndroidJavaObject>("RECTANGLE_HEIGHT_250");
					}
				}
				else
				{
					result = androidJavaClass.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_90");
				}
			}
			else
			{
				result = androidJavaClass.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_50");
			}
			return result;
		}

		public override int Create(string placementId, AdView adView, AdSize size)
		{
			AdUtility.prepare();
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.facebook.ads.AdView", new object[]
			{
				androidJavaObject,
				placementId,
				this.javaAdSizeFromAdSize(size)
			});
			AdViewBridgeListenerProxy adViewBridgeListenerProxy = new AdViewBridgeListenerProxy(adView, androidJavaObject2);
			androidJavaObject2.Call("setAdListener", new object[]
			{
				adViewBridgeListenerProxy
			});
			AdViewContainer adViewContainer = new AdViewContainer(adView);
			adViewContainer.bridgedAdView = androidJavaObject2;
			adViewContainer.listenerProxy = adViewBridgeListenerProxy;
			int num = AdViewBridgeAndroid.lastKey;
			AdViewBridgeAndroid.adViews.Add(num, adViewContainer);
			AdViewBridgeAndroid.lastKey++;
			return num;
		}

		public override int Load(int uniqueId)
		{
			AdUtility.prepare();
			AndroidJavaObject androidJavaObject = this.adViewForAdViewId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("loadAd", new object[0]);
			}
			return uniqueId;
		}

		public override bool Show(int uniqueId, double x, double y, double width, double height)
		{
			AndroidJavaObject adView = this.adViewForAdViewId(uniqueId);
			if (adView == null)
			{
				return false;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new object[]
			{
				new AndroidJavaRunnable(delegate()
				{
					AndroidJavaObject androidJavaObject = activity.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
					AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getResources", new object[0]);
					AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getDisplayMetrics", new object[0]);
					float num = androidJavaObject3.Get<float>("density");
					AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("android.widget.LinearLayout$LayoutParams", new object[]
					{
						(int)(width * (double)num),
						(int)(height * (double)num)
					});
					AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("android.widget.LinearLayout", new object[]
					{
						activity
					});
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.R$id");
					AndroidJavaObject androidJavaObject6 = activity.Call<AndroidJavaObject>("findViewById", new object[]
					{
						androidJavaClass2.GetStatic<int>("content")
					});
					androidJavaObject4.Call("setMargins", new object[]
					{
						(int)(x * (double)num),
						(int)(y * (double)num),
						0,
						0
					});
					androidJavaObject5.Call("addView", new object[]
					{
						adView,
						androidJavaObject4
					});
					androidJavaObject6.Call("addView", new object[]
					{
						androidJavaObject5
					});
				})
			});
			return true;
		}

		public override void DisableAutoRefresh(int uniqueId)
		{
			AndroidJavaObject androidJavaObject = this.adViewForAdViewId(uniqueId);
			if (androidJavaObject != null)
			{
				androidJavaObject.Call("disableAutoRefresh", new object[0]);
			}
		}

		public override void Release(int uniqueId)
		{
			AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject adView = this.adViewForAdViewId(uniqueId);
			AdViewBridgeAndroid.adViews.Remove(uniqueId);
			if (adView != null)
			{
				@static.Call("runOnUiThread", new object[]
				{
					new AndroidJavaRunnable(delegate()
					{
						adView.Call("destroy", new object[0]);
						AndroidJavaObject androidJavaObject = adView.Call<AndroidJavaObject>("getParent", new object[0]);
						androidJavaObject.Call("removeView", new object[]
						{
							adView
						});
					})
				});
			}
		}

		public override void OnLoad(int uniqueId, FBAdViewBridgeCallback callback)
		{
		}

		public override void OnImpression(int uniqueId, FBAdViewBridgeCallback callback)
		{
		}

		public override void OnClick(int uniqueId, FBAdViewBridgeCallback callback)
		{
		}

		public override void OnError(int uniqueId, FBAdViewBridgeErrorCallback callback)
		{
		}

		public override void OnFinishedClick(int uniqueId, FBAdViewBridgeCallback callback)
		{
		}

		private static Dictionary<int, AdViewContainer> adViews = new Dictionary<int, AdViewContainer>();

		private static int lastKey = 0;
	}
}
