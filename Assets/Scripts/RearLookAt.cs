using System;
using UnityEngine;

public class RearLookAt : MonoBehaviour
{
	private void Update()
	{
		Vector3 a = base.transform.localPosition - this.target.localPosition;
		Quaternion localRotation = Quaternion.LookRotation(a + this.position);
		base.transform.localRotation = localRotation;
	}

	public Transform target;

	public Vector3 position;
}
