using System;
using UnityEngine;

public class VirtualScreen : MonoSingleton<VirtualScreen>
{
	public static event VirtualScreen.On_ScreenResizeHandler On_ScreenResize;

	private void Awake()
	{
		this.realWidth = (this.oldRealWidth = (float)Screen.width);
		this.realHeight = (this.oldRealHeight = (float)Screen.height);
		this.ComputeScreen();
	}

	private void Update()
	{
		this.realWidth = (float)Screen.width;
		this.realHeight = (float)Screen.height;
		if (this.realWidth != this.oldRealWidth || this.realHeight != this.oldRealHeight)
		{
			this.ComputeScreen();
			if (VirtualScreen.On_ScreenResize != null)
			{
				VirtualScreen.On_ScreenResize();
			}
		}
		this.oldRealWidth = this.realWidth;
		this.oldRealHeight = this.realHeight;
	}

	public void ComputeScreen()
	{
		VirtualScreen.width = this.virtualWidth;
		VirtualScreen.height = this.virtualHeight;
		VirtualScreen.xRatio = 1f;
		VirtualScreen.yRatio = 1f;
		float num;
		float num2;
		if (Screen.width > Screen.height)
		{
			num = (float)Screen.width / (float)Screen.height;
			num2 = VirtualScreen.width;
		}
		else
		{
			num = (float)Screen.height / (float)Screen.width;
			num2 = VirtualScreen.height;
		}
		float num3 = num2 / num;
		if (Screen.width > Screen.height)
		{
			VirtualScreen.height = num3;
			VirtualScreen.xRatio = (float)Screen.width / VirtualScreen.width;
			VirtualScreen.yRatio = (float)Screen.height / VirtualScreen.height;
		}
		else
		{
			VirtualScreen.width = num3;
			VirtualScreen.xRatio = (float)Screen.width / VirtualScreen.width;
			VirtualScreen.yRatio = (float)Screen.height / VirtualScreen.height;
		}
	}

	public static void ComputeVirtualScreen()
	{
		MonoSingleton<VirtualScreen>.instance.ComputeScreen();
	}

	public static void SetGuiScaleMatrix()
	{
		GUI.matrix = Matrix4x4.Scale(new Vector3(VirtualScreen.xRatio, VirtualScreen.yRatio, 1f));
	}

	public static Rect GetRealRect(Rect rect)
	{
		return new Rect(rect.x * VirtualScreen.xRatio, rect.y * VirtualScreen.yRatio, rect.width * VirtualScreen.xRatio, rect.height * VirtualScreen.yRatio);
	}

	public float virtualWidth = 1024f;

	public float virtualHeight = 768f;

	public static float width = 1024f;

	public static float height = 768f;

	public static float xRatio = 1f;

	public static float yRatio = 1f;

	private float realWidth;

	private float realHeight;

	private float oldRealWidth;

	private float oldRealHeight;

	public delegate void On_ScreenResizeHandler();

	public enum ScreenResolution
	{
		IPhoneTall,
		IPhoneWide,
		IPhone4GTall,
		IPhone4GWide,
		IPadTall,
		IPadWide
	}
}
