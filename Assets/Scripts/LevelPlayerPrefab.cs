using System;
using UnityEngine;

public class LevelPlayerPrefab : MonoBehaviour
{
	private void Start()
	{
		Time.timeScale = 1f;
		MainMenuPrefab.mainmenufirst = false;
		MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
	}
}
