using System;
using System.Collections;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.name == "Waypoint")
				{
					Gizmos.color = this.color2;
					Gizmos.DrawWireSphere(transform.position, this.radius);
				}
				else if (transform.name == "WaypointStart" || transform.name == "WaypointEnd")
				{
					Gizmos.color = this.color1;
					Gizmos.DrawWireCube(transform.position, this.size);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (this.drawStraight)
		{
			this.DrawStraight();
		}
		if (this.drawCurved)
		{
			this.DrawCurved();
		}
	}

	private void DrawStraight()
	{
		iTween.DrawLine(this.waypoints, this.color2);
	}

	private void DrawCurved()
	{
		if (this.waypoints.Length < 2)
		{
			return;
		}
		this.points = new Vector3[this.waypoints.Length + 2];
		for (int i = 0; i < this.waypoints.Length; i++)
		{
			this.points[i + 1] = this.waypoints[i].position;
		}
		this.points[0] = this.points[1];
		this.points[this.points.Length - 1] = this.points[this.points.Length - 2];
		Gizmos.color = this.color3;
		int num = this.points.Length * 10;
		Vector3[] array = new Vector3[num + 1];
		for (int j = 0; j <= num; j++)
		{
			float t = (float)j / (float)num;
			Vector3 vector = this.GetPoint(t);
			array[j] = vector;
		}
		Vector3 to = array[0];
		for (int k = 1; k < array.Length; k++)
		{
			Vector3 vector = array[k];
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
	}

	private Vector3 GetPoint(float t)
	{
		int num = this.points.Length - 3;
		int num2 = (int)Math.Floor((double)(t * (float)num));
		int num3 = num - 1;
		if (num3 > num2)
		{
			num3 = num2;
		}
		float num4 = t * (float)num - (float)num3;
		Vector3 a = this.points[num3];
		Vector3 a2 = this.points[num3 + 1];
		Vector3 vector = this.points[num3 + 2];
		Vector3 b = this.points[num3 + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num4 * num4 * num4) + (2f * a - 5f * a2 + 4f * vector - b) * (num4 * num4) + (-a + vector) * num4 + 2f * a2);
	}

	public Transform[] waypoints;

	public bool drawStraight = true;

	public bool drawCurved = true;

	public Color color1 = new Color(1f, 0f, 1f, 0.5f);

	public Color color2 = new Color(1f, 0.921568632f, 0.0156862754f, 0.5f);

	public Color color3 = new Color(1f, 0.921568632f, 0.0156862754f, 0.5f);

	private float radius = 0.4f;

	private Vector3 size = new Vector3(0.7f, 0.7f, 0.7f);

	public GameObject waypointPrefab;

	private Vector3[] points;
}
