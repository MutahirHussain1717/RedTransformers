using System;
using UnityEngine;

public class RCC_SuspensionArm : MonoBehaviour
{
	private void Start()
	{
		this.orgRot = base.transform.localEulerAngles;
		this.totalSuspensionDistance = this.GetSuspensionDistance();
	}

	private void FixedUpdate()
	{
		float num = this.GetSuspensionDistance() - this.totalSuspensionDistance;
		base.transform.localEulerAngles = this.orgRot;
		RCC_SuspensionArm.Axis axis = this.axis;
		if (axis != RCC_SuspensionArm.Axis.X)
		{
			if (axis != RCC_SuspensionArm.Axis.Y)
			{
				if (axis == RCC_SuspensionArm.Axis.Z)
				{
					base.transform.Rotate(Vector3.forward, num * this.angleFactor - this.offsetAngle, Space.Self);
				}
			}
			else
			{
				base.transform.Rotate(Vector3.up, num * this.angleFactor - this.offsetAngle, Space.Self);
			}
		}
		else
		{
			base.transform.Rotate(Vector3.right, num * this.angleFactor - this.offsetAngle, Space.Self);
		}
	}

	private float GetSuspensionDistance()
	{
		Vector3 position;
		Quaternion quaternion;
		this.wheelcollider.GetWorldPose(out position, out quaternion);
		return this.wheelcollider.transform.InverseTransformPoint(position).y;
	}

	public WheelCollider wheelcollider;

	public RCC_SuspensionArm.Axis axis;

	private Vector3 orgRot;

	private float totalSuspensionDistance;

	public float offsetAngle = 30f;

	public float angleFactor = 150f;

	public enum Axis
	{
		X,
		Y,
		Z
	}
}
