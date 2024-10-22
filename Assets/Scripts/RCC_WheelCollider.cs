using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Wheel Collider")]
[RequireComponent(typeof(WheelCollider))]
public class RCC_WheelCollider : MonoBehaviour
{
	public WheelCollider wheelCollider
	{
		get
		{
			if (this._wheelCollider == null)
			{
				this._wheelCollider = base.GetComponent<WheelCollider>();
			}
			return this._wheelCollider;
		}
		set
		{
			this._wheelCollider = value;
		}
	}

	private RCC_GroundMaterials physicsMaterials
	{
		get
		{
			return RCC_GroundMaterials.Instance;
		}
	}

	private RCC_GroundMaterials.GroundMaterialFrictions[] physicsFrictions
	{
		get
		{
			return RCC_GroundMaterials.Instance.frictions;
		}
	}

	private void Start()
	{
		this.carController = base.GetComponentInParent<RCC_CarControllerV3>();
		this.carRigid = this.carController.GetComponent<Rigidbody>();
		this.wheelCollider = base.GetComponent<WheelCollider>();
		this.allWheelColliders = this.carController.allWheelColliders.ToList<RCC_WheelCollider>();
		this.allWheelColliders.Remove(this);
		if (UnityEngine.Object.FindObjectOfType(typeof(RCC_Skidmarks)))
		{
			this.skidmarks = (UnityEngine.Object.FindObjectOfType(typeof(RCC_Skidmarks)) as RCC_Skidmarks);
		}
		else
		{
			UnityEngine.Debug.Log("No skidmarks object found. Creating new one...");
			this.skidmarks = UnityEngine.Object.Instantiate<RCC_Skidmarks>(RCC_Settings.Instance.skidmarksManager, Vector3.zero, Quaternion.identity);
		}
		this.wheelCollider.mass = this.carRigid.mass / 20f;
		this.forwardFrictionCurve = this.wheelCollider.forwardFriction;
		this.sidewaysFrictionCurve = this.wheelCollider.sidewaysFriction;
		this.camber = ((!(this == this.carController.FrontLeftWheelCollider) && !(this == this.carController.FrontRightWheelCollider)) ? this.carController.rearCamber : this.carController.frontCamber);
		switch (RCC_Settings.Instance.behaviorType)
		{
		case RCC_Settings.BehaviorType.Simulator:
			this.forwardFrictionCurve = this.SetFrictionCurves(this.forwardFrictionCurve, 0.2f, 1f, 0.8f, 0.75f);
			this.sidewaysFrictionCurve = this.SetFrictionCurves(this.sidewaysFrictionCurve, 0.25f, 1f, 0.5f, 0.75f);
			this.wheelCollider.forceAppPointDistance = Mathf.Clamp(this.wheelCollider.forceAppPointDistance, 0.1f, 1f);
			break;
		case RCC_Settings.BehaviorType.Racing:
			this.forwardFrictionCurve = this.SetFrictionCurves(this.forwardFrictionCurve, 0.2f, 1f, 0.8f, 0.75f);
			this.sidewaysFrictionCurve = this.SetFrictionCurves(this.sidewaysFrictionCurve, 0.3f, 1f, 0.25f, 0.75f);
			this.wheelCollider.forceAppPointDistance = Mathf.Clamp(this.wheelCollider.forceAppPointDistance, 0.25f, 1f);
			break;
		case RCC_Settings.BehaviorType.SemiArcade:
			this.forwardFrictionCurve = this.SetFrictionCurves(this.forwardFrictionCurve, 0.2f, 1f, 1f, 1f);
			this.sidewaysFrictionCurve = this.SetFrictionCurves(this.sidewaysFrictionCurve, 0.25f, 1f, 1f, 1f);
			this.forwardFrictionCurve.stiffness = 1f;
			this.sidewaysFrictionCurve.stiffness = 1f;
			this.wheelCollider.forceAppPointDistance = Mathf.Clamp(this.wheelCollider.forceAppPointDistance, 0.35f, 1f);
			break;
		case RCC_Settings.BehaviorType.Drift:
			this.forwardFrictionCurve = this.SetFrictionCurves(this.forwardFrictionCurve, 0.25f, 1f, 0.8f, 0.75f);
			this.sidewaysFrictionCurve = this.SetFrictionCurves(this.sidewaysFrictionCurve, 0.35f, 1f, 0.5f, 0.75f);
			this.wheelCollider.forceAppPointDistance = Mathf.Clamp(this.wheelCollider.forceAppPointDistance, 0.1f, 1f);
			if (this.carController._wheelTypeChoise == RCC_CarControllerV3.WheelType.FWD)
			{
				UnityEngine.Debug.LogError("Current behavior mode is ''Drift'', but your vehicle named " + this.carController.name + " was FWD. You have to use RWD, AWD, or BIASED to rear wheels. Setting it to *RWD* now. ");
				this.carController._wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
			}
			break;
		case RCC_Settings.BehaviorType.Fun:
			this.forwardFrictionCurve = this.SetFrictionCurves(this.forwardFrictionCurve, 0.2f, 2f, 2f, 2f);
			this.sidewaysFrictionCurve = this.SetFrictionCurves(this.sidewaysFrictionCurve, 0.25f, 2f, 2f, 2f);
			this.wheelCollider.forceAppPointDistance = Mathf.Clamp(this.wheelCollider.forceAppPointDistance, 0.75f, 2f);
			break;
		}
		this.orgForwardStiffness = this.forwardFrictionCurve.stiffness;
		this.orgSidewaysStiffness = this.sidewaysFrictionCurve.stiffness;
		this.wheelCollider.forwardFriction = this.forwardFrictionCurve;
		this.wheelCollider.sidewaysFriction = this.sidewaysFrictionCurve;
		if (RCC_Settings.Instance.useSharedAudioSources)
		{
			if (!this.carController.transform.Find("All Audio Sources/Skid Sound AudioSource"))
			{
				this.audioSource = RCC_CreateAudioSource.NewAudioSource(this.carController.gameObject, "Skid Sound AudioSource", 5f, 50f, 0f, this.audioClip, true, true, false);
			}
			else
			{
				this.audioSource = this.carController.transform.Find("All Audio Sources/Skid Sound AudioSource").GetComponent<AudioSource>();
			}
		}
		else
		{
			this.audioSource = RCC_CreateAudioSource.NewAudioSource(this.carController.gameObject, "Skid Sound AudioSource", 5f, 50f, 0f, this.audioClip, true, true, false);
			this.audioSource.transform.position = base.transform.position;
		}
		for (int i = 0; i < RCC_GroundMaterials.Instance.frictions.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RCC_GroundMaterials.Instance.frictions[i].groundParticles, base.transform.position, base.transform.rotation);
			this.emission = gameObject.GetComponent<ParticleSystem>().emission;
			this.emission.enabled = false;
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			this.allWheelParticles.Add(gameObject.GetComponent<ParticleSystem>());
		}
	}

	private WheelFrictionCurve SetFrictionCurves(WheelFrictionCurve curve, float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue)
	{
		WheelFrictionCurve result = curve;
		result.extremumSlip = extremumSlip;
		result.extremumValue = extremumValue;
		result.asymptoteSlip = asymptoteSlip;
		result.asymptoteValue = asymptoteValue;
		return result;
	}

	private void Update()
	{
		if (!this.carController.sleepingRigid)
		{
			this.WheelAlign();
			this.WheelCamber();
		}
	}

	private void FixedUpdate()
	{
		WheelHit wheelHit;
		this.isGrounded = this.wheelCollider.GetGroundHit(out wheelHit);
		this.steerAngle = this.wheelCollider.steerAngle;
		this.rpm = this.wheelCollider.rpm;
		this.wheelRPMToSpeed = this.wheelCollider.rpm * this.wheelCollider.radius / 2.8f * Mathf.Lerp(1f, 0.75f, wheelHit.forwardSlip) * this.carRigid.transform.lossyScale.y;
		this.SkidMarks();
		this.Frictions();
		this.Audio();
	}

	public void WheelAlign()
	{
		if (!this.wheelModel)
		{
			UnityEngine.Debug.LogError(base.transform.name + " wheel of the " + this.carController.transform.name + " is missing wheel model. This wheel is disabled");
			base.enabled = false;
		}
		Vector3 vector = this.wheelCollider.transform.TransformPoint(this.wheelCollider.center);
		WheelHit wheelHit;
		this.wheelCollider.GetGroundHit(out wheelHit);
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -this.wheelCollider.transform.up, out raycastHit, (this.wheelCollider.suspensionDistance + this.wheelCollider.radius) * base.transform.localScale.y) && raycastHit.transform.gameObject.layer != (int)Mathf.Log((float)RCC_Settings.Instance.vehicleLayer.value, 2f) && !raycastHit.collider.isTrigger)
		{
			this.wheelModel.transform.position = raycastHit.point + this.wheelCollider.transform.up * this.wheelCollider.radius * base.transform.localScale.y;
			float num = (-this.wheelCollider.transform.InverseTransformPoint(wheelHit.point).y - this.wheelCollider.radius) / this.wheelCollider.suspensionDistance;
			UnityEngine.Debug.DrawLine(wheelHit.point, wheelHit.point + this.wheelCollider.transform.up * (wheelHit.force / this.carRigid.mass), ((double)num > 0.0) ? Color.white : Color.magenta);
			UnityEngine.Debug.DrawLine(wheelHit.point, wheelHit.point - this.wheelCollider.transform.forward * wheelHit.forwardSlip * 2f, Color.green);
			UnityEngine.Debug.DrawLine(wheelHit.point, wheelHit.point - this.wheelCollider.transform.right * wheelHit.sidewaysSlip * 2f, Color.red);
		}
		else
		{
			this.wheelModel.transform.position = vector - this.wheelCollider.transform.up * this.wheelCollider.suspensionDistance * base.transform.localScale.y;
		}
		this.wheelRotation += this.wheelCollider.rpm * 6f * Time.deltaTime;
		this.wheelModel.transform.rotation = this.wheelCollider.transform.rotation * Quaternion.Euler(this.wheelRotation, this.wheelCollider.steerAngle, this.wheelCollider.transform.rotation.z);
	}

	public void WheelCamber()
	{
		Vector3 euler;
		if (this.wheelCollider.transform.localPosition.x < 0f)
		{
			euler = new Vector3(this.wheelCollider.transform.localEulerAngles.x, this.wheelCollider.transform.localEulerAngles.y, -this.camber);
		}
		else
		{
			euler = new Vector3(this.wheelCollider.transform.localEulerAngles.x, this.wheelCollider.transform.localEulerAngles.y, this.camber);
		}
		Quaternion localRotation = Quaternion.Euler(euler);
		this.wheelCollider.transform.localRotation = localRotation;
	}

	private void SkidMarks()
	{
		WheelHit wheelHit;
		this.wheelCollider.GetGroundHit(out wheelHit);
		this.wheelSlipAmountSideways = Mathf.Abs(wheelHit.sidewaysSlip);
		this.wheelSlipAmountForward = Mathf.Abs(wheelHit.forwardSlip);
		this.totalSlip = this.wheelSlipAmountSideways / 2f + this.wheelSlipAmountForward / 2f;
		if (this.skidmarks)
		{
			if (this.wheelSlipAmountSideways > this.startSlipValue * 2f || this.wheelSlipAmountForward > this.startSlipValue * 2f)
			{
				Vector3 pos = wheelHit.point + 2f * this.carRigid.velocity * Time.deltaTime;
				if (this.carRigid.velocity.magnitude > 1f)
				{
					this.lastSkidmark = this.skidmarks.AddSkidMark(pos, wheelHit.normal, this.wheelSlipAmountSideways / 2f + this.wheelSlipAmountForward / 2f, this.lastSkidmark);
					this.wheelTemparature += (this.wheelSlipAmountSideways / 2f + this.wheelSlipAmountForward / 2f) / (Time.fixedDeltaTime * 100f * Mathf.Lerp(1f, 5f, this.wheelTemparature / 150f));
				}
				else
				{
					this.lastSkidmark = -1;
					this.wheelTemparature -= Time.fixedDeltaTime * 5f;
				}
			}
			else
			{
				this.lastSkidmark = -1;
				this.wheelTemparature -= Time.fixedDeltaTime * 5f;
			}
			this.wheelTemparature = Mathf.Clamp(this.wheelTemparature, 0f, 150f);
		}
	}

	private void Frictions()
	{
		WheelHit wheelHit;
		this.wheelCollider.GetGroundHit(out wheelHit);
		bool flag = false;
		for (int i = 0; i < this.physicsFrictions.Length; i++)
		{
			if (wheelHit.point != Vector3.zero && wheelHit.collider.sharedMaterial == this.physicsFrictions[i].groundMaterial)
			{
				flag = true;
				this.forwardFrictionCurve.stiffness = this.physicsFrictions[i].forwardStiffness;
				this.sidewaysFrictionCurve.stiffness = this.physicsFrictions[i].sidewaysStiffness * this.tractionHelpedSidewaysStiffness;
				if (RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift)
				{
					this.Drift(Mathf.Abs(wheelHit.forwardSlip));
				}
				this.wheelCollider.forwardFriction = this.forwardFrictionCurve;
				this.wheelCollider.sidewaysFriction = this.sidewaysFrictionCurve;
				this.wheelCollider.wheelDampingRate = this.physicsFrictions[i].damp;
				this.emission = this.allWheelParticles[i].emission;
				this.audioClip = this.physicsFrictions[i].groundSound;
				if (this.wheelSlipAmountSideways > this.physicsFrictions[i].slip || this.wheelSlipAmountForward > this.physicsFrictions[i].slip * 2f)
				{
					this.emission.enabled = true;
				}
				else
				{
					this.emission.enabled = false;
				}
			}
		}
		if (!flag && this.physicsMaterials.useTerrainSplatMapForGroundFrictions)
		{
			for (int j = 0; j < this.physicsMaterials.terrainSplatMapIndex.Length; j++)
			{
				if (wheelHit.point != Vector3.zero && wheelHit.collider.sharedMaterial == this.physicsMaterials.terrainPhysicMaterial && TerrainSurface.GetTextureMix(base.transform.position) != null && TerrainSurface.GetTextureMix(base.transform.position)[j] > 0.5f)
				{
					flag = true;
					this.forwardFrictionCurve.stiffness = this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].forwardStiffness;
					this.sidewaysFrictionCurve.stiffness = this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].sidewaysStiffness * this.tractionHelpedSidewaysStiffness;
					if (RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift)
					{
						this.Drift(Mathf.Abs(wheelHit.forwardSlip));
					}
					this.wheelCollider.forwardFriction = this.forwardFrictionCurve;
					this.wheelCollider.sidewaysFriction = this.sidewaysFrictionCurve;
					this.wheelCollider.wheelDampingRate = this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].damp;
					this.emission = this.allWheelParticles[this.physicsMaterials.terrainSplatMapIndex[j]].emission;
					this.audioClip = this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].groundSound;
					if (this.wheelSlipAmountSideways > this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].slip || this.wheelSlipAmountForward > this.physicsFrictions[this.physicsMaterials.terrainSplatMapIndex[j]].slip * 2f)
					{
						this.emission.enabled = true;
					}
					else
					{
						this.emission.enabled = false;
					}
				}
			}
		}
		if (!flag)
		{
			this.forwardFrictionCurve.stiffness = this.orgForwardStiffness;
			this.sidewaysFrictionCurve.stiffness = this.orgSidewaysStiffness * this.tractionHelpedSidewaysStiffness;
			if (RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift)
			{
				this.Drift(Mathf.Abs(wheelHit.forwardSlip));
			}
			this.wheelCollider.forwardFriction = this.forwardFrictionCurve;
			this.wheelCollider.sidewaysFriction = this.sidewaysFrictionCurve;
			this.wheelCollider.wheelDampingRate = this.physicsFrictions[0].damp;
			this.emission = this.allWheelParticles[0].emission;
			this.audioClip = this.physicsFrictions[0].groundSound;
			if (this.wheelSlipAmountSideways > this.physicsFrictions[0].slip || this.wheelSlipAmountForward > this.physicsFrictions[0].slip * 2f)
			{
				this.emission.enabled = true;
			}
			else
			{
				this.emission.enabled = false;
			}
		}
		for (int k = 0; k < this.allWheelParticles.Count; k++)
		{
			if (this.wheelSlipAmountSideways <= this.startSlipValue && this.wheelSlipAmountForward <= this.startSlipValue * 2f)
			{
				this.emission = this.allWheelParticles[k].emission;
				this.emission.enabled = false;
			}
		}
	}

	private void Drift(float forwardSlip)
	{
		Vector3 vector = base.transform.InverseTransformDirection(this.carRigid.velocity);
		float num = vector.x * vector.x / 200f;
		if (this.wheelCollider == this.carController.FrontLeftWheelCollider.wheelCollider || this.wheelCollider == this.carController.FrontRightWheelCollider.wheelCollider)
		{
			this.forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.1f, this.maxForwardStiffness);
			this.forwardFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.1f, this.minForwardStiffness);
		}
		else
		{
			this.forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.5f, this.maxForwardStiffness);
			this.forwardFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.8f, this.minForwardStiffness);
		}
		if (this.wheelCollider == this.carController.FrontLeftWheelCollider.wheelCollider || this.wheelCollider == this.carController.FrontRightWheelCollider.wheelCollider)
		{
			this.sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - num / 1f, 0.5f, this.maxSidewaysStiffness);
			this.sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.5f, this.minSidewaysStiffness);
		}
		else
		{
			this.sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.5f, this.maxSidewaysStiffness);
			this.sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.5f, this.minSidewaysStiffness);
		}
	}

	private void Audio()
	{
		if (RCC_Settings.Instance.useSharedAudioSources && this.isSkidding())
		{
			return;
		}
		if (this.totalSlip > this.startSlipValue)
		{
			if (this.audioSource.clip != this.audioClip)
			{
				this.audioSource.clip = this.audioClip;
			}
			if (!this.audioSource.isPlaying)
			{
				this.audioSource.Play();
			}
			if (this.carRigid.velocity.magnitude > 1f)
			{
				this.audioSource.volume = Mathf.Lerp(0f, 1f, this.totalSlip);
			}
			else
			{
				this.audioSource.volume = Mathf.Lerp(this.audioSource.volume, 0f, Time.deltaTime * 5f);
			}
		}
		else
		{
			this.audioSource.volume = Mathf.Lerp(this.audioSource.volume, 0f, Time.deltaTime * 5f);
			if (this.audioSource.volume <= 0.05f)
			{
				this.audioSource.Stop();
			}
		}
	}

	private bool isSkidding()
	{
		for (int i = 0; i < this.allWheelColliders.Count; i++)
		{
			if (this.allWheelColliders[i].totalSlip > this.totalSlip)
			{
				return true;
			}
		}
		return false;
	}

	private RCC_CarControllerV3 carController;

	private Rigidbody carRigid;

	private WheelCollider _wheelCollider;

	private List<RCC_WheelCollider> allWheelColliders = new List<RCC_WheelCollider>();

	public Transform wheelModel;

	private float wheelRotation;

	public float camber;

	private PhysicMaterial groundMaterial;

	internal float steerAngle;

	internal bool isGrounded;

	internal float totalSlip;

	internal float rpm;

	internal float wheelRPMToSpeed;

	internal float wheelTemparature;

	private RCC_Skidmarks skidmarks;

	private float startSlipValue = 0.25f;

	private int lastSkidmark = -1;

	private float wheelSlipAmountSideways;

	private float wheelSlipAmountForward;

	private float orgSidewaysStiffness = 1f;

	private float orgForwardStiffness = 1f;

	public WheelFrictionCurve forwardFrictionCurve;

	public WheelFrictionCurve sidewaysFrictionCurve;

	private AudioSource audioSource;

	private AudioClip audioClip;

	private List<ParticleSystem> allWheelParticles = new List<ParticleSystem>();

	private ParticleSystem.EmissionModule emission;

	internal float tractionHelpedSidewaysStiffness = 1f;

	private float minForwardStiffness = 0.75f;

	private float maxForwardStiffness = 1f;

	private float minSidewaysStiffness = 0.75f;

	private float maxSidewaysStiffness = 1f;
}
