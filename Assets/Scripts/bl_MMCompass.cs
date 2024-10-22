using System;
using UnityEngine;

public class bl_MMCompass : MonoBehaviour
{
	private void Update()
	{
		if (this.Target != null)
		{
			this.Opposite = (int)Mathf.Abs(this.Target.eulerAngles.y);
		}
		else
		{
			this.Opposite = (int)Mathf.Abs(this.m_Transform.eulerAngles.y);
		}
		if (this.Opposite > 360)
		{
			this.Opposite %= 360;
		}
		this.Grade = this.Opposite;
		if (this.Grade > 180)
		{
			this.Grade -= 360;
		}
		this.North.anchoredPosition = new Vector2(this.CompassRoot.sizeDelta.x * 0.5f - (float)(this.Grade * 2) - this.CompassRoot.sizeDelta.x * 0.5f, 0f);
		this.South.anchoredPosition = new Vector2(this.CompassRoot.sizeDelta.x * 0.5f - (float)(this.Opposite * 2) + 360f - this.CompassRoot.sizeDelta.x * 0.5f, 0f);
		this.East.anchoredPosition = new Vector2(this.CompassRoot.sizeDelta.x * 0.5f - (float)(this.Grade * 2) + 180f - this.CompassRoot.sizeDelta.x * 0.5f, 0f);
		this.West.anchoredPosition = new Vector2(this.CompassRoot.sizeDelta.x * 0.5f - (float)(this.Opposite * 2) + 540f - this.CompassRoot.sizeDelta.x * 0.5f, 0f);
	}

	private Transform m_Transform
	{
		get
		{
			if (this.t == null)
			{
				this.t = base.GetComponent<Transform>();
			}
			return this.t;
		}
	}

	public void setmapplayer_obj(GameObject setplayer)
	{
		this.Target = setplayer.transform;
	}

	public Transform Target;

	[Space(7f)]
	public RectTransform CompassRoot;

	public RectTransform North;

	public RectTransform South;

	public RectTransform East;

	public RectTransform West;

	private int Opposite;

	public int Grade;

	private Transform t;
}
