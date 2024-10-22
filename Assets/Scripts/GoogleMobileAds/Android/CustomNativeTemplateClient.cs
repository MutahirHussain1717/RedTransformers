using System;
using System.Collections.Generic;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class CustomNativeTemplateClient : ICustomNativeTemplateClient
	{
		public CustomNativeTemplateClient(AndroidJavaObject customNativeAd)
		{
			this.customNativeAd = customNativeAd;
		}

		public List<string> GetAvailableAssetNames()
		{
			return new List<string>(this.customNativeAd.Call<string[]>("getAvailableAssetNames", new object[0]));
		}

		public string GetTemplateId()
		{
			return this.customNativeAd.Call<string>("getTemplateId", new object[0]);
		}

		public byte[] GetImageByteArray(string key)
		{
			byte[] array = this.customNativeAd.Call<byte[]>("getImage", new object[]
			{
				key
			});
			if (array.Length == 0)
			{
				return null;
			}
			return array;
		}

		public string GetText(string key)
		{
			string text = this.customNativeAd.Call<string>("getText", new object[]
			{
				key
			});
			if (text.Equals(string.Empty))
			{
				return null;
			}
			return text;
		}

		public void PerformClick(string assetName)
		{
			this.customNativeAd.Call("performClick", new object[]
			{
				assetName
			});
		}

		public void RecordImpression()
		{
			this.customNativeAd.Call("recordImpression", new object[0]);
		}

		private AndroidJavaObject customNativeAd;
	}
}
