using System;
using StartApp;
using UnityEngine;

public class Banners : MonoBehaviour
{
	private void Start()
	{
		StartAppWrapper.addBanner(StartAppWrapper.BannerType.STANDARD, StartAppWrapper.BannerPosition.BOTTOM);
		StartAppWrapper.addBanner(StartAppWrapper.BannerType.THREED, StartAppWrapper.BannerPosition.TOP);
	}
}
