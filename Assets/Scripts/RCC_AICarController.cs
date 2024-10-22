using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/AI Controller")]
public class RCC_AICarController : MonoBehaviour
{
	private void Awake()
	{
		this.carController = base.GetComponent<RCC_CarControllerV3>();
		this.rigid = base.GetComponent<Rigidbody>();
		this.carController.AIController = true;
		this.waypointsContainer = (UnityEngine.Object.FindObjectOfType(typeof(RCC_AIWaypointsContainer)) as RCC_AIWaypointsContainer);
		this.navigatorObject = new GameObject("Navigator");
		this.navigatorObject.transform.parent = base.transform;
		this.navigatorObject.transform.localPosition = Vector3.zero;
		this.navigatorObject.AddComponent<NavMeshAgent>();
		this.navigatorObject.GetComponent<NavMeshAgent>().radius = 1f;
		this.navigatorObject.GetComponent<NavMeshAgent>().speed = 1f;
		this.navigatorObject.GetComponent<NavMeshAgent>().height = 1f;
		this.navigatorObject.GetComponent<NavMeshAgent>().avoidancePriority = 99;
		this.navigator = this.navigatorObject.GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		this.navigator.transform.localPosition = new Vector3(0f, this.carController.FrontLeftWheelCollider.transform.localPosition.y, this.carController.FrontLeftWheelCollider.transform.localPosition.z);
	}

	private void FixedUpdate()
	{
		if (!this.carController.canControl)
		{
			return;
		}
		this.Navigation();
		this.FixedRaycasts();
		this.ApplyTorques();
		this.Resetting();
	}

	private void Navigation()
	{
		if (!this.waypointsContainer)
		{
			UnityEngine.Debug.LogError("Waypoints Container Couldn't Found!");
			base.enabled = false;
			return;
		}
		if (this.waypointsContainer && this.waypointsContainer.waypoints.Count < 1)
		{
			UnityEngine.Debug.LogError("Waypoints Container Doesn't Have Any Waypoints!");
			base.enabled = false;
			return;
		}
		Vector3 vector = base.transform.InverseTransformPoint(new Vector3(this.waypointsContainer.waypoints[this.currentWaypoint].position.x, base.transform.position.y, this.waypointsContainer.waypoints[this.currentWaypoint].position.z));
		float num = Mathf.Clamp(base.transform.InverseTransformDirection(this.navigator.desiredVelocity).x * 1.5f, -1f, 1f);
		this.navigator.SetDestination(this.waypointsContainer.waypoints[this.currentWaypoint].position);
		if (this.carController.direction == 1)
		{
			if (!this.ignoreWaypointNow)
			{
				this.steerInput = Mathf.Clamp(num + this.rayInput, -1f, 1f);
			}
			else
			{
				this.steerInput = Mathf.Clamp(this.rayInput, -1f, 1f);
			}
		}
		else
		{
			this.steerInput = Mathf.Clamp(-num - this.rayInput, -1f, 1f);
		}
		if (!this.inBrakeZone)
		{
			if (this.carController.speed >= 25f)
			{
				this.brakeInput = Mathf.Lerp(0f, 0.25f, Mathf.Abs(this.steerInput));
			}
			else
			{
				this.brakeInput = 0f;
			}
		}
		else
		{
			this.brakeInput = Mathf.Lerp(0f, 1f, (this.carController.speed - this.maximumSpeedInBrakeZone) / this.maximumSpeedInBrakeZone);
		}
		if (!this.inBrakeZone)
		{
			if (this.carController.speed >= 10f)
			{
				if (!this.carController.changingGear)
				{
					this.gasInput = Mathf.Clamp(1f - (Mathf.Abs(num / 5f) - Mathf.Abs(this.rayInput / 5f)), 0.5f, 1f);
				}
				else
				{
					this.gasInput = 0f;
				}
			}
			else if (!this.carController.changingGear)
			{
				this.gasInput = 1f;
			}
			else
			{
				this.gasInput = 0f;
			}
		}
		else if (!this.carController.changingGear)
		{
			this.gasInput = Mathf.Lerp(1f, 0f, this.carController.speed / this.maximumSpeedInBrakeZone);
		}
		else
		{
			this.gasInput = 0f;
		}
		if (vector.magnitude < (float)this.nextWaypointPassRadius)
		{
			this.currentWaypoint++;
			this.totalWaypointPassed++;
			if (this.currentWaypoint >= this.waypointsContainer.waypoints.Count)
			{
				this.currentWaypoint = 0;
				this.lap++;
			}
		}
	}

	private void Resetting()
	{
		if (this.carController.speed <= 5f && base.transform.InverseTransformDirection(this.rigid.velocity).z < 1f)
		{
			this.resetTime += Time.deltaTime;
		}
		if (this.resetTime >= 4f)
		{
			this.carController.direction = -1;
		}
		if (this.resetTime >= 6f || this.carController.speed >= 25f)
		{
			this.carController.direction = 1;
			this.resetTime = 0f;
		}
	}

