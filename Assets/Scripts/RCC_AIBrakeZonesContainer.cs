using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/Brake Zones Container")]
public class RCC_AIBrakeZonesContainer : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		for (int i = 0; i < this.brakeZones.Count; i++)
		{
			Gizmos.matrix = this.brakeZones[i].transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
			Vector3 size = this.brakeZones[i].GetComponent<BoxCollider>().size;
			Gizmos.DrawCube(Vector3.zero, size);
		}
	}

	public List<Transform> brakeZones = new List<Transform>();
}
