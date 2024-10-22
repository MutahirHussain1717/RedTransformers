using System;
using UnityEngine;

public class ManualControl : MonoBehaviour
{
	private void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer && this.Joystick != null)
		{
			this.Joystick.visible = false;
			this.Joystick.On_Manual(new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical")));
		}
	}

	public EasyJoystick Joystick;
}
