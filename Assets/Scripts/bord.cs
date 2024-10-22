using System;
using UnityEngine;

[Serializable]
public class bord : MonoBehaviour
{
	public virtual void Start()
	{
		this.cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (this.type == 1)
		{
			this.transform.position = this.cam.ScreenToWorldPoint(new Vector3((float)0, (float)0, (float)8));
		}
		else if (this.type == 2)
		{
			this.transform.position = this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)0, (float)8));
		}
	}

	public virtual void Main()
	{
	}

	public int type;

	private Camera cam;
}
