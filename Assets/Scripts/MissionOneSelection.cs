using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionOneSelection : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Open Tanks1 Level: " + PlayerPrefs.GetInt("Tank1"));
		if (PlayerPrefs.GetInt("Tank1") <= 0)
		{
			PlayerPrefs.SetInt("Tank1", 1);
		}
		if (PlayerPrefs.GetInt("Tank1") >= 5)
		{
			PlayerPrefs.SetInt("Tank1", 5);
		}
		this.levelOpen = PlayerPrefs.GetInt("Tank1");
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
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void OnBtnLevel1()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.loading.gameObject.SetActive(true);
		GlobalScripts.CurrLevelIndex = 0;
		base.Invoke("OnLoading", 10f);
	}

	public void OnBtnLevel2()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.loading.gameObject.SetActive(true);
		GlobalScripts.CurrLevelIndex = 1;
		base.Invoke("OnLoading", 10f);
	}

	public void OnBtnLevel3()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		GlobalScripts.CurrLevelIndex = 2;
		base.Invoke("OnLoading", 10f);
	}

	public void OnBtnLevel4()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.loading.gameObject.SetActive(true);
		GlobalScripts.CurrLevelIndex = 3;
		base.Invoke("OnLoading", 10f);
	}

	public void OnBtnLevel5()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.loading.gameObject.SetActive(true);
		GlobalScripts.CurrLevelIndex = 4;
		base.Invoke("OnLoading", 10f);
	}

	public void OnBtnLevel6()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		GlobalScripts.CurrLevelIndex = 5;
		this.loading.gameObject.SetActive(true);
		base.Invoke("OnLoading", 10f);
	}

	public void OnLoading()
	{
		SceneManager.LoadScene("The City 1");
	}

	public GameObject loading;

	public AudioClip buttonSound;

	public int levelOpen;

	public GameObject[] levelsContent;
}
