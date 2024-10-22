using System;
using System.Collections;
using Analytics;
using AudienceNetwork;
using Facebook.Unity;
using GssAdSdk;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class NativeAdTest : MonoBehaviour
{
	private void Awake()
	{
		this.off_childgameobject();
		if (TenlogixAds.FBClickFill)
		{
			this.add_buttonOnimage();
		}
		if (TenlogixAds.FBDelayFill && (this.Adplace == NativeAdTest.Adplacementtype.SplashLeft || this.Adplace == NativeAdTest.Adplacementtype.LoadingLeft))
		{
			this.off_actionbt_interactable();
		}
		else if (TenlogixAds.FBDelayPanelFill && this.nativetype != NativeAdTest.NativeAdtype.Levelloading && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
		{
			this.off_actionbt_interactable();
		}
		else
		{
			this.on_actionbt_interactable();
		}
		if (this.nativetype == NativeAdTest.NativeAdtype.Simple)
		{
			this.nativeID = AdIDs.FBNativeID;
			this.check1 = true;
			this.check2 = true;
			this.LoadAd();
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Splashloading && TenlogixAds.FBloadingFill)
		{
			if (SceneManager.GetActiveScene().buildIndex == 0 & this.Adplace == NativeAdTest.Adplacementtype.SplashRight)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().setnextscentimer(5f);
			}
			if (base.transform.Find("GifSplash"))
			{
				this.off_childgameobject();
				GameObject gameObject = base.transform.Find("GifSplash").gameObject;
				gameObject.SetActive(true);
			}
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Panel && !TenlogixAds.FBpanelFill)
		{
			this.off_childgameobject();
			if (base.transform.Find("GifSplash"))
			{
				GameObject gameObject2 = base.transform.Find("GifSplash").gameObject;
				gameObject2.SetActive(true);
			}
		}
		else if ((this.nativetype == NativeAdTest.NativeAdtype.Panel && TenlogixAds.FBpanelFill) || (this.nativetype == NativeAdTest.NativeAdtype.StartSplash && TenlogixAds.FBSplashFill) || (this.nativetype == NativeAdTest.NativeAdtype.Levelloading && TenlogixAds.FBLevelFill) || (this.nativetype == NativeAdTest.NativeAdtype.Exit && TenlogixAds.FBExitFill) || (this.nativetype == NativeAdTest.NativeAdtype.Pause && TenlogixAds.FBPauseFill) || (this.nativetype == NativeAdTest.NativeAdtype.Fail && TenlogixAds.FBFailFill) || (this.nativetype == NativeAdTest.NativeAdtype.Success && TenlogixAds.FBSucessFill))
		{
			this.check1 = false;
			this.check2 = false;
			this.LoadAd_Custom();
		}
		else
		{
			this.event_failnativeAD("GAMEOBjEctFAlse");
		}
	}

	private void add_buttonOnimage()
	{
		if (this.iconImage.gameObject.GetComponent<Button>() != null)
		{
			this.iconimagebt = this.iconImage.gameObject.GetComponent<Button>();
		}
		else
		{
			this.iconImage.gameObject.AddComponent<Button>();
			this.iconimagebt = this.iconImage.gameObject.GetComponent<Button>();
		}
		if (this.coverImage.gameObject.GetComponent<Button>() != null)
		{
			this.coverimagebt = this.coverImage.gameObject.GetComponent<Button>();
		}
		else
		{
			this.coverImage.gameObject.AddComponent<Button>();
			this.coverimagebt = this.coverImage.gameObject.GetComponent<Button>();
		}
	}

	private void OnGUI()
	{
		if (this.nativeAd != null && this.nativeAd.CoverImage != null)
		{
			this.coverImage.sprite = this.nativeAd.CoverImage;
		}
		if (this.nativeAd != null && this.nativeAd.IconImage != null)
		{
			this.iconImage.sprite = this.nativeAd.IconImage;
		}
		if (this.nativeAd != null && this.nativeAd.AdChoicesImage != null)
		{
			this.adchoiceImage.sprite = this.nativeAd.AdChoicesImage;
		}
	}

	private void OnDisable()
	{
		this.reload = true;
		if (base.transform.Find("GifSplash"))
		{
			this.on_childgameobject();
			GameObject gameObject = base.transform.Find("GifSplash").gameObject;
			gameObject.SetActive(false);
		}
		this.OnDestroy();
	}

	private void OnEnable()
	{
		if (this.reload)
		{
			UnityEngine.Debug.Log("enable");
			this.Awake();
		}
	}

	private void OnDestroy()
	{
		if (this.nativeAd)
		{
			this.nativeAd.Dispose();
		}
	}

	public void LoadAd_All()
	{
		if (this.nativetype == NativeAdTest.NativeAdtype.Simple)
		{
			this.nativeID = AdIDs.FBNativeID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.StartSplash)
		{
			this.nativeID = AdIDs.FBNativeID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Levelloading)
		{
			this.nativeID = AdIDs.FBNativeLoadingID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Splashloading)
		{
			this.nativeID = AdIDs.FBNativeID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Success)
		{
			this.nativeID = AdIDs.FBNativeSucessID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Fail)
		{
			this.nativeID = AdIDs.FBNativeFailID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Exit)
		{
			this.nativeID = AdIDs.FBNativeExitID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Pause)
		{
			this.nativeID = AdIDs.FBNativePauseID_All;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Panel)
		{
			this.nativeID = AdIDs.FBNativeExitID_All;
		}
		NativeAd nativeAd = new NativeAd(this.nativeID);
		this.nativeAd = nativeAd;
		if (!TenlogixAds.FBClickFill)
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton
			});
		}
		else
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton,
				this.iconimagebt,
				this.coverimagebt
			});
		}
		nativeAd.NativeAdDidLoad = delegate()
		{
			this.StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
			this.StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
			this.StartCoroutine(nativeAd.LoadAdChoicesImage(nativeAd.AdChoicesImageURL));
			UnityEngine.Debug.Log("---Images loaded.");
			this.title.text = nativeAd.Title;
			this.socialContext.text = nativeAd.SocialContext;
			this.callToAction.text = nativeAd.CallToAction;
			this.addAdchoicebuttonlistner();
			this.count_coverdelaytime = 0;
			if (TenlogixAds.FBDelayFill && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
			else if (TenlogixAds.FBDelayPanelFill && this.nativetype != NativeAdTest.NativeAdtype.Levelloading && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
		};
		nativeAd.NativeAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("Native ad failed to load with error: " + error);
			this.event_failnativeAD(error);
		};
		nativeAd.NativeAdWillLogImpression = delegate()
		{
		};
		nativeAd.NativeAdDidClick = delegate()
		{
		};
		nativeAd.LoadAd();
	}

	public void LoadAd_Custom()
	{
		if (this.nativetype == NativeAdTest.NativeAdtype.Simple)
		{
			this.nativeID = AdIDs.FBNativeID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.StartSplash)
		{
			this.nativeID = AdIDs.FBNativeID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Levelloading)
		{
			this.nativeID = AdIDs.FBNativeLoadingID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Splashloading)
		{
			this.nativeID = AdIDs.FBNativeID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Success)
		{
			this.nativeID = AdIDs.FBNativeSucessID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Fail)
		{
			this.nativeID = AdIDs.FBNativeFailID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Exit)
		{
			this.nativeID = AdIDs.FBNativeExitID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Pause)
		{
			this.nativeID = AdIDs.FBNativePauseID_Custom;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Panel)
		{
			this.nativeID = AdIDs.FBNativeExitID_Custom;
		}
		NativeAd nativeAd = new NativeAd(this.nativeID);
		this.nativeAd = nativeAd;
		if (!TenlogixAds.FBClickFill)
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton
			});
		}
		else
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton,
				this.iconimagebt,
				this.coverimagebt
			});
		}
		nativeAd.NativeAdDidLoad = delegate()
		{
			this.StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
			this.StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
			this.StartCoroutine(nativeAd.LoadAdChoicesImage(nativeAd.AdChoicesImageURL));
			UnityEngine.Debug.Log("---Images loaded.");
			this.title.text = nativeAd.Title;
			this.socialContext.text = nativeAd.SocialContext;
			this.callToAction.text = nativeAd.CallToAction;
			this.addAdchoicebuttonlistner();
			this.count_coverdelaytime = 0;
			if (SceneManager.GetActiveScene().buildIndex == 0 && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().setnextscentimer(8f);
			}
			this.is_nativeadload();
			if (TenlogixAds.FBDelayFill && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
			else if (TenlogixAds.FBDelayPanelFill && this.nativetype != NativeAdTest.NativeAdtype.Levelloading && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
		};
		nativeAd.NativeAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("Native ad failed to load with error: " + error);
			this.event_failnativeAD(error);
		};
		nativeAd.NativeAdWillLogImpression = delegate()
		{
		};
		nativeAd.NativeAdDidClick = delegate()
		{
		};
		nativeAd.LoadAd();
	}

	public void LoadAd()
	{
		if (this.nativetype == NativeAdTest.NativeAdtype.Simple)
		{
			this.nativeID = AdIDs.FBNativeID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.StartSplash)
		{
			this.nativeID = AdIDs.FBNativeID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Levelloading)
		{
			this.nativeID = AdIDs.FBNativeLoadingID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Splashloading)
		{
			this.nativeID = AdIDs.FBNativeID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Success)
		{
			this.nativeID = AdIDs.FBNativeSucessID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Fail)
		{
			this.nativeID = AdIDs.FBNativeFailID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Exit)
		{
			this.nativeID = AdIDs.FBNativeExitID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Pause)
		{
			this.nativeID = AdIDs.FBNativePauseID;
		}
		else if (this.nativetype == NativeAdTest.NativeAdtype.Panel)
		{
			this.nativeID = AdIDs.FBNativeExitID;
		}
		NativeAd nativeAd = new NativeAd(this.nativeID);
		this.nativeAd = nativeAd;
		if (!TenlogixAds.FBClickFill)
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton
			});
		}
		else
		{
			nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
			{
				this.callToActionButton,
				this.iconimagebt,
				this.coverimagebt
			});
		}
		nativeAd.NativeAdDidLoad = delegate()
		{
			this.StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
			this.StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
			this.StartCoroutine(nativeAd.LoadAdChoicesImage(nativeAd.AdChoicesImageURL));
			UnityEngine.Debug.Log("---Images loaded.");
			this.title.text = nativeAd.Title;
			this.socialContext.text = nativeAd.SocialContext;
			this.callToAction.text = nativeAd.CallToAction;
			this.addAdchoicebuttonlistner();
			this.count_coverdelaytime = 0;
			if (SceneManager.GetActiveScene().buildIndex == 0 && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().setnextscentimer(8f);
			}
			this.is_nativeadload();
			if (TenlogixAds.FBDelayFill && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
			else if (TenlogixAds.FBDelayPanelFill && this.nativetype != NativeAdTest.NativeAdtype.Levelloading && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.on_childgameobject();
			}
		};
		nativeAd.NativeAdDidFailWithError = delegate(string error)
		{
			UnityEngine.Debug.Log("Native ad failed to load with error: " + error);
			this.event_failnativeAD(error);
		};
		nativeAd.NativeAdWillLogImpression = delegate()
		{
		};
		nativeAd.NativeAdDidClick = delegate()
		{
		};
		nativeAd.LoadAd();
	}

	private void is_nativeadload()
	{
		if ((this.Adplace == NativeAdTest.Adplacementtype.SplashLeft && TenlogixAds.FBDelayFill) || (this.nativetype != NativeAdTest.NativeAdtype.Levelloading && TenlogixAds.FBDelayPanelFill && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft))
		{
			if (this.count_coverdelaytime < 12)
			{
				this.count_coverdelaytime++;
				if (this.count_coverdelaytime == 5)
				{
					this.status.text = "Loading Ad in 2 Sec ....";
				}
				else if (this.count_coverdelaytime == 9)
				{
					this.status.text = "Loading Ad in 1 Sec ....";
				}
				else if (this.count_coverdelaytime == 1)
				{
					this.status.text = "Loading Ad in 3 Sec ....";
				}
				base.StartCoroutine(this.Call_nativeloadRoutine(0.3f));
			}
			else
			{
				this.event_sucessnativeAD();
			}
		}
		else if (this.nativeAd.fbAd_isload)
		{
			this.event_sucessnativeAD();
		}
		else if (this.count_coverdelaytime < 15)
		{
			this.count_coverdelaytime++;
			base.StartCoroutine(this.Call_nativeloadRoutine(0.3f));
		}
		else
		{
			this.event_sucessnativeAD();
		}
	}

	private IEnumerator Call_nativeloadRoutine(float sec)
	{
		yield return new WaitForSecondsRealtime(sec);
		this.is_nativeadload();
		yield break;
	}

	private void off_actionbt_interactable()
	{
		this.callToActionButton.interactable = false;
		if (this.iconImage.gameObject.GetComponent<Button>() != null)
		{
			this.iconImage.gameObject.GetComponent<Button>().interactable = false;
		}
		if (this.coverImage.gameObject.GetComponent<Button>() != null)
		{
			this.coverImage.gameObject.GetComponent<Button>().interactable = false;
		}
	}

	private void on_actionbt_interactable()
	{
		this.callToActionButton.interactable = true;
		if (this.iconImage.gameObject.GetComponent<Button>() != null)
		{
			this.iconImage.gameObject.GetComponent<Button>().interactable = true;
		}
		if (this.coverImage.gameObject.GetComponent<Button>() != null)
		{
			this.coverImage.gameObject.GetComponent<Button>().interactable = true;
		}
	}

	private void event_failnativeAD(string error)
	{
		UnityEngine.Debug.Log("OBJNAME" + base.gameObject.name + "-" + this.Adplace.ToString());
		if (!this.check1)
		{
			this.check1 = true;
			this.LoadAd();
			return;
		}
		if (!this.check2)
		{
			this.check2 = true;
			this.LoadAd_All();
			return;
		}
		this.off_childgameobject();
		if (SceneManager.GetActiveScene().buildIndex == 0 & this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
		{
			GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().on_secondsplash();
			GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().loadAddsicon();
		}
		else if (this.Adplace == NativeAdTest.Adplacementtype.LoadingLeft && base.transform.parent.GetChild(1))
		{
			base.transform.parent.GetChild(1).gameObject.SetActive(true);
		}
		if (base.transform.Find("GifSplash"))
		{
			GameObject gameObject = base.transform.Find("GifSplash").gameObject;
			gameObject.SetActive(true);
		}
		this.fbfail_LogEvent(error);
	}

	private void event_sucessnativeAD()
	{
		if (SceneManager.GetActiveScene().buildIndex == 0 && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
		{
			if (GameObject.FindWithTag("MainCamera") && GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>())
			{
				GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().setnextscentimer(8f);
				GameObject.FindWithTag("MainCamera").GetComponent<Tenlogiclocal>().loadAddsicon();
			}
			else
			{
				UnityEngine.Debug.LogError("main camera or tenlogical not found");
			}
		}
		else
		{
			AdsManagerMainMenu.instance.hidebanner();
			MoPubAds.hideBanner(MoPubAds._bannerAdUnitId);
		}
		this.on_childgameobject();
		if (TenlogixAds.FBDelayFill && (this.Adplace == NativeAdTest.Adplacementtype.SplashLeft || this.Adplace == NativeAdTest.Adplacementtype.LoadingLeft))
		{
			base.StartCoroutine(this.CallActionBtRoutine(2f));
			this.off_loadingtext();
		}
		else if (TenlogixAds.FBDelayPanelFill && this.nativetype != NativeAdTest.NativeAdtype.Levelloading && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
		{
			base.StartCoroutine(this.CallActionBtRoutine(2f));
			this.off_loadingtext();
		}
		if (this.Adplace == NativeAdTest.Adplacementtype.LoadingLeft && base.transform.parent.GetChild(1))
		{
			base.transform.parent.GetChild(1).gameObject.SetActive(false);
		}
		this.fbsucess_LogEvent();
	}

	private void fbsucess_LogEvent()
	{
		if (FB.IsInitialized)
		{
			FB.LogAppEvent("FB-Native-AD- " + this.Adplace.ToString() + " -Shown", new float?(1f), null);
		}
		Analytics.MonoSingleton<Flurry>.Instance.LogEvent("FB-Native-AD- " + this.Adplace.ToString() + " -Shown");
	}

	private void fbfail_LogEvent(string error)
	{
		if (FB.IsInitialized)
		{
			FB.LogAppEvent("FB-Native-AD- " + this.Adplace.ToString() + " -Fail :" + error, new float?(1f), null);
		}
		Analytics.MonoSingleton<Flurry>.Instance.LogEvent("FB-Native-AD- " + this.Adplace.ToString() + " -Fail:" + error);
	}

	private void addAdchoicebuttonlistner()
	{
		Button component = this.AdchoiceLinkbtn.GetComponent<Button>();
		component.onClick.AddListener(delegate()
		{
			this.openAdchoicelink();
		});
	}

	private void openAdchoicelink()
	{
		Application.OpenURL(this.nativeAd.AdChoicesLinkURL);
	}

	public void startanim()
	{
		if (this.checkanim)
		{
			this.checkanim = false;
			this.animadchoice = this.mainAdchoiceobj.gameObject.GetComponent<Animator>();
			this.animadchoice.SetTrigger("start");
			base.StartCoroutine(this.CallManageRoutine(3f));
		}
	}

	private IEnumerator CallManageRoutine(float sec)
	{
		yield return new WaitForSecondsRealtime(sec);
		this.checkanim = true;
		this.animadchoice.SetTrigger("end");
		yield break;
	}

	private void off_loadingtext()
	{
		this.status.gameObject.transform.parent.gameObject.SetActive(false);
		this.status.gameObject.SetActive(false);
	}

	private IEnumerator CallActionBtRoutine(float sec)
	{
		yield return new WaitForSecondsRealtime(sec);
		this.on_actionbt_interactable();
		yield break;
	}

	private void on_childgameobject()
	{
		this.title.gameObject.SetActive(true);
		this.socialContext.gameObject.SetActive(true);
		if (TenlogixAds.FBDelayFill)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(true);
			this.status.gameObject.SetActive(true);
		}
		else if (TenlogixAds.FBDelayPanelFill)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(true);
			this.status.gameObject.SetActive(true);
		}
		else if (!TenlogixAds.FBDelayFill || !TenlogixAds.FBDelayPanelFill)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(false);
			this.status.gameObject.SetActive(false);
		}
		if (this.Adplace == NativeAdTest.Adplacementtype.LoadingLeft)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(false);
			this.status.gameObject.SetActive(false);
		}
		if (!TenlogixAds.FBDelayFill && this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(false);
			this.status.gameObject.SetActive(false);
		}
		else if (!TenlogixAds.FBDelayPanelFill && this.Adplace != NativeAdTest.Adplacementtype.SplashLeft && this.Adplace != NativeAdTest.Adplacementtype.LoadingLeft)
		{
			this.status.gameObject.transform.parent.gameObject.SetActive(false);
			this.status.gameObject.SetActive(false);
		}
		this.mainAdchoiceobj.gameObject.SetActive(true);
		this.coverImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(true);
		this.callToAction.gameObject.SetActive(true);
		this.callToActionButton.gameObject.SetActive(true);
		base.gameObject.GetComponent<Image>().color = new Color32(50, 50, 50, 150);
	}

	private void off_childgameobject()
	{
		this.title.gameObject.SetActive(false);
		this.socialContext.gameObject.SetActive(false);
		this.mainAdchoiceobj.gameObject.SetActive(false);
		if (!TenlogixAds.FBDelayFill)
		{
			if (this.Adplace == NativeAdTest.Adplacementtype.SplashLeft)
			{
				this.status.gameObject.transform.parent.gameObject.SetActive(false);
			}
			else if (!TenlogixAds.FBDelayPanelFill || this.nativetype == NativeAdTest.NativeAdtype.Levelloading)
			{
				this.status.gameObject.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (TenlogixAds.FBDelayFill)
		{
			if (this.Adplace != NativeAdTest.Adplacementtype.SplashLeft)
			{
				if (!TenlogixAds.FBDelayPanelFill || this.nativetype == NativeAdTest.NativeAdtype.Levelloading)
				{
					this.status.gameObject.transform.parent.gameObject.SetActive(false);
				}
			}
		}
		this.coverImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.callToAction.gameObject.SetActive(false);
		this.callToActionButton.gameObject.SetActive(false);
		base.gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
	}

	private void Log(string s)
	{
	}

	public void NextScene()
	{
		SceneManager.LoadScene("RewardedVideoAdScene");
	}

	private NativeAd nativeAd;

	public GameObject mainAdchoiceobj;

	public GameObject AdchoiceLinkbtn;

	public NativeAdTest.NativeAdtype nativetype;

	public NativeAdTest.Adplacementtype Adplace;

	[Header("Text:")]
	public Text title;

	public Text socialContext;

	public Text status;

	[Header("Images:")]
	public Image coverImage;

	public Image iconImage;

	public Image adchoiceImage;

	[Header("Buttons:")]
	public Text callToAction;

	public Button callToActionButton;

	private Button iconimagebt;

	private Button coverimagebt;

	private Animator animadchoice;

	private bool checkanim = true;

	private bool reload;

	private int count_coverdelaytime;

	private string nativeID = string.Empty;

	private bool check1;

	private bool check2;

	public enum NativeAdtype
	{
		Simple,
		Splashloading,
		Panel,
		StartSplash,
		Levelloading,
		Exit,
		Success,
		Fail,
		Pause
	}

	public enum Adplacementtype
	{
		SplashLeft,
		SplashRight,
		LoadingLeft,
		LoadingRight,
		Pause,
		Success,
		Fail,
		Exit,
		CommericalBreak,
		TimeUp
	}
}
