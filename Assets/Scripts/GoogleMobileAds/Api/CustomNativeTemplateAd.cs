using System;
using System.Collections.Generic;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Api
{
	public class CustomNativeTemplateAd
	{
		internal CustomNativeTemplateAd(ICustomNativeTemplateClient client)
		{
			this.client = client;
		}

		public List<string> GetAvailableAssetNames()
		{
			return this.client.GetAvailableAssetNames();
		}

		public string GetCustomTemplateId()
		{
			return this.client.GetTemplateId();
		}

		public Texture2D GetTexture2D(string key)
		{
			byte[] imageByteArray = this.client.GetImageByteArray(key);
			if (imageByteArray == null)
			{
				return null;
			}
			return Utils.GetTexture2DFromByteArray(imageByteArray);
		}

		public string GetText(string key)
		{
			return this.client.GetText(key);
		}

		public void PerformClick(string assetName)
		{
			this.client.PerformClick(assetName);
		}

		public void RecordImpression()
		{
			this.client.RecordImpression();
		}

		private ICustomNativeTemplateClient client;
	}
}
