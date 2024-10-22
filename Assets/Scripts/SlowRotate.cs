using System;
using UnityEngine;

public class SlowRotate : MonoBehaviour
{
	private void Start()
	{
		this.rotateSpeed = UnityEngine.Random.Range(-60f, 60f);
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, this.rotateSpeed) * Time.deltaTime);
	}

	public float rotateSpeed;
}
