using System;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_TouchDown += this.On_TouchDown;
		EasyTouch.On_Swipe += this.On_Swipe;
	}

	private void OnDisable()
	{
		this.UnsubscribeEvent();
	}

	private void OnDestroy()
	{
		this.UnsubscribeEvent();
	}

	private void UnsubscribeEvent()
	{
		EasyTouch.On_TouchDown -= this.On_TouchDown;
		EasyTouch.On_Swipe -= this.On_Swipe;
	}

	private void Start()
	{
		this.cam = Camera.main;
	}

	private void On_TouchDown(Gesture gesture)
	{
		if (gesture.touchCount == 2)
		{
			this.cam.transform.Translate(new Vector3(0f, 0f, 1f) * Time.deltaTime);
		}
		if (gesture.touchCount == 3)
		{
			this.cam.transform.Translate(new Vector3(0f, 0f, -1f) * Time.deltaTime);
		}
	}

	private void On_Swipe(Gesture gesture)
	{
		this.rotationX += gesture.deltaPosition.x;
		this.rotationY += gesture.deltaPosition.y;
		this.cam.transform.localRotation = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		this.cam.transform.localRotation *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
	}

	private float rotationX;

	private float rotationY;

	private Camera cam;
}
