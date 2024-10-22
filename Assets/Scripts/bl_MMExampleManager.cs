using System;
using UnityEngine;

public class bl_MMExampleManager : MonoBehaviour
{
	private void Awake()
	{
		this.MapID = PlayerPrefs.GetInt("MMExampleMapID", 0);
		this.ApplyMap();
	}

	private void ApplyMap()
	{
		this.Maps[this.MapID].SetActive(true);
	}

	public void ChangeMap(int i)
	{
		PlayerPrefs.SetInt("MMExampleMapID", i);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public GameObject ExampleGo;

	public int MapID = 2;

	public const string MMName = "MMManagerExample";

	public GameObject[] Maps;
}
