using System;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
	private void Start()
	{
		if (this.Sounds.Length > 0)
		{
			AudioSource.PlayClipAtPoint(this.Sounds[UnityEngine.Random.Range(0, this.Sounds.Length)], base.transform.position);
		}
		if (this.Prefab)
		{
			for (int i = 0; i < this.Num; i++)
			{
				Vector3 b = new Vector3((float)UnityEngine.Random.Range(-10, 10), (float)UnityEngine.Random.Range(-10, 20), (float)UnityEngine.Random.Range(-10, 10)) / 10f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab, base.transform.position + b, base.transform.rotation);
				UnityEngine.Object.Destroy(gameObject, this.LifeTimeObject);
				float num = (float)this.Scale;
				if (this.RandomScale)
				{
					num = (float)UnityEngine.Random.Range(1, this.Scale);
				}
				if (num > 0f)
				{
					gameObject.transform.localScale = new Vector3(num, num, num);
				}
				if (gameObject.GetComponent<Rigidbody>())
				{
					Vector3 force = new Vector3(UnityEngine.Random.Range(-this.Force.x, this.Force.x), UnityEngine.Random.Range(-this.Force.y, this.Force.y), UnityEngine.Random.Range(-this.Force.z, this.Force.z));
					gameObject.GetComponent<Rigidbody>().AddForce(force);
				}
			}
		}
	}

	public Vector3 Force;

	public GameObject Prefab;

	public int Num;

	public int Scale = 20;

	public AudioClip[] Sounds;

	public float LifeTimeObject = 2f;

	public bool RandomScale;
}
