using System;
using UnityEngine;

public class Tranformationplayer : MonoBehaviour
{
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	private void Update()
	{
	}

	public void play_transformation()
	{
		this.anim.SetBool("start", true);
	}

	public void off_transformation()
	{
		this.anim.SetBool("start", false);
	}

	private Animator anim;
}
