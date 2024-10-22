using System;
using UnityEngine;
using UnityEngine.AI;

public class bl_RandomBot : MonoBehaviour
{
	private void FixedUpdate()
	{
		if (!this.Agent.hasPath)
		{
			this.RandomBot();
		}
	}

	private void RandomBot()
	{
		Vector3 vector = UnityEngine.Random.insideUnitSphere * 50f;
		vector += base.transform.position;
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(vector, out navMeshHit, 50f, 1);
		Vector3 position = navMeshHit.position;
		this.Agent.SetDestination(position);
	}

	private NavMeshAgent Agent
	{
		get
		{
			if (this.m_Agent == null)
			{
				this.m_Agent = base.GetComponent<NavMeshAgent>();
			}
			return this.m_Agent;
		}
	}

	private NavMeshAgent m_Agent;
}
