using System;
using System.Collections;
using UnityEngine;

public class TSSimpleCar : MonoBehaviour
{
	private void Awake()
	{
		this.myTransform = base.transform;
		this.body = base.GetComponent<Rigidbody>();
		this.bodies = base.GetComponentsInChildren<Rigidbody>();
		this.trafficAI = base.GetComponent<TSTrafficAI>();
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource == null)
		{
			this.engineAudioEnabled = false;
		}
	}

	private void Start()
	{
		if (this.trafficAI != null)
		{
			TSTrafficAI tstrafficAI = this.trafficAI;
			tstrafficAI.OnUpdateAI = (TSTrafficAI.OnUpdateAIDelegate)Delegate.Combine(tstrafficAI.OnUpdateAI, new TSTrafficAI.OnUpdateAIDelegate(this.OnAIUpdate));
			TSTrafficAI tstrafficAI2 = this.trafficAI;
			tstrafficAI2.UpdateCarSpeed = (TSTrafficAI.GetCarSpeedDelegate)Delegate.Combine(tstrafficAI2.UpdateCarSpeed, new TSTrafficAI.GetCarSpeedDelegate(this.UpdateCarSpeed));
			TSTrafficAI tstrafficAI3 = this.trafficAI;
			tstrafficAI3.OnTurnLeft = (TSTrafficAI.OnTurnLeftDelegate)Delegate.Combine(tstrafficAI3.OnTurnLeft, new TSTrafficAI.OnTurnLeftDelegate(this.OnTurnLeft));
			TSTrafficAI tstrafficAI4 = this.trafficAI;
			tstrafficAI4.OnTurnRight = (TSTrafficAI.OnTurnRigthDelegate)Delegate.Combine(tstrafficAI4.OnTurnRight, new TSTrafficAI.OnTurnRigthDelegate(this.OnTurnRight));
		}
		if (this.turnLeftLight != null)
		{
			this.turnLeftLight.SetActive(false);
		}
		if (this.turnRightLight != null)
		{
			this.turnRightLight.SetActive(false);
		}
		if (this.brakeLightsShader == null)
		{
			this.brakeLightsShader = Shader.Find("Car/LightsEmmissive");
		}
		if (this.superSimplePhysics)
		{
			this.frontWheels = new Transform[this.FrontWheels.Length];
			this.rearWheels = new Transform[this.BackWheels.Length];
			for (int i = 0; i < this.FrontWheels.Length; i++)
			{
				this.frontWheels[i] = this.FrontWheels[i].transform;
				this.FrontWheels[i].enabled = false;
			}
			for (int j = 0; j < this.BackWheels.Length; j++)
			{
				this.rearWheels[j] = this.BackWheels[j].transform;
				this.BackWheels[j].enabled = false;
			}
			for (int k = 0; k < this.bodies.Length; k++)
			{
				if (this.CoM != null && k == 0)
				{
					this.bodies[k].centerOfMass = new Vector3(this.CoM.localPosition.x, -2f, this.CoM.localPosition.z);
				}
				else
				{
					this.bodies[k].centerOfMass = new Vector3(this.bodies[k].centerOfMass.x, -2f, this.bodies[k].centerOfMass.z);
				}
			}
		}
		else if (this.CoM != null)
		{
			for (int l = 0; l < this.bodies.Length; l++)
			{
				if (l == 0)
				{
					this.bodies[l].centerOfMass = new Vector3(this.CoM.localPosition.x * base.transform.localScale.x, this.CoM.localPosition.y * base.transform.localScale.y, this.CoM.localPosition.z * base.transform.localScale.z);
				}
			}
			this.body.centerOfMass = new Vector3(this.CoM.localPosition.x * base.transform.localScale.x, this.CoM.localPosition.y * base.transform.localScale.y, this.CoM.localPosition.z * base.transform.localScale.z);
		}
	}

	private void OnEnable()
	{
		if (this.CoM != null)
		{
			for (int i = 0; i < this.bodies.Length; i++)
			{
				if (i == 0)
				{
					this.bodies[i].centerOfMass = new Vector3(this.CoM.localPosition.x * base.transform.localScale.x, this.CoM.localPosition.y * base.transform.localScale.y, this.CoM.localPosition.z * base.transform.localScale.z);
				}
			}
			this.body.centerOfMass = new Vector3(this.CoM.localPosition.x * base.transform.localScale.x, this.CoM.localPosition.y * base.transform.localScale.y, this.CoM.localPosition.z * base.transform.localScale.z);
		}
		this.crashed = false;
	}

	private void OnTurnRight(bool isTurning)
	{
		if (this.turnRightLight != null)
		{
			if (isTurning)
			{
				base.StartCoroutine(this.LightBlinking(this.turnRightLight));
			}
			else
			{
				this.blinking = false;
				this.turnRightLight.SetActive(false);
			}
		}
	}

	private void OnTurnLeft(bool isTurning)
	{
		if (this.turnLeftLight != null)
		{
			if (isTurning)
			{
				base.StartCoroutine(this.LightBlinking(this.turnLeftLight));
			}
			else
			{
				this.blinking = false;
				this.turnLeftLight.SetActive(false);
			}
		}
	}

	private IEnumerator LightBlinking(GameObject blinkingLight)
	{
		this.blinking = true;
		while (this.blinking)
		{
			if (blinkingLight.activeSelf)
			{
				blinkingLight.SetActive(false);
			}
			else
			{
				blinkingLight.SetActive(true);
			}
			yield return TSSimpleCar.waitforseconds;
		}
		blinkingLight.SetActive(false);
		yield break;
	}

	private void Update()
	{
		if (!this.crashed)
		{
			if (this.superSimplePhysics && !this.upSideDown)
			{
				for (int i = 0; i < this.bodies.Length; i++)
				{
					float num = Mathf.Clamp((this.bodies[i].velocity.magnitude - 10f) / 35f, -1f, 2f);
					this.bodies[i].centerOfMass = new Vector3(this.bodies[i].centerOfMass.x, -num, this.bodies[i].centerOfMass.z);
					this.body.centerOfMass = new Vector3(this.bodies[i].centerOfMass.x, -num, this.bodies[i].centerOfMass.z);
				}
				if (this.engineAudioEnabled)
				{
					this.audioSource.pitch = Mathf.Abs(this.mySpeed / this.maxSpeed) + 0.75f;
					if (this.audioSource.pitch > 2f)
					{
						this.audioSource.pitch = 2f;
					}
				}
				this.BrakeLight(this.brake);
			}
			else if (!this.superSimplePhysics)
			{
				if (this.playerControlled)
				{
					this.steering = UnityEngine.Input.GetAxis("Horizontal");
					float num2 = this.EngineTorque * UnityEngine.Input.GetAxis("Vertical");
					float num3 = Mathf.Max(1f - this.mySpeed / this.maxSpeed, 0.05f);
					float steerAngle = 55f * Mathf.Clamp(this.steering, -1f * num3, 1f * num3);
					float num4 = 0f;
					for (int j = 0; j < this.FrontWheels.Length; j++)
					{
						this.FrontWheels[j].steerAngle = steerAngle;
						this.FrontWheels[j].brakeTorque = num4;
					}
					for (int k = 0; k < this.BackWheels.Length; k++)
					{
						if (this.mySpeed < this.maxSpeed)
						{
							this.BackWheels[k].motorTorque = num2 / ((float)this.BackWheels.Length / 2f);
						}
						else
						{
							this.BackWheels[k].motorTorque = -this.EngineTorque / 5f;
						}
						this.BackWheels[k].brakeTorque = num4;
					}
					for (int l = 0; l < this.aditionalBrakeWheels.Length; l++)
					{
						this.aditionalBrakeWheels[l].brakeTorque = num4;
					}
					if (num4 != 0f)
					{
						this.brake += 1f;
					}
					else
					{
						this.brake -= 0.05f;
					}
					this.brake = Mathf.Clamp01(this.brake);
				}
				for (int m = 0; m < this.BackWheels.Length; m++)
				{
					this.RPM += this.BackWheels[m].rpm;
				}
				this.RPM /= (float)this.BackWheels.Length;
				this.EngineRPM = this.RPM * this.GearRatio[this.CurrentGear];
				this.ShiftGears();
				if (this.engineAudioEnabled)
				{
					this.audioSource.pitch = Mathf.Abs(this.EngineRPM / this.MaxEngineRPM) + 0.75f;
					if (this.audioSource.pitch > 2f)
					{
						this.audioSource.pitch = 2f;
					}
				}
				if (this.FrontWheels != null && this.FrontWheels.Length > 0)
				{
					this.BrakeLight(this.FrontWheels[0].brakeTorque);
				}
			}
		}
	}

	public void BrakeLight(float brakeInput)
	{
		if (this.enableDisableBrakeLights && this.brakeLigths != null)
		{
			for (int i = 0; i < this.brakeLigths.Length; i++)
			{
				if (this.brake != 0f && !this.brakeLigths[i].enabled)
				{
					this.brakeLigths[i].enabled = true;
				}
				else if (this.brake == 0f && this.brakeLigths[i].enabled)
				{
					this.brakeLigths[i].enabled = false;
				}
			}
		}
	}

	private void RotateVisualWheels(float steering)
	{
		float num = this.mySpeed * 1.6f * Time.deltaTime;
		this.rotation += num;
		this.rotationAmount = Vector3.right * num * 57.29578f;
		for (int i = 0; i < this.frontWheels.Length; i++)
		{
			this.frontWheels[i].localEulerAngles = new Vector3(this.rotation * 57.29578f, steering, 0f);
		}
		for (int j = 0; j < this.rearWheels.Length; j++)
		{
			this.rearWheels[j].Rotate(this.rotationAmount);
		}
	}

	private void SuperSimplePhysicsUpdate(float throttle, float steering)
	{
		this.velo = this.body.velocity;
		this.tempVec = new Vector3(this.velo.x, 0f, this.velo.z);
		this.flatVelo = this.tempVec;
		this.dir = this.myTransform.TransformDirection(Vector3.forward);
		this.tempVec = new Vector3(this.dir.x, 0f, this.dir.z);
		this.flatDir = Vector3.Normalize(this.tempVec);
		this.slideSpeed = Vector3.Dot(this.myTransform.right, this.flatVelo);
		this.mySpeed = this.flatVelo.magnitude;
		this.engineForce = this.flatDir * (this.EngineTorque * 10f * throttle) * this.body.mass;
		this.actualGrip = Mathf.Lerp(100f, this.carGrip, this.mySpeed * 0.02f);
		this.impulse = this.myTransform.right * (-this.slideSpeed * this.body.mass * Mathf.Abs(Physics.gravity.y) * this.actualGrip);
	}

	private void FixedUpdate()
	{
		if (!this.crashed && this.superSimplePhysics && !this.upSideDown)
		{
			float d = 0.02f / Time.fixedDeltaTime;
			if (this.mySpeed < this.maxSpeed)
			{
				if (this.frontWheels != null && this.frontWheels.Length > 0)
				{
					this.body.AddForceAtPosition(this.engineForce * Time.deltaTime * d, new Vector3(this.myTransform.position.x, this.FrontWheels[0].transform.position.y, this.myTransform.position.z));
				}
				else
				{
					this.body.AddForce(this.engineForce * Time.deltaTime * d);
				}
			}
			if (this.mySpeed > this.maxSpeedToTurn)
			{
				Quaternion rhs = Quaternion.Euler(new Vector3(0f, this.turnSpeed * this.steering, 0f));
				this.body.MoveRotation(this.body.rotation * rhs);
			}
			if (this.brake > 0f && this.body.velocity.sqrMagnitude > 0f)
			{
				if (this.frontWheels != null && this.frontWheels.Length > 0)
				{
					this.body.AddForceAtPosition(-this.body.velocity.normalized * this.brakeTorque * this.brake * Time.deltaTime * d, new Vector3(this.myTransform.position.x, this.FrontWheels[0].transform.position.y, this.myTransform.position.z));
				}
				else
				{
					this.body.AddForce(Mathf.Sign(this.localspeed.z) * -this.flatDir * this.brakeTorque * 10f * this.body.mass * this.brake * Time.deltaTime * d);
				}
			}
			this.body.AddForce(this.impulse * Time.deltaTime * d);
		}
		else
		{
			this.mySpeed = this.body.velocity.magnitude;
		}
		this.myAccel = (this.speed - this.lastSpeed) / Time.deltaTime;
		if (this.myAccel > this.maxAcceleration)
		{
			this.body.drag += 0.1f;
		}
		else if (this.body.drag > 0.1f)
		{
			this.body.drag -= 0.1f;
		}
		if (this.body.drag < this.speed / 125f)
		{
			this.body.drag = this.speed / 125f;
		}
		this.lastSpeed = this.speed;
	}

	public void OnAIUpdate(float steering, float brake, float throttle, bool isUpSideDown)
	{
		this.upSideDown = isUpSideDown;
		this.steering = steering;
		float steerAngle = 55f * Mathf.Clamp(steering, -1f, 1f);
		float num = this.brakeTorque * brake;
		float num2 = this.EngineTorque * throttle;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		if (this.crashed)
		{
			steerAngle = 0f;
			num = this.brakeTorque * 1f;
			num2 = 0f;
			throttle = 0f;
			steering = 0f;
		}
		if (this.superSimplePhysics)
		{
			this.RotateVisualWheels(steerAngle);
			this.SuperSimplePhysicsUpdate(throttle, steering);
		}
		else
		{
			for (int i = 0; i < this.FrontWheels.Length; i++)
			{
				this.FrontWheels[i].steerAngle = steerAngle;
				this.FrontWheels[i].brakeTorque = num;
			}
			for (int j = 0; j < this.BackWheels.Length; j++)
			{
				if (this.mySpeed < this.maxSpeed)
				{
					this.BackWheels[j].motorTorque = num2 / ((float)this.BackWheels.Length / 2f);
				}
				this.BackWheels[j].brakeTorque = num;
			}
			for (int k = 0; k < this.aditionalBrakeWheels.Length; k++)
			{
				this.aditionalBrakeWheels[k].brakeTorque = num;
			}
		}
		if (num != 0f)
		{
			this.brake += 1f;
		}
		else
		{
			this.brake -= 0.05f;
		}
		this.brake = Mathf.Clamp01(this.brake);
	}

	public void UpdateCarSpeed(out float carSpeed)
	{
		this.localspeed = this.myTransform.InverseTransformDirection(this.body.velocity);
		this.speed = (carSpeed = this.localspeed.magnitude);
	}

	private void ShiftGears()
	{
		int currentGear = this.CurrentGear;
		if (this.EngineRPM >= this.MaxEngineRPM)
		{
			for (int i = 0; i < this.GearRatio.Length; i++)
			{
				if (this.RPM * this.GearRatio[i] < this.MaxEngineRPM)
				{
					currentGear = i;
					break;
				}
			}
			this.CurrentGear = currentGear;
		}
		if (this.EngineRPM <= this.MinEngineRPM)
		{
			currentGear = this.CurrentGear;
			for (int j = this.GearRatio.Length - 1; j >= 0; j--)
			{
				if (this.RPM * this.GearRatio[j] > this.MinEngineRPM)
				{
					currentGear = j;
					break;
				}
			}
			this.CurrentGear = currentGear;
		}
	}

	private int[] getBrakeLights()
	{
		int[] array = new int[this.brakeLigths.Length];
		int num = 0;
		if (this.brakeLigths.Length != 0)
		{
			foreach (Renderer renderer in this.brakeLigths)
			{
				int num2 = 0;
				if (renderer != null)
				{
					foreach (Material material in renderer.sharedMaterials)
					{
						if (material != null && material.shader == this.brakeLightsShader)
						{
							array[num] = num2;
						}
						num2++;
					}
				}
				num++;
			}
		}
		return array;
	}

	public void ActivatecrashedSmoke()
	{
		for (int i = 0; i < this.crashedSmokes.Length; i++)
		{
			this.crashedSmokes[i].enableEmission = true;
			this.crashedSmokes[i].Play(true);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (this.carCanCrash && col.relativeVelocity.magnitude > this.minSpdForCrash)
		{
			this.crashed = true;
			if (this.trafficAI != null)
			{
				this.trafficAI.crashed = true;
			}
			this.ActivatecrashedSmoke();
		}
	}

	public WheelCollider[] FrontWheels = new WheelCollider[0];

	public WheelCollider[] BackWheels = new WheelCollider[0];

	public WheelCollider[] aditionalBrakeWheels = new WheelCollider[0];

	public bool playerControlled;

	public bool engineAudioEnabled = true;

	public float[] GearRatio = new float[]
	{
		3.5f,
		2.5f,
		2f,
		1.5f,
		1f
	};

	public int CurrentGear;

	public float EngineTorque = 600f;

	public float MaxEngineRPM = 3000f;

	public float MinEngineRPM = 1000f;

	public float maxAcceleration = 5f;

	public float brakeTorque = 400f;

	public Renderer[] brakeLigths;

	public float brakeIntensityRate = 0.2f;

	public bool enableDisableBrakeLights;

	public Shader brakeLightsShader;

	public string propertyName = "_Intensity";

	public bool superSimplePhysics;

	public float turnSpeed = 3f;

	public float carGrip = 70f;

	public float maxSpeed = 50f;

	public float maxSpeedToTurn = 0.2f;

	private bool upSideDown;

	[HideInInspector]
	public bool crashed;

	public bool carCanCrash;

	public ParticleSystem[] crashedSmokes;

	public float minSpdForCrash = 10f;

	public GameObject turnRightLight;

	public GameObject turnLeftLight;

	public Transform CoM;

	private float EngineRPM;

	private float brake;

	private float RPM;

	private Rigidbody body;

	private Rigidbody[] bodies;

	private float steering;

	private Transform myTransform;

	private TSTrafficAI trafficAI;

	private float lastSpeed;

	private float speed;

	private float myAccel;

	private bool blinking;

	private Transform[] frontWheels;

	private Transform[] rearWheels;

	private float mySpeed;

	private Vector3 velo = Vector3.zero;

	private Vector3 tempVec;

	private Vector3 flatVelo;

	private Vector3 flatDir;

	private float slideSpeed;

	private Vector3 engineForce = Vector3.zero;

	private float actualGrip;

	private Vector3 impulse = Vector3.zero;

	private Vector3 dir;

	private AudioSource audioSource;

	private static WaitForSeconds waitforseconds = new WaitForSeconds(0.2f);

	private Vector3 rotationAmount;

	private float rotation;

	private Vector3 localspeed;
}
