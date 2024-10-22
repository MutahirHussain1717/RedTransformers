using System;
using UnityEngine;

public class Lookat : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.LookAt(this.player.transform);
	}

	public GameObject player;
}
