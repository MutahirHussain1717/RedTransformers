using System;
using UnityEngine;
using UnityEngine.UI;

public class bl_IconItem : MonoBehaviour
{
	public void DestroyIcon(bool inmediate)
	{
		if (inmediate)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.TargetGrapihc.sprite = this.DeathIcon;
			UnityEngine.Object.Destroy(base.gameObject, this.DestroyIn);
		}
	}

	public void DestroyIcon(bool inmediate, Sprite death)
	{
		if (inmediate)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.TargetGrapihc.sprite = death;
			UnityEngine.Object.Destroy(base.gameObject, this.DestroyIn);
		}
	}

	public void GetInfoItem(string info)
	{
		if (this.InfoText == null)
		{
			return;
		}
		this.InfoText.text = info;
	}

	public void InfoItem()
	{
		this.open = !this.open;
		Animation component = base.GetComponent<Animation>();
		if (this.open)
		{
			component["OpenInfo"].time = 0f;
			component["OpenInfo"].speed = 1f;
			component.CrossFade("OpenInfo", 0.2f);
		}
		else
		{
			component["OpenInfo"].time = component["OpenInfo"].length;
			component["OpenInfo"].speed = -1f;
			component.CrossFade("OpenInfo", 0.2f);
		}
	}

	public Image TargetGrapihc;

	public Sprite DeathIcon;

	public Text InfoText;

	public float DestroyIn = 5f;

	private bool open;
}
