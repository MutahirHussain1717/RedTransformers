using System;
using UnityEngine;

public class Pinch : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_TouchStart2Fingers += this.On_TouchStart2Fingers;
		EasyTouch.On_PinchIn += this.On_PinchIn;
		EasyTouch.On_PinchOut += this.On_PinchOut;
		EasyTouch.On_PinchEnd += this.On_PinchEnd;
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
		EasyTouch.On_TouchStart2Fingers -= this.On_TouchStart2Fingers;
		EasyTouch.On_PinchIn -= this.On_PinchIn;
		EasyTouch.On_PinchOut -= this.On_PinchOut;
		EasyTouch.On_PinchEnd -= this.On_PinchEnd;
		EasyTouch.On_Cancel2Fingers -= this.On_Cancel2Fingers;
	}

	private void Start()
	{
		this.textMesh = (base.transform.Find("TextPinch").transform.gameObject.GetComponent("TextMesh") as TextMesh);
	}

	private void On_TouchStart2Fingers(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			EasyTouch.SetEnableTwist(false);
			EasyTouch.SetEnablePinch(true);
		}
	}

	private void On_PinchIn(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			float num = Time.deltaTime * gesture.deltaPinch;
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(localScale.x - num, localScale.y - num, localScale.z - num);
			this.textMesh.text = "Delta pinch : " + gesture.deltaPinch.ToString();
		}
	}

	private void On_PinchOut(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			float num = Time.deltaTime * gesture.deltaPinch;
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(localScale.x + num, localScale.y + num, localScale.z + num);
			this.textMesh.text = "Delta pinch : " + gesture.deltaPinch.ToString();
		}
	}

	private void On_PinchEnd(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
			EasyTouch.SetEnableTwist(true);
			this.textMesh.text = "Pinch me";
		}
	}

	private void On_Cancel2Fingers(Gesture gesture)
	{
	}

	private TextMesh textMesh;
}
