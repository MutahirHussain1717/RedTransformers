using System;
using UnityEngine;

public class TSAntiRollBar : MonoBehaviour
{
	private void FixedUpdate()
	{
		float num = (this.wheel1.compression - this.wheel2.compression) * this.coefficient;
		this.wheel1.AntiRollBarForce = num;
		this.wheel2.AntiRollBarForce = -num;
	}

	public TSSimpleCar_Wheel wheel1;

	public TSSimpleCar_Wheel wheel2;

	public float coefficient = 6000f;
}
