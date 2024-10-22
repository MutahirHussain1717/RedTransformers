using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Fixed System/Child Camera")]
public class RCC_ChildFixedCamera : MonoBehaviour
{
	private void Update()
	{
		if (!this.player)
		{
			return;
		}
		base.transform.LookAt(new Vector3(this.player.position.x, this.player.position.y, this.player.position.z));
	}

	[HideInInspector]
	public Transform player;

	public float distance = 50f;
}
