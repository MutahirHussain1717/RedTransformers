using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - this.startTime < 2f)
		{
			base.transform.Translate(new Vector3(0f, 0f, 1f) * 0.8f * Time.deltaTime);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private float startTime;
}
