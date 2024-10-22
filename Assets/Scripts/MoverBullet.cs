using System;
using UnityEngine;

public class MoverBullet : WeaponBase
{
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, (float)this.Lifetime);
	}

	private void FixedUpdate()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			return;
		}
		if (!this.RigidbodyProjectile)
		{
			base.GetComponent<Rigidbody>().velocity = base.transform.forward * this.Speed;
		}
		else if (base.GetComponent<Rigidbody>().velocity.normalized != Vector3.zero)
		{
			base.transform.forward = base.GetComponent<Rigidbody>().velocity.normalized;
		}
		if (this.Speed < this.SpeedMax)
		{
			this.Speed += this.SpeedMult * Time.fixedDeltaTime;
		}
	}

	public int Lifetime;

	public float Speed = 80f;

	public float SpeedMax = 80f;

	public float SpeedMult = 1f;
}
