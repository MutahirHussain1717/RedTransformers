using System;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
	private void OnEnable()
	{
		EasyButton.On_ButtonUp += this.On_ButtonUp;
	}

	private void OnDisable()
	{
		EasyButton.On_ButtonUp -= this.On_ButtonUp;
	}

	private void OnDestroy()
	{
		EasyButton.On_ButtonUp -= this.On_ButtonUp;
	}

	private void On_ButtonUp(string buttonName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
	}
}
