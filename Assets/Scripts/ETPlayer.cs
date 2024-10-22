using System;
using UnityEngine;

public class ETPlayer : MonoBehaviour
{
	private void OnEnable()
	{
		EasyJoystick.On_JoystickMove += this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd += this.On_JoystickMoveEnd;
		EasyButton.On_ButtonUp += this.On_ButtonUp;
	}

	private void Fire()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.bullet, this.gun.transform.position, this.gun.rotation);
	}

	private void OnDisable()
	{
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
		EasyButton.On_ButtonUp -= this.On_ButtonUp;
	}

	private void OnDestroy()
	{
		EasyJoystick.On_JoystickMove -= this.On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd -= this.On_JoystickMoveEnd;
		EasyButton.On_ButtonUp -= this.On_ButtonUp;
	}

	private void Start()
	{
		this.model = base.transform.Find("Model").transform;
		this.gun = base.transform.Find("Gun").transform;
	}

	private void On_JoystickMove(MovingJoystick move)
	{
		float y = move.Axis2Angle(true);
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, y, 0f));
		base.transform.Translate(Vector3.forward * move.joystickValue.magnitude * Time.deltaTime);
		this.model.GetComponent<Animation>().CrossFade("Run");
	}

	private void On_JoystickMoveEnd(MovingJoystick move)
	{
		this.model.GetComponent<Animation>().CrossFade("idle");
	}

	private void On_ButtonUp(string buttonName)
	{
		if (buttonName == "Exit")
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
		}
	}

	public GameObject bullet;

	private Transform model;

	private Transform gun;
}
