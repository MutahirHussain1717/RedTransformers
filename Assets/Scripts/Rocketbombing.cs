using System;
using UnityEngine;

public class Rocketbombing : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void onfirerocket()
	{
		this.player.PlayOneShot(this.rocketsound);
		Rigidbody rigidbody = UnityEngine.Object.Instantiate<Rigidbody>(this.bullet, this.muzzlePoint.position, this.muzzlePoint.rotation);
		rigidbody.velocity = this.muzzlePoint.forward * this.speed;
	}

	public Rigidbody bullet;

	public Transform muzzlePoint;

	public float speed = 10f;

	public AudioClip rocketsound;

	public AudioSource player;
}
