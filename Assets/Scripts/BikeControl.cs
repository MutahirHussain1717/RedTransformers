using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeControl : MonoBehaviour
{
	private BikeControl.WheelComponent SetWheelComponent(Transform wheel, Transform axle, bool drive, float maxSteer, float pos_y)
	{
		BikeControl.WheelComponent wheelComponent = new BikeControl.WheelComponent();
		GameObject gameObject = new GameObject(wheel.name + "WheelCollider");
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = wheel.position;
		gameObject.transform.eulerAngles = base.transform.eulerAngles;
		pos_y = gameObject.transform.localPosition.y;
		gameObject.AddComponent(typeof(WheelCollider));
		wheelComponent.drive = drive;
		wheelComponent.wheel = wheel;
		wheelComponent.axle = axle;
		wheelComponent.collider = gameObject.GetComponent<WheelCollider>();
		wheelComponent.pos_y = pos_y;
		wheelComponent.maxSteer = maxSteer;
		wheelComponent.startPos = axle.transform.localPosition;
		return wheelComponent;
	}

	private void Awake()
	{
		if (this.bikeSetting.automaticGear)
		{
			this.NeutralGear = false;
		}
		this.myRigidbody = base.transform.GetComponent<Rigidbody>();
		this.SteerRotation = this.bikeSetting.bikeSteer.localRotation;
		this.wheels = new BikeControl.WheelComponent[2];
		this.wheels[0] = this.SetWheelComponent(this.bikeWheels.wheels.wheelFront, this.bikeWheels.wheels.AxleFront, false, this.bikeSetting.maxSteerAngle, this.bikeWheels.wheels.AxleFront.localPosition.y);
		this.wheels[1] = this.SetWheelComponent(this.bikeWheels.wheels.wheelBack, this.bikeWheels.wheels.AxleBack, true, 0f, this.bikeWheels.wheels.AxleBack.localPosition.y);
		this.wheels[0].collider.transform.localPosition = new Vector3(0f, this.wheels[0].collider.transform.localPosition.y, this.wheels[0].collider.transform.localPosition.z);
		this.wheels[1].collider.transform.localPosition = new Vector3(0f, this.wheels[1].collider.transform.localPosition.y, this.wheels[1].collider.transform.localPosition.z);
		foreach (BikeControl.WheelComponent wheelComponent in this.wheels)
		{
			WheelCollider collider = wheelComponent.collider;
			collider.suspensionDistance = this.bikeWheels.setting.Distance;
			JointSpring suspensionSpring = collider.suspensionSpring;
			suspensionSpring.spring = this.bikeSetting.springs;
			suspensionSpring.damper = this.bikeSetting.dampers;
			collider.suspensionSpring = suspensionSpring;
			collider.radius = this.bikeWheels.setting.Radius;
			collider.mass = this.bikeWheels.setting.Weight;
			WheelFrictionCurve wheelFrictionCurve = collider.forwardFriction;
			wheelFrictionCurve.asymptoteValue = 0.5f;
			wheelFrictionCurve.extremumSlip = 0.4f;
			wheelFrictionCurve.asymptoteSlip = 0.8f;
			wheelFrictionCurve.stiffness = this.bikeSetting.stiffness;
			collider.forwardFriction = wheelFrictionCurve;
			wheelFrictionCurve = collider.sidewaysFriction;
			wheelFrictionCurve.asymptoteValue = 0.75f;
			wheelFrictionCurve.extremumSlip = 0.2f;
			wheelFrictionCurve.asymptoteSlip = 0.5f;
			wheelFrictionCurve.stiffness = this.bikeSetting.stiffness;
			collider.sidewaysFriction = wheelFrictionCurve;
		}
		this.audioSource = (AudioSource)base.GetComponent(typeof(AudioSource));
		if (this.audioSource == null)
		{
			UnityEngine.Debug.Log("No audio please add one");
		}
	}

	public void ShiftUp()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		if (timeSinceLevelLoad < this.shiftDelay)
		{
			return;
		}
		if (this.currentGear < this.bikeSetting.gears.Length - 1)
		{
			this.bikeSounds.switchGear.GetComponent<AudioSource>().Play();
			if (!this.bikeSetting.automaticGear)
			{
				if (this.currentGear == 0)
				{
					if (this.NeutralGear)
					{
						this.currentGear++;
						this.NeutralGear = false;
					}
					else
					{
						this.NeutralGear = true;
					}
				}
				else
				{
					this.currentGear++;
				}
			}
			else
			{
				this.currentGear++;
			}
			this.shiftDelay = timeSinceLevelLoad + 1f;
		}
	}

	public void ShiftDown()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		if (timeSinceLevelLoad < this.shiftDelay)
		{
			return;
		}
		if (this.currentGear > 0 || this.NeutralGear)
		{
			this.bikeSounds.switchGear.GetComponent<AudioSource>().Play();
			if (!this.bikeSetting.automaticGear)
			{
				if (this.currentGear == 1)
				{
					if (!this.NeutralGear)
					{
						this.currentGear--;
						this.NeutralGear = true;
					}
				}
				else if (this.currentGear == 0)
				{
					this.NeutralGear = false;
				}
				else
				{
					this.currentGear--;
				}
			}
			else
			{
				this.currentGear--;
			}
			this.shiftDelay = timeSinceLevelLoad + 0.1f;
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		this.bikeSounds.lowCrash.Play();
	}

	private void Update()
	{
		if (this.activeControl && !this.bikeSetting.automaticGear)
		{
			if (UnityEngine.Input.GetKeyDown("page up"))
			{
				this.ShiftUp();
			}
			if (UnityEngine.Input.GetKeyDown("page down"))
			{
				this.ShiftDown();
			}
		}
		this.steer2 = Mathf.LerpAngle(this.steer2, this.steer * -this.bikeSetting.maxSteerAngle, Time.deltaTime * 10f);
		this.MotorRotation = Mathf.LerpAngle(this.MotorRotation, this.steer2 * this.bikeSetting.maxTurn * Mathf.Clamp(this.speed / 100f, 0f, 1f), Time.deltaTime * 5f);
		if (this.bikeSetting.bikeSteer)
		{
			this.bikeSetting.bikeSteer.localRotation = this.SteerRotation * Quaternion.Euler(0f, this.wheels[0].collider.steerAngle, 0f);
		}
		if (!this.crash)
		{
			this.flipRotate = ((base.transform.eulerAngles.z <= 90f || base.transform.eulerAngles.z >= 270f) ? 0f : 180f);
			this.Wheelie = Mathf.Clamp(this.Wheelie, 0f, this.bikeSetting.maxWheelie);
			if (this.shifting)
			{
				this.Wheelie += this.bikeSetting.speedWheelie * Time.deltaTime * 1.3f;
			}
			else
			{
				this.Wheelie = Mathf.MoveTowards(this.Wheelie, 0f, this.bikeSetting.speedWheelie * 2f * Time.deltaTime * 1.3f);
			}
			this.deltaRotation1 = Quaternion.Euler(-this.Wheelie, 0f, this.flipRotate - base.transform.localEulerAngles.z + this.MotorRotation);
			this.deltaRotation2 = Quaternion.Euler(0f, 0f, this.flipRotate - base.transform.localEulerAngles.z);
			this.myRigidbody.MoveRotation(this.myRigidbody.rotation * this.deltaRotation2);
			this.bikeSetting.MainBody.localRotation = this.deltaRotation1;
		}
		else
		{
			this.bikeSetting.MainBody.localRotation = Quaternion.identity;
			this.Wheelie = 0f;
		}
	}

	private void FixedUpdate()
	{
		this.speed = this.myRigidbody.velocity.magnitude * 2.7f;
		if (this.crash)
		{
			this.myRigidbody.constraints = RigidbodyConstraints.None;
			this.myRigidbody.centerOfMass = Vector3.zero;
		}
		else
		{
			this.myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
			this.myRigidbody.centerOfMass = this.bikeSetting.shiftCentre;
		}
		if (this.activeControl)
		{
			if (this.controlMode == ControlMode.simple)
			{
				this.accel = 0f;
				this.shift = false;
				this.brake = false;
				if (!this.crash)
				{
					this.steer = Mathf.MoveTowards(this.steer, UnityEngine.Input.GetAxis("Horizontal"), 0.1f);
					this.accel = UnityEngine.Input.GetAxis("Vertical");
					this.brake = Input.GetButton("Jump");
					this.shift = (UnityEngine.Input.GetKey(KeyCode.LeftShift) | UnityEngine.Input.GetKey(KeyCode.RightShift));
				}
				else
				{
					this.steer = 0f;
				}
			}
			else if (this.controlMode == ControlMode.touch)
			{
				if (this.accelFwd != 0f)
				{
					this.accel = this.accelFwd;
				}
				else
				{
					this.accel = this.accelBack;
				}
				this.steer = Mathf.MoveTowards(this.steer, this.steerAmount, 0.07f);
			}
		}
		else
		{
			this.accel = 0f;
			this.steer = 0f;
			this.shift = false;
			this.brake = false;
		}
		foreach (Light light in this.bikeLights.brakeLights)
		{
			if (this.accel < 0f || this.speed < 1f)
			{
				light.intensity = Mathf.Lerp(light.intensity, 8f, 0.1f);
			}
			else
			{
				light.intensity = Mathf.Lerp(light.intensity, 0f, 0.1f);
			}
		}
		if (this.bikeSetting.automaticGear && this.currentGear == 1 && this.accel < 0f)
		{
			if (this.speed < 1f)
			{
				this.ShiftDown();
			}
		}
		else if (this.bikeSetting.automaticGear && this.currentGear == 0 && this.accel > 0f)
		{
			if (this.speed < 5f)
			{
				this.ShiftUp();
			}
		}
		else if (this.bikeSetting.automaticGear && this.motorRPM > this.bikeSetting.shiftUpRPM && this.accel > 0f && this.speed > 10f && !this.brake)
		{
			this.ShiftUp();
		}
		else if (this.bikeSetting.automaticGear && this.motorRPM < this.bikeSetting.shiftDownRPM && this.currentGear > 1)
		{
			this.ShiftDown();
		}
		if (this.speed < 1f)
		{
			this.Backward = true;
		}
		if (this.currentGear == 0 && this.Backward)
		{
			if (this.speed < this.bikeSetting.gears[0] * -10f)
			{
				this.accel = -this.accel;
			}
		}
		else
		{
			this.Backward = false;
		}
		this.wantedRPM = 5500f * this.accel * 0.1f + this.wantedRPM * 0.9f;
		float num = 0f;
		int num2 = 0;
		bool flag = false;
		int num3 = 0;
		foreach (BikeControl.WheelComponent wheelComponent in this.wheels)
		{
			WheelCollider collider = wheelComponent.collider;
			if (wheelComponent.drive)
			{
				if (!this.NeutralGear && this.brake && this.currentGear < 2)
				{
					num += this.accel * this.bikeSetting.idleRPM;
					if (num > 1f)
					{
						this.bikeSetting.shiftCentre.z = Mathf.PingPong(Time.time * (this.accel * 10f), 0.5f) - 0.25f;
					}
					else
					{
						this.bikeSetting.shiftCentre.z = 0f;
					}
				}
				else if (!this.NeutralGear)
				{
					num += collider.rpm;
				}
				else
				{
					num += this.bikeSetting.idleRPM * 2f * this.accel;
				}
				num2++;
			}
			if (this.crash)
			{
				wheelComponent.collider.enabled = false;
				wheelComponent.wheel.GetComponent<Collider>().enabled = true;
			}
			else
			{
				wheelComponent.collider.enabled = true;
				wheelComponent.wheel.GetComponent<Collider>().enabled = false;
			}
			if (this.brake || this.accel < 0f)
			{
				if (this.accel < 0f || (this.brake && wheelComponent == this.wheels[1]))
				{
					if (this.brake && this.accel > 0f)
					{
						this.slip = Mathf.Lerp(this.slip, this.bikeSetting.slipBrake, this.accel * 0.01f);
					}
					else if (this.speed > 1f)
					{
						this.slip = Mathf.Lerp(this.slip, 1f, 0.002f);
					}
					else
					{
						this.slip = Mathf.Lerp(this.slip, 1f, 0.02f);
					}
					this.wantedRPM = 0f;
					collider.brakeTorque = this.bikeSetting.brakePower;
					wheelComponent.rotation = this.w_rotate;
				}
			}
			else
			{
				WheelCollider wheelCollider = collider;
				float brakeTorque;
				if (this.accel == 0f)
				{
					float num4 = 3000f;
					collider.brakeTorque = num4;
					brakeTorque = num4;
				}
				else
				{
					float num4 = 0f;
					collider.brakeTorque = num4;
					brakeTorque = num4;
				}
				wheelCollider.brakeTorque = brakeTorque;
				this.slip = Mathf.Lerp(this.slip, 1f, 0.02f);
				this.w_rotate = wheelComponent.rotation;
			}
			WheelFrictionCurve wheelFrictionCurve = collider.forwardFriction;
			if (wheelComponent == this.wheels[1])
			{
				wheelFrictionCurve.stiffness = this.bikeSetting.stiffness / this.slip;
				collider.forwardFriction = wheelFrictionCurve;
				wheelFrictionCurve = collider.sidewaysFriction;
				wheelFrictionCurve.stiffness = this.bikeSetting.stiffness / this.slip;
				collider.sidewaysFriction = wheelFrictionCurve;
			}
			if (this.shift && this.currentGear > 1 && this.speed > 50f && this.shifmotor)
			{
				this.shifting = true;
				if (this.powerShift == 0f)
				{
					this.shifmotor = false;
				}
				this.powerShift = Mathf.MoveTowards(this.powerShift, 0f, Time.deltaTime * 10f);
				this.bikeSounds.nitro.volume = Mathf.Lerp(this.bikeSounds.nitro.volume, 1f, Time.deltaTime * 10f);
				if (!this.bikeSounds.nitro.isPlaying)
				{
					this.bikeSounds.nitro.GetComponent<AudioSource>().Play();
				}
				this.curTorque = ((this.powerShift <= 0f) ? this.bikeSetting.bikePower : this.bikeSetting.shiftPower);
				this.bikeParticles.shiftParticle1.emissionRate = Mathf.Lerp(this.bikeParticles.shiftParticle1.emissionRate, (float)((this.powerShift <= 0f) ? 0 : 50), Time.deltaTime * 10f);
				this.bikeParticles.shiftParticle2.emissionRate = Mathf.Lerp(this.bikeParticles.shiftParticle2.emissionRate, (float)((this.powerShift <= 0f) ? 0 : 50), Time.deltaTime * 10f);
			}
			else
			{
				this.shifting = false;
				if (this.powerShift > 20f)
				{
					this.shifmotor = true;
				}
				this.bikeSounds.nitro.volume = Mathf.MoveTowards(this.bikeSounds.nitro.volume, 0f, Time.deltaTime * 2f);
				if (this.bikeSounds.nitro.volume == 0f)
				{
					this.bikeSounds.nitro.Stop();
				}
				this.powerShift = Mathf.MoveTowards(this.powerShift, 100f, Time.deltaTime * 5f);
				this.curTorque = this.bikeSetting.bikePower;
				this.bikeParticles.shiftParticle1.emissionRate = Mathf.Lerp(this.bikeParticles.shiftParticle1.emissionRate, 0f, Time.deltaTime * 10f);
				this.bikeParticles.shiftParticle2.emissionRate = Mathf.Lerp(this.bikeParticles.shiftParticle2.emissionRate, 0f, Time.deltaTime * 10f);
			}
			wheelComponent.rotation = Mathf.Repeat(wheelComponent.rotation + Time.deltaTime * collider.rpm * 360f / 60f, 360f);
			wheelComponent.wheel.localRotation = Quaternion.Euler(wheelComponent.rotation, 0f, 0f);
			Vector3 localPosition = wheelComponent.axle.localPosition;
			WheelHit wheelHit;
			if (collider.GetGroundHit(out wheelHit) && (wheelComponent == this.wheels[1] || (wheelComponent == this.wheels[0] && this.Wheelie == 0f)))
			{
				if (this.bikeParticles.brakeParticlePrefab)
				{
					if (this.Particle[num3] == null)
					{
						this.Particle[num3] = UnityEngine.Object.Instantiate<GameObject>(this.bikeParticles.brakeParticlePrefab, wheelComponent.wheel.position, Quaternion.identity);
						this.Particle[num3].name = "WheelParticle";
						this.Particle[num3].transform.parent = base.transform;
						this.Particle[num3].AddComponent<AudioSource>();
					}
					ParticleSystem component = this.Particle[num3].GetComponent<ParticleSystem>();
					bool flag2 = false;
					for (int k = 0; k < this.bikeSetting.hitGround.Length; k++)
					{
					}
					if (flag2 && this.speed > 5f && !this.brake)
					{
						component.enableEmission = true;
						this.Particle[num3].GetComponent<AudioSource>().volume = 0.1f;
						if (!this.Particle[num3].GetComponent<AudioSource>().isPlaying)
						{
							this.Particle[num3].GetComponent<AudioSource>().Play();
						}
					}
					else if ((this.brake || Mathf.Abs(wheelHit.sidewaysSlip) > 0.6f) && this.speed > 1f)
					{
						if (this.accel < 0f || ((this.brake || Mathf.Abs(wheelHit.sidewaysSlip) > 0.2f) && wheelComponent == this.wheels[1]))
						{
							if (!this.Particle[num3].GetComponent<AudioSource>().isPlaying)
							{
								this.Particle[num3].GetComponent<AudioSource>().Play();
							}
							component.enableEmission = true;
							this.Particle[num3].GetComponent<AudioSource>().volume = Mathf.Clamp(this.speed / 20f, 0f, 0.6f);
						}
					}
					else
					{
						component.enableEmission = false;
						this.Particle[num3].GetComponent<AudioSource>().volume = Mathf.Lerp(this.Particle[num3].GetComponent<AudioSource>().volume, 0f, Time.deltaTime * 10f);
					}
				}
				localPosition.y -= Vector3.Dot(wheelComponent.wheel.position - wheelHit.point, base.transform.TransformDirection(0f, 1f, 0f)) - collider.radius;
				localPosition.y = Mathf.Clamp(localPosition.y, wheelComponent.startPos.y - this.bikeWheels.setting.Distance, wheelComponent.startPos.y + this.bikeWheels.setting.Distance);
				flag = (flag || wheelComponent.drive);
				if (!this.crash)
				{
					this.myRigidbody.angularDrag = 10f;
				}
				else
				{
					this.myRigidbody.angularDrag = 0f;
				}
				this.grounded = true;
				if (wheelComponent.collider.GetComponent<WheelSkidmarks>())
				{
					wheelComponent.collider.GetComponent<WheelSkidmarks>().enabled = true;
				}
			}
			else
			{
				this.grounded = false;
				if (wheelComponent.collider.GetComponent<WheelSkidmarks>())
				{
					wheelComponent.collider.GetComponent<WheelSkidmarks>().enabled = false;
				}
				if (this.Particle[num3] != null)
				{
					ParticleSystem component2 = this.Particle[num3].GetComponent<ParticleSystem>();
					component2.enableEmission = false;
				}
				localPosition.y = wheelComponent.startPos.y - this.bikeWheels.setting.Distance;
				this.myRigidbody.angularDrag = 0f;
				this.myRigidbody.AddForce(0f, -1000f, 0f);
			}
			num3++;
			wheelComponent.axle.localPosition = localPosition;
		}
		if (num2 > 1)
		{
			num /= (float)num2;
		}
		this.motorRPM = 0.95f * this.motorRPM + 0.05f * Mathf.Abs(num * this.bikeSetting.gears[this.currentGear]);
		if (this.motorRPM > 5500f)
		{
			this.motorRPM = 5200f;
		}
		int num5 = (int)(this.motorRPM / this.efficiencyTableStep);
		if (num5 >= this.efficiencyTable.Length)
		{
			num5 = this.efficiencyTable.Length - 1;
		}
		if (num5 < 0)
		{
			num5 = 0;
		}
		float num6 = this.curTorque * this.bikeSetting.gears[this.currentGear] * this.efficiencyTable[num5];
		foreach (BikeControl.WheelComponent wheelComponent2 in this.wheels)
		{
			WheelCollider collider2 = wheelComponent2.collider;
			if (wheelComponent2.drive)
			{
				if (Mathf.Abs(collider2.rpm) > Mathf.Abs(this.wantedRPM))
				{
					collider2.motorTorque = 0f;
				}
				else
				{
					float motorTorque = collider2.motorTorque;
					if (!this.brake && this.accel != 0f && !this.NeutralGear)
					{
						if ((this.speed < this.bikeSetting.LimitForwardSpeed && this.currentGear > 0) || (this.speed < this.bikeSetting.LimitBackwardSpeed && this.currentGear == 0))
						{
							collider2.motorTorque = motorTorque * 0.9f + num6 * 1f;
						}
						else
						{
							collider2.motorTorque = 0f;
							collider2.brakeTorque = 2000f;
						}
					}
					else
					{
						collider2.motorTorque = 0f;
					}
				}
			}
			float num7 = Mathf.Clamp(this.speed / this.bikeSetting.maxSteerAngle, 1f, this.bikeSetting.maxSteerAngle);
			collider2.steerAngle = this.steer * (wheelComponent2.maxSteer / num7);
		}
		if (this.audioSource != null)
		{
			float pitch = Mathf.Clamp(1f + (this.motorRPM - this.bikeSetting.idleRPM) / (this.bikeSetting.shiftUpRPM - this.bikeSetting.idleRPM), 1f, 10f);
			this.audioSource.pitch = pitch;
			this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, 0.15f + Mathf.Abs(this.accel), 0.0001f);
		}
	}

	private void OnDrawGizmos()
	{
		if (!this.bikeSetting.showNormalGizmos || Application.isPlaying)
		{
			return;
		}
		Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
		Gizmos.matrix = matrix;
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawCube(Vector3.up / 1.6f, new Vector3(0.5f, 1f, 2.5f));
		Gizmos.DrawSphere(this.bikeSetting.shiftCentre, 0.2f);
	}

	public ControlMode controlMode = ControlMode.simple;

	public bool activeControl;

	public BikeControl.BikeWheels bikeWheels;

	public BikeControl.BikeLights bikeLights;

	public BikeControl.BikeSounds bikeSounds;

	public BikeControl.BikeParticles bikeParticles;

	private GameObject[] Particle = new GameObject[4];

	public BikeControl.BikeSetting bikeSetting;

	private Quaternion SteerRotation;

	[HideInInspector]
	public bool grounded = true;

	private float MotorRotation;

	[HideInInspector]
	public bool crash;

	[HideInInspector]
	public float steer;

	[HideInInspector]
	public bool brake;

	private float slip;

	[HideInInspector]
	public bool Backward;

	[HideInInspector]
	public float steer2;

	private float accel;

	private bool shifmotor;

	[HideInInspector]
	public float curTorque = 100f;

	[HideInInspector]
	public float powerShift = 100f;

	[HideInInspector]
	public bool shift;

	private float flipRotate;

	[HideInInspector]
	public float speed;

	private float[] efficiencyTable = new float[]
	{
		0.6f,
		0.65f,
		0.7f,
		0.75f,
		0.8f,
		0.85f,
		0.9f,
		1f,
		1f,
		0.95f,
		0.8f,
		0.7f,
		0.6f,
		0.5f,
		0.45f,
		0.4f,
		0.36f,
		0.33f,
		0.3f,
		0.2f,
		0.1f,
		0.05f
	};

	private float efficiencyTableStep = 250f;

	private float shiftDelay;

	private AudioSource audioSource;

	[HideInInspector]
	public int currentGear;

	[HideInInspector]
	public bool NeutralGear = true;

	[HideInInspector]
	public float motorRPM;

	private float wantedRPM;

	private float w_rotate;

	private Rigidbody myRigidbody;

	private bool shifting;

	private float Wheelie;

	private Quaternion deltaRotation1;

	private Quaternion deltaRotation2;

	[HideInInspector]
	public float accelFwd;

	[HideInInspector]
	public float accelBack;

	[HideInInspector]
	public float steerAmount;

	private BikeControl.WheelComponent[] wheels;

	[Serializable]
	public class BikeWheels
	{
		public BikeControl.ConnectWheel wheels;

		public BikeControl.WheelSetting setting;
	}

	[Serializable]
	public class ConnectWheel
	{
		public Transform wheelFront;

		public Transform wheelBack;

		public Transform AxleFront;

		public Transform AxleBack;
	}

	[Serializable]
	public class WheelSetting
	{
		public float Radius = 0.3f;

		public float Weight = 1000f;

		public float Distance = 0.2f;
	}

	[Serializable]
	public class BikeLights
	{
		public Light[] brakeLights;
	}

	[Serializable]
	public class BikeSounds
	{
		public AudioSource lowCrash;

		public AudioSource nitro;

		public AudioSource switchGear;
	}

	[Serializable]
	public class BikeParticles
	{
		public GameObject brakeParticlePrefab;

		public ParticleSystem shiftParticle1;

		public ParticleSystem shiftParticle2;
	}

	[Serializable]
	public class HitGround
	{
		public string tag = "street";

		public bool grounded;

		public AudioClip brakeSound;

		public AudioClip groundSound;

		public Color brakeColor;
	}

	[Serializable]
	public class BikeSetting
	{
		public bool showNormalGizmos;

		public BikeControl.HitGround[] hitGround;

		public Transform bikerMan;

		public List<Transform> cameraSwitchView;

		public Transform MainBody;

		public Transform bikeSteer;

		public float maxWheelie = 40f;

		public float speedWheelie = 30f;

		public float slipBrake = 3f;

		public float springs = 35000f;

		public float dampers = 4000f;

		public float bikePower = 120f;

		public float shiftPower = 150f;

		public float brakePower = 8000f;

		public Vector3 shiftCentre = new Vector3(0f, -0.6f, 0f);

		public float maxSteerAngle = 30f;

		public float maxTurn = 1.5f;

		public float shiftDownRPM = 1500f;

		public float shiftUpRPM = 4000f;

		public float idleRPM = 700f;

		public float stiffness = 1f;

		public bool automaticGear = true;

		public float[] gears = new float[]
		{
			-10f,
			9f,
			6f,
			4.5f,
			3f,
			2.5f
		};

		public float LimitBackwardSpeed = 60f;

		public float LimitForwardSpeed = 220f;
	}

	private class WheelComponent
	{
		public Transform wheel;

		public Transform axle;

		public WheelCollider collider;

		public Vector3 startPos;

		public float rotation;

		public float maxSteer;

		public bool drive;

		public float pos_y;
	}
}
