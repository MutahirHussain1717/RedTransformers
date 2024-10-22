using System;
using UnityEngine;

public class Damage : DamageBase
{
	private void Start()
	{
		if (!this.Owner || !this.Owner.GetComponent<Collider>())
		{
			return;
		}
		Physics.IgnoreCollision(base.GetComponent<Collider>(), this.Owner.GetComponent<Collider>());
		this.timetemp = Time.time;
	}

	private void Update()
	{
		if (!this.HitedActive && Time.time >= this.timetemp + this.TimeActive)
		{
			this.Active();
		}
	}

	public void Active()
	{
		if (this.Effect)
		{
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.Effect, base.transform.position, base.transform.rotation);
			UnityEngine.Object.Destroy(obj, 3f);
		}
		if (this.Explosive)
		{
			this.ExplosionDamage();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void ExplosionDamage()
	{
		foreach (Collider collider in Physics.OverlapSphere(base.transform.position, this.ExplosionRadius))
		{
			if (collider)
			{
				if (collider.gameObject.GetComponent<DamageManager>() && collider.gameObject.GetComponent<DamageManager>())
				{
					collider.gameObject.GetComponent<DamageManager>().ApplyDamage(this.Damage);
				}
				if (collider.GetComponent<Rigidbody>())
				{
					collider.GetComponent<Rigidbody>().AddExplosionForce(this.ExplosionForce, base.transform.position, this.ExplosionRadius, 3f);
				}
			}
		}
	}

	private void NormalDamage(Collision collision)
	{
		if (collision.gameObject.GetComponent<DamageManager>())
		{
			MonoBehaviour.print("hitiactive-" + collision.gameObject.name);
			collision.gameObject.GetComponent<DamageManager>().ApplyDamage(this.Damage);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (this.HitedActive && collision.gameObject.tag != "Particle" && collision.gameObject.tag != base.gameObject.tag)
		{
			this.NormalDamage(collision);
			this.Active();
		}
	}

	public bool Explosive;

	public float ExplosionRadius = 20f;

	public float ExplosionForce = 1000f;

	public bool HitedActive = true;

	public float TimeActive;

	private float timetemp;
}
