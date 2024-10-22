using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class InterstitialAdContainer
	{
		internal InterstitialAdContainer(InterstitialAd interstitialAd)
		{
			this.interstitialAd = interstitialAd;
		}

		internal InterstitialAd interstitialAd { get; set; }

		internal FBInterstitialAdBridgeCallback onLoad { get; set; }

		internal FBInterstitialAdBridgeCallback onImpression { get; set; }

		internal FBInterstitialAdBridgeCallback onClick { get; set; }

		internal FBInterstitialAdBridgeErrorCallback onError { get; set; }

		internal FBInterstitialAdBridgeCallback onDidClose { get; set; }

		internal FBInterstitialAdBridgeCallback onWillClose { get; set; }

		public override string ToString()
		{
			return string.Format("[InterstitialAdContainer: interstitialAd={0}, onLoad={1}]", this.interstitialAd, this.onLoad);
		}

		public static implicit operator bool(InterstitialAdContainer obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		internal AndroidJavaProxy listenerProxy;

		internal AndroidJavaObject bridgedInterstitialAd;
	}
}
