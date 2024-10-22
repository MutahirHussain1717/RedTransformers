using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	private void Start()
	{
		Vector3 position = base.transform.position;
		Collider[] array = Physics.OverlapSphere(position, (float)this.Radius);
		if (this.Sounds.Length > 0)
		{
			AudioSource.PlayClipAtPoint(this.Sounds[UnityEngine.Random.Range(0, this.Sounds.Length)], base.transform.position);
		}
		foreach (Collider collider in array)
		{
			if (collider.GetComponent<Rigidbody>())
			{
				collider.GetComponent<Rigidbody>().AddExplosionForce((float)this.Force, position, (float)this.Radius, 3f);
			}
		}
	}

	public int Force;

	public int Radius;

	public AudioClip[] Sounds;
}
