using System;
using UnityEngine;

public class GuiPhoto : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3((float)Screen.width / 1024f, (float)Screen.height / 768f, 1f));
		GUI.Box(new Rect(0f, -4f, 1024f, 30f), string.Empty);
		GUILayout.Label("Manipulation of an image : Twist, Pinch, Drag  with 1 or 2 fingers, ctrl key to swipe and  alt key to Twist and pinch to simulate the second finger", new GUILayoutOption[0]);
		GUILayout.Space(15f);
		this.bTwist = GUILayout.Toggle(this.bTwist, "Enable Twist", new GUILayoutOption[0]);
		EasyTouch.SetEnableTwist(this.bTwist);
		GUILayout.Space(15f);
		this.bPinch = GUILayout.Toggle(this.bPinch, "Enable Pinch", new GUILayoutOption[0]);
		EasyTouch.SetEnablePinch(this.bPinch);
		if (GUI.Button(new Rect(412f, 700f, 200f, 50f), "Main menu"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
		}
	}

	private bool bTwist = true;

	private bool bPinch = true;
}
