using System;
using UnityEngine;

public class animationchecker : MonoBehaviour
{
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	private void Update()
	{
		if (this.speed == 0)
		{
			this.anim.SetFloat("runspeed", 0f);
		}
		else if (this.speed == 1)
		{
			this.anim.SetFloat("runspeed", 1f);
		}
		else if (this.speed == 2)
		{
			this.anim.SetFloat("runspeed", -1f);
		}
	}

	public int speed;

	private Animator anim;
}
