using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class NativeAdContainer
	{
		internal NativeAdContainer(NativeAd nativeAd)
		{
			this.nativeAd = nativeAd;
		}

		internal NativeAd nativeAd { get; set; }

		internal FBNativeAdBridgeCallback onLoad { get; set; }

		internal FBNativeAdBridgeCallback onImpression { get; set; }

		internal FBNativeAdBridgeCallback onClick { get; set; }

		internal FBNativeAdBridgeErrorCallback onError { get; set; }

		internal FBNativeAdBridgeCallback onFinishedClick { get; set; }

		public static implicit operator bool(NativeAdContainer obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		internal AndroidJavaProxy listenerProxy;

		internal AndroidJavaObject bridgedNativeAd;
	}
}
