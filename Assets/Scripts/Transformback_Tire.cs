using System;
using UnityEngine;

public class Transformback_Tire : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.gameObject.transform.Rotate(10f * Time.fixedDeltaTime * this.bikeScript.speed, 0f, 0f);
	}

	public BikeControl bikeScript;
}
