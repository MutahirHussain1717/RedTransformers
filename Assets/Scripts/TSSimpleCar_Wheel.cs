using System;
using UnityEngine;

public class TSSimpleCar_Wheel : MonoBehaviour
{
	public string WheelPosition
	{
		get
		{
			return this.wheelPosition;
		}
		set
		{
			this.wheelPosition = value;
		}
	}

	public float AntiRollBarForce
	{
		get
		{
			return this._antiRollBarForce;
		}
		set
		{
			this._antiRollBarForce = value;
		}
	}

	public float compression
	{
		get
		{
			return this._compression;
		}
	}

	private void Awake()
	{
		this.myBody = base.transform.parent.parent.GetComponent<Rigidbody>();
		this.myTransform = base.transform;
		this.myParentTransform = this.myTransform.parent;
		this.simpleCarScript = base.transform.root.GetComponent<TSSimpleCar>();
		this.wheelTransform = this.CorrespondingCollider.transform;
		this.suspensionTravel = this.CorrespondingCollider.suspensionDistance;
		this.radius = this.CorrespondingCollider.radius;
		if (this.simpleCarScript.superSimplePhysics)
		{
			base.enabled = false;
		}
		else
		{
			Renderer componentInChildren = base.GetComponentInChildren<Renderer>();
			Collider component = componentInChildren.gameObject.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}

	private void Update()
	{
		if (this.simpleCarScript.superSimplePhysics)
		{
			this.CorrespondingCollider.enabled = false;
			base.enabled = false;
			return;
		}
		this.pos1 = this.wheelTransform.position;
		this.up = this.wheelTransform.up;
		WheelHit wheelHit;
		bool groundHit = this.CorrespondingCollider.GetGroundHit(out wheelHit);
		if (groundHit)
		{
			this._compression = 1f - (Vector3.Dot(this.pos1 - wheelHit.point, this.up) - this.radius) / this.suspensionTravel;
		}
		else
		{
			this._compression = this.suspensionTravel;
		}
		this.myTransform.localPosition = Vector3.up * (this._compression - 1f) * this.suspensionTravel;
		this.myTransform.rotation = this.CorrespondingCollider.transform.rotation * Quaternion.Euler(this.RotationValue, this.CorrespondingCollider.steerAngle, 0f);
		this.RotationValue += this.CorrespondingCollider.rpm * 6f * Time.deltaTime;
		if (Mathf.Abs(wheelHit.sidewaysSlip) > 2f && this.SlipPrefab)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.SlipPrefab, wheelHit.point, Quaternion.identity);
		}
	}

	private void FixedUpdate()
	{
		if (this.simpleCarScript.superSimplePhysics)
		{
			this.CorrespondingCollider.enabled = false;
			base.enabled = false;
			return;
		}
		this.myBody.AddForceAtPosition(this._antiRollBarForce * this.myParentTransform.up, this.myParentTransform.position);
	}

	public WheelCollider CorrespondingCollider;

	public GameObject SlipPrefab;

	private float RotationValue;

	private Vector3 pos1 = Vector3.zero;

	private Vector3 up = Vector3.zero;

	private float suspensionTravel;

	private float radius;

	private Transform wheelTransform;

	private float _compression;

	private TSSimpleCar simpleCarScript;

	private Transform myTransform;

	private float _antiRollBarForce;

	private Rigidbody myBody;

	private string wheelPosition;

	private Transform myParentTransform;
}
