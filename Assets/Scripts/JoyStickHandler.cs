using System;
using UnityEngine;

public class JoyStickHandler : MonoBehaviour
{
	private void Start()
	{
		this.joystick = GameObject.Find("playerjoystick");
		this.anim = base.gameObject.GetComponent<Animator>();
		this.soundcheck = true;
	}

	private void Update()
	{
		if (!RoboTransformControler.flymode)
		{
			this.anim.SetBool("flymode", false);
		}
		else if (RoboTransformControler.flymode)
		{
			this.anim.SetBool("flymode", true);
		}
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
		if (move.joystickName == "playerjoystick")
		{
			if (!RoboTransformControler.flymode)
			{
				this.anim.SetBool("flymode", false);
				if (this.soundcheck)
				{
					this.audio.clip = null;
					this.audio.Stop();
					this.soundcheck = false;
				}
				this.anim.SetFloat("horizontal", move.joystickAxis.x);
				this.anim.SetFloat("vertical", move.joystickAxis.y);
			}
		}
		else if (RoboTransformControler.flymode)
		{
			this.anim.SetBool("flymode", true);
		}
	}

	private void On_JoystickMove(MovingJoystick move)
	{
		if (move.joystickName == "playerjoystick")
		{
			if (!RoboTransformControler.flymode)
			{
				this.anim.SetBool("flymode", false);
				if (!this.soundcheck)
				{
					this.audio.clip = this.footstepsound;
					this.audio.loop = true;
					this.audio.Play();
					this.audio.volume = 0.1f;
					this.soundcheck = true;
				}
				this.anim.SetFloat("horizontal", move.joystickAxis.x);
				this.anim.SetFloat("vertical", move.joystickAxis.y);
			}
			else if (RoboTransformControler.flymode)
			{
				this.anim.SetBool("flymode", true);
			}
		}
	}

	private GameObject joystick;

	private Animator anim;

	public AudioClip footstepsound;

	public AudioSource audio;

	private bool soundcheck;
}
