using System;
using UnityEngine;

public class TwoSwipe : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_SwipeStart2Fingers += this.On_SwipeStart2Fingers;
		EasyTouch.On_Swipe2Fingers += this.On_Swipe2Fingers;
		EasyTouch.On_SwipeEnd2Fingers += this.On_SwipeEnd2Fingers;
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
		EasyTouch.On_SwipeStart2Fingers -= this.On_SwipeStart2Fingers;
		EasyTouch.On_Swipe2Fingers -= this.On_Swipe2Fingers;
		EasyTouch.On_SwipeEnd2Fingers -= this.On_SwipeEnd2Fingers;
		EasyTouch.On_Cancel2Fingers -= this.On_Cancel2Fingers;
	}

	private void Start()
	{
		this.textMesh = (GameObject.Find("LastSwipeText").transform.gameObject.GetComponent("TextMesh") as TextMesh);
	}

	private void On_SwipeStart2Fingers(Gesture gesture)
	{
		if (this.trail == null)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.trail = (UnityEngine.Object.Instantiate(Resources.Load("Trail"), touchToWordlPoint, Quaternion.identity) as GameObject);
			EasyTouch.SetEnableTwist(false);
			EasyTouch.SetEnablePinch(false);
		}
	}

	private void On_Swipe2Fingers(Gesture gesture)
	{
		if (this.trail != null)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.trail.transform.position = touchToWordlPoint;
		}
	}

	private void On_SwipeEnd2Fingers(Gesture gesture)
	{
		if (this.trail != null)
		{
			UnityEngine.Object.Destroy(this.trail);
			float swipeOrDragAngle = gesture.GetSwipeOrDragAngle();
			this.textMesh.text = string.Concat(new object[]
			{
				"Last swipe : ",
				gesture.swipe.ToString(),
				" /  vector : ",
				gesture.swipeVector.normalized,
				" / angle : ",
				swipeOrDragAngle.ToString("f2")
			});
			EasyTouch.SetEnableTwist(true);
			EasyTouch.SetEnablePinch(true);
		}
	}

	private void On_Cancel2Fingers(Gesture gesture)
	{
		if (this.trail != null)
		{
			UnityEngine.Object.Destroy(this.trail);
			EasyTouch.SetEnableTwist(true);
			EasyTouch.SetEnablePinch(true);
		}
	}

	private TextMesh textMesh;

	private GameObject trail;
}
