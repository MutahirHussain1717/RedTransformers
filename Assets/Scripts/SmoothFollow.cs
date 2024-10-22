using System;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	private void Start()
	{
		SmoothFollow.cameraSwitch = 0;
	}

	private void Update()
	{
	}

	public void CameraSwitch(Transform setcam, float setdistance, float setheight)
	{
		this.target = setcam;
		this.distance = setdistance;
		this.height = setheight;
	}

	public void player_cams()
	{
		this.target = this.playercam;
		this.distance = 0f;
		this.height = 0f;
	}

	public void player_cams1()
	{
		this.target = this.playercam1;
		this.distance = 9f;
		this.height = 3f;
	}

	public void Truck_Cam_rotation()
	{
		this.target = this.truck_camrotation;
		this.distance = 4.52f;
		this.height = 1.79f;
	}

	private void LateUpdate()
	{
		if (!this.target)
		{
			return;
		}
		float b = this.target.eulerAngles.y + this.X;
		float b2 = this.target.position.y + this.height;
		float num = base.transform.eulerAngles.y;
		float num2 = base.transform.position.y;
		num = Mathf.LerpAngle(num, b, this.rotationDamping * Time.deltaTime);
		num2 = Mathf.Lerp(num2, b2, this.heightDamping * Time.deltaTime);
		Quaternion rotation = Quaternion.Euler(0f, num, 0f);
		base.transform.position = this.target.position;
		base.transform.position -= rotation * Vector3.forward * this.distance;
		base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
		base.transform.LookAt(this.target);
	}

	public Transform target;

	public float distance = 5f;

	public float height = 2f;

	public float heightDamping = 2f;

	public float rotationDamping = 3f;

	public static int cameraSwitch;

	public float X;

	public float Y;

	public Transform[] cameraSwitchView;

	public GameObject CivilianHelper;

	public Transform truck_camrotation;

	public Transform playercam;

	public Transform playercam1;
}
