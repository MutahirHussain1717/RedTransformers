using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Truck Trailer")]
public class RCC_TruckTrailer : MonoBehaviour
{
	private void Start()
	{
		this.rigid = base.GetComponent<Rigidbody>();
		this.carController = base.transform.GetComponentInParent<RCC_CarControllerV3>();
		base.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		base.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		base.GetComponent<Rigidbody>().centerOfMass = base.transform.InverseTransformPoint(this.COM.transform.position);
		this.antiRoll = this.carController.antiRollFrontHorizontal;
		for (int i = 0; i < this.wheelColliders.Length; i++)
		{
			if (this.wheelColliders[i].transform.localPosition.x < 0f)
			{
				this.leftWheelColliders.Add(this.wheelColliders[i]);
			}
			else
			{
				this.rightWheelColliders.Add(this.wheelColliders[i]);
			}
		}
	}

	private void FixedUpdate()
	{
		this.AntiRollBars();
		foreach (WheelCollider wheelCollider in this.wheelColliders)
		{
			wheelCollider.motorTorque = this.carController._gasInput * (this.carController.engineTorque / 10f);
		}
	}

	public void AntiRollBars()
	{
		for (int i = 0; i < this.leftWheelColliders.Count; i++)
		{
			float num = 1f;
			float num2 = 1f;
			WheelHit wheelHit;
			bool groundHit = this.leftWheelColliders[i].GetGroundHit(out wheelHit);
			if (groundHit)
			{
				num = (-this.leftWheelColliders[i].transform.InverseTransformPoint(wheelHit.point).y - this.leftWheelColliders[i].radius) / this.leftWheelColliders[i].suspensionDistance;
			}
			bool groundHit2 = this.rightWheelColliders[i].GetGroundHit(out wheelHit);
			if (groundHit2)
			{
				num2 = (-this.rightWheelColliders[i].transform.InverseTransformPoint(wheelHit.point).y - this.rightWheelColliders[i].radius) / this.rightWheelColliders[i].suspensionDistance;
			}
			float num3 = (num - num2) * this.antiRoll;
			if (groundHit)
			{
				this.rigid.AddForceAtPosition(this.leftWheelColliders[i].transform.up * -num3, this.leftWheelColliders[i].transform.position);
			}
			if (groundHit2)
			{
				this.rigid.AddForceAtPosition(this.rightWheelColliders[i].transform.up * num3, this.rightWheelColliders[i].transform.position);
			}
		}
	}

	private RCC_CarControllerV3 carController;

	private Rigidbody rigid;

	public Transform COM;

	public WheelCollider[] wheelColliders;

	private List<WheelCollider> leftWheelColliders = new List<WheelCollider>();

	private List<WheelCollider> rightWheelColliders = new List<WheelCollider>();

	public float antiRoll = 50000f;
}
