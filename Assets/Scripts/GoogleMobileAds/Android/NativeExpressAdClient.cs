using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class NativeExpressAdClient : AndroidJavaProxy, INativeExpressAdClient
	{
		public NativeExpressAdClient() : base("com.google.unity.ads.UnityAdListener")
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			this.nativeExpressAdView = new AndroidJavaObject("com.google.unity.ads.NativeExpressAd", new object[]
			{
				@static,
				this
			});
		}

		public event EventHandler<EventArgs> OnAdLoaded;

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		public event EventHandler<EventArgs> OnAdOpening;

		public event EventHandler<EventArgs> OnAdClosed;

		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
		{
			this.nativeExpressAdView.Call("create", new object[]
			{
				adUnitId,
				Utils.GetAdSizeJavaObject(adSize),
				(int)position
			});
		}

		public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int x, int y)
		{
			this.nativeExpressAdView.Call("create", new object[]
			{
				adUnitId,
				Utils.GetAdSizeJavaObject(adSize),
				x,
				y
			});
		}

		public void LoadAd(AdRequest request)
		{
			this.nativeExpressAdView.Call("loadAd", new object[]
			{
				Utils.GetAdRequestJavaObject(request)
			});
		}

		public void SetAdSize(AdSize adSize)
		{
			this.nativeExpressAdView.Call("setAdSize", new object[]
			{
				Utils.GetAdSizeJavaObject(adSize)
			});
		}

		public void ShowNativeExpressAdView()
		{
			this.nativeExpressAdView.Call("show", new object[0]);
		}

		public void HideNativeExpressAdView()
		{
			this.nativeExpressAdView.Call("hide", new object[0]);
		}

		public void DestroyNativeExpressAdView()
		{
			this.nativeExpressAdView.Call("destroy", new object[0]);
		}

		public void onAdLoaded()
		{
			if (this.OnAdLoaded != null)
			{
				this.OnAdLoaded(this, EventArgs.Empty);
			}
		}

		public void onAdFailedToLoad(string errorReason)
		{
			if (this.OnAdFailedToLoad != null)
			{
				AdFailedToLoadEventArgs e = new AdFailedToLoadEventArgs
				{
					Message = errorReason
				};
				this.OnAdFailedToLoad(this, e);
			}
		}

		public void onAdOpened()
		{
			if (this.OnAdOpening != null)
			{
				this.OnAdOpening(this, EventArgs.Empty);
			}
		}

		public void onAdClosed()
		{
			if (this.OnAdClosed != null)
			{
				this.OnAdClosed(this, EventArgs.Empty);
			}
		}

		public void onAdLeftApplication()
		{
			if (this.OnAdLeavingApplication != null)
			{
				this.OnAdLeavingApplication(this, EventArgs.Empty);
			}
		}

		private AndroidJavaObject nativeExpressAdView;
	}
}
