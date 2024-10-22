using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemRenderer))]
public class WFX_ParticleMeshBillboard : MonoBehaviour
{
	private void Awake()
	{
		this.mesh = UnityEngine.Object.Instantiate<Mesh>(base.GetComponent<ParticleSystemRenderer>().mesh);
		base.GetComponent<ParticleSystemRenderer>().mesh = this.mesh;
		this.vertices = new Vector3[this.mesh.vertices.Length];
		for (int i = 0; i < this.vertices.Length; i++)
		{
			this.vertices[i] = this.mesh.vertices[i];
		}
		this.rvertices = new Vector3[this.vertices.Length];
	}

	private void OnWillRenderObject()
	{
		if (this.mesh == null || Camera.current == null)
		{
			return;
		}
		Quaternion rotation = Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up);
		Quaternion rotation2 = Quaternion.Inverse(base.transform.rotation);
		for (int i = 0; i < this.rvertices.Length; i++)
		{
			this.rvertices[i] = rotation * this.vertices[i];
			this.rvertices[i] = rotation2 * this.rvertices[i];
		}
		this.mesh.vertices = this.rvertices;
	}

	private Mesh mesh;

	private Vector3[] vertices;

	private Vector3[] rvertices;
}
