using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdViewBridgeListenerProxy : AndroidJavaProxy
	{
		public AdViewBridgeListenerProxy(AdView adView, AndroidJavaObject bridgedAdView) : base("com.facebook.ads.AdListener")
		{
			this.adView = adView;
			this.bridgedAdView = bridgedAdView;
		}

		private void onError(AndroidJavaObject ad, AndroidJavaObject error)
		{
			string errorMessage = error.Call<string>("getErrorMessage", new object[0]);
			this.adView.executeOnMainThread(delegate
			{
				if (this.adView.AdViewDidFailWithError != null)
				{
					this.adView.AdViewDidFailWithError(errorMessage);
				}
			});
		}

		private void onAdLoaded(AndroidJavaObject ad)
		{
			this.adView.executeOnMainThread(delegate
			{
				if (this.adView.AdViewDidLoad != null)
				{
					this.adView.AdViewDidLoad();
				}
			});
		}

		private void onAdClicked(AndroidJavaObject ad)
		{
			this.adView.executeOnMainThread(delegate
			{
				if (this.adView.AdViewDidClick != null)
				{
					this.adView.AdViewDidClick();
				}
			});
		}

		private void onLoggingImpression(AndroidJavaObject ad)
		{
			this.adView.executeOnMainThread(delegate
			{
				if (this.adView.AdViewWillLogImpression != null)
				{
					this.adView.AdViewWillLogImpression();
				}
			});
		}

		private AdView adView;

		private AndroidJavaObject bridgedAdView;
	}
}
