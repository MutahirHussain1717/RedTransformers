using System;
using UnityEngine;
using UnityEngine.UI;

public class JoystickEvent : MonoBehaviour
{
	private void Start()
	{
		this.Player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnEnable()
	{
		EasyJoystick.On_JoystickMove += this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd += this.On_JoystickMoveEnd;
	}

	private void OnDisable()
	{
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
	}

	private void OnDestroy()
	{
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
	}

	private void On_JoystickMoveEnd(MovingJoystick move)
	{
		if (move.joystickName == "LeftJoy")
		{
			this.Player.GetComponent<Animation>().Stop();
		}
	}

	private void On_JoystickMove(MovingJoystick move)
	{
		if (move.joystickName == "RightJoy")
		{
			if (this.Player.transform.localPosition.y < this.maxlowlevel)
			{
				this.Player.transform.localPosition = new Vector3(this.Player.transform.localPosition.x, this.maxlowlevel, this.Player.transform.localPosition.z);
			}
			if (this.Player.transform.localPosition.y > this.maxHeight)
			{
				this.Player.transform.localPosition = new Vector3(this.Player.transform.localPosition.x, this.maxHeight, this.Player.transform.localPosition.z);
			}
		}
	}

	private GameObject Player;

	public Text HeliSpeeed;

	public float maxlowlevel;

	public float maxHeight;
}
