using System;
using UnityEngine;

public class TankMover : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, UnityEngine.Input.GetAxis("Horizontal") * this.TurnSpeed * Time.deltaTime, 0f));
		base.transform.position += base.transform.forward * UnityEngine.Input.GetAxis("Vertical") * this.Speed * Time.deltaTime;
	}

	public float Speed = 20f;

	public float TurnSpeed = 100f;
}
