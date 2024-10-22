using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BikeAnimation : MonoBehaviour
{
	private void Awake()
	{
		this.BikeScript = this.myBike.GetComponent<BikeControl>();
		this.animator = this.player.GetComponent<Animator>();
		this.myPosition = this.player.localPosition;
		this.myRotation = this.player.localRotation;
		this.DisableRagdoll(true);
		this.bikeRigidbody = this.myBike.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (this.timer != 0f)
		{
			this.timer = Mathf.MoveTowards(this.timer, 0f, Time.deltaTime);
		}
		Vector3 vector;
		if (this.BikeScript.grounded)
		{
			vector = this.eventPoint.TransformDirection(Vector3.forward);
		}
		else
		{
			vector = this.eventPoint.TransformDirection(0f, -0.25f, 1f);
		}
		UnityEngine.Debug.DrawRay(this.eventPoint.position, vector, Color.red);
		RaycastHit raycastHit;
		if (Physics.Raycast(this.eventPoint.position, vector, out raycastHit, 1f) && this.BikeScript.speed > 50f)
		{
			if (raycastHit.collider.GetType() == typeof(BoxCollider))
			{
				if (raycastHit.collider.gameObject.GetComponent<BoxCollider>().isTrigger)
				{
					UnityEngine.Debug.Log("nothit");
				}
				else
				{
					UnityEngine.Debug.Log("hit");
					if (this.player.parent != null)
					{
						this.crashSound.GetComponent<AudioSource>().Play();
						this.player.parent = null;
					}
					this.DisableRagdoll(true);
					this.player.GetComponent<Animator>().enabled = false;
					this.BikeScript.crash = true;
					this.timer = this.RestTime;
				}
			}
			else
			{
				UnityEngine.Debug.Log("hit");
				if (this.player.parent != null)
				{
					this.crashSound.GetComponent<AudioSource>().Play();
					this.player.parent = null;
				}
				this.DisableRagdoll(true);
				this.player.GetComponent<Animator>().enabled = false;
				this.BikeScript.crash = true;
				this.timer = this.RestTime;
			}
		}
		if (this.timer == 0f)
		{
			this.player.GetComponent<Animator>().enabled = true;
			this.DisableRagdoll(false);
			this.player.parent = this.BikeScript.bikeSetting.MainBody.transform;
			this.player.localPosition = this.myPosition;
			this.player.localRotation = this.myRotation;
			if (this.BikeScript.crash)
			{
				this.bikeRigidbody.AddForce(Vector3.up * 10000f);
				this.bikeRigidbody.MoveRotation(Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f));
				this.BikeScript.crash = false;
			}
		}
		if (!this.player.GetComponent<Animator>().enabled)
		{
			return;
		}
		if (this.BikeScript.speed > 50f && this.grounded)
		{
			this.steer = this.BikeScript.steer;
		}
		else
		{
			this.steer = Mathf.MoveTowards(this.steer, 0f, Time.deltaTime * 10f);
		}
		if (this.BikeScript.grounded)
		{
			this.grounded = true;
			this.groundedTime = 2f;
		}
		else
		{
			this.groundedTime = Mathf.MoveTowards(this.groundedTime, 0f, Time.deltaTime * 10f);
			if (this.groundedTime == 0f)
			{
				this.grounded = false;
			}
		}
		if (this.BikeScript.currentGear > 0 || !this.BikeScript.Backward)
		{
			this.speed = this.BikeScript.speed;
		}
		else
		{
			this.speed = -this.BikeScript.speed;
		}
		this.animator.SetFloat("speed", this.speed);
		this.animator.SetFloat("right", this.steer);
		this.animator.SetBool("grounded", this.grounded);
	}

	private void DisableRagdoll(bool active)
	{
		Component[] componentsInChildren = this.player.GetComponentsInChildren(typeof(Rigidbody));
		foreach (Rigidbody rigidbody in componentsInChildren)
		{
			rigidbody.isKinematic = !active;
		}
		Component[] componentsInChildren2 = this.player.GetComponentsInChildren(typeof(Collider));
		foreach (Collider collider in componentsInChildren2)
		{
			collider.enabled = active;
		}
	}

	private void OnAnimatorIK()
	{
		if (!this.player.GetComponent<Animator>().enabled)
		{
			return;
		}
		if (this.animator)
		{
			if (this.ikActive)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
				if (this.IKPoints.leftHand != null)
				{
					this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.IKPoints.leftHand.position);
					this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.IKPoints.leftHand.rotation);
				}
				if (this.speed > -1f)
				{
					if (this.IKPoints.rightHand != null)
					{
						this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.IKPoints.rightHand.position);
						this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.IKPoints.rightHand.rotation);
					}
					if (this.IKPoints.rightFoot != null)
					{
						this.animator.SetIKPosition(AvatarIKGoal.RightFoot, this.IKPoints.rightFoot.position);
						this.animator.SetIKRotation(AvatarIKGoal.RightFoot, this.IKPoints.rightFoot.rotation);
					}
					if (this.IKPoints.leftFoot != null && this.BikeScript.speed > 30f)
					{
						this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, this.IKPoints.leftFoot.position);
						this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, this.IKPoints.leftFoot.rotation);
					}
				}
			}
			else
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
			}
		}
	}

	protected Animator animator;

	public bool ikActive;

	public float RestTime = 5f;

	public BikeAnimation.IKPointsClass IKPoints;

	public Transform myBike;

	public Transform player;

	public Transform eventPoint;

	public AudioSource crashSound;

	private Rigidbody bikeRigidbody;

	private BikeControl BikeScript;

	private Vector3 myPosition;

	private Quaternion myRotation;

	private float timer;

	private float steer;

	private float speed;

	private float groundedTime;

	private bool grounded = true;

	[Serializable]
	public class IKPointsClass
	{
		public Transform rightHand;

		public Transform leftHand;

		public Transform rightFoot;

		public Transform leftFoot;
	}
}
