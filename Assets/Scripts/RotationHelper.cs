using System;
using UnityEngine;

public class RotationHelper : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(Vector3.right * (this.multiplier * 10f) * Time.deltaTime);
	}

	public float multiplier = 1f;
}
