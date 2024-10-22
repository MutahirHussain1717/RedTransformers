using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/Waypoints Container")]
public class RCC_AIWaypointsContainer : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		for (int i = 0; i < this.waypoints.Count; i++)
		{
			Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
			Gizmos.DrawSphere(this.waypoints[i].transform.position, 2f);
			Gizmos.DrawWireSphere(this.waypoints[i].transform.position, 20f);
			if (i < this.waypoints.Count - 1 && this.waypoints[i] && this.waypoints[i + 1] && this.waypoints.Count > 0)
			{
				Gizmos.color = Color.green;
				if (i < this.waypoints.Count - 1)
				{
					Gizmos.DrawLine(this.waypoints[i].position, this.waypoints[i + 1].position);
				}
				if (i < this.waypoints.Count - 2)
				{
					Gizmos.DrawLine(this.waypoints[this.waypoints.Count - 1].position, this.waypoints[0].position);
				}
			}
		}
	}

	public List<Transform> waypoints = new List<Transform>();
}
