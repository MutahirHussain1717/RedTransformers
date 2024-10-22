using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Animator Controller")]
public class RCC_CharacterController : MonoBehaviour
{
	private void Start()
	{
		this.animator = base.GetComponent<Animator>();
		this.carController = base.GetComponent<RCC_CarControllerV3>();
		this.carRigid = base.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		this.steerInput = Mathf.Lerp(this.steerInput, this.carController.steerInput, Time.deltaTime * 5f);
		this.directionInput = this.carRigid.transform.InverseTransformDirection(this.carRigid.velocity).z;
		this.impactInput -= Time.deltaTime * 5f;
		if (this.impactInput < 0f)
		{
			this.impactInput = 0f;
		}
		if (this.impactInput > 1f)
		{
			this.impactInput = 1f;
		}
		if (this.directionInput <= -2f)
		{
			this.reversing = true;
		}
		else if (this.directionInput > -1f)
		{
			this.reversing = false;
		}
		if (this.carController.changingGear)
		{
			this.gearInput = 1f;
		}
		else
		{
			this.gearInput -= Time.deltaTime * 5f;
		}
		if (this.gearInput < 0f)
		{
			this.gearInput = 0f;
		}
		if (this.gearInput > 1f)
		{
			this.gearInput = 1f;
		}
		if (!this.reversing)
		{
			this.animator.SetBool(this.driverReversingParameter, false);
		}
		else
		{
			this.animator.SetBool(this.driverReversingParameter, true);
		}
		if (this.impactInput > 0.5f)
		{
			this.animator.SetBool(this.driverDangerParameter, true);
		}
		else
		{
			this.animator.SetBool(this.driverDangerParameter, false);
		}
		if (this.gearInput > 0.5f)
		{
			this.animator.SetBool(this.driverShiftingGearParameter, true);
		}
		else
		{
			this.animator.SetBool(this.driverShiftingGearParameter, false);
		}
		this.animator.SetFloat(this.driverSteeringParameter, this.steerInput);
	}

	private void OnCollisionEnter(Collision col)
	{
		if (col.relativeVelocity.magnitude < 2.5f)
		{
			return;
		}
		this.impactInput = 1f;
	}

	private RCC_CarControllerV3 carController;

	private Rigidbody carRigid;

	private Animator animator;

	public string driverSteeringParameter;

	public string driverShiftingGearParameter;

	public string driverDangerParameter;

	public string driverReversingParameter;

	public float steerInput;

	public float directionInput;

	public bool reversing;

	public float impactInput;

	public float gearInput;
}
