using System;
using StartApp;
using UnityEngine;

public class StartAppBackPlugin : MonoBehaviour
{
	private void Start()
	{
		StartAppWrapper.loadAd();
	}

	private void exit()
	{
		Application.Quit();
	}
}
