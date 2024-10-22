using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Realistic Car Controller V3")]
[RequireComponent(typeof(Rigidbody))]
public class RCC_CarControllerV3 : MonoBehaviour
{
	private RCC_Settings RCCSettings
	{
		get
		{
			return RCC_Settings.Instance;
		}
	}

	public bool runEngineAtAwake
	{
		get
		{
			return this.RCCSettings.runEngineAtAwake;
		}
	}

	public bool autoReverse
	{
		get
		{
			return this.RCCSettings.autoReverse;
		}
	}

	public bool automaticGear
	{
		get
		{
			return this.RCCSettings.useAutomaticGear;
		}
	}

	private AudioClip[] gearShiftingClips
	{
		get
		{
			return this.RCCSettings.gearShiftingClips;
		}
	}

	private AudioClip[] crashClips
	{
		get
		{
			return this.RCCSettings.crashClips;
		}
	}

	private AudioClip reversingClip
	{
		get
		{
			return this.RCCSettings.reversingClip;
		}
	}

	private AudioClip windClip
	{
		get
		{
			return this.RCCSettings.windClip;
		}
	}

	private AudioClip brakeClip
	{
		get
		{
			return this.RCCSettings.brakeClip;
		}
	}

	private AudioClip NOSClip
	{
		get
		{
			return this.RCCSettings.NOSClip;
		}
	}

	private AudioClip turboClip
	{
		get
		{
			return this.RCCSettings.turboClip;
		}
	}

	private AudioClip blowClip
	{
		get
		{
			return this.RCCSettings.turboClip;
		}
	}

	internal float _gasInput
	{
		get
		{
			if (this.fuelInput <= 0.25f)
			{
				return 0f;
			}
			if (!this.automaticGear || this.semiAutomaticGear)
			{
				if (!this.changingGear && !this.cutGas)
				{
					return Mathf.Clamp01(this.gasInput);
				}
				return 0f;
			}
			else
			{
				if (!this.changingGear && !this.cutGas)
				{
					return (this.direction != 1) ? Mathf.Clamp01(this.brakeInput) : Mathf.Clamp01(this.gasInput);
				}
				return 0f;
			}
		}
		set
		{
			this.gasInput = value;
		}
	}

	internal float _brakeInput
	{
		get
		{
			if (!this.automaticGear || this.semiAutomaticGear)
			{
				return Mathf.Clamp01(this.brakeInput);
			}
			if (!this.cutGas)
			{
				return (this.direction != 1) ? Mathf.Clamp01(this.gasInput) : Mathf.Clamp01(this.brakeInput);
			}
			return 0f;
		}
		set
		{
			this.brakeInput = value;
		}
	}

	internal float _boostInput
	{
		get
		{
			if (this.useNOS && this.NoS > 5f && this._gasInput >= 0.5f)
			{
				return this.boostInput;
			}
			return 1f;
		}
		set
		{
			this.boostInput = value;
		}
	}

	public GameObject contactSparkle
	{
		get
		{
			return this.RCCSettings.contactParticles;
		}
	}

	private void Awake()
	{
		Time.fixedDeltaTime = this.RCCSettings.fixedTimeStep;
		this.rigid = base.GetComponent<Rigidbody>();
		this.rigid.maxAngularVelocity = this.RCCSettings.maxAngularVelocity;
		this.rigid.drag = 0.05f;
		this.rigid.angularDrag = 0.5f;
		this.allWheelColliders = base.GetComponentsInChildren<RCC_WheelCollider>();
		this.FrontLeftRCCWheelCollider = this.FrontLeftWheelCollider.GetComponent<RCC_WheelCollider>();
		this.FrontRightRCCWheelCollider = this.FrontRightWheelCollider.GetComponent<RCC_WheelCollider>();
		this.FrontLeftWheelCollider.wheelModel = this.FrontLeftWheelTransform;
		this.FrontRightWheelCollider.wheelModel = this.FrontRightWheelTransform;
		this.RearLeftWheelCollider.wheelModel = this.RearLeftWheelTransform;
		this.RearRightWheelCollider.wheelModel = this.RearRightWheelTransform;
		for (int i = 0; i < this.ExtraRearWheelsCollider.Length; i++)
		{
			this.ExtraRearWheelsCollider[i].wheelModel = this.ExtraRearWheelsTransform[i];
		}
		this.orgSteerAngle = this.steerAngle;
		this.allAudioSources = new GameObject("All Audio Sources");
		this.allAudioSources.transform.SetParent(base.transform, false);
		this.allContactParticles = new GameObject("All Contact Particles");
		this.allContactParticles.transform.SetParent(base.transform, false);
		switch (this.RCCSettings.behaviorType)
		{
		case RCC_Settings.BehaviorType.Simulator:
			this.antiRollFrontHorizontal = Mathf.Clamp(this.antiRollFrontHorizontal, 2500f, float.PositiveInfinity);
			this.antiRollRearHorizontal = Mathf.Clamp(this.antiRollRearHorizontal, 2500f, float.PositiveInfinity);
			break;
		case RCC_Settings.BehaviorType.Racing:
			this.steeringHelper = true;
			this.tractionHelper = true;
			this.steerHelperStrength = Mathf.Clamp(this.steerHelperStrength, 0.25f, 1f);
			this.tractionHelperStrength = Mathf.Clamp(this.tractionHelperStrength, 0.25f, 1f);
			this.antiRollFrontHorizontal = Mathf.Clamp(this.antiRollFrontHorizontal, 10000f, float.PositiveInfinity);
			this.antiRollRearHorizontal = Mathf.Clamp(this.antiRollRearHorizontal, 10000f, float.PositiveInfinity);
			break;
		case RCC_Settings.BehaviorType.SemiArcade:
			this.steeringHelper = true;
			this.tractionHelper = true;
			this.ABS = false;
			this.ESP = false;
			this.TCS = false;
			this.steerHelperStrength = Mathf.Clamp(this.steerHelperStrength, 0.25f, 1f);
			this.tractionHelperStrength = Mathf.Clamp(this.tractionHelperStrength, 0.25f, 1f);
			this.antiRollFrontHorizontal = Mathf.Clamp(this.antiRollFrontHorizontal, 10000f, float.PositiveInfinity);
			this.antiRollRearHorizontal = Mathf.Clamp(this.antiRollRearHorizontal, 10000f, float.PositiveInfinity);
			break;
		case RCC_Settings.BehaviorType.Drift:
			this.steeringHelper = false;
			this.tractionHelper = false;
			this.ABS = false;
			this.ESP = false;
			this.TCS = false;
			this.highspeedsteerAngle = Mathf.Clamp(this.highspeedsteerAngle, 40f, 50f);
			this.highspeedsteerAngleAtspeed = Mathf.Clamp(this.highspeedsteerAngleAtspeed, 100f, this.maxspeed);
			this.applyCounterSteering = true;
			this.engineTorque = Mathf.Clamp(this.engineTorque, 5000f, float.PositiveInfinity);
			this.antiRollFrontHorizontal = Mathf.Clamp(this.antiRollFrontHorizontal, 3500f, float.PositiveInfinity);
			this.antiRollRearHorizontal = Mathf.Clamp(this.antiRollRearHorizontal, 3500f, float.PositiveInfinity);
			this.gearShiftingDelay = Mathf.Clamp(this.gearShiftingDelay, 0f, 0.15f);
			break;
		case RCC_Settings.BehaviorType.Fun:
			this.steeringHelper = false;
			this.tractionHelper = false;
			this.ABS = false;
			this.ESP = false;
			this.TCS = false;
			this.highspeedsteerAngle = Mathf.Clamp(this.highspeedsteerAngle, 30f, 50f);
			this.highspeedsteerAngleAtspeed = Mathf.Clamp(this.highspeedsteerAngleAtspeed, 100f, this.maxspeed);
			this.antiRollFrontHorizontal = Mathf.Clamp(this.antiRollFrontHorizontal, 50000f, float.PositiveInfinity);
			this.antiRollRearHorizontal = Mathf.Clamp(this.antiRollRearHorizontal, 50000f, float.PositiveInfinity);
			this.gearShiftingDelay = Mathf.Clamp(this.gearShiftingDelay, 0f, 0.1f);
			break;
		}
	}

	private void Start()
	{
		if (base.GetComponent<RCC_AICarController>())
		{
			this.AIController = true;
		}
		if (this.autoGenerateGearCurves)
		{
			this.TorqueCurve();
		}
		this.SoundsInitialize();
		if (this.useDamage)
		{
			this.DamageInit();
		}
		if (this.runEngineAtAwake)
		{
			this.KillOrStartEngine();
		}
		this.ChassisJoint();
		this.rigid.centerOfMass = base.transform.InverseTransformPoint(this.COM.transform.position);
		if (this.canControl)
		{
			if (RCC_Settings.Instance.controllerType == RCC_Settings.ControllerType.Mobile)
			{
				UnityEngine.Object.FindObjectOfType<RCC_MobileButtons>().GetVehicles();
			}
			if (UnityEngine.Object.FindObjectOfType<RCC_DashboardInputs>())
			{
				UnityEngine.Object.FindObjectOfType<RCC_DashboardInputs>().GetVehicle(this);
			}
		}
	}

