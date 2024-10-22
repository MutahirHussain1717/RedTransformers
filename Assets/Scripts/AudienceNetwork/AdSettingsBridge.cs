using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdSettingsBridge : IAdSettingsBridge
	{
		internal AdSettingsBridge()
		{
		}

		private static IAdSettingsBridge createInstance()
		{
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				return new AdSettingsBridgeAndroid();
			}
			return new AdSettingsBridge();
		}

		public virtual void addTestDevice(string deviceID)
		{
		}

		public virtual void setUrlPrefix(string urlPrefix)
		{
		}

		public static readonly IAdSettingsBridge Instance = AdSettingsBridge.createInstance();
	}
}
