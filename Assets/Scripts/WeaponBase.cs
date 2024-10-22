using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
	[HideInInspector]
	public GameObject Owner;

	[HideInInspector]
	public GameObject Target;

	public string[] TargetTag = new string[]
	{
		"Enemy"
	};

	public bool RigidbodyProjectile;

	public Vector3 TorqueSpeedAxis;

	public GameObject TorqueObject;
}
