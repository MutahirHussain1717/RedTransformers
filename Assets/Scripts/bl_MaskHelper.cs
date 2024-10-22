using System;
using UnityEngine;
using UnityEngine.UI;

public class bl_MaskHelper : MonoBehaviour
{
	private void Start()
	{
		this.m_image.texture = this.MiniMapMask;
	}

	private RawImage m_image
	{
		get
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<RawImage>();
			}
			return this._image;
		}
	}

	public void OnChange(bool full = false)
	{
		if (full)
		{
			this.m_image.texture = this.WorldMapMask;
			if (this.Background != null)
			{
				this.Background.sprite = this.WorldMapBackGround;
			}
		}
		else
		{
			this.m_image.texture = this.MiniMapMask;
			if (this.Background != null)
			{
				this.Background.sprite = this.MiniMapBackGround;
			}
		}
	}

	public Texture2D MiniMapMask;

	public Texture2D WorldMapMask;

	[Space(5f)]
	public Image Background;

	public Sprite MiniMapBackGround;

	public Sprite WorldMapBackGround;

	private RawImage _image;
}
