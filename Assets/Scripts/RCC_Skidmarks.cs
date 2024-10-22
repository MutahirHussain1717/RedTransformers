using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Skidmarks")]
public class RCC_Skidmarks : MonoBehaviour
{
	private void Start()
	{
		if (base.transform.position != Vector3.zero)
		{
			base.transform.position = Vector3.zero;
		}
	}

	private void Awake()
	{
		this.skidmarks = new RCC_Skidmarks.markSection[this.maxMarks];
		for (int i = 0; i < this.maxMarks; i++)
		{
			this.skidmarks[i] = new RCC_Skidmarks.markSection();
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
		RCC_Skidmarks.markSection markSection = this.skidmarks[this.numMarks % this.maxMarks];
		markSection.pos = pos + normal * this.groundOffset;
		markSection.normal = normal;
		markSection.intensity = intensity;
		markSection.lastIndex = lastIndex;
		if (lastIndex != -1)
		{
			RCC_Skidmarks.markSection markSection2 = this.skidmarks[lastIndex % this.maxMarks];
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
		Vector3[] array = new Vector3[num * 4];
		Vector3[] array2 = new Vector3[num * 4];
		Vector4[] array3 = new Vector4[num * 4];
		Color[] array4 = new Color[num * 4];
		Vector2[] array5 = new Vector2[num * 4];
		int[] array6 = new int[num * 6];
		num = 0;
		int num3 = 0;
		while (num3 < this.numMarks && num3 < this.maxMarks)
		{
			if (this.skidmarks[num3].lastIndex != -1 && this.skidmarks[num3].lastIndex > this.numMarks - this.maxMarks)
			{
				RCC_Skidmarks.markSection markSection = this.skidmarks[num3];
				RCC_Skidmarks.markSection markSection2 = this.skidmarks[markSection.lastIndex % this.maxMarks];
				array[num * 4] = markSection2.posl;
				array[num * 4 + 1] = markSection2.posr;
				array[num * 4 + 2] = markSection.posl;
				array[num * 4 + 3] = markSection.posr;
				array2[num * 4] = markSection2.normal;
				array2[num * 4 + 1] = markSection2.normal;
				array2[num * 4 + 2] = markSection.normal;
				array2[num * 4 + 3] = markSection.normal;
				array3[num * 4] = markSection2.tangent;
				array3[num * 4 + 1] = markSection2.tangent;
				array3[num * 4 + 2] = markSection.tangent;
				array3[num * 4 + 3] = markSection.tangent;
				array4[num * 4] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 1] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 2] = new Color(0f, 0f, 0f, markSection.intensity);
				array4[num * 4 + 3] = new Color(0f, 0f, 0f, markSection.intensity);
				array5[num * 4] = new Vector2(0f, 0f);
				array5[num * 4 + 1] = new Vector2(1f, 0f);
				array5[num * 4 + 2] = new Vector2(0f, 1f);
				array5[num * 4 + 3] = new Vector2(1f, 1f);
				array6[num * 6] = num * 4;
				array6[num * 6 + 2] = num * 4 + 1;
				array6[num * 6 + 1] = num * 4 + 2;
				array6[num * 6 + 3] = num * 4 + 2;
				array6[num * 6 + 5] = num * 4 + 1;
				array6[num * 6 + 4] = num * 4 + 3;
				num++;
			}
			num3++;
		}
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.tangents = array3;
		mesh.triangles = array6;
		mesh.colors = array4;
		mesh.uv = array5;
	}

	public int maxMarks = 1024;

	public float markWidth = 0.275f;

	public float groundOffset = 0.02f;

	public float minDistance = 0.1f;

	private int indexShift;

	private int numMarks;

	private RCC_Skidmarks.markSection[] skidmarks;

	private bool updated;

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
