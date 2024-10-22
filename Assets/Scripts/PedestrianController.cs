using System;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
	private void Start()
	{
		this.body = base.GetComponent<Rigidbody>();
		this.animator = base.GetComponent<Animator>();
		this.locomotion = new Locomotion(this.animator);
		TSTrafficAI component = base.GetComponent<TSTrafficAI>();
		component.OnUpdateAI = new TSTrafficAI.OnUpdateAIDelegate(this.OnAIUpdate);
		component.UpdateCarSpeed = new TSTrafficAI.GetCarSpeedDelegate(this.UpdateSpeed);
	}

	private void UpdateSpeed(out float carSpeed)
	{
		carSpeed = this.body.velocity.z;
	}

	private void OnAIUpdate(float steering, float brake, float throttle, bool isUpSideDown)
	{
		this.speed = Mathf.Clamp01(throttle - brake);
		this.direction = steering;
	}

	private void Update()
	{
		if (this.animator)
		{
			if (this.body.constraints != RigidbodyConstraints.FreezeRotation)
			{
				this.body.constraints = RigidbodyConstraints.FreezeRotation;
			}
			this.locomotion.Do(this.speed, 45f * this.direction);
		}
	}

	protected Animator animator;

	private float speed;

	public float direction;

	private Locomotion locomotion;

	private Rigidbody body;
}
