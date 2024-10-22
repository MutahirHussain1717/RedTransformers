using System;
using System.Collections.Generic;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubAndroidRewardedVideo
{
	public MoPubAndroidRewardedVideo(string adUnitId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public static void initializeRewardedVideo()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		MoPubAndroidRewardedVideo._pluginClass.CallStatic("initializeRewardedVideo", new object[0]);
	}

	public static void initializeRewardedVideoWithNetworks(MoPubRewardedNetwork[] networks)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text = null;
		if (networks != null && networks.Length > 0)
		{
			text = string.Join(",", Array.ConvertAll<MoPubRewardedNetwork, string>(networks, (MoPubRewardedNetwork x) => x.ToString()));
		}
		MoPubAndroidRewardedVideo._pluginClass.CallStatic("initializeRewardedVideoWithNetworks", new object[]
		{
			text
		});
	}

	public void requestRewardedVideo(List<MoPubMediationSetting> mediationSettings = null, string keywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text = (mediationSettings != null) ? Json.Serialize(mediationSettings) : null;
		this._plugin.Call("requestRewardedVideo", new object[]
		{
			text,
			keywords,
			latitude,
			longitude,
			customerId
		});
	}

	public void showRewardedVideo()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		this._plugin.Call("showRewardedVideo", new object[0]);
	}

	public bool hasRewardedVideo()
	{
		return Application.platform == RuntimePlatform.Android && this._plugin.Call<bool>("hasRewardedVideo", new object[0]);
	}

	public List<MoPubManager.MoPubReward> getAVailableRewards()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		this._rewardsDict.Clear();
		using (AndroidJavaObject androidJavaObject = this._plugin.Call<AndroidJavaObject>("getAvailableRewards", new object[0]))
		{
			AndroidJavaObject[] array = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(androidJavaObject.GetRawObject());
			if (array.Length > 1)
			{
				foreach (AndroidJavaObject androidJavaObject2 in array)
				{
					string label = androidJavaObject2.Call<string>("getLabel", new object[0]);
					int amount = androidJavaObject2.Call<int>("getAmount", new object[0]);
					this._rewardsDict.Add(new MoPubManager.MoPubReward(label, amount), androidJavaObject2);
				}
			}
		}
		return new List<MoPubManager.MoPubReward>(this._rewardsDict.Keys);
	}

	public void selectReward(MoPubManager.MoPubReward selectedReward)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		AndroidJavaObject androidJavaObject;
		if (this._rewardsDict.TryGetValue(selectedReward, out androidJavaObject))
		{
			this._plugin.Call("selectReward", new object[]
			{
				androidJavaObject
			});
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("Selected reward {0} is not available.", selectedReward));
		}
	}

	private static readonly AndroidJavaClass _pluginClass = new AndroidJavaClass("com.mopub.unity.MoPubRewardedVideoUnityPlugin");

	private readonly AndroidJavaObject _plugin;

	private Dictionary<MoPubManager.MoPubReward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubManager.MoPubReward, AndroidJavaObject>();

	private MoPubManager.MoPubReward _selectedReward;
}
