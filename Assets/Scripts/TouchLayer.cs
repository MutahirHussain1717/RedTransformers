using System;
using UnityEngine;

public class TouchLayer : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_TouchStart += this.On_TouchStart;
	}

	private void OnDisable()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
	}

	private void OnDestroy()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
	}

	private void Start()
	{
		this.textMesh = (TextMesh)GameObject.Find("TouchOnLayer").transform.gameObject.GetComponent("TextMesh");
		EasyTouch.AddReservedGuiArea(this.rect1);
		EasyTouch.AddReservedGuiArea(this.rect2);
	}

	private void OnGUI()
	{
		GUI.Box(this.rect1, "Reserved area");
		GUI.Box(this.rect2, "Reserved area");
	}

	public void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject != null && !gesture.isHoverReservedArea)
		{
			gesture.pickObject.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
			if (gesture.pickCamera == null)
			{
				this.textMesh.text = "Touch on layer :" + LayerMask.LayerToName(gesture.pickObject.layer);
			}
			else
			{
				this.textMesh.text = "Touch on layer :" + LayerMask.LayerToName(gesture.pickObject.layer) + " / Camera : " + gesture.pickCamera.name;
			}
		}
		else if (gesture.isHoverReservedArea)
		{
			this.textMesh.text = "You touch a reserved area";
		}
		else
		{
			this.textMesh.text = "Yout touch a free zone";
		}
	}

	private TextMesh textMesh;

	private Rect rect1 = new Rect(0f, 50f, 200f, 200f);

	private Rect rect2 = new Rect(300f, 50f, 200f, 135f);
}
