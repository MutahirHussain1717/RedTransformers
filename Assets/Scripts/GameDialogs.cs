using System;
using System.Collections;
using Analytics;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class GameDialogs : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		Time.timeScale = 1f;
		GameDialogs.isGameOver = false;
		this.diaCalled = false;
		GameDialogs.NitroControler = true;
		this.IndexZoom = 0;
		GameDialogs.zooming = false;
		GameDialogs.noOfbullets = this.maxBullets;
		GameDialogs.noOfmissiles = this.maxMissiles;
		this.fovTemp = base.transform.GetComponent<Camera>().fieldOfView;
	}

	public void ClearUI()
	{
		this.GameControls.SetActive(false);
		this.btn_Paused.SetActive(false);
		this.timebg.SetActive(false);
		this.rcccanvas.SetActive(false);
	}

	public void ShowUI()
	{
		this.btn_Paused.SetActive(true);
		this.timebg.SetActive(true);
		this.GameControls.SetActive(true);
	}

	public IEnumerator TimeNormilization(float delay)
	{
		UnityEngine.Debug.Log("Corotuine called");
		Time.timeScale = 1f;
		yield return new WaitForSeconds(delay);
		UnityEngine.Debug.Log("Corotuine Ended");
		Time.timeScale = 1f;
		base.GetComponent<Camera>().enabled = true;
		this.ShowUI();
		yield break;
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			this.mtime += Time.deltaTime;
			this.timeGame = (int)this.mtime;
			this.finalTime.text = "Time recorded: " + this.timeGame.ToString() + " Sec";
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
			if (!startmission.again)
			{
				TimeController.isGamePaused = true;
				this.btn_Paused.SetActive(false);
				this.Dia_Paused(true);
				Time.timeScale = 0f;
				startmission.again = true;
				this.GameControls.SetActive(false);
			}
		}
	}

	public void PlayGame()
	{
		this.DirectionPanal.SetActive(false);
		this.DialogMission.SetActive(true);
	}

	public void ShowAdd()
	{
		if (MainMenu.adsalternative)
		{
			MainMenu.adsalternative = false;
			MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
		}
		else
		{
			MainMenu.adsalternative = true;
			MoPubAds.showAd(MoPubAds._interstitialOnGpEndId);
		}
		this.btn_Paused.SetActive(false);
	}

	public void Dia_Success()
	{
		UnityEngine.Debug.Log("misionsucess");
		UnityEngine.Debug.Log("Tank2" + PlayerPrefs.GetInt("Tank2"));
		UnityEngine.Debug.Log("CurrLevelIndex" + GlobalScripts.CurrLevelIndex);
		if (!this.diaCalled)
		{
			startmission.again = true;
			this.diaCalled = true;
			GameDialogs.isGameOver = true;
			this.ClearUI();
			TimeController.isGamePaused = true;
			base.GetComponent<AudioSource>().PlayOneShot(this.lvlComplete);
			this.success.SetActive(true);
			if (FB.IsInitialized)
			{
				FB.LogAppEvent("Level" + GlobalScripts.CurrLevelIndex + "Sucess", new float?(1f), null);
			}
			Analytics.MonoSingleton<Flurry>.Instance.LogEvent("Level-" + GlobalScripts.CurrLevelIndex + "-Sucess");
			if (GlobalScripts.CurrLevelIndex >= 0 && GlobalScripts.CurrLevelIndex <= 3)
			{
				if (GlobalScripts.CurrLevelIndex == 0 && PlayerPrefs.GetInt("Tank1") == 1)
				{
					PlayerPrefs.SetInt("Tank1", GlobalScripts.CurrLevelIndex + 2);
				}
				else if (GlobalScripts.CurrLevelIndex + 1 == PlayerPrefs.GetInt("Tank1"))
				{
					PlayerPrefs.SetInt("Tank1", GlobalScripts.CurrLevelIndex + 2);
				}
			}
			UnityEngine.Debug.Log("levelopen" + PlayerPrefs.GetInt("LevelOpen"));
			if (GlobalScripts.CurrLevelIndex == 4)
			{
				if (GlobalScripts.CurrLevelIndex <= PlayerPrefs.GetInt("Tank1"))
				{
					PlayerPrefs.SetInt("Tank1", GlobalScripts.CurrLevelIndex + 2);
				}
				UnityEngine.Debug.Log("levelopenbefore" + PlayerPrefs.GetInt("LevelOpen"));
				if (PlayerPrefs.GetInt("LevelOpen") <= 2)
				{
					PlayerPrefs.SetInt("LevelOpen", 2);
				}
			}
			UnityEngine.Debug.Log("levelopenafter" + PlayerPrefs.GetInt("LevelOpen"));
			if (GlobalScripts.CurrLevelIndex >= 5 && GlobalScripts.CurrLevelIndex <= 8)
			{
				UnityEngine.Debug.Log("inCurrLevelIndex" + GlobalScripts.CurrLevelIndex);
				if (GlobalScripts.CurrLevelIndex == 5 && PlayerPrefs.GetInt("Tank2") == 1)
				{
					PlayerPrefs.SetInt("Tank2", GlobalScripts.CurrLevelIndex - 5 + 2);
				}
				else if (GlobalScripts.CurrLevelIndex - 4 == PlayerPrefs.GetInt("Tank2"))
				{
					PlayerPrefs.SetInt("Tank2", GlobalScripts.CurrLevelIndex - 5 + 2);
				}
			}
			if (GlobalScripts.CurrLevelIndex == 9)
			{
				if (GlobalScripts.CurrLevelIndex - 5 <= PlayerPrefs.GetInt("Tank2"))
				{
					PlayerPrefs.SetInt("Tank2", GlobalScripts.CurrLevelIndex - 5 + 2);
				}
				if (PlayerPrefs.GetInt("LevelOpen") <= 3)
				{
					PlayerPrefs.SetInt("LevelOpen", 3);
				}
			}
			this.timebg.SetActive(false);
			this.btn_Paused.SetActive(false);
			this.GameControls.SetActive(false);
			UnityEngine.Debug.Log(PlayerPrefs.GetInt("Cash"));
			this.ShowAdd();
			Time.timeScale = 0f;
		}
	}

	public void GameResume()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.GetMoreNitros.SetActive(false);
		this.btn_Paused.SetActive(true);
		this.DiagetHealth.SetActive(false);
		this.timebg.SetActive(true);
		this.ShowUI();
		this.GameControls.SetActive(true);
		this.zoomSprite.SetActive(true);
	}

	public void cross()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.DiagetHealth.SetActive(false);
		this.Dia_Failed();
	}

	public void Nitrocross()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.GetMoreNitros.SetActive(false);
		GameDialogs.NitroControler = false;
		this.GameResume();
	}

	public void Dia_Failed()
	{
		UnityEngine.Debug.Log("misionfail");
		if (!this.diaCalled)
		{
			if (FB.IsInitialized)
			{
				FB.LogAppEvent("Level" + GlobalScripts.CurrLevelIndex + "Fail", new float?(1f), null);
			}
			Analytics.MonoSingleton<Flurry>.Instance.LogEvent("Level-" + GlobalScripts.CurrLevelIndex + "-Fail");
			this.diaCalled = true;
			GameDialogs.isGameOver = true;
			TimeController.isGamePaused = true;
			base.GetComponent<AudioSource>().PlayOneShot(this.lvlfail);
			this.timebg.SetActive(false);
			this.GameControls.SetActive(false);
			this.failed.SetActive(true);
			this.ShowAdd();
			startmission.again = true;
			this.btn_Paused.SetActive(false);
			this.timebg.SetActive(false);
			Time.timeScale = 0f;
		}
	}

	public void Dia_Paused(bool a)
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		startmission.again = true;
		this.timebg.SetActive(false);
		Time.timeScale = 0f;
		this.paused.SetActive(a);
		this.rcccanvas.SetActive(a);
		if (a)
		{
			this.ClearUI();
			MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
		}
	}

	public void Dia_GameOver()
	{
		startmission.again = true;
		TimeController.isGamePaused = true;
		this.gameover.SetActive(true);
		this.ShowAdd();
		this.ClearUI();
	}

	public void Dia_TimesUp()
	{
		if (FB.IsInitialized)
		{
			FB.LogAppEvent("Level" + GlobalScripts.CurrLevelIndex + "Timeup", new float?(1f), null);
		}
		Analytics.MonoSingleton<Flurry>.Instance.LogEvent("Level-" + GlobalScripts.CurrLevelIndex + "-Timesup");
		base.StopAllCoroutines();
		startmission.again = true;
		base.GetComponent<AudioSource>().PlayOneShot(this.lvlfail);
		TimeController.isGamePaused = true;
		Time.timeScale = 0f;
		this.timesup.SetActive(true);
		this.GameControls.SetActive(false);
		this.timebg.SetActive(false);
		this.btn_Paused.SetActive(false);
		this.ClearUI();
		this.ShowAdd();
	}

	public void Btn_Main()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void InnerView()
	{
	}

	public void Btn_Restart()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public void Btn_Next()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		GlobalScripts.CurrLevelIndex++;
		if (GlobalScripts.CurrLevelIndex >= 9)
		{
			GlobalScripts.CurrLevelIndex = 0;
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void Btn_Resume()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.Dia_Paused(false);
		//AdsManagerMainMenu.instance.request_smart_banner();
		this.btn_Paused.SetActive(true);
		TimeController.isGamePaused = false;
		Time.timeScale = 1f;
		startmission.again = false;
		this.ShowUI();
		GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().Play();
	}

	public GameObject DirectionPanal;

	public GameObject DialogMission;

	public GameObject success;

	public GameObject DiagetHealth;

	public GameObject GetMoreNitros;

	public GameObject failed;

	public GameObject timesup;

	public GameObject gameover;

	public GameObject paused;

	public GameObject timebg;

	public GameObject GameControls;

	public GameObject rcccanvas;

	public GameObject Cross;

	public GameObject levelnumber;

	public GameObject LeftJoyStick;

	public static bool isGameOver;

	public static bool ads;

	public GameObject btn_Paused;

	public AudioClip lvlComplete;

	public AudioClip lvlfail;

	public AudioClip buttonSound;

	private bool diaCalled;

	private bool cam;

	public Text TotalReward;

	public Text TotalCoins;

	public Text NoofMissileText;

	public Text NoOfBulletsText;

	private TextMesh uiTime;

	public Text finalTime;

	public Text CurrentCoins;

	private float mtime;

	private int timeGame;

	public static bool NitroControler = true;

	public static bool zooming;

	public float[] ZoomFOVLists;

	public int IndexZoom;

	public Scrollbar zoomScroll;

	private float fovTemp;

	public GameObject zoomCamera;

	public GameObject zoomSprite;

	public GameObject bulletBtn;

	public GameObject missilesBtn;

	public GameObject MiniMapCamera;

	public GameObject[] enemies;

	public GameObject[] enemiesCanvas;

	public static int noOfbullets;

	public static int noOfmissiles;

	public int maxBullets;

	public int maxMissiles;
}
