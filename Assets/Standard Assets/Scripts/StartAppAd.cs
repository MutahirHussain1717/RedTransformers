using System;
using StartApp;
using UnityEngine;

public class StartAppAd : MonoBehaviour
{
	private void Start()
	{
		this.adEventListener = new StartAppAd.AdEventListenerImplementation();
		this.videoListener = new StartAppAd.VideoListenerImplementation();
		StartAppWrapper.setVideoListener(this.videoListener);
	}

	private void OnGUI()
	{
		this.initializeButtons();
		this.addShowFullscreenButton(this.showFullscreenButton);
		this.addShowOfferwallButton(this.showOfferwallButton);
		this.addShowRewardedVideoButton(this.showRewardedVideoButton);
		this.addShowBannersButton(this.showBannersButton);
	}

	public void addShowFullscreenButton(Rect showFullscreenButton)
	{
		if (GUI.Button(showFullscreenButton, "Show Fullscreen", this.guiStyle))
		{
			StartAppWrapper.loadAd(StartAppWrapper.AdMode.FULLPAGE, this.adEventListener);
		}
	}

	public void addShowOfferwallButton(Rect showOfferwallButton)
	{
		if (GUI.Button(showOfferwallButton, "Show Offerwall", this.guiStyle))
		{
			StartAppWrapper.loadAd(StartAppWrapper.AdMode.OFFERWALL, this.adEventListener);
		}
	}

	public void addShowRewardedVideoButton(Rect showRewardedVideoButton)
	{
		if (GUI.Button(showRewardedVideoButton, "Show Rewarded Video", this.guiStyle))
		{
			StartAppWrapper.loadAd(StartAppWrapper.AdMode.REWARDED_VIDEO, this.adEventListener);
		}
	}

	public void addShowBannersButton(Rect showBannersButton)
	{
		if (GUI.Button(showBannersButton, "Show Banners", this.guiStyle))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}
	}

	public void initializeButtons()
	{
		int num = Screen.height / 6;
		Rect logoRect = new Rect((float)(Screen.width / 4), (float)(num / 3), (float)(Screen.width / 2), (float)(num / 3));
		this.showFullscreenButton = new Rect(0f, (float)num, (float)Screen.width, (float)num);
		this.showOfferwallButton = new Rect(0f, (float)(2 * num), (float)Screen.width, (float)num);
		this.showRewardedVideoButton = new Rect(0f, (float)(3 * num), (float)Screen.width, (float)num);
		this.showBannersButton = new Rect(0f, (float)(4 * num), (float)Screen.width, (float)num);
		this.guiStyle = new GUIStyle(GUI.skin.button);
		if (Screen.orientation == ScreenOrientation.Portrait)
		{
			this.guiStyle.fontSize = Screen.width / 12;
		}
		else
		{
			logoRect = new Rect((float)(Screen.width / 3), (float)(num / 3), (float)(Screen.width / 3), (float)(num / 3));
			this.guiStyle.fontSize = Screen.height / 12;
		}
		this.drawLogo(logoRect);
	}

	public void drawLogo(Rect logoRect)
	{
		Texture2D image = Resources.Load("StartAppLogo") as Texture2D;
		GUI.DrawTexture(logoRect, image);
	}

	private StartAppWrapper.AdEventListener adEventListener;

	private StartAppWrapper.VideoListener videoListener;

	private GUIStyle guiStyle;

	private Rect showFullscreenButton;

	private Rect showOfferwallButton;

	private Rect showRewardedVideoButton;

	private Rect showBannersButton;

	public class AdEventListenerImplementation : StartAppWrapper.AdEventListener
	{
		public void onReceiveAd()
		{
			UnityEngine.Debug.Log("Ad received");
			StartAppWrapper.showAd(this.adDisplayListener);
		}

		public void onFailedToReceiveAd()
		{
			UnityEngine.Debug.Log("Ad failed to receive");
		}

		private StartAppWrapper.AdDisplayListener adDisplayListener = new StartAppAd.AdDisplayListenerImplementation();
	}

	public class AdDisplayListenerImplementation : StartAppWrapper.AdDisplayListener
	{
		public void adHidden()
		{
			UnityEngine.Debug.Log("Ad Hidden");
		}

		public void adDisplayed()
		{
			UnityEngine.Debug.Log("Ad Displayed");
		}

		public void adClicked()
		{
			UnityEngine.Debug.Log("Ad Clicked");
		}
	}

	public class VideoListenerImplementation : StartAppWrapper.VideoListener
	{
		public void onVideoCompleted()
		{
			UnityEngine.Debug.Log("Rewarded Video Completed");
		}
	}
}
