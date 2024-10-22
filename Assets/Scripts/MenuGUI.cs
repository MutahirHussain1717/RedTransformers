using System;
using UnityEngine;

public class MenuGUI : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		Screen.lockCursor = false;
	}

	private void OnGUI()
	{
		GUI.skin.button.fontSize = 20;
		GUI.DrawTexture(new Rect((float)(Screen.width / 2 - this.logo.width / 2), 10f, (float)this.logo.width, (float)this.logo.height), this.logo);
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 400), 300f, 300f, 50f), "Demo 1"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Demo1");
		}
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 400), 360f, 300f, 50f), "Demo 2"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Demo2");
		}
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 400), 420f, 300f, 50f), "Demo 3"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Demo3");
		}
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 400), 480f, 300f, 50f), "Get this project"))
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#/content/7676");
		}
		GUI.skin.label.fontSize = 14;
		GUI.Label(new Rect(20f, (float)(Screen.height - 60), 300f, 50f), "Weapon System 3.0 By Rachan Neamprasert www.hardworkerstudio.com");
	}

	public Texture2D logo;
}