	private void OnEnable()
	{
		base.StartCoroutine("ReEnable");
	}

	private IEnumerator ReEnable()
	{
		if (!this.chassis.GetComponentInParent<ConfigurableJoint>())
		{
			yield return null;
		}
		GameObject _joint = this.chassis.GetComponentInParent<ConfigurableJoint>().gameObject;
		_joint.SetActive(false);
		yield return new WaitForFixedUpdate();
		_joint.SetActive(true);
		this.rigid.centerOfMass = base.transform.InverseTransformPoint(this.COM.transform.position);
		this.changingGear = false;
		yield break;
	}

	public void CreateWheelColliders()
	{
		List<Transform> list = new List<Transform>();
		list.Add(this.FrontLeftWheelTransform);
		list.Add(this.FrontRightWheelTransform);
		list.Add(this.RearLeftWheelTransform);
		list.Add(this.RearRightWheelTransform);
		if (list != null && list[0] == null)
		{
			UnityEngine.Debug.LogError("You haven't choose your Wheel Models. Please select all of your Wheel Models before creating Wheel Colliders. Script needs to know their sizes and positions, aye?");
			return;
		}
		base.transform.rotation = Quaternion.identity;
		GameObject gameObject = new GameObject("Wheel Colliders");
		gameObject.transform.SetParent(base.transform, false);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		foreach (Transform transform in list)
		{
			GameObject gameObject2 = new GameObject(transform.transform.name);
			gameObject2.transform.position = transform.transform.position;
			gameObject2.transform.rotation = base.transform.rotation;
			gameObject2.transform.name = transform.transform.name;
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.AddComponent<WheelCollider>();
			Bounds bounds = default(Bounds);
			Renderer[] componentsInChildren = transform.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != base.GetComponent<Renderer>() && renderer.bounds.size.z > bounds.size.z)
				{
					bounds = renderer.bounds;
				}
			}
			gameObject2.GetComponent<WheelCollider>().radius = bounds.extents.y / base.transform.localScale.y;
			gameObject2.AddComponent<RCC_WheelCollider>();
			JointSpring suspensionSpring = gameObject2.GetComponent<WheelCollider>().suspensionSpring;
			suspensionSpring.spring = 40000f;
			suspensionSpring.damper = 2000f;
			suspensionSpring.targetPosition = 0.4f;
			gameObject2.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			gameObject2.GetComponent<WheelCollider>().suspensionDistance = 0.2f;
			gameObject2.GetComponent<WheelCollider>().forceAppPointDistance = 0.1f;
			gameObject2.GetComponent<WheelCollider>().mass = 40f;
			gameObject2.GetComponent<WheelCollider>().wheelDampingRate = 1f;
			WheelFrictionCurve sidewaysFriction = gameObject2.GetComponent<WheelCollider>().sidewaysFriction;
			WheelFrictionCurve forwardFriction = gameObject2.GetComponent<WheelCollider>().forwardFriction;
			gameObject2.transform.localPosition = new Vector3(gameObject2.transform.localPosition.x, gameObject2.transform.localPosition.y + gameObject2.GetComponent<WheelCollider>().suspensionDistance, gameObject2.transform.localPosition.z);
			forwardFriction.extremumSlip = 0.2f;
			forwardFriction.extremumValue = 1f;
			forwardFriction.asymptoteSlip = 0.8f;
			forwardFriction.asymptoteValue = 0.75f;
			forwardFriction.stiffness = 1.5f;
			sidewaysFriction.extremumSlip = 0.25f;
			sidewaysFriction.extremumValue = 1f;
			sidewaysFriction.asymptoteSlip = 0.5f;
			sidewaysFriction.asymptoteValue = 0.75f;
			sidewaysFriction.stiffness = 1.5f;
			gameObject2.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction;
			gameObject2.GetComponent<WheelCollider>().forwardFriction = forwardFriction;
		}
		RCC_WheelCollider[] array2 = new RCC_WheelCollider[list.Count];
		array2 = base.GetComponentsInChildren<RCC_WheelCollider>();
		this.FrontLeftWheelCollider = array2[0];
		this.FrontRightWheelCollider = array2[1];
		this.RearLeftWheelCollider = array2[2];
		this.RearRightWheelCollider = array2[3];
	}

	private void SoundsInitialize()
	{
		this.engineSoundOn = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Engine Sound On AudioSource", 5f, 100f, 0f, this.engineClipOn, true, true, false);
		this.engineSoundOff = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Engine Sound Off AudioSource", 5f, 100f, 0f, this.engineClipOff, true, true, false);
		this.engineSoundIdle = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Engine Sound Idle AudioSource", 5f, 100f, 0f, this.engineClipIdle, true, true, false);
		this.reversingSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Reverse Sound AudioSource", 5f, 10f, 0f, this.reversingClip, true, false, false);
		this.windSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Wind Sound AudioSource", 5f, 10f, 0f, this.windClip, true, true, false);
		this.brakeSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Brake Sound AudioSource", 5f, 10f, 0f, this.brakeClip, true, true, false);
		if (this.useNOS)
		{
			this.NOSSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "NOS Sound AudioSource", 5f, 10f, 1f, this.NOSClip, true, false, false);
		}
		if (this.useNOS || this.useTurbo)
		{
			this.blowSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "NOS Blow", 3f, 10f, 1f, null, false, false, false);
		}
		if (this.useTurbo)
		{
			this.turboSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Turbo Sound AudioSource", 0.1f, 0.5f, 0f, this.turboClip, true, true, false);
			RCC_CreateAudioSource.NewHighPassFilter(this.turboSound, 10000f, 10);
		}
	}

	public void KillOrStartEngine()
	{
		if (this.engineRunning && !this.engineStarting)
		{
			this.engineRunning = false;
			this.fuelInput = 0f;
		}
		else if (!this.engineStarting)
		{
			base.StartCoroutine("StartEngine");
		}
	}

	public IEnumerator StartEngine()
	{
		this.engineRunning = false;
		this.engineStarting = true;
		this.engineStartSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Engine Start AudioSource", 5f, 10f, 1f, this.engineStartClip, false, true, true);
		if (this.engineStartSound.isPlaying)
		{
			this.engineStartSound.Play();
		}
		yield return new WaitForSeconds(1f);
		this.engineRunning = true;
		this.fuelInput = 1f;
		yield return new WaitForSeconds(1f);
		this.engineStarting = false;
		yield break;
	}

	private void ChassisJoint()
	{
		GameObject gameObject = new GameObject("Colliders");
		gameObject.transform.SetParent(base.transform, false);
		Transform[] componentsInChildren = this.chassis.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (transform.gameObject.activeSelf && transform.GetComponent<Collider>())
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject, transform.transform.position, transform.transform.rotation);
				gameObject2.transform.SetParent(gameObject.transform, true);
				gameObject2.transform.localScale = transform.lossyScale;
				Component[] components = gameObject2.GetComponents(typeof(Component));
				foreach (Component component in components)
				{
					if (!(component is Transform) && !(component is Collider))
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			}
		}
		GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("RCCAssets/Chassis Joint", typeof(GameObject)), Vector3.zero, Quaternion.identity);
		gameObject3.transform.SetParent(base.transform, false);
		gameObject3.GetComponent<ConfigurableJoint>().connectedBody = this.rigid;
		this.chassis.transform.SetParent(gameObject3.transform, false);
		Collider[] componentsInChildren2 = this.chassis.GetComponentsInChildren<Collider>();
		foreach (Collider obj in componentsInChildren2)
		{
			UnityEngine.Object.Destroy(obj);
		}
		RCC_Settings.BehaviorType behaviorType = this.RCCSettings.behaviorType;
		if (behaviorType == RCC_Settings.BehaviorType.Fun)
		{
			SoftJointLimit softJointLimit = default(SoftJointLimit);
			softJointLimit.limit = -10f;
			gameObject3.GetComponent<ConfigurableJoint>().lowAngularXLimit = softJointLimit;
			softJointLimit.limit = 10f;
			gameObject3.GetComponent<ConfigurableJoint>().linearLimit = softJointLimit;
			gameObject3.GetComponent<ConfigurableJoint>().highAngularXLimit = softJointLimit;
			gameObject3.GetComponent<ConfigurableJoint>().angularYLimit = softJointLimit;
			gameObject3.GetComponent<ConfigurableJoint>().angularZLimit = softJointLimit;
			JointDrive jointDrive = default(JointDrive);
			jointDrive.positionSpring = 300f;
			jointDrive.positionDamper = 5f;
			jointDrive.maximumForce = gameObject3.GetComponent<ConfigurableJoint>().xDrive.maximumForce;
			gameObject3.GetComponent<ConfigurableJoint>().xDrive = jointDrive;
			gameObject3.GetComponent<ConfigurableJoint>().yDrive = jointDrive;
			gameObject3.GetComponent<ConfigurableJoint>().zDrive = jointDrive;
			gameObject3.GetComponent<ConfigurableJoint>().angularXDrive = jointDrive;
			gameObject3.GetComponent<ConfigurableJoint>().angularYZDrive = jointDrive;
			gameObject3.GetComponent<Rigidbody>().centerOfMass = base.transform.InverseTransformPoint(new Vector3(gameObject3.transform.position.x, gameObject3.transform.position.y + 1f, gameObject3.transform.position.z));
		}
	}

	private void DamageInit()
	{
		if (this.deformableMeshFilters.Length == 0)
		{
			MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
			List<MeshFilter> list = new List<MeshFilter>();
			foreach (MeshFilter meshFilter in componentsInChildren)
			{
				if (!meshFilter.transform.IsChildOf(this.FrontLeftWheelTransform) && !meshFilter.transform.IsChildOf(this.FrontRightWheelTransform) && !meshFilter.transform.IsChildOf(this.RearLeftWheelTransform) && !meshFilter.transform.IsChildOf(this.RearRightWheelTransform))
				{
					list.Add(meshFilter);
				}
			}
			this.deformableMeshFilters = list.ToArray();
		}
		this.LoadOriginalMeshData();
		if (this.contactSparkle)
		{
			for (int j = 0; j < this.maximumContactSparkle; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.contactSparkle, base.transform.position, Quaternion.identity);
				gameObject.transform.SetParent(this.allContactParticles.transform);
				this.contactSparkeList.Add(gameObject.GetComponent<ParticleSystem>());
                bool  moduleEnabled = false;
                var emission = gameObject.GetComponent<ParticleSystem>().emission;
                emission.enabled = moduleEnabled;
                //.emission.enabled = moduleEnabled;
			}
		}
	}

	private void LoadOriginalMeshData()
	{
		this.originalMeshData = new RCC_CarControllerV3.originalMeshVerts[this.deformableMeshFilters.Length];
		for (int i = 0; i < this.deformableMeshFilters.Length; i++)
		{
			this.originalMeshData[i].meshVerts = this.deformableMeshFilters[i].mesh.vertices;
		}
	}

	private void Damage()
	{
		if (!this.sleep && this.repairNow)
		{
			this.sleep = true;
			for (int i = 0; i < this.deformableMeshFilters.Length; i++)
			{
				Vector3[] vertices = this.deformableMeshFilters[i].mesh.vertices;
				if (this.originalMeshData == null)
				{
					this.LoadOriginalMeshData();
				}
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] += (this.originalMeshData[i].meshVerts[j] - vertices[j]) * (Time.deltaTime * 2f);
					if ((this.originalMeshData[i].meshVerts[j] - vertices[j]).magnitude >= this.minimumVertDistanceForDamagedMesh)
					{
						this.sleep = false;
					}
				}
				this.deformableMeshFilters[i].mesh.vertices = vertices;
				this.deformableMeshFilters[i].mesh.RecalculateNormals();
				this.deformableMeshFilters[i].mesh.RecalculateBounds();
			}
			if (this.sleep)
			{
				this.repairNow = false;
			}
		}
	}

	private void DeformMesh(Mesh mesh, Vector3[] originalMesh, Collision collision, float cos, Transform meshTransform, Quaternion rot)
	{
		Vector3[] vertices = mesh.vertices;
		foreach (ContactPoint contactPoint in collision.contacts)
		{
			Vector3 a = meshTransform.InverseTransformPoint(contactPoint.point);
			for (int j = 0; j < vertices.Length; j++)
			{
				if ((a - vertices[j]).magnitude < this.damageRadius)
				{
					vertices[j] += rot * (this.localVector * (this.damageRadius - (a - vertices[j]).magnitude) / this.damageRadius * cos + UnityEngine.Random.onUnitSphere * (this.randomizeVertices / 500f));
					if (this.maximumDamage > 0f && (vertices[j] - originalMesh[j]).magnitude > this.maximumDamage)
					{
						vertices[j] = originalMesh[j] + (vertices[j] - originalMesh[j]).normalized * this.maximumDamage;
					}
				}
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	private void CollisionParticles(Vector3 contactPoint)
	{
		for (int i = 0; i < this.contactSparkeList.Count; i++)
		{
			if (this.contactSparkeList[i].isPlaying)
			{
				return;
			}
            bool moduleEnabled = true;
            var emission = this.contactSparkeList[i].emission;
            emission.enabled = moduleEnabled;
            this.contactSparkeList[i].transform.position = contactPoint;
			emission.enabled = moduleEnabled;
			this.contactSparkeList[i].Play();
		}
	}

	private void Update()
	{
		if (this.canControl)
		{
			if (!this.AIController)
			{
				this.Inputs();
			}
			this.GearBox();
			this.Clutch();
		}
		else if (!this.AIController)
		{
		}
		this.Turbo();
		this.Sounds();
		this.ResetCar();
		if (this.useDamage)
		{
			this.Damage();
		}
		this.indicatorTimer += Time.deltaTime;
	}

	private void Inputs()
	{
		RCC_Settings.ControllerType controllerType = this.RCCSettings.controllerType;
		if (controllerType == RCC_Settings.ControllerType.Keyboard)
		{
			this.gasInput = UnityEngine.Input.GetAxis(this.RCCSettings.verticalInput);
			this.brakeInput = Mathf.Clamp01(-Input.GetAxis(this.RCCSettings.verticalInput));
			this.handbrakeInput = ((!Input.GetKey(this.RCCSettings.handbrakeKB)) ? 0f : 1f);
			this.steerInput = UnityEngine.Input.GetAxis(this.RCCSettings.horizontalInput);
			this.boostInput = ((!Input.GetKey(this.RCCSettings.boostKB)) ? 1f : 2.5f);
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.lowBeamHeadlightsKB))
			{
				this.lowBeamHeadLightsOn = !this.lowBeamHeadLightsOn;
			}
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.highBeamHeadlightsKB))
			{
				this.highBeamHeadLightsOn = true;
			}
			else if (UnityEngine.Input.GetKeyUp(this.RCCSettings.highBeamHeadlightsKB))
			{
				this.highBeamHeadLightsOn = false;
			}
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.startEngineKB))
			{
				this.KillOrStartEngine();
			}
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.rightIndicatorKB))
			{
				if (this.indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Right)
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Right;
				}
				else
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}
			}
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.leftIndicatorKB))
			{
				if (this.indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Left)
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Left;
				}
				else
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}
			}
			if (UnityEngine.Input.GetKeyDown(this.RCCSettings.hazardIndicatorKB))
			{
				if (this.indicatorsOn != RCC_CarControllerV3.IndicatorsOn.All)
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.All;
				}
				else
				{
					this.indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}
			}
			if (!this.automaticGear)
			{
				if (this.currentGear < this.totalGears - 1 && !this.changingGear && UnityEngine.Input.GetKeyDown(this.RCCSettings.shiftGearUp))
				{
					if (this.direction != -1)
					{
						base.StartCoroutine("ChangingGear", this.currentGear + 1);
					}
					else
					{
						base.StartCoroutine("ChangingGear", 0);
					}
				}
				if (this.currentGear >= 0 && UnityEngine.Input.GetKeyDown(this.RCCSettings.shiftGearDown))
				{
					base.StartCoroutine("ChangingGear", this.currentGear - 1);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		this.Engine();
		this.Braking();
		this.AntiRollBars();
		this.DriftVariables();
		this.RevLimiter();
		this.NOS();
		this.ApplySteering(this.FrontLeftWheelCollider);
		this.ApplySteering(this.FrontRightWheelCollider);
		if (this.steeringHelper)
		{
			this.SteerHelper();
		}
		if (this.tractionHelper)
		{
			this.TractionHelper();
		}
		if (this.ESP)
		{
			this.ESPCheck(this.rigid.angularVelocity.y, this.FrontLeftWheelCollider.steerAngle);
		}
		if (this.autoGenerateGearCurves)
		{
			this.TorqueCurve();
		}
		if (this.RCCSettings.behaviorType == RCC_Settings.BehaviorType.Drift && this.RearLeftWheelCollider.isGrounded)
		{
			this.rigid.angularVelocity = new Vector3(this.rigid.angularVelocity.x, this.rigid.angularVelocity.y + (float)this.direction * this.steerInput / 30f, this.rigid.angularVelocity.z);
		}
		if ((this.RCCSettings.behaviorType == RCC_Settings.BehaviorType.SemiArcade || this.RCCSettings.behaviorType == RCC_Settings.BehaviorType.Fun) && this.RearLeftWheelCollider.isGrounded)
		{
			this.rigid.angularVelocity = new Vector3(this.rigid.angularVelocity.x, (float)this.direction * (this.steerInput * Mathf.Lerp(0f, 2f, this.speed / 20f)), this.rigid.angularVelocity.z);
		}
	}

	private void Engine()
	{
		this.speed = this.rigid.velocity.magnitude * 3.6f;
		this.steerAngle = Mathf.Lerp(this.orgSteerAngle, this.highspeedsteerAngle, this.speed / this.highspeedsteerAngleAtspeed);
		if (this.SteeringWheel)
		{
			this.SteeringWheel.transform.rotation = base.transform.rotation * Quaternion.Euler(20f, 0f, this.FrontLeftWheelCollider.steerAngle * -6f);
		}
		if (this.rigid.velocity.magnitude < 0.01f && Mathf.Abs(this.steerInput) < 0.01f && Mathf.Abs(this._gasInput) < 0.01f && Mathf.Abs(this.rigid.angularVelocity.magnitude) < 0.01f)
		{
			this.sleepingRigid = true;
		}
		else
		{
			this.sleepingRigid = false;
		}
		this.rawEngineRPM = Mathf.Clamp(Mathf.MoveTowards(this.rawEngineRPM, this.maxEngineRPM * 1.1f * Mathf.Clamp01(Mathf.Lerp(0f, 1f, (1f - this.clutchInput) * ((this.RearLeftWheelCollider.wheelRPMToSpeed + this.RearRightWheelCollider.wheelRPMToSpeed) * (float)this.direction / 2f / this.gearSpeed[this.currentGear])) + (this._gasInput * this.clutchInput + this.idleInput) * this.fuelInput), this.engineInertia * 100f), 0f, this.maxEngineRPM * 1.1f);
		this.engineRPM = Mathf.Lerp(this.engineRPM, this.rawEngineRPM, Mathf.Lerp(Time.fixedDeltaTime * 5f, Time.fixedDeltaTime * 50f, this.rawEngineRPM / this.maxEngineRPM));
		if (this.autoReverse)
		{
			this.canGoReverseNow = true;
		}
		else if (this._brakeInput < 0.1f && this.speed < 5f)
		{
			this.canGoReverseNow = true;
		}
		else if (this._brakeInput > 0f && base.transform.InverseTransformDirection(this.rigid.velocity).z > 1f)
		{
			this.canGoReverseNow = false;
		}
		switch (this._wheelTypeChoise)
		{
		case RCC_CarControllerV3.WheelType.FWD:
			this.ApplyMotorTorque(this.FrontLeftWheelCollider, this.engineTorque);
			this.ApplyMotorTorque(this.FrontRightWheelCollider, this.engineTorque);
			break;
		case RCC_CarControllerV3.WheelType.RWD:
			this.ApplyMotorTorque(this.RearLeftWheelCollider, this.engineTorque);
			this.ApplyMotorTorque(this.RearRightWheelCollider, this.engineTorque);
			break;
		case RCC_CarControllerV3.WheelType.AWD:
			this.ApplyMotorTorque(this.FrontLeftWheelCollider, this.engineTorque / 2f);
			this.ApplyMotorTorque(this.FrontRightWheelCollider, this.engineTorque / 2f);
			this.ApplyMotorTorque(this.RearLeftWheelCollider, this.engineTorque / 2f);
			this.ApplyMotorTorque(this.RearRightWheelCollider, this.engineTorque / 2f);
			break;
		case RCC_CarControllerV3.WheelType.BIASED:
			this.ApplyMotorTorque(this.FrontLeftWheelCollider, this.engineTorque * (100f - this.biasedWheelTorque) / 100f);
			this.ApplyMotorTorque(this.FrontRightWheelCollider, this.engineTorque * (100f - this.biasedWheelTorque) / 100f);
			this.ApplyMotorTorque(this.RearLeftWheelCollider, this.engineTorque * this.biasedWheelTorque / 100f);
			this.ApplyMotorTorque(this.RearRightWheelCollider, this.engineTorque * this.biasedWheelTorque / 100f);
			break;
		}
		if (this.ExtraRearWheelsCollider.Length > 0 && this.applyEngineTorqueToExtraRearWheelColliders)
		{
			for (int i = 0; i < this.ExtraRearWheelsCollider.Length; i++)
			{
				this.ApplyMotorTorque(this.ExtraRearWheelsCollider[i], this.engineTorque);
			}
		}
	}

	private void Sounds()
	{
		this.windSound.volume = Mathf.Lerp(0f, this.RCCSettings.maxWindSoundVolume, this.speed / 300f);
		this.windSound.pitch = UnityEngine.Random.Range(0.9f, 1f);
		if (this.direction == 1)
		{
			this.brakeSound.volume = Mathf.Lerp(0f, this.RCCSettings.maxBrakeSoundVolume, Mathf.Clamp01((this.FrontLeftWheelCollider.wheelCollider.brakeTorque + this.FrontRightWheelCollider.wheelCollider.brakeTorque) / (this.brake * 2f)) * Mathf.Lerp(0f, 1f, this.FrontLeftWheelCollider.rpm / 50f));
		}
		else
		{
			this.brakeSound.volume = 0f;
		}
	}

	private void ApplyMotorTorque(RCC_WheelCollider wc, float torque)
	{
		if (this.TCS)
		{
			WheelHit wheelHit;
			wc.wheelCollider.GetGroundHit(out wheelHit);
			if (Mathf.Abs(wc.rpm) >= 100f)
			{
				if (wheelHit.forwardSlip > 0.25f)
				{
					this.TCSAct = true;
					torque -= Mathf.Clamp(torque * wheelHit.forwardSlip * this.TCSStrength, 0f, this.engineTorque);
				}
				else
				{
					this.TCSAct = false;
					torque += Mathf.Clamp(torque * wheelHit.forwardSlip * this.TCSStrength, -this.engineTorque, 0f);
				}
			}
			else
			{
				this.TCSAct = false;
			}
		}
		if (this.OverTorque())
		{
			torque = 0f;
		}
		wc.wheelCollider.motorTorque = torque * (1f - this.clutchInput) * this._boostInput * this._gasInput * (this.engineTorqueCurve[this.currentGear].Evaluate(wc.wheelRPMToSpeed * (float)this.direction) * (float)this.direction);
		this.ApplyEngineSound(wc.wheelCollider.motorTorque);
	}

	private void ESPCheck(float velocity, float steering)
	{
		WheelHit wheelHit;
		this.FrontLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
		WheelHit wheelHit2;
		this.FrontRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit2);
		this.frontSlip = wheelHit.sidewaysSlip + wheelHit2.sidewaysSlip;
		WheelHit wheelHit3;
		this.RearLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit3);
		WheelHit wheelHit4;
		this.RearRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit4);
		this.rearSlip = wheelHit3.sidewaysSlip + wheelHit4.sidewaysSlip;
		if (Mathf.Abs(this.frontSlip) >= this.ESPThreshold)
		{
			this.overSteering = true;
		}
		else
		{
			this.overSteering = false;
		}
		if (Mathf.Abs(this.rearSlip) >= this.ESPThreshold && !this.overSteering)
		{
			this.underSteering = true;
		}
		else
		{
			this.underSteering = false;
		}
		if (this.underSteering || this.overSteering)
		{
			this.ESPAct = true;
		}
		else
		{
			this.ESPAct = false;
		}
		if (Mathf.Abs(this.frontSlip) < this.ESPThreshold || Math.Abs(this.rearSlip) < this.ESPThreshold)
		{
			return;
		}
		if (this.underSteering)
		{
			this.ApplyBrakeTorque(this.RearLeftWheelCollider, this.brake * this.ESPStrength * Mathf.Clamp(this.frontSlip, 0f, float.PositiveInfinity));
			this.ApplyBrakeTorque(this.RearRightWheelCollider, this.brake * this.ESPStrength * Mathf.Clamp(-this.frontSlip, 0f, float.PositiveInfinity));
		}
		if (this.overSteering)
		{
			this.ApplyBrakeTorque(this.FrontLeftWheelCollider, this.brake * this.ESPStrength * Mathf.Clamp(-this.rearSlip, 0f, float.PositiveInfinity));
			this.ApplyBrakeTorque(this.FrontRightWheelCollider, this.brake * this.ESPStrength * Mathf.Clamp(this.rearSlip, 0f, float.PositiveInfinity));
		}
	}

	private void ApplyBrakeTorque(RCC_WheelCollider wc, float brake)
	{
		if (this.ABS && this.handbrakeInput <= 0.1f)
		{
			WheelHit wheelHit;
			wc.wheelCollider.GetGroundHit(out wheelHit);
			if (Mathf.Abs(wheelHit.forwardSlip) * Mathf.Clamp01(brake) >= this.ABSThreshold)
			{
				this.ABSAct = true;
				brake = 0f;
			}
			else
			{
				this.ABSAct = false;
			}
		}
		wc.wheelCollider.brakeTorque = brake;
	}

	private void ApplySteering(RCC_WheelCollider wc)
	{
		if (this.applyCounterSteering)
		{
			wc.wheelCollider.steerAngle = Mathf.Clamp(this.steerAngle * (this.steerInput + this.driftAngle), -this.steerAngle, this.steerAngle);
		}
		else
		{
			wc.wheelCollider.steerAngle = Mathf.Clamp(this.steerAngle * this.steerInput, -this.steerAngle, this.steerAngle);
		}
	}

	private void ApplyEngineSound(float input)
	{
		if (!this.engineRunning)
		{
			this.engineSoundOn.pitch = Mathf.Lerp(this.engineSoundOn.pitch, 0f, Time.deltaTime * 50f);
			this.engineSoundOff.pitch = Mathf.Lerp(this.engineSoundOff.pitch, 0f, Time.deltaTime * 50f);
			this.engineSoundIdle.pitch = Mathf.Lerp(this.engineSoundOff.pitch, 0f, Time.deltaTime * 50f);
			if (this.engineSoundOn.pitch <= 0.1f && this.engineSoundOff.pitch <= 0.1f && this.engineSoundIdle.pitch <= 0.1f)
			{
				this.engineSoundOn.Stop();
				this.engineSoundOff.Stop();
				this.engineSoundIdle.Stop();
				return;
			}
		}
		else
		{
			if (!this.engineSoundOn.isPlaying)
			{
				this.engineSoundOn.Play();
			}
			if (!this.engineSoundOff.isPlaying)
			{
				this.engineSoundOff.Play();
			}
			if (!this.engineSoundIdle.isPlaying)
			{
				this.engineSoundIdle.Play();
			}
		}
		if (this.engineSoundOn)
		{
			this.engineSoundOn.volume = this._gasInput;
			this.engineSoundOn.pitch = Mathf.Lerp(this.engineSoundOn.pitch, Mathf.Lerp(this.minEngineSoundPitch, this.maxEngineSoundPitch, this.engineRPM / 7000f), Time.deltaTime * 50f);
		}
		if (this.engineSoundOff)
		{
			this.engineSoundOff.volume = 1f - this._gasInput - this.engineSoundIdle.volume;
			this.engineSoundOff.pitch = Mathf.Lerp(this.engineSoundOff.pitch, Mathf.Lerp(this.minEngineSoundPitch, this.maxEngineSoundPitch, this.engineRPM / 7000f), Time.deltaTime * 50f);
		}
		if (this.engineSoundIdle)
		{
			this.engineSoundIdle.volume = Mathf.Lerp(1f, 0f, this.engineRPM / (this.maxEngineRPM / 2f));
			this.engineSoundIdle.pitch = Mathf.Lerp(this.engineSoundIdle.pitch, Mathf.Lerp(this.minEngineSoundPitch, this.maxEngineSoundPitch, this.engineRPM / 7000f), Time.deltaTime * 50f);
		}
	}

	private void Braking()
	{
		if (this.handbrakeInput > 0.1f)
		{
			this.ApplyBrakeTorque(this.RearLeftWheelCollider, this.brake * 1.5f * this.handbrakeInput);
			this.ApplyBrakeTorque(this.RearRightWheelCollider, this.brake * 1.5f * this.handbrakeInput);
		}
		else
		{
			this.ApplyBrakeTorque(this.FrontLeftWheelCollider, this.brake * Mathf.Clamp(this._brakeInput, 0f, 1f));
			this.ApplyBrakeTorque(this.FrontRightWheelCollider, this.brake * Mathf.Clamp(this._brakeInput, 0f, 1f));
			this.ApplyBrakeTorque(this.RearLeftWheelCollider, this.brake * Mathf.Clamp(this._brakeInput, 0f, 1f) / 2f);
			this.ApplyBrakeTorque(this.RearRightWheelCollider, this.brake * Mathf.Clamp(this._brakeInput, 0f, 1f) / 2f);
		}
	}

	private void AntiRollBars()
	{
		float num = 1f;
		float num2 = 1f;
		WheelHit wheelHit;
		bool groundHit = this.FrontLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
		if (groundHit)
		{
			num = (-this.FrontLeftWheelCollider.transform.InverseTransformPoint(wheelHit.point).y - this.FrontLeftWheelCollider.wheelCollider.radius) / this.FrontLeftWheelCollider.wheelCollider.suspensionDistance;
		}
		bool groundHit2 = this.FrontRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
		if (groundHit2)
		{
			num2 = (-this.FrontRightWheelCollider.transform.InverseTransformPoint(wheelHit.point).y - this.FrontRightWheelCollider.wheelCollider.radius) / this.FrontRightWheelCollider.wheelCollider.suspensionDistance;
		}
		float num3 = (num - num2) * this.antiRollFrontHorizontal;
		if (groundHit)
		{
			this.rigid.AddForceAtPosition(this.FrontLeftWheelCollider.transform.up * -num3, this.FrontLeftWheelCollider.transform.position);
		}
		if (groundHit2)
		{
			this.rigid.AddForceAtPosition(this.FrontRightWheelCollider.transform.up * num3, this.FrontRightWheelCollider.transform.position);
		}
		float num4 = 1f;
		float num5 = 1f;
		WheelHit wheelHit2;
		bool groundHit3 = this.RearLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit2);
		if (groundHit3)
		{
			num4 = (-this.RearLeftWheelCollider.transform.InverseTransformPoint(wheelHit2.point).y - this.RearLeftWheelCollider.wheelCollider.radius) / this.RearLeftWheelCollider.wheelCollider.suspensionDistance;
		}
		bool groundHit4 = this.RearRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit2);
		if (groundHit4)
		{
			num5 = (-this.RearRightWheelCollider.transform.InverseTransformPoint(wheelHit2.point).y - this.RearRightWheelCollider.wheelCollider.radius) / this.RearRightWheelCollider.wheelCollider.suspensionDistance;
		}
		float num6 = (num4 - num5) * this.antiRollRearHorizontal;
		if (groundHit3)
		{
			this.rigid.AddForceAtPosition(this.RearLeftWheelCollider.transform.up * -num6, this.RearLeftWheelCollider.transform.position);
		}
		if (groundHit4)
		{
			this.rigid.AddForceAtPosition(this.RearRightWheelCollider.transform.up * num6, this.RearRightWheelCollider.transform.position);
		}
		float num7 = (num - num4) * this.antiRollVertical;
		if (groundHit)
		{
			this.rigid.AddForceAtPosition(this.FrontLeftWheelCollider.transform.up * -num7, this.FrontLeftWheelCollider.transform.position);
		}
		if (groundHit3)
		{
			this.rigid.AddForceAtPosition(this.RearLeftWheelCollider.transform.up * num7, this.RearLeftWheelCollider.transform.position);
		}
		float num8 = (num2 - num5) * this.antiRollVertical;
		if (groundHit2)
		{
			this.rigid.AddForceAtPosition(this.FrontRightWheelCollider.transform.up * -num8, this.FrontRightWheelCollider.transform.position);
		}
		if (groundHit4)
		{
			this.rigid.AddForceAtPosition(this.RearRightWheelCollider.transform.up * num8, this.RearRightWheelCollider.transform.position);
		}
	}

	private void SteerHelper()
	{
		for (int i = 0; i < this.allWheelColliders.Length; i++)
		{
			WheelHit wheelHit;
			this.allWheelColliders[i].wheelCollider.GetGroundHit(out wheelHit);
			if (wheelHit.normal == Vector3.zero)
			{
				return;
			}
		}
		if (Mathf.Abs(this.oldRotation - base.transform.eulerAngles.y) < 10f)
		{
			float num = (base.transform.eulerAngles.y - this.oldRotation) * this.steerHelperStrength;
			Quaternion rotation = Quaternion.AngleAxis(num, Vector3.up);
			this.rigid.velocity = rotation * this.rigid.velocity;
		}
		this.oldRotation = base.transform.eulerAngles.y;
	}

	private void TractionHelper()
	{
		Vector3 vector = this.rigid.velocity;
		vector -= base.transform.up * Vector3.Dot(vector, base.transform.up);
		vector.Normalize();
		this.angle = -Mathf.Asin(Vector3.Dot(Vector3.Cross(base.transform.forward, vector), base.transform.up));
		this.angularVelo = this.rigid.angularVelocity.y;
		if (this.angle * this.FrontLeftWheelCollider.steerAngle < 0f)
		{
			this.FrontLeftRCCWheelCollider.tractionHelpedSidewaysStiffness = 1f - Mathf.Clamp01(this.tractionHelperStrength * Mathf.Abs(this.angularVelo));
		}
		else
		{
			this.FrontLeftRCCWheelCollider.tractionHelpedSidewaysStiffness = 1f;
		}
		if (this.angle * this.FrontRightWheelCollider.steerAngle < 0f)
		{
			this.FrontRightRCCWheelCollider.tractionHelpedSidewaysStiffness = 1f - Mathf.Clamp01(this.tractionHelperStrength * Mathf.Abs(this.angularVelo));
		}
		else
		{
			this.FrontRightRCCWheelCollider.tractionHelpedSidewaysStiffness = 1f;
		}
	}

	private void Clutch()
	{
		if (this.speed <= 10f && !this.cutGas)
		{
			this.clutchInput = Mathf.Lerp(this.clutchInput, Mathf.Lerp(1f, Mathf.Lerp(0.2f, 0f, (this.RearLeftWheelCollider.wheelRPMToSpeed + this.RearRightWheelCollider.wheelRPMToSpeed) / 2f / 10f), Mathf.Abs(this._gasInput)), Time.deltaTime * 50f);
		}
		else if (!this.cutGas)
		{
			if (this.changingGear)
			{
				this.clutchInput = Mathf.Lerp(this.clutchInput, 1f, Time.deltaTime * 10f);
			}
			else
			{
				this.clutchInput = Mathf.Lerp(this.clutchInput, 0f, Time.deltaTime * 10f);
			}
		}
		if (this.cutGas || this.handbrakeInput >= 0.1f)
		{
			this.clutchInput = 1f;
		}
		this.clutchInput = Mathf.Clamp01(this.clutchInput);
	}

	private void GearBox()
	{
		if (this.engineRunning)
		{
			this.idleInput = Mathf.Lerp(1f, 0f, this.engineRPM / this.minEngineRPM);
		}
		else
		{
			this.idleInput = 0f;
		}
		if (!this.AIController)
		{
			if (this.brakeInput > 0.9f && base.transform.InverseTransformDirection(this.rigid.velocity).z < 1f && this.canGoReverseNow && this.automaticGear && !this.semiAutomaticGear && !this.changingGear && this.direction != -1)
			{
				base.StartCoroutine("ChangingGear", -1);
			}
			else if (this.brakeInput < 0.1f && base.transform.InverseTransformDirection(this.rigid.velocity).z > -1f && this.direction == -1 && !this.changingGear && this.automaticGear && !this.semiAutomaticGear)
			{
				base.StartCoroutine("ChangingGear", 0);
			}
		}
		if (this.automaticGear)
		{
			if (this.currentGear < this.totalGears - 1 && !this.changingGear && this.speed >= this.gearSpeed[this.currentGear] * 0.7f && this.FrontLeftWheelCollider.rpm > 0f)
			{
				if (!this.semiAutomaticGear)
				{
					base.StartCoroutine("ChangingGear", this.currentGear + 1);
				}
				else if (this.semiAutomaticGear && this.direction != -1)
				{
					base.StartCoroutine("ChangingGear", this.currentGear + 1);
				}
			}
			if (this.currentGear > 0 && !this.changingGear && this.speed < this.gearSpeed[this.currentGear - 1] * 0.5f && this.direction != -1)
			{
				base.StartCoroutine("ChangingGear", this.currentGear - 1);
			}
		}
		if (this.direction == -1)
		{
			if (!this.reversingSound.isPlaying)
			{
				this.reversingSound.Play();
			}
			this.reversingSound.volume = Mathf.Lerp(0f, 1f, this.speed / 60f);
			this.reversingSound.pitch = this.reversingSound.volume;
		}
		else
		{
			if (this.reversingSound.isPlaying)
			{
				this.reversingSound.Stop();
			}
			this.reversingSound.volume = 0f;
			this.reversingSound.pitch = 0f;
		}
	}

	internal IEnumerator ChangingGear(int gear)
	{
		this.changingGear = true;
		if (this.RCCSettings.useTelemetry)
		{
			MonoBehaviour.print("Shifted to: " + gear.ToString());
		}
		if (this.gearShiftingClips.Length > 0)
		{
			this.gearShiftingSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Gear Shifting AudioSource", 5f, 5f, this.RCCSettings.maxGearShiftingSoundVolume, this.gearShiftingClips[UnityEngine.Random.Range(0, this.gearShiftingClips.Length)], false, true, true);
			if (!this.gearShiftingSound.isPlaying)
			{
				this.gearShiftingSound.Play();
			}
		}
		yield return new WaitForSeconds(this.gearShiftingDelay);
		if (gear == -1)
		{
			this.currentGear = 0;
			this.direction = -1;
		}
		else
		{
			this.currentGear = gear;
			this.direction = 1;
		}
		this.changingGear = false;
		yield break;
	}

	private void RevLimiter()
	{
		if (this.useRevLimiter && this.engineRPM >= this.maxEngineRPM * 1.05f)
		{
			this.cutGas = true;
		}
		else if (this.engineRPM < this.maxEngineRPM)
		{
			this.cutGas = false;
		}
	}

	private void NOS()
	{
		if (!this.useNOS)
		{
			return;
		}
		if (this.boostInput > 1.5f && this._gasInput >= 0.8f && this.NoS > 5f)
		{
			this.NoS -= this.NoSConsumption * Time.deltaTime;
			this.NoSRegenerateTime = 0f;
			if (!this.NOSSound.isPlaying)
			{
				this.NOSSound.Play();
			}
		}
		else
		{
			if (this.NoS < 100f && this.NoSRegenerateTime > 3f)
			{
				this.NoS += this.NoSConsumption / 1.5f * Time.deltaTime;
			}
			this.NoSRegenerateTime += Time.deltaTime;
			if (this.NOSSound.isPlaying)
			{
				this.NOSSound.Stop();
				this.blowSound.clip = this.RCCSettings.blowoutClip[UnityEngine.Random.Range(0, this.RCCSettings.blowoutClip.Length)];
				this.blowSound.Play();
			}
		}
	}

	private void Turbo()
	{
		if (!this.useTurbo)
		{
			return;
		}
		this.turboBoost = Mathf.Lerp(this.turboBoost, Mathf.Clamp(Mathf.Pow(this._gasInput, 10f) * 30f + Mathf.Pow(this.engineRPM / this.maxEngineRPM, 10f) * 30f, 0f, 30f), Time.deltaTime * 10f);
		if (this.turboBoost >= 25f && this.turboBoost < this.turboSound.volume * 30f && !this.blowSound.isPlaying)
		{
			this.blowSound.clip = this.RCCSettings.blowoutClip[UnityEngine.Random.Range(0, this.RCCSettings.blowoutClip.Length)];
			this.blowSound.Play();
		}
		this.turboSound.volume = Mathf.Lerp(this.turboSound.volume, this.turboBoost / 30f, Time.deltaTime * 5f);
		this.turboSound.pitch = Mathf.Lerp(Mathf.Clamp(this.turboSound.pitch, 2f, 3f), this.turboBoost / 30f * 2f, Time.deltaTime * 5f);
	}

	private void DriftVariables()
	{
		WheelHit wheelHit;
		this.RearRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
		if (this.speed > 1f && this.driftingNow)
		{
			this.driftAngle = wheelHit.sidewaysSlip * 1f;
		}
		else
		{
			this.driftAngle = 0f;
		}
		if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.25f)
		{
			this.driftingNow = true;
		}
		else
		{
			this.driftingNow = false;
		}
	}

	private void ResetCar()
	{
		if (this.speed < 5f && !this.rigid.isKinematic && base.transform.eulerAngles.z < 300f && base.transform.eulerAngles.z > 60f)
		{
			this.resetTime += Time.deltaTime;
			if (this.resetTime > 3f)
			{
				base.transform.rotation = Quaternion.identity;
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 3f, base.transform.position.z);
				this.resetTime = 0f;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.contacts.Length < 1 || collision.relativeVelocity.magnitude < this.minimumCollisionForce)
		{
			return;
		}
		if (this.crashClips.Length > 0 && collision.contacts[0].thisCollider.gameObject.transform != base.transform.parent)
		{
			this.crashSound = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Crash Sound AudioSource", 5f, 20f, this.RCCSettings.maxCrashSoundVolume, this.crashClips[UnityEngine.Random.Range(0, this.crashClips.Length)], false, true, true);
			if (!this.crashSound.isPlaying)
			{
				this.crashSound.Play();
			}
		}
		if (this.useDamage)
		{
			this.CollisionParticles(collision.contacts[0].point);
			Vector3 a = collision.relativeVelocity;
			a *= 1f - Mathf.Abs(Vector3.Dot(base.transform.up, collision.contacts[0].normal));
			float num = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, a.normalized));
			if (a.magnitude * num >= this.minimumCollisionForce)
			{
				this.sleep = false;
				this.localVector = base.transform.InverseTransformDirection(a) * (this.damageMultiplier / 50f);
				if (this.originalMeshData == null)
				{
					this.LoadOriginalMeshData();
				}
				for (int i = 0; i < this.deformableMeshFilters.Length; i++)
				{
					this.DeformMesh(this.deformableMeshFilters[i].mesh, this.originalMeshData[i].meshVerts, collision, num, this.deformableMeshFilters[i].transform, this.rot);
				}
			}
		}
		if (UnityEngine.Object.FindObjectOfType<RCC_Camera>() && UnityEngine.Object.FindObjectOfType<RCC_Camera>().playerCar == base.transform)
		{
			UnityEngine.Object.FindObjectOfType<RCC_Camera>().Collision(collision);
		}
	}

	private void OnGUI()
	{
		if (this.RCCSettings.useTelemetry && this.canControl)
		{
			GUI.skin.label.fontSize = 12;
			GUI.skin.box.fontSize = 12;
			GUI.backgroundColor = Color.gray;
			float num = (float)Screen.width / 2f;
			GUI.Box(new Rect((float)(Screen.width - 400) - num, 10f, 800f, 270f), string.Empty);
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 10f, 400f, 150f), "Engine RPM : " + Mathf.CeilToInt(this.engineRPM));
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 10f, 400f, 150f), "Engine Running : " + ((!this.engineRunning) ? "Stopped" : "Running").ToString());
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 30f, 400f, 150f), "Engine Starter : " + ((!this.engineStarting) ? "Stopped" : "Starting").ToString());
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 90f, 400f, 150f), "Engine Sound On Volume: " + this.engineSoundOn.volume.ToString("F1"));
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 110f, 400f, 150f), "Engine Sound On Pitch: " + this.engineSoundOn.pitch.ToString("F1"));
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 130f, 400f, 150f), "Engine Sound Off Volume: " + this.engineSoundOff.volume.ToString("F1"));
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 150f, 400f, 150f), "Engine Sound Off Pitch: " + this.engineSoundOff.pitch.ToString("F1"));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 30f, 400f, 150f), "Speed " + ((this.RCCSettings.units != RCC_Settings.Units.KMH) ? "(MP/H)" : "(KM/H)") + Mathf.CeilToInt(this.speed));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 50f, 400f, 150f), "Steer Angle : " + Mathf.CeilToInt(this.FrontLeftWheelCollider.steerAngle));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 70f, 400f, 150f), "Automatic Shifting : " + ((!this.automaticGear) ? "Manual" : "Automatic").ToString());
			if (!this.changingGear)
			{
				GUI.Label(new Rect((float)(Screen.width - 390) - num, 90f, 400f, 150f), "Gear No : " + ((this.direction != 1) ? "R" : (this.currentGear + 1).ToString()));
			}
			else
			{
				GUI.Label(new Rect((float)(Screen.width - 390) - num, 90f, 400f, 150f), "Gear No : N");
			}
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 230f, 400f, 150f), "Mobile Horizontal Tilt : " + Input.acceleration.x);
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 250f, 400f, 150f), "Mobile Vertical Tilt : " + Input.acceleration.y);
			GUI.Label(new Rect((float)Screen.width - num, 10f, 400f, 150f), "Front Left Wheel RPM : " + Mathf.CeilToInt(this.FrontLeftWheelCollider.rpm));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 10f, 400f, 150f), "Front Right Wheel RPM : " + Mathf.CeilToInt(this.FrontRightWheelCollider.rpm));
			GUI.Label(new Rect((float)Screen.width - num, 30f, 400f, 150f), "Front Left Wheel Torque : " + Mathf.CeilToInt(this.FrontLeftWheelCollider.wheelCollider.motorTorque));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 30f, 400f, 150f), "Front Right Wheel Torque : " + Mathf.CeilToInt(this.FrontRightWheelCollider.wheelCollider.motorTorque));
			GUI.Label(new Rect((float)Screen.width - num, 50f, 400f, 150f), "Front Left Wheel brake : " + Mathf.CeilToInt(this.FrontLeftWheelCollider.wheelCollider.brakeTorque));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 50f, 400f, 150f), "Front Right Wheel brake : " + Mathf.CeilToInt(this.FrontRightWheelCollider.wheelCollider.brakeTorque));
			WheelHit wheelHit;
			this.FrontLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 210f, 400f, 150f), "WCSpeed: " + this.RearLeftWheelCollider.wheelRPMToSpeed);
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 230f, 400f, 150f), "UnderSteer: " + this.overSteering);
			GUI.Label(new Rect((float)(Screen.width - 200) - num, 250f, 400f, 150f), "OverSteer: " + this.underSteering);
			if (this.FrontLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit))
			{
				GUI.Label(new Rect((float)(Screen.width - 200) - num, 70f, 400f, 150f), "Ground Grip : " + this.FrontLeftWheelCollider.forwardFrictionCurve.stiffness);
			}
			GUI.Label(new Rect((float)Screen.width - num, 70f, 400f, 150f), "Front Left Wheel Force : " + Mathf.CeilToInt(wheelHit.force));
			GUI.Label(new Rect((float)Screen.width - num, 90f, 400f, 150f), "Front Left Wheel Sideways Grip : " + (1f - Mathf.Abs(wheelHit.sidewaysSlip)).ToString("F2"));
			GUI.Label(new Rect((float)Screen.width - num, 110f, 400f, 150f), "Front Left Wheel Forward Grip : " + (1f - Mathf.Abs(wheelHit.forwardSlip)).ToString("F2"));
			this.FrontRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 70f, 400f, 150f), "Front Right Wheel Force : " + Mathf.CeilToInt(wheelHit.force));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 90f, 400f, 150f), "Front Right Wheel Sideways Grip : " + (1f - Mathf.Abs(wheelHit.sidewaysSlip)).ToString("F2"));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 110f, 400f, 150f), "Front Right Wheel Forward Grip : " + (1f - Mathf.Abs(wheelHit.forwardSlip)).ToString("F2"));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 170f, 400f, 150f), string.Concat(new object[]
			{
				"ABS: ",
				this.ABS,
				". Current State: ",
				((!this.ABSAct) ? "Safe" : "Engaged").ToString()
			}));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 190f, 400f, 150f), string.Concat(new object[]
			{
				"TCS: ",
				this.TCS,
				". Current State: ",
				((!this.TCSAct) ? "Safe" : "Engaged").ToString()
			}));
			GUI.Label(new Rect((float)(Screen.width - 390) - num, 210f, 400f, 150f), string.Concat(new object[]
			{
				"ESP: ",
				this.ESP,
				". Current State: ",
				((!this.ESPAct) ? "Safe" : "Engaged").ToString()
			}));
			GUI.Label(new Rect((float)Screen.width - num, 150f, 400f, 150f), "Rear Left Wheel RPM : " + Mathf.CeilToInt(this.RearLeftWheelCollider.rpm));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 150f, 400f, 150f), "Rear Right Wheel RPM : " + Mathf.CeilToInt(this.RearRightWheelCollider.rpm));
			GUI.Label(new Rect((float)Screen.width - num, 170f, 400f, 150f), "Rear Left Wheel Torque : " + Mathf.CeilToInt(this.RearLeftWheelCollider.wheelCollider.motorTorque));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 170f, 400f, 150f), "Rear Right Wheel Torque : " + Mathf.CeilToInt(this.RearRightWheelCollider.wheelCollider.motorTorque));
			GUI.Label(new Rect((float)Screen.width - num, 190f, 400f, 150f), "Rear Left Wheel brake : " + Mathf.CeilToInt(this.RearLeftWheelCollider.wheelCollider.brakeTorque));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 190f, 400f, 150f), "Rear Right Wheel brake : " + Mathf.CeilToInt(this.RearRightWheelCollider.wheelCollider.brakeTorque));
			this.RearLeftWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
			GUI.Label(new Rect((float)Screen.width - num, 210f, 400f, 150f), "Rear Left Wheel Force : " + Mathf.CeilToInt(wheelHit.force));
			GUI.Label(new Rect((float)Screen.width - num, 230f, 400f, 150f), "Rear Left Wheel Sideways Grip : " + (1f - Mathf.Abs(wheelHit.sidewaysSlip)).ToString("F2"));
			GUI.Label(new Rect((float)Screen.width - num, 250f, 400f, 150f), "Rear Left Wheel Forward Grip : " + (1f - Mathf.Abs(wheelHit.forwardSlip)).ToString("F2"));
			this.RearRightWheelCollider.wheelCollider.GetGroundHit(out wheelHit);
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 210f, 400f, 150f), "Rear Right Wheel Force : " + Mathf.CeilToInt(wheelHit.force));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 230f, 400f, 150f), "Rear Right Wheel Sideways Grip : " + (1f - Mathf.Abs(wheelHit.sidewaysSlip)).ToString("F2"));
			GUI.Label(new Rect((float)(Screen.width + 200) - num, 250f, 400f, 150f), "Rear Right Wheel Forward Grip : " + (1f - Mathf.Abs(wheelHit.forwardSlip)).ToString("F2"));
			GUI.backgroundColor = Color.green;
			GUI.Button(new Rect((float)(Screen.width - 20) - num, 260f, 10f, Mathf.Clamp(-this._gasInput * 100f, -100f, 0f)), string.Empty);
			GUI.backgroundColor = Color.red;
			GUI.Button(new Rect((float)(Screen.width - 35) - num, 260f, 10f, Mathf.Clamp(-this._brakeInput * 100f, -100f, 0f)), string.Empty);
			GUI.backgroundColor = Color.blue;
			GUI.Button(new Rect((float)(Screen.width - 50) - num, 260f, 10f, Mathf.Clamp(-this.clutchInput * 100f, -100f, 0f)), string.Empty);
		}
	}

	private bool OverTorque()
	{
		return this.speed > this.maxspeed || !this.engineRunning;
	}

	private void OnDrawGizmos()
	{
	}

	public void TorqueCurve()
	{
		if (this.defMaxSpeed != this.maxspeed)
		{
			if (this.totalGears < 1)
			{
				UnityEngine.Debug.LogError("You are trying to set your vehicle gear to 0 or below! Why you trying to do this???");
				this.totalGears = 1;
				return;
			}
			this.gearSpeed = new float[this.totalGears];
			this.engineTorqueCurve = new AnimationCurve[this.totalGears];
			this.currentGear = 0;
			for (int i = 0; i < this.engineTorqueCurve.Length; i++)
			{
				this.engineTorqueCurve[i] = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 1f)
				});
			}
			for (int j = 0; j < this.totalGears; j++)
			{
				this.gearSpeed[j] = Mathf.Lerp(0f, this.maxspeed, (float)(j + 1) / (float)this.totalGears);
				if (j != 0)
				{
					this.engineTorqueCurve[j].MoveKey(0, new Keyframe(0f, Mathf.Lerp(0.25f, 0f, (float)(j + 1) / (float)this.totalGears)));
					this.engineTorqueCurve[j].AddKey(Mathf.Lerp(0f, this.maxspeed / 1f, (float)j / (float)this.totalGears), Mathf.Lerp(1f, 0.25f, (float)j / (float)this.totalGears));
					this.engineTorqueCurve[j].AddKey(this.gearSpeed[j], 0.1f);
					this.engineTorqueCurve[j].AddKey(this.gearSpeed[j] * 2f, -3f);
					this.engineTorqueCurve[j].postWrapMode = WrapMode.Once;
				}
				else
				{
					this.engineTorqueCurve[j].MoveKey(0, new Keyframe(0f, 1f));
					this.engineTorqueCurve[j].AddKey(Mathf.Lerp(0f, this.maxspeed / 1f, (float)(j + 1) / (float)this.totalGears), 1f);
					this.engineTorqueCurve[j].AddKey(Mathf.Lerp(25f, this.maxspeed / 1f, (float)(j + 1) / (float)this.totalGears), 0f);
					this.engineTorqueCurve[j].postWrapMode = WrapMode.Once;
				}
			}
		}
		this.defMaxSpeed = this.maxspeed;
	}

	private void OnDisable()
	{
		if (this.canControl && base.gameObject.GetComponentInChildren<RCC_Camera>())
		{
			base.gameObject.GetComponentInChildren<RCC_Camera>().transform.SetParent(null);
		}
	}

	private Rigidbody rigid;

	internal bool sleepingRigid;

	public bool AIController;

	public Transform FrontLeftWheelTransform;

	public Transform FrontRightWheelTransform;

	public Transform RearLeftWheelTransform;

	public Transform RearRightWheelTransform;

	public RCC_WheelCollider FrontLeftWheelCollider;

	public RCC_WheelCollider FrontRightWheelCollider;

	public RCC_WheelCollider RearLeftWheelCollider;

	public RCC_WheelCollider RearRightWheelCollider;

	private RCC_WheelCollider FrontLeftRCCWheelCollider;

	private RCC_WheelCollider FrontRightRCCWheelCollider;

	internal RCC_WheelCollider[] allWheelColliders;

	public Transform[] ExtraRearWheelsTransform;

	public RCC_WheelCollider[] ExtraRearWheelsCollider;

	public bool applyEngineTorqueToExtraRearWheelColliders = true;

	public Transform SteeringWheel;

	public RCC_CarControllerV3.WheelType _wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;

	[Range(0f, 100f)]
	public float biasedWheelTorque = 100f;

	public Transform COM;

	public bool canControl = true;

	public bool engineRunning;

	public bool semiAutomaticGear;

	public bool automaticClutch = true;

	private bool canGoReverseNow;

	public AnimationCurve[] engineTorqueCurve;

	public float[] gearSpeed;

	public float engineTorque = 3000f;

	public float maxEngineRPM = 7000f;

	public float minEngineRPM = 1000f;

	[Range(0.75f, 2f)]
	public float engineInertia = 1f;

	public bool useRevLimiter = true;

	public bool useExhaustFlame = true;

	public float steerAngle = 40f;

	public float highspeedsteerAngle = 15f;

	public float highspeedsteerAngleAtspeed = 100f;

	public float antiRollFrontHorizontal = 5000f;

	public float antiRollRearHorizontal = 5000f;

	public float antiRollVertical = 500f;

	public float downForce = 25f;

	public float speed;

	public float brake = 2500f;

	private float defMaxSpeed;

	public float maxspeed = 220f;

	private float resetTime;

	private float orgSteerAngle;

	public float fuelInput = 1f;

	public int currentGear;

	public int totalGears = 6;

	[Range(0f, 0.5f)]
	public float gearShiftingDelay = 0.35f;

	public bool changingGear;

	public int direction = 1;

	public bool autoGenerateGearCurves = true;

	public bool autoGenerateTargetSpeedsForChangingGear = true;

	private bool engineStarting;

	private AudioSource engineStartSound;

	public AudioClip engineStartClip;

	internal AudioSource engineSoundOn;

	public AudioClip engineClipOn;

	private AudioSource engineSoundOff;

	public AudioClip engineClipOff;

	private AudioSource engineSoundIdle;

	public AudioClip engineClipIdle;

	private AudioSource gearShiftingSound;

	private AudioSource crashSound;

	private AudioSource reversingSound;

	private AudioSource windSound;

	private AudioSource brakeSound;

	private AudioSource NOSSound;

	private AudioSource turboSound;

	private AudioSource blowSound;

	[Range(0.25f, 1f)]
	public float minEngineSoundPitch = 0.75f;

	[Range(1.25f, 2f)]
	public float maxEngineSoundPitch = 1.75f;

	[Range(0f, 1f)]
	public float minEngineSoundVolume = 0.05f;

	[Range(0f, 1f)]
	public float maxEngineSoundVolume = 0.85f;

	private GameObject allAudioSources;

	private GameObject allContactParticles;

	[HideInInspector]
	public float gasInput;

	[HideInInspector]
	public float brakeInput;

	[HideInInspector]
	public float steerInput;

	[HideInInspector]
	public float clutchInput;

	[HideInInspector]
	public float handbrakeInput;

	[HideInInspector]
	public float boostInput = 1f;

	[HideInInspector]
	public bool cutGas;

	[HideInInspector]
	public float idleInput;

	internal float engineRPM;

	internal float rawEngineRPM;

	public GameObject chassis;

	public bool lowBeamHeadLightsOn;

	public bool highBeamHeadLightsOn;

	public RCC_CarControllerV3.IndicatorsOn indicatorsOn;

	public float indicatorTimer;

	public bool useDamage = true;

	public MeshFilter[] deformableMeshFilters;

	public float randomizeVertices = 1f;

	public float damageRadius = 0.5f;

	private float minimumVertDistanceForDamagedMesh = 0.002f;

	private Vector3[] colliderVerts;

	private RCC_CarControllerV3.originalMeshVerts[] originalMeshData;

	[HideInInspector]
	public bool sleep = true;

	public float maximumDamage = 0.5f;

	private float minimumCollisionForce = 5f;

	public float damageMultiplier = 1f;

	public int maximumContactSparkle = 5;

	private List<ParticleSystem> contactSparkeList = new List<ParticleSystem>();

	public bool repairNow;

	private Vector3 localVector;

	private Quaternion rot = Quaternion.identity;

	private float oldRotation;

	public bool ABS = true;

	public bool TCS = true;

	public bool ESP = true;

	public bool steeringHelper = true;

	public bool tractionHelper = true;

	public bool ABSAct;

	public bool TCSAct;

	public bool ESPAct;

	[Range(0.05f, 0.5f)]
	public float ABSThreshold = 0.35f;

	[Range(0.05f, 0.5f)]
	public float TCSThreshold = 0.25f;

	[Range(0f, 1f)]
	public float TCSStrength = 1f;

	[Range(0.05f, 0.5f)]
	public float ESPThreshold = 0.25f;

	[Range(0.1f, 1f)]
	public float ESPStrength = 0.5f;

	[Range(0f, 1f)]
	public float steerHelperStrength = 0.1f;

	[Range(0f, 1f)]
	public float tractionHelperStrength = 0.1f;

	public bool overSteering;

	public bool underSteering;

	internal float driftAngle;

	internal bool driftingNow;

	private bool applyCounterSteering;

	public float frontCamber;

	public float rearCamber;

	public float frontSlip;

	public float rearSlip;

	private float angle;

	private float angularVelo;

	public float turboBoost;

	public float NoS = 100f;

	private float NoSConsumption = 25f;

	private float NoSRegenerateTime = 10f;

	public bool useNOS;

	public bool useTurbo;

	public enum WheelType
	{
		FWD,
		RWD,
		AWD,
		BIASED
	}

	public enum IndicatorsOn
	{
		Off,
		Right,
		Left,
		All
	}

	private struct originalMeshVerts
	{
		public Vector3[] meshVerts;
	}
}
