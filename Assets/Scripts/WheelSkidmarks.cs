using System;
using UnityEngine;

public class WheelSkidmarks : MonoBehaviour
{
	private void Start()
	{
		this.skidCaller = base.transform.root.gameObject;
		this.wheel_col = base.GetComponent<WheelCollider>();
		if (UnityEngine.Object.FindObjectOfType(typeof(Skidmarks)))
		{
			this.skidmarks = (UnityEngine.Object.FindObjectOfType(typeof(Skidmarks)) as Skidmarks);
		}
		else
		{
			UnityEngine.Debug.Log("No skidmarks object found. Skidmarks will not be drawn");
		}
	}

	private void FixedUpdate()
	{
		WheelHit wheelHit;
		this.wheel_col.GetGroundHit(out wheelHit);
		float num = Mathf.Abs(wheelHit.sidewaysSlip);
		if (num > this.startSlipValue)
		{
			Vector3 pos = wheelHit.point + 2f * this.skidCaller.GetComponent<Rigidbody>().velocity * Time.deltaTime;
			this.lastSkidmark = this.skidmarks.AddSkidMark(pos, wheelHit.normal, num / 2f, this.lastSkidmark);
		}
		else
		{
			this.lastSkidmark = -1;
		}
	}

	public GameObject skidCaller;

	public float startSlipValue = 0.4f;

	private Skidmarks skidmarks;

	private int lastSkidmark = -1;

	private WheelCollider wheel_col;
}
