using System;
using UnityEngine;

public class GuiTwoFinger : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3((float)Screen.width / 1024f, (float)Screen.height / 768f, 1f));
		GUI.Box(new Rect(0f, -4f, 1024f, 30f), string.Empty);
		GUILayout.Label("Examples with two fingers : ctrl key to swipe and  alt key to Twist and pinch to simulate the second finger", new GUILayoutOption[0]);
		GUILayout.Label("For the 3 last balls, you need first positioning the orange target on the sphere, with the CTRL key, and next, press the ALT key, and do the action.", new GUILayoutOption[0]);
		if (GUI.Button(new Rect(412f, 700f, 200f, 50f), "Main menu"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
		}
	}
}
