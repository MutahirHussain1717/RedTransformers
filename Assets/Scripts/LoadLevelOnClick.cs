using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(this.levelName);
		}
	}

	public string levelName;
}
