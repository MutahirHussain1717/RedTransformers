using System;
using UnityEngine;

public class GameplayPrefab : MonoBehaviour
{
	private void Start()
	{
		Time.timeScale = 1f;
		MainMenuPrefab.mainmenufirst = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			MoPubAds.hideBanner(MoPubAds._bannerAdUnitId);
			AdsManagerMainMenu.instance.request_smart_banner();
		}
	}
}
