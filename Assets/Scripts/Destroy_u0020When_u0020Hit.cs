using System;
using UnityEngine;

[Serializable]
public class Destroy : MonoBehaviour
{
	public virtual void OnCollisionEnter(Collision collision)
	{
		ContactPoint contactPoint = collision.contacts[0];
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
		Vector3 point = contactPoint.point;
		UnityEngine.Object.Instantiate<Transform>(this.explosionPrefab, point, rotation);
		UnityEngine.Object.Destroy(this.gameObject, (float)0);
	}

	public virtual void Main()
	{
	}

	public Transform explosionPrefab;
}
