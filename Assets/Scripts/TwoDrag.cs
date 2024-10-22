using System;
using UnityEngine;

public class TwoDrag : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_DragStart2Fingers += this.On_DragStart2Fingers;
		EasyTouch.On_Drag2Fingers += this.On_Drag2Fingers;
		EasyTouch.On_DragEnd2Fingers += this.On_DragEnd2Fingers;
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
		EasyTouch.On_DragStart2Fingers -= this.On_DragStart2Fingers;
		EasyTouch.On_Drag2Fingers -= this.On_Drag2Fingers;
		EasyTouch.On_DragEnd2Fingers -= this.On_DragEnd2Fingers;
		EasyTouch.On_Cancel2Fingers -= this.On_Cancel2Fingers;
	}

	private void Start()
	{
		this.textMesh = (base.transform.Find("TextDrag").transform.gameObject.GetComponent("TextMesh") as TextMesh);
	}

	private void On_DragStart2Fingers(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.gameObject.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			this.deltaPosition = touchToWordlPoint - base.transform.position;
		}
	}

	private void On_Drag2Fingers(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(5f, false);
			base.transform.position = touchToWordlPoint - this.deltaPosition;
			float swipeOrDragAngle = gesture.GetSwipeOrDragAngle();
			this.textMesh.text = gesture.swipe.ToString() + " / angle :" + swipeOrDragAngle.ToString("f2");
		}
	}

	private void On_DragEnd2Fingers(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.transform.position = new Vector3(2.5f, -0.5f, -5f);
			base.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
			this.textMesh.text = "Drag me";
		}
	}

	private void On_Cancel2Fingers(Gesture gesture)
	{
		base.transform.position = new Vector3(2.5f, -0.5f, -5f);
		base.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
		this.textMesh.text = "Drag me";
	}

	private TextMesh textMesh;

	private Vector3 deltaPosition;
}
