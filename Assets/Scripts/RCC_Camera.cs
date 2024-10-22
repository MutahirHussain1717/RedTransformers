using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Main Camera")]
public class RCC_Camera : MonoBehaviour
{
	public Transform _playerCar
	{
		get
		{
			return this.playerCar;
		}
		set
		{
			this.playerCar = value;
			this.GetPlayerCar();
		}
	}

	private void Awake()
	{
		this.cam = base.GetComponentInChildren<Camera>();
	}

	private void GetPlayerCar()
	{
		if (!this.playerCar)
		{
			return;
		}
		this.playerRigid = this.playerCar.GetComponent<Rigidbody>();
		this.hoodCam = this.playerCar.GetComponentInChildren<RCCHoodCamera>();
		this.wheelCam = this.playerCar.GetComponentInChildren<RCCWheelCamera>();
		this.fixedCam = UnityEngine.Object.FindObjectOfType<RCC_MainFixedCamera>();
		base.transform.position = this.playerCar.transform.position;
		base.transform.rotation = this.playerCar.transform.rotation * Quaternion.Euler(10f, 0f, 0f);
		if (this.playerCar.GetComponent<RCC_CameraConfig>())
		{
			this.playerCar.GetComponent<RCC_CameraConfig>().SetCameraSettings();
		}
	}

	public void SetPlayerCar(GameObject player)
	{
		this._playerCar = player.transform;
	}

	private void Update()
	{
		if (!this.playerCar || !this.playerRigid)
		{
			this.GetPlayerCar();
			return;
		}
		this.speed = Mathf.Lerp(this.speed, this.playerRigid.velocity.magnitude * 3.6f, Time.deltaTime * 0.5f);
		if (this.index > 0f)
		{
			this.index -= Time.deltaTime * 5f;
		}
		if (this.cameraMode == RCC_Camera.CameraMode.TPS)
		{
		}
		this.cam.fieldOfView = this.targetFieldOfView;
	}

