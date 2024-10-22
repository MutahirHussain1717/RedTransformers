using System;
using UnityEngine;

namespace AudienceNetwork
{
	internal class AdSettingsBridgeAndroid : AdSettingsBridge
	{
		public override void addTestDevice(string deviceID)
		{
			AndroidJavaClass adSettingsObject = this.getAdSettingsObject();
			adSettingsObject.CallStatic("addTestDevice", new object[]
			{
				deviceID
			});
		}

		public override void setUrlPrefix(string urlPrefix)
		{
			AndroidJavaClass adSettingsObject = this.getAdSettingsObject();
			adSettingsObject.CallStatic("setUrlPrefix", new object[]
			{
				urlPrefix
			});
		}

		private AndroidJavaClass getAdSettingsObject()
		{
			return new AndroidJavaClass("com.facebook.ads.AdSettings");
		}
	}
}
