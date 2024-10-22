using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEventSystemHandler
{
	private void Awake()
	{
		this.direction = Vector2.zero;
		this.touched = false;
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!this.touched)
		{
			this.touched = true;
			this.pointerId = data.pointerId;
			this.origin = data.position;
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if (this.pointerId == data.pointerId)
		{
			this.direction = Vector2.zero;
			this.touched = false;
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (this.pointerId == data.pointerId)
		{
			Vector2 position = data.position;
			this.direction = (position - this.origin).normalized;
			UnityEngine.Debug.Log(this.direction);
		}
	}

	public Vector2 GetDirection()
	{
		this.smoothDirection = Vector2.MoveTowards(this.smoothDirection, this.direction, this.smoothing);
		return this.smoothDirection;
	}

	public float smoothing;

	private Vector2 origin;

	private Vector2 direction;

	private Vector2 smoothDirection;

	private bool touched;

	private int pointerId;
}
