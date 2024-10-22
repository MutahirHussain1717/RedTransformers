using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	private void Start()
	{
		if (base.GetComponent<Renderer>())
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}

	private void Update()
	{
		if (!this.ObjectSpawn)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		if ((float)array.Length < this.ObjectCount && Time.time >= this.timeSpawnTemp + this.TimeSpawn)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ObjectSpawn, base.transform.position + new Vector3((float)UnityEngine.Random.Range(-this.Radiun, this.Radiun), 20f, (float)UnityEngine.Random.Range(-this.Radiun, this.Radiun)), Quaternion.identity);
			float num = (float)UnityEngine.Random.Range(5, 20);
			gameObject.transform.localScale = new Vector3(num, num, num);
			this.timeSpawnTemp = Time.time;
		}
	}

	public GameObject ObjectSpawn;

	private float timeSpawnTemp;

	public float TimeSpawn = 20f;

	public float ObjectCount;

	public int Radiun;
}
