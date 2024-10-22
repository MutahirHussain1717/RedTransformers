using System;
using UnityEngine;

public class Skidmarks : MonoBehaviour
{
	private void Start()
	{
		if (base.transform.position != new Vector3(0f, 0f, 0f))
		{
			base.transform.position = new Vector3(0f, 0f, 0f);
		}
	}

	private void Awake()
	{
		this.skidmarks = new Skidmarks.markSection[this.maxMarks];
		for (int i = 0; i < this.maxMarks; i++)
		{
			this.skidmarks[i] = new Skidmarks.markSection();
		}
		if (base.GetComponent<MeshFilter>().mesh == null)
		{
			base.GetComponent<MeshFilter>().mesh = new Mesh();
		}
	}

	public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex)
	{
		if (intensity > 1f)
		{
			intensity = 1f;
		}
		if (intensity < 0f)
		{
			return -1;
		}
		Skidmarks.markSection markSection = this.skidmarks[this.numMarks % this.maxMarks];
		markSection.pos = pos + normal * this.groundOffset;
		markSection.normal = normal;
		markSection.intensity = intensity;
		markSection.lastIndex = lastIndex;
		if (lastIndex != -1)
		{
			Skidmarks.markSection markSection2 = this.skidmarks[lastIndex % this.maxMarks];
			Vector3 lhs = markSection.pos - markSection2.pos;
			Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
			markSection.posl = markSection.pos + normalized * this.markWidth * 0.5f;
			markSection.posr = markSection.pos - normalized * this.markWidth * 0.5f;
			markSection.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markSection2.lastIndex == -1)
			{
				markSection2.tangent = markSection.tangent;
				markSection2.posl = markSection.pos + normalized * this.markWidth * 0.5f;
				markSection2.posr = markSection.pos - normalized * this.markWidth * 0.5f;
			}
		}
		this.numMarks++;
		this.updated = true;
		return this.numMarks - 1;
	}

	private void LateUpdate()
	{
		WheelCollider[] array = UnityEngine.Object.FindObjectsOfType(typeof(WheelCollider)) as WheelCollider[];
		foreach (WheelCollider wheelCollider in array)
		{
			if (!this.skidmake)
			{
				wheelCollider.gameObject.AddComponent<WheelSkidmarks>();
			}
		}
		this.skidmake = true;
		if (!this.updated)
		{
			return;
		}
		this.updated = false;
		Mesh mesh = base.GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		int num = 0;
		int num2 = 0;
		while (num2 < this.numMarks && num2 < this.maxMarks)
		{
			if (this.skidmarks[num2].lastIndex != -1 && this.skidmarks[num2].lastIndex > this.numMarks - this.maxMarks)
			{
				num++;
			}
			num2++;
		}
		Vector3[] array3 = new Vector3[num * 4];
		Vector3[] array4 = new Vector3[num * 4];
		Vector4[] array5 = new Vector4[num * 4];
		Color[] array6 = new Color[num * 4];
		Vector2[] array7 = new Vector2[num * 4];
		int[] array8 = new int[num * 6];
		num = 0;
		int num3 = 0;
		while (num3 < this.numMarks && num3 < this.maxMarks)
		{
			if (this.skidmarks[num3].lastIndex != -1 && this.skidmarks[num3].lastIndex > this.numMarks - this.maxMarks)
			{
				Skidmarks.markSection markSection = this.skidmarks[num3];
				Skidmarks.markSection markSection2 = this.skidmarks[markSection.lastIndex % this.maxMarks];
				array3[num * 4] = markSection2.posl;
				array3[num * 4 + 1] = markSection2.posr;
				array3[num * 4 + 2] = markSection.posl;
				array3[num * 4 + 3] = markSection.posr;
				array4[num * 4] = markSection2.normal;
				array4[num * 4 + 1] = markSection2.normal;
				array4[num * 4 + 2] = markSection.normal;
				array4[num * 4 + 3] = markSection.normal;
				array5[num * 4] = markSection2.tangent;
				array5[num * 4 + 1] = markSection2.tangent;
				array5[num * 4 + 2] = markSection.tangent;
				array5[num * 4 + 3] = markSection.tangent;
				array6[num * 4] = new Color(0f, 0f, 0f, markSection2.intensity);
				array6[num * 4 + 1] = new Color(0f, 0f, 0f, markSection2.intensity);
				array6[num * 4 + 2] = new Color(0f, 0f, 0f, markSection.intensity);
				array6[num * 4 + 3] = new Color(0f, 0f, 0f, markSection.intensity);
				array7[num * 4] = new Vector2(0f, 0f);
				array7[num * 4 + 1] = new Vector2(1f, 0f);
				array7[num * 4 + 2] = new Vector2(0f, 1f);
				array7[num * 4 + 3] = new Vector2(1f, 1f);
				array8[num * 6] = num * 4;
				array8[num * 6 + 2] = num * 4 + 1;
				array8[num * 6 + 1] = num * 4 + 2;
				array8[num * 6 + 3] = num * 4 + 2;
				array8[num * 6 + 5] = num * 4 + 1;
				array8[num * 6 + 4] = num * 4 + 3;
				num++;
			}
			num3++;
		}
		mesh.vertices = array3;
		mesh.normals = array4;
		mesh.tangents = array5;
		mesh.triangles = array8;
		mesh.colors = array6;
		mesh.uv = array7;
	}

	public int maxMarks = 1024;

	public float markWidth = 0.275f;

	public float groundOffset = 0.02f;

	public float minDistance = 0.1f;

	private int indexShift;

	private int numMarks;

	private Skidmarks.markSection[] skidmarks;

	private bool updated;

	public bool skidmake;

	private class markSection
	{
		public Vector3 pos = Vector3.zero;

		public Vector3 normal = Vector3.zero;

		public Vector4 tangent = Vector4.zero;

		public Vector3 posl = Vector3.zero;

		public Vector3 posr = Vector3.zero;

		public float intensity;

		public int lastIndex;
	}
}
