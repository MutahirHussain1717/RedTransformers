using System;
using UnityEngine;

[ExecuteInEditMode]
public class UniGifImageAspectController : MonoBehaviour
{
	public RectTransform rectTransform
	{
		get
		{
			return (!(this.m_rectTransform != null)) ? (this.m_rectTransform = base.GetComponent<RectTransform>()) : this.m_rectTransform;
		}
	}

	private void Update()
	{
		if (this.m_fixOnUpdate)
		{
			this.FixAspectRatio(-1, -1);
		}
	}

	public void FixAspectRatio(int originalWidth = -1, int originalHeight = -1)
	{
		bool flag = false;
		if (originalWidth > 0 && originalHeight > 0)
		{
			this.m_originalWidth = originalWidth;
			this.m_originalHeight = originalHeight;
			flag = true;
		}
		if (this.m_originalWidth <= 0 || this.m_originalHeight <= 0)
		{
			return;
		}
		bool flag2;
		if (flag || this.m_lastSize.x != this.rectTransform.sizeDelta.x)
		{
			flag2 = true;
		}
		else
		{
			if (this.m_lastSize.y == this.rectTransform.sizeDelta.y)
			{
				return;
			}
			flag2 = false;
		}
		if (flag2)
		{
			float num = this.rectTransform.sizeDelta.x / (float)this.m_originalWidth;
			this.m_newSize.Set(this.rectTransform.sizeDelta.x, (float)this.m_originalHeight * num);
		}
		else
		{
			float num2 = this.rectTransform.sizeDelta.y / (float)this.m_originalHeight;
			this.m_newSize.Set((float)this.m_originalWidth * num2, this.rectTransform.sizeDelta.y);
		}
		this.m_newSize.Set(base.gameObject.GetComponent<RectTransform>().sizeDelta.x, base.gameObject.GetComponent<RectTransform>().sizeDelta.y);
		this.rectTransform.sizeDelta = this.m_newSize;
		this.m_lastSize = this.rectTransform.sizeDelta;
	}

	public int m_originalWidth;

	public int m_originalHeight;

	public bool m_fixOnUpdate;

	private Vector2 m_lastSize = Vector2.zero;

	private Vector2 m_newSize = Vector2.zero;

	private RectTransform m_rectTransform;
}
