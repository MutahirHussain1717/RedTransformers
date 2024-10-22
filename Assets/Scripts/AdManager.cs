using System;
using AudienceNetwork;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
	private void Start()
	{
		this.adLoaded = false;
		this.LoadAd();
	}

	private void OnDestroy()
	{
		if (this.nativeAd)
		{
			this.nativeAd.Dispose();
		}
		UnityEngine.Debug.Log("NativeAdTest was destroyed!");
	}

	public bool IsAdLoaded()
	{
		return this.adLoaded;
	}

	public void LoadAd()
	{
		NativeAd nativeAd = new NativeAd("YOUR_PLACEMENT_ID");
		this.nativeAd = nativeAd;
		if (this.targetAdObject)
		{
			if (this.targetButton)
			{
				nativeAd.RegisterGameObjectForImpression(this.targetAdObject, new Button[]
				{
					this.targetButton
				});
			}
			else
			{
				nativeAd.RegisterGameObjectForImpression(this.targetAdObject, new Button[0]);
			}
		}
		else
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[0]);
		}
		nativeAd.NativeAdDidLoad = delegate()
		{
			this.adLoaded = true;
			UnityEngine.Debug.Log("Native ad loaded.");
			UnityEngine.Debug.Log("Loading images...");
			this.StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
			this.StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
			UnityEngine.Debug.Log("Images loaded.");
		};
		nativeAd.NativeAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("Native ad failed to load with error: " + error);
		};
		nativeAd.NativeAdWillLogImpression = delegate()
		{
			UnityEngine.Debug.Log("Native ad logged impression.");
		};
		nativeAd.NativeAdDidClick = delegate()
		{
			UnityEngine.Debug.Log("Native ad clicked.");
		};
		nativeAd.LoadAd();
		UnityEngine.Debug.Log("Native ad loading...");
	}

	public NativeAd nativeAd;

	public GameObject targetAdObject;

	public Button targetButton;

	private bool adLoaded;
}
