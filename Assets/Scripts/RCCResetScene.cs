using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RCCResetScene : MonoBehaviour
{
	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
