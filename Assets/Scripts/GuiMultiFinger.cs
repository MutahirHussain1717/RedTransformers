using System;
using UnityEngine;

public class GuiMultiFinger : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3((float)Screen.width / 1024f, (float)Screen.height / 768f, 1f));
		GUI.Box(new Rect(0f, -4f, 1024f, 30f), string.Empty);
		GUILayout.Label("Example of multiple fingers (10 max) :  touch, tap, long tap, pinch : ctrl or alt key to simulate the second finger", new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.Label("Touch => Creation of a circle under each touches", new GUILayoutOption[0]);
		GUILayout.Label("Tap on circle => Add ring under each touches ", new GUILayoutOption[0]);
		GUILayout.Label("Long tap on circle => Change the ring speed under each touches", new GUILayoutOption[0]);
		GUILayout.Label("Drag => move the circles under the touches", new GUILayoutOption[0]);
		GUILayout.Label("Pinch on circle => Size change", new GUILayoutOption[0]);
		if (GUI.Button(new Rect(412f, 700f, 200f, 50f), "Main menu"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
		}
	}
}
