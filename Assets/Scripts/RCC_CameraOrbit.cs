using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Orbit")]
public class RCC_CameraOrbit : MonoBehaviour
{
	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
	}

	private void LateUpdate()
	{
		if (this.target)
		{
			this.x += UnityEngine.Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.y -= UnityEngine.Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.y = RCC_CameraOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
			Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.target.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public Transform target;

	public float distance = 10f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;
}
