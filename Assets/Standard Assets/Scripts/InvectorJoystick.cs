using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class InvectorJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
	private void Start()
	{
		this.m_StartPos = base.transform.position;
		this.CreateVirtualAxes();
	}

	private void UpdateVirtualAxes(Vector3 value)
	{
		Vector3 a = this.m_StartPos - value;
		a.y = -a.y;
		a /= (float)this.MovementRange;
		if (this.m_UseX)
		{
			this.m_HorizontalVirtualAxis.Update(-a.x);
		}
		if (this.m_UseY)
		{
			this.m_VerticalVirtualAxis.Update(a.y);
		}
	}

	private void CreateVirtualAxes()
	{
		this.m_UseX = (this.axesToUse == InvectorJoystick.AxisOption.Both || this.axesToUse == InvectorJoystick.AxisOption.OnlyHorizontal);
		this.m_UseY = (this.axesToUse == InvectorJoystick.AxisOption.Both || this.axesToUse == InvectorJoystick.AxisOption.OnlyVertical);
		if (this.m_UseX)
		{
			this.m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(this.m_HorizontalVirtualAxis);
		}
		if (this.m_UseY)
		{
			this.m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(this.m_VerticalVirtualAxis);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		InvectorJoystick.playWalkingSoundCheck = true;
		Vector3 zero = Vector3.zero;
		if (this.m_UseX)
		{
			int num = (int)(data.position.x - this.m_StartPos.x);
			zero.x = (float)num;
		}
		if (this.m_UseY)
		{
			int num2 = (int)(data.position.y - this.m_StartPos.y);
			zero.y = (float)num2;
		}
		base.transform.position = Vector3.ClampMagnitude(new Vector3(zero.x, zero.y, zero.z), (float)this.MovementRange) + this.m_StartPos;
		this.UpdateVirtualAxes(base.transform.position);
	}

	public void OnPointerUp(PointerEventData data)
	{
		InvectorJoystick.playWalkingSoundCheck = false;
		base.transform.position = this.m_StartPos;
		this.UpdateVirtualAxes(this.m_StartPos);
	}

	public void OnPointerDown(PointerEventData data)
	{
	}

	private void OnDisable()
	{
		if (this.m_UseX)
		{
			this.m_HorizontalVirtualAxis.Remove();
		}
		if (this.m_UseY)
		{
			this.m_VerticalVirtualAxis.Remove();
		}
	}

	public static bool playWalkingSoundCheck;

	public int MovementRange = 100;

	public InvectorJoystick.AxisOption axesToUse;

	public string horizontalAxisName = "Horizontal";

	public string verticalAxisName = "Vertical";

	private Vector3 m_StartPos;

	private bool m_UseX;

	private bool m_UseY;

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	public enum AxisOption
	{
		Both,
		OnlyHorizontal,
		OnlyVertical
	}
}
