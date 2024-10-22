using System;
using UnityEngine;
using UnityEngine.UI;

public class HorselevelSelection : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Open Dog Level: " + PlayerPrefs.GetInt("Horse1"));
		if (PlayerPrefs.GetInt("Horse1") <= 0)
		{
			PlayerPrefs.SetInt("Horse1", 1);
		}
		if (PlayerPrefs.GetInt("Horse1") >= 5)
		{
			PlayerPrefs.SetInt("Horse1", 4);
		}
		this.levelOpen = PlayerPrefs.GetInt("Horse1");
	}

	private void Update()
	{
		for (int i = 0; i < this.levelOpen; i++)
		{
			this.levelsContent[i].transform.GetComponent<Button>().enabled = true;
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
		UnityEngine.SceneManagement.SceneManager.LoadScene("Horse1");
	}

	public void OnBtnLevel2()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Horse2");
	}

	public void OnBtnLevel3()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Horse3");
	}

	public void OnBtnLevel4()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.OnLoading();
		UnityEngine.SceneManagement.SceneManager.LoadScene("level2");
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
