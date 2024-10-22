using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class InterstitialAdBridgeListenerProxy : AndroidJavaProxy
	{
		public InterstitialAdBridgeListenerProxy(InterstitialAd interstitialAd, AndroidJavaObject bridgedInterstitialAd) : base("com.facebook.ads.InterstitialAdListener")
		{
			this.interstitialAd = interstitialAd;
			this.bridgedInterstitialAd = bridgedInterstitialAd;
		}

		private void onError(AndroidJavaObject ad, AndroidJavaObject error)
		{
			string errorMessage = error.Call<string>("getErrorMessage", new object[0]);
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdDidFailWithError != null)
				{
					this.interstitialAd.InterstitialAdDidFailWithError(errorMessage);
				}
			});
		}

		private void onAdLoaded(AndroidJavaObject ad)
		{
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdDidLoad != null)
				{
					this.interstitialAd.InterstitialAdDidLoad();
				}
			});
		}

		private void onAdClicked(AndroidJavaObject ad)
		{
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdDidClick != null)
				{
					this.interstitialAd.InterstitialAdDidClick();
				}
			});
		}

		private void onInterstitialDisplayed(AndroidJavaObject ad)
		{
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdWillLogImpression != null)
				{
					this.interstitialAd.InterstitialAdWillLogImpression();
				}
			});
		}

		private void onInterstitialDismissed(AndroidJavaObject ad)
		{
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdDidClose != null)
				{
					this.interstitialAd.InterstitialAdDidClose();
				}
			});
		}

		private void onLoggingImpression(AndroidJavaObject ad)
		{
			this.interstitialAd.executeOnMainThread(delegate
			{
				if (this.interstitialAd.InterstitialAdWillLogImpression != null)
				{
					this.interstitialAd.InterstitialAdWillLogImpression();
				}
			});
		}

		private InterstitialAd interstitialAd;

		private AndroidJavaObject bridgedInterstitialAd;
	}
}
