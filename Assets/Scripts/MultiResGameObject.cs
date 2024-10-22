using System;
using UnityEngine;

public class MultiResGameObject : MonoBehaviour
{
	private void Start()
	{
		if (this.camera == null)
		{
			this.camera = Camera.main;
		}
		if (!this.isBackground)
		{
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			float x = base.transform.localScale.x;
			float y = base.transform.localScale.y;
			float x2 = x * (float)this.DefaultScreenHeight / (float)this.DefaultScreenWidth * num / num2;
			float num3 = y * (float)this.DefaultScreenWidth / (float)this.DefaultScreenHeight * num / num2;
			if (this.MultiresScale)
			{
				base.transform.localScale = new Vector3(x2, base.transform.localScale.y, base.transform.localScale.z);
			}
			if (this.MultiresPosition)
			{
				float x3 = base.transform.localPosition.x * (float)this.DefaultScreenHeight / (float)this.DefaultScreenWidth * num / num2;
				base.transform.localPosition = new Vector3(x3, base.transform.localPosition.y, base.transform.localPosition.z);
			}
		}
		else
		{
			this.ResizeSpriteToScreen();
		}
	}

	public void ResizeSpriteToScreen()
	{
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			return;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		float x = component.sprite.bounds.size.x;
		float y = component.sprite.bounds.size.y;
		float num = this.camera.orthographicSize * 2f;
		float num2 = num / (float)Screen.height * (float)Screen.width;
		base.transform.localScale = new Vector3(num2 / x, num / y, base.transform.localScale.z);
	}

	public bool MultiresPosition = true;

	public bool MultiresScale = true;

	public Camera camera;

	public int DefaultScreenWidth = 800;

	public int DefaultScreenHeight = 480;

	public bool isBackground;

	public bool maintainAspectRation;
}