	private void FixedRaycasts()
	{
		Vector3 point = base.transform.TransformDirection(new Vector3(0f, 0f, 1f));
		Vector3 vector = new Vector3(base.transform.localPosition.x, this.carController.FrontLeftWheelCollider.transform.position.y, base.transform.localPosition.z);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(25f, base.transform.up) * point * (float)this.wideRayLength, Color.white);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-25f, base.transform.up) * point * (float)this.wideRayLength, Color.white);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(7f, base.transform.up) * point * (float)this.tightRayLength, Color.white);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-7f, base.transform.up) * point * (float)this.tightRayLength, Color.white);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(90f, base.transform.up) * point * (float)this.sideRayLength, Color.white);
		UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-90f, base.transform.up) * point * (float)this.sideRayLength, Color.white);
		RaycastHit raycastHit;
		float num;
		bool flag;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(25f, base.transform.up) * point, out raycastHit, (float)this.wideRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(25f, base.transform.up) * point * (float)this.wideRayLength, Color.red);
			num = Mathf.Lerp(-0.5f, 0f, raycastHit.distance / (float)this.wideRayLength);
			flag = true;
		}
		else
		{
			num = 0f;
			flag = false;
		}
		float num2;
		bool flag2;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(-25f, base.transform.up) * point, out raycastHit, (float)this.wideRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-25f, base.transform.up) * point * (float)this.wideRayLength, Color.red);
			num2 = Mathf.Lerp(0.5f, 0f, raycastHit.distance / (float)this.wideRayLength);
			flag2 = true;
		}
		else
		{
			num2 = 0f;
			flag2 = false;
		}
		float num3;
		bool flag3;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(7f, base.transform.up) * point, out raycastHit, (float)this.tightRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(7f, base.transform.up) * point * (float)this.tightRayLength, Color.red);
			num3 = Mathf.Lerp(-1f, 0f, raycastHit.distance / (float)this.tightRayLength);
			flag3 = true;
		}
		else
		{
			num3 = 0f;
			flag3 = false;
		}
		float num4;
		bool flag4;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(-7f, base.transform.up) * point, out raycastHit, (float)this.tightRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-7f, base.transform.up) * point * (float)this.tightRayLength, Color.red);
			num4 = Mathf.Lerp(1f, 0f, raycastHit.distance / (float)this.tightRayLength);
			flag4 = true;
		}
		else
		{
			num4 = 0f;
			flag4 = false;
		}
		float num5;
		bool flag5;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(90f, base.transform.up) * point, out raycastHit, (float)this.sideRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(90f, base.transform.up) * point * (float)this.sideRayLength, Color.red);
			num5 = Mathf.Lerp(-1f, 0f, raycastHit.distance / (float)this.sideRayLength);
			flag5 = true;
		}
		else
		{
			num5 = 0f;
			flag5 = false;
		}
		float num6;
		bool flag6;
		if (Physics.Raycast(vector, Quaternion.AngleAxis(-90f, base.transform.up) * point, out raycastHit, (float)this.sideRayLength, this.obstacleLayers) && !raycastHit.collider.isTrigger && raycastHit.transform.root != base.transform)
		{
			UnityEngine.Debug.DrawRay(vector, Quaternion.AngleAxis(-90f, base.transform.up) * point * (float)this.sideRayLength, Color.red);
			num6 = Mathf.Lerp(1f, 0f, raycastHit.distance / (float)this.sideRayLength);
			flag6 = true;
		}
		else
		{
			num6 = 0f;
			flag6 = false;
		}
		if (flag || flag2 || flag3 || flag4 || flag5 || flag6)
		{
			this.raycasting = true;
		}
		else
		{
			this.raycasting = false;
		}
		if (this.raycasting)
		{
			this.rayInput = num + num4 + num3 + num2 + num5 + num6;
		}
		else
		{
			this.rayInput = 0f;
		}
		if (this.raycasting && Mathf.Abs(this.rayInput) > 0.5f)
		{
			this.ignoreWaypointNow = true;
		}
		else
		{
			this.ignoreWaypointNow = false;
		}
	}

	private void ApplyTorques()
	{
		if (this.carController.direction == 1)
		{
			if (!this.limitSpeed)
			{
				this.carController.gasInput = this.gasInput;
			}
			else
			{
				this.carController.gasInput = this.gasInput * Mathf.Clamp01(Mathf.Lerp(10f, 0f, this.carController.speed / this.maximumSpeed));
			}
		}
		else
		{
			this.carController.gasInput = 0f;
		}
		if (this.smoothedSteer)
		{
			this.carController.steerInput = Mathf.Lerp(this.carController.steerInput, this.steerInput, Time.deltaTime * 20f);
		}
		else
		{
			this.carController.steerInput = this.steerInput;
		}
		if (this.carController.direction == 1)
		{
			this.carController.brakeInput = this.brakeInput;
		}
		else
		{
			this.carController.brakeInput = this.gasInput;
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.GetComponent<RCC_AIBrakeZone>())
		{
			this.inBrakeZone = true;
			this.maximumSpeedInBrakeZone = col.gameObject.GetComponent<RCC_AIBrakeZone>().targetSpeed;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.gameObject.GetComponent<RCC_AIBrakeZone>())
		{
			this.inBrakeZone = false;
			this.maximumSpeedInBrakeZone = 0f;
		}
	}

	private RCC_CarControllerV3 carController;

	private Rigidbody rigid;

	private RCC_AIWaypointsContainer waypointsContainer;

	public int currentWaypoint;

	public LayerMask obstacleLayers = -1;

	public int wideRayLength = 20;

	public int tightRayLength = 20;

	public int sideRayLength = 3;

	private float rayInput;

	private bool raycasting;

	private float resetTime;

	private float steerInput;

	private float gasInput;

	private float brakeInput;

	public bool limitSpeed;

	public float maximumSpeed = 100f;

	public bool smoothedSteer = true;

	private float maximumSpeedInBrakeZone;

	private bool inBrakeZone;

	public int lap;

	public int totalWaypointPassed;

	public int nextWaypointPassRadius = 40;

	public bool ignoreWaypointNow;

	private NavMeshAgent navigator;

	private GameObject navigatorObject;
}
