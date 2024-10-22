using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Button")]
public class RCC_UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private float sensitivity
	{
		get
		{
			return RCC_Settings.Instance.UIButtonSensitivity;
		}
	}

	private float gravity
	{
		get
		{
			return RCC_Settings.Instance.UIButtonGravity;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressing = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.pressing = false;
	}

	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			this.pressing = true;
		}
		else
		{
			this.pressing = false;
		}
	}

	private void Update()
	{
		if (this.pressing)
		{
			this.input += Time.deltaTime * this.sensitivity;
		}
		else
		{
			this.input -= Time.deltaTime * this.gravity;
		}
		if (this.input < 0f)
		{
			this.input = 0f;
		}
		if (this.input > 1f)
		{
			this.input = 1f;
		}
	}

	private void OnDisable()
	{
		this.input = 0f;
		this.pressing = false;
	}

	internal float input;

	public bool pressing;
}
