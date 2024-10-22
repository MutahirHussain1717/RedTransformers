using System;
using UnityEngine;

public class CharterScript : MonoBehaviour
{
	private void Start()
	{
		this.Targets = new GameObject[this.pathsGiven.transform.childCount];
		for (int i = 0; i < this.pathsGiven.transform.childCount; i++)
		{
			this.Targets[i] = this.pathsGiven.transform.GetChild(i).gameObject;
		}
		this.counter = 0;
	}

	private void Update()
	{
		Vector3 vector = base.transform.TransformDirection(Vector3.forward);
		UnityEngine.Debug.DrawRay(base.transform.position + new Vector3(this.frontoffSets.x, this.frontoffSets.y, this.frontoffSets.z), vector, Color.red);
		if (Physics.Raycast(base.transform.position + new Vector3(this.frontoffSets.x, this.frontoffSets.y, this.frontoffSets.z), vector, out this.hit, 6f))
		{
			if (!(this.hit.collider.name == "pasted__polySurface1"))
			{
				this.Move();
			}
		}
		else
		{
			this.Move();
		}
	}

	public void Move()
	{
		if (!this.isCollideWithPoliceCar)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.Targets[this.counter].transform.position, this.speed * Time.deltaTime);
			Vector3 worldPosition = new Vector3(this.Targets[this.counter].transform.position.x, base.transform.position.y, this.Targets[this.counter].transform.position.z);
			base.transform.LookAt(worldPosition);
			if (Vector3.Distance(this.Targets[this.counter].transform.position, base.transform.position) < 0.1f && !this.isComplete)
			{
				if (this.counter == this.Targets.Length - 1)
				{
					this.isComplete = true;
				}
				else
				{
					this.counter++;
				}
			}
			else if (this.isComplete && Vector3.Distance(this.Targets[this.counter].transform.position, base.transform.position) < 0.1f)
			{
				this.isComplete = false;
				this.counter = 0;
			}
		}
	}

	private void OnCollisionEnter(Collision col)
	{
	}

	public float speed;

	private RaycastHit hit;

	private Ray ray;

	public GameObject[] Targets;

	public GameObject pathsGiven;

	private bool isComplete;

	private int counter;

	public Vector3 frontoffSets;

	public bool isCollideWithPoliceCar;

	public GameObject temp;

	private Vector3 rayOrg;
}
