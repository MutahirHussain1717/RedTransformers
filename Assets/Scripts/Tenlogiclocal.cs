using System;
using System.Collections;
using System.IO;
using EncryptStringSample;
using Facebook.Unity;
using Facebook.Unity.Settings;
using GssAdSdk;
using StartApp;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tenlogiclocal : GSSNetworkHandlerDelegate
{
	private void Awake()
	{
		this.LoadingBar.GetComponent<Animator>().SetFloat("Barspeed", 0.2f);
		this.iterateicon = 0;
		if (Tenlogiclocal.onetimecall)
		{
			this.filePath = Application.streamingAssetsPath + "/UserDetails.xml";
			Tenlogiclocal.xmltype = XML.Local;
		}
	}

	private void Start()
	{
		Time.timeScale = 1f;
		this.time = 0f;
		if (Tenlogiclocal.onetimecall)
		{
			MonoBehaviour.print("onetimecall");
			if (Application.platform == RuntimePlatform.Android)
			{
				base.StartCoroutine(this.userDetailsXmlPath1());
			}
			else
			{
				this.userDetailsXmlPath();
			}
			this.firstcheck = true;
			this.secondcheck = true;
			this.thirdcheck = true;
			Tenlogiclocal.onetimecall = true;
			this.fourthcheck = true;
		}
	}

	public void setnextscentimer(float settime)
	{
		this.scenetime += settime;
	}

	public void on_secondsplash()
	{
		this.fbnativesmall.SetActive(true);
	}

	private void Update()
	{
		if (this.time > this.scenetime && Tenlogiclocal.onetimetimer)
		{
			Tenlogiclocal.onetimetimer = false;
			Tenlogiclocal.onetimecall = false;
			this.loadstart_upads();
			UnityEngine.Debug.Log("nextscene");
			this.LoadingBar.GetComponent<Animator>().SetFloat("Barspeed", 30f);
			base.Invoke("loadnextscene", 1f);
		}
		else
		{
			this.time += Time.deltaTime;
		}
		if (Tenlogiclocal.onetimecall && Tenlogiclocal.xmltype == XML.Global)
		{
			if (!string.IsNullOrEmpty(TenlogixAds.UR))
			{
				if (this.thirdcheck)
				{
					this.thirdcheck = false;
					if (!TenlogixAds.tenlogixAdsSdk_initialized)
					{
						Screen.sleepTimeout = -1;
						NetworkHandler networkHandObj = new NetworkHandler(this);
						TenlogixAds.setConfig(false, AGameUtils.PACKAGE_NAME, AGameUtils.PRODUCT_NAME, "1", networkHandObj, TenlogixAds.ScreenOrientation_Landscape);
					}
				}
			}
			if (TenlogixAds.isadIDS_loadcounter == 1 && this.secondcheck)
			{
				MonoBehaviour.print("L");
				Tenlogiclocal.onetimecall = false;
				this.secondcheck = false;
				this.loadstart_upads();
				base.Invoke("loadnextscene", 1f);
			}
			else if (TenlogixAds.isadIDS_loadcounter == 2 && this.secondcheck)
			{
				MonoBehaviour.print("G");
				Tenlogiclocal.onetimecall = false;
				this.secondcheck = false;
				this.loadstart_upads();
			}
		}
		else if (Tenlogiclocal.onetimecall && Tenlogiclocal.xmltype == XML.AGlobal)
		{
			if (!string.IsNullOrEmpty(TenlogixAds.AUR))
			{
				if (this.firstcheck)
				{
					base.gameObject.GetComponent<PrivacyTextGetter>().onloadprivacycontent();
					this.firstcheck = false;
					if (!TenlogixAds.tenlogixAdsSdk_initialized)
					{
						Screen.sleepTimeout = -1;
						NetworkHandler networkHandObj2 = new NetworkHandler(this);
						TenlogixAds.setConfig(false, AGameUtils.PACKAGE_NAME, AGameUtils.PRODUCT_NAME, "1", networkHandObj2, TenlogixAds.ScreenOrientation_Landscape);
					}
				}
			}
		}
	}

	public void loadAddsicon()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (TenlogixAds.isBackFilledEnabled && TenlogixAds.arlist.Count > 0 && this.iterateicon < TenlogixAds.arlist.Count)
			{
				this.url = TenlogixAds.arlist[this.iterateicon];
				this.tempaddpackage = TenlogixAds.GetProductName(this.url);
				base.StartCoroutine("Temp_atbackend_GetAddPackage");
			}
		}
		else if (this.iterateicon < TenlogixAds.arrpackages.Length)
		{
			this.url = TenlogixAds.arrpackages[this.iterateicon];
			this.tempaddpackage = TenlogixAds.GetProductName(this.url);
			base.StartCoroutine("Temp_atbackend_GetAddPackage");
		}
	}

	private IEnumerator Temp_atbackend_GetAddPackage()
	{
		Texture2D text = new Texture2D(512, 512, TextureFormat.DXT1, false);
		if (File.Exists(Application.persistentDataPath + "/" + this.tempaddpackage + ".png"))
		{
			this.iterateicon++;
			this.loadAddsicon();
		}
		else
		{
			WWW www = new WWW(this.url);
			yield return www;
			if (www.error != null)
			{
				UnityEngine.Debug.Log("Internet not connected-- couldnot find icon");
			}
			else
			{
				www.LoadImageIntoTexture(text);
				File.WriteAllBytes(Application.persistentDataPath + "/" + this.tempaddpackage + ".png", www.bytes);
				this.iterateicon++;
				this.loadAddsicon();
			}
		}
		yield break;
	}

	private void changeid()
	{
		FacebookSettings.AppIds[0] = AdIDs.FBAnalyticID;
	}

	private void loadstart_upads()
	{
		if (this.fourthcheck)
		{
			this.fourthcheck = false;
			FacebookSettings.AppIds[0] = AdIDs.FBAnalyticID;
			PlayerPrefs.SetString("StartAppID", AdIDs.StartAppID);
			if (Application.platform == RuntimePlatform.Android)
			{
				StartAppWrapper.showSplash();
			}
			AGameUtils.initAnalytics();
			UnityEngine.Object.Instantiate<GameObject>(this.admanagerprefab, base.gameObject.transform.position, Quaternion.identity);
			base.Invoke("laodfbantivead", 3f);
			if (FB.IsInitialized)
			{
				FB.ActivateApp();
			}
			else
			{
				FB.Init(delegate()
				{
					FB.ActivateApp();
					FB.LogAppEvent("fb_mobile_level_achieved", new float?(1f), null);
				}, null, null);
			}
		}
	}

	private void laodfbantivead()
	{
		this.fbnativemedium.SetActive(true);
	}

	private IEnumerator userDetailsXmlPath1()
	{
		UnityEngine.Debug.Log("path1");
		WWW www = new WWW(this.filePath);
		yield return www;
		if (www.error != null)
		{
			MonoBehaviour.print("filenotfound");
		}
		else
		{
			string text = www.text;
			TextReader textReader = new StringReader(text);
			string cipherText = textReader.ReadToEnd();
			text = StringCipher.Decrypt(cipherText, AGameUtils.Cipher_Passwords);
			TenlogixAds.temp_parsingxml(text);
		}
		yield break;
	}

	private void userDetailsXmlPath()
	{
		StreamReader streamReader = new StreamReader(this.filePath);
		string text = streamReader.ReadToEnd();
		this.result = text.ToString();
		MonoBehaviour.print(this.result);
		this.result = StringCipher.Decrypt(this.result, AGameUtils.Cipher_Passwords);
		TenlogixAds.temp_parsingxml(this.result);
	}

	private void calltestbanner()
	{
	}

	public void loadnextscene()
	{
		SceneManager.LoadScene(1);
	}

	public override void NetworkCallFailure(string errorMsg)
	{
		TenlogixAds.init(null);
	}

	public override void NetworkCallSuccess(string data)
	{
		TenlogixAds.init(data);
	}

	public static XML xmltype;

	public GameObject admanagerprefab;

	public GameObject fbnativemedium;

	public GameObject fbnativesmall;

	public GameObject LoadingBar;

	public string filePath = string.Empty;

	public string result = string.Empty;

	public static bool onetimecall = true;

	public static bool onetimetimer = true;

	private bool firstcheck;

	private bool secondcheck;

	private bool thirdcheck;

	private bool fourthcheck;

	public Text adtext;

	private float time;

	private float scenetime = 25f;

	private string tempaddpackage;

	private string url;

	private int iterateicon;
}
