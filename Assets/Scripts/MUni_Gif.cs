using System;
using GssAdSdk;
using UnityEngine;

public class MUni_Gif : MonoBehaviour
{
	private void Start()
	{
		if (TenlogixAds.isBackFilledEnabled && !string.IsNullOrEmpty(TenlogixAds.GUR))
		{
			this.m_uniGifImage.SetGifFromUrl(TenlogixAds.GUR, true);
		}
		else
		{
			if (this.switchAdicon)
			{
				this.switchAdicon.SetActive(true);
			}
			base.gameObject.SetActive(false);
		}
	}

	public void OnGifaddicon()
	{
		Application.OpenURL("market://details?id=" + TenlogixAds.GetProductName(TenlogixAds.GUR));
	}

	[SerializeField]
	private UniGifImage m_uniGifImage;

	public GameObject switchAdicon;
}
