using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdViewContainer
	{
		internal AdViewContainer(AdView adView)
		{
			this.adView = adView;
		}

		internal AdView adView { get; set; }

		internal FBAdViewBridgeCallback onLoad { get; set; }

		internal FBAdViewBridgeCallback onImpression { get; set; }

		internal FBAdViewBridgeCallback onClick { get; set; }

		internal FBAdViewBridgeErrorCallback onError { get; set; }

		internal FBAdViewBridgeCallback onFinishedClick { get; set; }

		public override string ToString()
		{
			return string.Format("[AdViewContainer: adView={0}, onLoad={1}]", this.adView, this.onLoad);
		}

		public static implicit operator bool(AdViewContainer obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		internal AndroidJavaProxy listenerProxy;

		internal AndroidJavaObject bridgedAdView;
	}
}
