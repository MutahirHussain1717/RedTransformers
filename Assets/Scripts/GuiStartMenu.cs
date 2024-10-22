using System;
using UnityEngine;

public class GuiStartMenu : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_SimpleTap += this.On_SimpleTap;
	}

	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3((float)Screen.width / 1024f, (float)Screen.height / 768f, 1f));
		GUI.Box(new Rect(0f, -4f, 1024f, 70f), string.Empty);
	}

	private void On_SimpleTap(Gesture gesture)
	{
		if (gesture.pickObject != null)
		{
			string name = gesture.pickObject.name;
			if (name == "OneFinger")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Onefinger");
			}
			else if (name == "TwoFinger")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("TwoFinger");
			}
			else if (name == "MultipleFinger")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("MultipleFingers");
			}
			else if (name == "MultiLayer")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("MultiLayers");
			}
			else if (name == "GameController")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("GameController");
			}
			else if (name == "FreeCamera")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("FreeCam");
			}
			else if (name == "ImageManipulation")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("ManipulationImage");
			}
			else if (name == "Joystick1")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("FirstPerson-DirectMode-DoubleJoystick");
			}
			else if (name == "Joystick2")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("ThirdPerson-DirectEventMode-DoubleJoystick");
			}
			else if (name == "Button")
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("ButtonExample");
			}
			else if (name == "Exit")
			{
				Application.Quit();
			}
		}
	}
}
