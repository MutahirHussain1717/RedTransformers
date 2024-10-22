using System;
using UnityEngine;

public class RCC_LockRot : MonoBehaviour
{
	private void Awake()
	{
		this.orgRotation = base.transform.localRotation;
	}

	private void Update()
	{
		base.transform.localRotation = this.orgRotation;
	}

	private Quaternion orgRotation;
}
