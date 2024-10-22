using System;
using UnityEngine;

public class Swipe : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_SwipeStart += this.On_SwipeStart;
		EasyTouch.On_Swipe += this.On_Swipe;
		EasyTouch.On_SwipeEnd += this.On_SwipeEnd;
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
		EasyTouch.On_SwipeStart -= this.On_SwipeStart;
		EasyTouch.On_Swipe -= this.On_Swipe;
		EasyTouch.On_SwipeEnd -= this.On_SwipeEnd;
	}

	private void Start()
	{
		this.textMesh = (GameObject.Find("LastSwipeText").transform.gameObject.GetComponent("TextMesh") as TextMesh);
	}

	private void On_SwipeStart(Gesture gesture)
	{
		if (gesture.fingerIndex == 0 && this.trail == null)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.trail = (UnityEngine.Object.Instantiate(Resources.Load("Trail"), touchToWordlPoint, Quaternion.identity) as GameObject);
		}
	}

	private void On_Swipe(Gesture gesture)
	{
		if (this.trail != null)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.trail.transform.position = touchToWordlPoint;
		}
	}

	private void On_SwipeEnd(Gesture gesture)
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
				swipeOrDragAngle.ToString("f2"),
				" / ",
				gesture.deltaPosition.x.ToString("f5")
			});
		}
	}

	private TextMesh textMesh;

	private GameObject trail;
}
