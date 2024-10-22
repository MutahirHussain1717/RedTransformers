using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Shadow Rotation Const")]
public class RCC_ShadowRotConst : MonoBehaviour
{
	private void Start()
	{
		this.root = base.GetComponentInParent<RCC_CarControllerV3>().transform;
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(90f, this.root.eulerAngles.y, 0f);
	}

	private Transform root;
}
