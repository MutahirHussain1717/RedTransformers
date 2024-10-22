using System;
using UnityEngine;

public class C_EasyJoystickTemplate : MonoBehaviour
{
	private void OnEnable()
	{
		EasyJoystick.On_JoystickTouchStart += this.On_JoystickTouchStart;
		EasyJoystick.On_JoystickMoveStart += this.On_JoystickMoveStart;
		EasyJoystick.On_JoystickMove += this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd += this.On_JoystickMoveEnd;
		EasyJoystick.On_JoystickTouchUp += this.On_JoystickTouchUp;
		EasyJoystick.On_JoystickTap += this.On_JoystickTap;
		EasyJoystick.On_JoystickDoubleTap += this.On_JoystickDoubleTap;
	}

	private void OnDisable()
	{
		EasyJoystick.On_JoystickTouchStart -= this.On_JoystickTouchStart;
		EasyJoystick.On_JoystickMoveStart -= this.On_JoystickMoveStart;
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
		EasyJoystick.On_JoystickTouchUp -= this.On_JoystickTouchUp;
		EasyJoystick.On_JoystickTap -= this.On_JoystickTap;
		EasyJoystick.On_JoystickDoubleTap -= this.On_JoystickDoubleTap;
	}

	private void OnDestroy()
	{
		EasyJoystick.On_JoystickTouchStart -= this.On_JoystickTouchStart;
		EasyJoystick.On_JoystickMoveStart -= this.On_JoystickMoveStart;
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
		EasyJoystick.On_JoystickTouchUp -= this.On_JoystickTouchUp;
		EasyJoystick.On_JoystickTap -= this.On_JoystickTap;
		EasyJoystick.On_JoystickDoubleTap -= this.On_JoystickDoubleTap;
	}

	private void On_JoystickDoubleTap(MovingJoystick move)
	{
	}

	private void On_JoystickTap(MovingJoystick move)
	{
	}

	private void On_JoystickTouchUp(MovingJoystick move)
	{
	}

	private void On_JoystickMoveEnd(MovingJoystick move)
	{
	}

	private void On_JoystickMove(MovingJoystick move)
	{
	}

	private void On_JoystickMoveStart(MovingJoystick move)
	{
	}

	private void On_JoystickTouchStart(MovingJoystick move)
	{
	}
}
