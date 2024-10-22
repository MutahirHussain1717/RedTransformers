using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{
	private void Start()
	{
		if (this.MovingPoints)
		{
			int childCount = this.MovingPoints.transform.childCount;
			this.targertArray = new Transform[childCount];
			for (int i = 0; i < this.targertArray.Length; i++)
			{
				this.targertArray[i] = this.MovingPoints.transform.GetChild(i);
			}
		}
		this.agentDestination = base.transform.Find("ReferenceMovingPoint");
		this.agent = base.GetComponent<NavMeshAgent>();
		this.agent.SetDestination(this.agentDestination.position);
		NavMeshMovement.canMove = true;
	}

	private void Update()
	{
		this.destination = this.targertArray[this.pointCount];
		if (NavMeshMovement.canMove)
		{
			this.agent.SetDestination(this.agentDestination.position);
			this.agentDestination.position = this.destination.position;
		}
		this.distance = Vector3.Distance(base.transform.position, this.destination.position);
		if (this.distance < this.targetSwitchDistance && this.pointCount != this.targertArray.Length)
		{
			this.pointCount++;
		}
		if (this.pointCount >= this.targertArray.Length)
		{
			this.pointCount = 0;
		}
	}

	private void PauseMovement()
	{
		this.agent.Stop(true);
		NavMeshMovement.canMove = false;
	}

	private void ResumeMovement()
	{
		this.agent.Resume();
		NavMeshMovement.canMove = true;
	}

	private NavMeshAgent agent;

	private Transform destination;

	public Transform MovingPoints;

	public int pointCount;

	private float distance;

	private Vector3 previousVel;

	public Transform[] targertArray;

	public static bool canMove;

	private Transform agentDestination;

	public float targetSwitchDistance = 5f;
}
