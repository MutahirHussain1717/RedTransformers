using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionThreeSelection : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Open Truck1 Level: " + PlayerPrefs.GetInt("Tank3"));
		if (PlayerPrefs.GetInt("Tank3") <= 0)
		{
			PlayerPrefs.SetInt("Tank3", 1);
		}
		if (PlayerPrefs.GetInt("Tank3") >= 5)
		{
			PlayerPrefs.SetInt("Tank3", 5);
		}
		this.levelOpen = PlayerPrefs.GetInt("Tank3");
	}

	private void Update()
	{
		for (int i = 0; i < this.levelOpen; i++)
		{
			this.levelsContent[i].transform.GetComponent<Button>().interactable = true;
			this.levelsContent[i].transform.GetChild(0).gameObject.SetActive(false);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
			if (Application.platform == RuntimePlatform.Android)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			}
		}
	}

	public void OnBtnBack()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void OnBtnLevel1()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 10;
		SceneManager.LoadScene("The City 3");
	}

	public void OnBtnLevel2()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 11;
		SceneManager.LoadScene("The City 3");
	}

	public void OnBtnLevel3()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 12;
		SceneManager.LoadScene("The City 3");
	}

	public void OnBtnLevel4()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 13;
		SceneManager.LoadScene("The City 3");
	}

	public void OnBtnLevel5()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 14;
		SceneManager.LoadScene("The City 3");
	}

	public void OnBtnLevel6()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 15;
		SceneManager.LoadScene("The City 3");
	}

	public void OnLoading()
	{
		this.loading.gameObject.SetActive(true);
	}

	public GameObject loading;

	public AudioClip buttonSound;

	public int levelOpen;

	public GameObject[] levelsContent;
}
