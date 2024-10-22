using System;
using UnityEngine;

public class GuiCam : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3((float)Screen.width / 1024f, (float)Screen.height / 768f, 1f));
		GUI.Box(new Rect(0f, -4f, 1024f, 30f), string.Empty);
		GUILayout.Label("Free camera ctrl or alt key to simulate the second finger", new GUILayoutOption[0]);
		GUILayout.Space(15f);
		GUILayout.Label("1 finger => Look around", new GUILayoutOption[0]);
		GUILayout.Label("2 fingers => Move forward", new GUILayoutOption[0]);
		GUILayout.Label("3 fingers => Move backward", new GUILayoutOption[0]);
		if (GUI.Button(new Rect(412f, 700f, 200f, 50f), "Main menu"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
		}
	}
}
