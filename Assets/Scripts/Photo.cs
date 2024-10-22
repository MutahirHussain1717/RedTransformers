using System;
using UnityEngine;

public class Photo : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_DragStart += this.On_DragStart;
		EasyTouch.On_Drag += this.On_Drag;
		EasyTouch.On_TouchStart2Fingers += this.On_TouchStart2Fingers;
		EasyTouch.On_TouchDown2Fingers += this.On_TouchDown2Fingers;
		EasyTouch.On_PinchIn += this.On_PinchIn;
		EasyTouch.On_PinchOut += this.On_PinchOut;
		EasyTouch.On_Twist += this.On_Twist;
		EasyTouch.On_Cancel2Fingers += this.On_Cancel2Fingers;
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
		EasyTouch.On_DragStart -= this.On_DragStart;
		EasyTouch.On_Drag -= this.On_Drag;
		EasyTouch.On_TouchStart2Fingers -= this.On_TouchStart2Fingers;
		EasyTouch.On_TouchDown2Fingers -= this.On_TouchDown2Fingers;
		EasyTouch.On_PinchIn -= this.On_PinchIn;
		EasyTouch.On_PinchOut -= this.On_PinchOut;
		EasyTouch.On_Twist -= this.On_Twist;
		EasyTouch.On_Cancel2Fingers -= this.On_Cancel2Fingers;
	}

	private void On_Cancel2Fingers(Gesture gesture)
	{
		if (gesture.touchCount > 0)
		{
			this.newPivot = true;
		}
	}

	private void On_DragStart(Gesture gesture)
	{
		if (gesture.touchCount == 1)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(1f, false);
			this.deltaPosition = touchToWordlPoint - base.transform.position;
		}
	}

	private void On_Drag(Gesture gesture)
	{
		if (gesture.touchCount == 1)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(1f, false);
			if (this.newPivot)
			{
				this.deltaPosition = touchToWordlPoint - base.transform.position;
				this.newPivot = false;
			}
			base.transform.position = touchToWordlPoint - this.deltaPosition;
		}
	}

	private void On_TouchStart2Fingers(Gesture gesture)
	{
		Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(1f, false);
		this.deltaPosition = touchToWordlPoint - base.transform.position;
	}

	private void On_TouchDown2Fingers(Gesture gesture)
	{
		Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(1f, false);
		base.transform.position = touchToWordlPoint - this.deltaPosition;
	}

	private void On_PinchIn(Gesture gesture)
	{
		float num = Time.deltaTime * gesture.deltaPinch / 25f;
		Vector3 localScale = base.transform.localScale;
		if ((double)(localScale.x - num) > 0.1)
		{
			base.transform.localScale = new Vector3(localScale.x - num, localScale.y - num, 1f);
		}
	}

	private void On_PinchOut(Gesture gesture)
	{
		float num = Time.deltaTime * gesture.deltaPinch / 25f;
		Vector3 localScale = base.transform.localScale;
		if (localScale.x + num < 3f)
		{
			base.transform.localScale = new Vector3(localScale.x + num, localScale.y + num, 1f);
		}
	}

	private void On_Twist(Gesture gesture)
	{
		base.transform.Rotate(new Vector3(0f, 0f, gesture.twistAngle));
	}

	private Vector3 deltaPosition;

	private Vector3 rotation;

	private bool newPivot;
}
