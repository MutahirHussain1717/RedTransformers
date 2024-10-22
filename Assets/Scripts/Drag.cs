using System;
using UnityEngine;

public class Drag : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_Drag += this.On_Drag;
		EasyTouch.On_DragStart += this.On_DragStart;
		EasyTouch.On_DragEnd += this.On_DragEnd;
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
		EasyTouch.On_Drag -= this.On_Drag;
		EasyTouch.On_DragStart -= this.On_DragStart;
		EasyTouch.On_DragEnd -= this.On_DragEnd;
	}

	private void Start()
	{
		this.textMesh = (base.transform.Find("TextDrag").transform.gameObject.GetComponent("TextMesh") as TextMesh);
	}

	private void On_DragStart(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.gameObject.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.deltaPosition = touchToWordlPoint - base.transform.position;
		}
	}

	private void On_Drag(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			base.transform.position = touchToWordlPoint - this.deltaPosition;
			float swipeOrDragAngle = gesture.GetSwipeOrDragAngle();
			this.textMesh.text = gesture.swipe.ToString() + " / angle :" + swipeOrDragAngle.ToString("f2");
		}
	}

	private void On_DragEnd(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.transform.position = new Vector3(3f, 1.8f, -5f);
			base.gameObject.GetComponent<Renderer>().material.color = Color.white;
			this.textMesh.text = "Drag me";
		}
	}

	private TextMesh textMesh;

	private Vector3 deltaPosition;
}
