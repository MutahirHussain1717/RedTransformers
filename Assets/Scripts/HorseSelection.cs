using System;
using UnityEngine;

public class HorseSelection : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		PlayerPrefs.SetInt("SelectedHorse", 0);
		Time.timeScale = 1f;
		if (this.GOLocationSpot == null)
		{
		}
		foreach (GameObject gameObject in this.GOList)
		{
			gameObject.gameObject.SetActive(false);
		}
		this.ShowID(0);
	}

	public void ButtonBack()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void ButtonNexLevel()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}

	private void Update()
	{
		UnityEngine.Debug.Log(PlayerPrefs.GetInt("SelectedHorse") + " Selected");
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.ShowNext();
			base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
			this.ShowPrevious();
		}
	}

	public void ShowNext()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.ShowID(this._currentID + 1);
	}

	public void ShowPrevious()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.ShowID(this._currentID - 1);
	}

	private void ShowID(int ID)
	{
		if (ID == this.GOList.Length)
		{
			ID = 0;
		}
		else if (ID <= -1)
		{
			ID = this.GOList.Length - 1;
		}
		this.GOList[this._currentID].gameObject.SetActive(false);
		this._currentID = ID;
		this.GOList[this._currentID].gameObject.SetActive(true);
		PlayerPrefs.SetInt("SelectedHorse", this._currentID);
	}

	public AudioClip buttonSound;

	public GameObject[] GOList;

	private int _currentID;

	public Transform GOLocationSpot;
}