	private void LateUpdate()
	{
		if (!this.playerCar || !this.playerRigid)
		{
			return;
		}
		if (!this.playerCar.gameObject.activeSelf)
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(RCC_Settings.Instance.changeCameraKB))
		{
			this.ChangeCamera();
		}
		switch (this.cameraSwitchCount)
		{
		case 0:
			this.cameraMode = RCC_Camera.CameraMode.TPS;
			break;
		case 1:
			this.cameraMode = RCC_Camera.CameraMode.FPS;
			break;
		case 2:
			this.cameraMode = RCC_Camera.CameraMode.WHEEL;
			break;
		case 3:
			this.cameraMode = RCC_Camera.CameraMode.FIXED;
			break;
		}
		this.pastFollowerPosition = base.transform.position;
		this.pastTargetPosition = this.targetPosition;
		switch (this.cameraMode)
		{
		case RCC_Camera.CameraMode.TPS:
			this.TPS();
			break;
		case RCC_Camera.CameraMode.FPS:
			if (this.hoodCam)
			{
				this.FPS();
			}
			else
			{
				this.cameraSwitchCount++;
				this.ChangeCamera();
			}
			break;
		case RCC_Camera.CameraMode.WHEEL:
			if (this.wheelCam)
			{
				this.WHEEL();
			}
			else
			{
				this.cameraSwitchCount++;
				this.ChangeCamera();
			}
			break;
		case RCC_Camera.CameraMode.FIXED:
			if (this.fixedCam)
			{
				this.FIXED();
			}
			else
			{
				this.cameraSwitchCount++;
				this.ChangeCamera();
			}
			break;
		}
	}

	public void ChangeCamera()
	{
		this.cameraSwitchCount++;
		if (this.cameraSwitchCount >= 4)
		{
			this.cameraSwitchCount = 0;
		}
		if (this.fixedCam)
		{
			this.fixedCam.canTrackNow = false;
		}
	}

	private void FPS()
	{
		if (base.transform.parent != this.hoodCam)
		{
			base.transform.SetParent(this.hoodCam.transform, false);
			base.transform.position = this.hoodCam.transform.position;
			base.transform.rotation = this.hoodCam.transform.rotation;
			this.targetFieldOfView = 70f;
		}
	}

	private void WHEEL()
	{
		if (base.transform.parent != this.wheelCam)
		{
			base.transform.SetParent(this.wheelCam.transform, false);
			base.transform.position = this.wheelCam.transform.position;
			base.transform.rotation = this.wheelCam.transform.rotation;
			this.targetFieldOfView = 60f;
		}
	}

	private void TPS()
	{
		if (base.transform.parent != null)
		{
			base.transform.SetParent(null);
		}
		if (this.targetPosition == Vector3.zero)
		{
			this.targetPosition = this._playerCar.position;
			this.targetPosition -= base.transform.rotation * Vector3.forward * this.distance;
			base.transform.position = this.targetPosition;
			this.pastFollowerPosition = base.transform.position;
			this.pastTargetPosition = this.targetPosition;
		}
		this.targetFieldOfView = Mathf.Lerp(this.cam.fieldOfView, Mathf.Lerp(this.minimumFOV, this.maximumFOV, this.speed / 150f) + 5f * Mathf.Cos(1f * this.index), Time.deltaTime * 10f);
		this.tiltAngle = Mathf.Lerp(0f, this.maximumTilt * (float)((int)Mathf.Clamp(-this.playerCar.InverseTransformDirection(this.playerRigid.velocity).x, -1f, 1f)), Mathf.Abs(this.playerCar.InverseTransformDirection(this.playerRigid.velocity).x) / 50f);
		float b = this.playerCar.eulerAngles.y + Mathf.Clamp(this.playerRigid.transform.InverseTransformDirection(this.playerRigid.velocity).z, -10f, 0f) * 18f;
		float b2 = this.playerCar.position.y + this.height;
		float num = this.targetPosition.y;
		float num2 = base.transform.eulerAngles.y;
		this.rotationDamping = Mathf.Lerp(1f, 5f, this.playerRigid.velocity.magnitude * 3.6f / 10f);
		num2 = Mathf.LerpAngle(num2, b, Time.deltaTime * this.rotationDamping);
		Quaternion rotation = Quaternion.Euler(0f, num2, this.tiltAngle);
		num = Mathf.Lerp(num, b2, this.heightDamping * Time.deltaTime);
		this.targetPosition = this.playerCar.position;
		if (RCC_Settings.Instance.behaviorType != RCC_Settings.BehaviorType.Drift)
		{
			this.targetPosition -= rotation * Vector3.forward * this.distance;
			this.targetPosition = new Vector3(this.targetPosition.x, num, this.targetPosition.z);
		}
		else
		{
			this.targetPosition -= base.transform.rotation * Vector3.forward * this.distance;
			this.targetPosition = new Vector3(this.targetPosition.x, num, this.targetPosition.z);
		}
		base.transform.position = this.SmoothApproach(this.pastFollowerPosition, this.pastTargetPosition, this.targetPosition, Mathf.Clamp(0.1f, this.speed, float.PositiveInfinity));
		this.pastFollowerPosition = base.transform.position;
		this.pastTargetPosition = this.targetPosition;
		base.transform.LookAt(new Vector3(this.playerCar.position.x, this.playerCar.position.y + 1f, this.playerCar.position.z));
		this.pivot.transform.localPosition = Vector3.Lerp(this.pivot.transform.localPosition, new Vector3(UnityEngine.Random.insideUnitSphere.x / 2f, UnityEngine.Random.insideUnitSphere.y, UnityEngine.Random.insideUnitSphere.z) * this.speed * this.maxShakeAmount, Time.deltaTime * 1f);
		this.collisionPos = Vector3.Lerp(this.collisionPos, Vector3.zero, Time.deltaTime * 5f);
		this.collisionRot = Quaternion.Lerp(this.collisionRot, Quaternion.identity, Time.deltaTime * 5f);
		this.pivot.transform.localPosition = Vector3.Lerp(this.pivot.transform.localPosition, this.collisionPos, Time.deltaTime * 5f);
		this.pivot.transform.localRotation = Quaternion.Lerp(this.pivot.transform.localRotation, this.collisionRot, Time.deltaTime * 5f);
	}

	private void FIXED()
	{
		if (base.transform.parent != null)
		{
			base.transform.SetParent(null);
		}
		this.fixedCam.canTrackNow = true;
		this.fixedCam.player = this.playerCar;
	}

	private Vector3 SmoothApproach(Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float delta)
	{
		if (float.IsNaN(delta) || float.IsInfinity(delta) || pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
		{
			return base.transform.position;
		}
		float num = Time.deltaTime * delta;
		Vector3 b = (targetPosition - pastTargetPosition) / num;
		Vector3 a = pastPosition - pastTargetPosition + b;
		return targetPosition - b + a * Mathf.Exp(-num);
	}

	public void Collision(Collision collision)
	{
		if (!base.enabled || this.cameraMode != RCC_Camera.CameraMode.TPS)
		{
			return;
		}
		Vector3 vector = collision.relativeVelocity;
		vector *= 1f - Mathf.Abs(Vector3.Dot(base.transform.up, collision.contacts[0].normal));
		float num = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, vector.normalized));
		if (vector.magnitude * num >= 5f)
		{
			this.localVector = base.transform.InverseTransformDirection(vector) / 50f;
			this.collisionPos -= this.localVector * 5f;
			this.collisionRot = Quaternion.Euler(new Vector3(-this.localVector.z * 5f, 0f, -this.localVector.x * 50f));
			this.cam.fieldOfView = this.cam.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 30f);
			this.index = Mathf.Clamp(collision.relativeVelocity.magnitude / 5f, 0f, 10f);
		}
	}

	public Transform playerCar;

	private Rigidbody playerRigid;

	private Camera cam;

	public GameObject pivot;

	private GameObject boundCenter;

	public RCC_Camera.CameraMode cameraMode;

	public float distance = 6f;

	public float height = 2f;

	private float heightDamping = 5f;

	private float rotationDamping = 3f;

	public float targetFieldOfView = 60f;

	public float minimumFOV = 55f;

	public float maximumFOV = 70f;

	public float maximumTilt = 15f;

	private float tiltAngle;

	internal int cameraSwitchCount;

	private RCCHoodCamera hoodCam;

	private RCCWheelCamera wheelCam;

	private RCC_MainFixedCamera fixedCam;

	private Vector3 targetPosition = Vector3.zero;

	private Vector3 pastFollowerPosition;

	private Vector3 pastTargetPosition = Vector3.zero;

	private float speed;

	public float maxShakeAmount = 0.01f;

	private Vector3 localVector;

	private Vector3 collisionPos;

	private Quaternion collisionRot;

	private float index;

	public enum CameraMode
	{
		TPS,
		FPS,
		WHEEL,
		FIXED
	}
}
