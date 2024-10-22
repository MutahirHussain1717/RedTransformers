using System;
using UnityEngine;

public class bl_WorldSpace : MonoBehaviour
{
	private void OnDrawGizmosSelected()
	{
		RectTransform component = base.GetComponent<RectTransform>();
		Vector3 vector = component.sizeDelta;
		Vector3 localPosition = component.localPosition;
		Gizmos.color = this.GizmoColor;
		Gizmos.DrawCube(localPosition, new Vector3(vector.x, 2f, vector.y));
		Gizmos.DrawWireCube(localPosition, new Vector3(vector.x, 2f, vector.y));
	}

	[Header("Use UI editor Tool for scale the wordSpace")]
	public Color GizmoColor = new Color(1f, 1f, 1f, 0.75f);
}
