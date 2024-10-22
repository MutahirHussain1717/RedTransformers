using System;
using UnityEngine;

public class GizmoObject : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = this.GColor;
		Vector3 direction = base.transform.TransformDirection(Vector3.forward) / 2f;
		Gizmos.DrawRay(base.transform.position, direction);
		Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one * this.GSize);
		Gizmos.matrix = matrix;
		Gizmos.DrawCube(Vector3.zero, Vector3.one * this.GSize);
	}

	public Color GColor = Color.white;

	public float GSize = 1f;
}
