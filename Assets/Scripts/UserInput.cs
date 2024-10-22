using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
	private void Start()
	{
		this.waypoints = this.pathContainer.waypoints;
		this.currentPath[0] = this.waypoints[this.currentPoint];
		this.currentPath[1] = this.waypoints[this.currentPoint + 1];
		this.avgSpeed = this.speed / Vector3.Distance(this.currentPath[0].position, this.currentPath[1].position) * 100f;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKey("right"))
		{
			this.progress += Time.deltaTime * this.avgSpeed;
		}
		if (UnityEngine.Input.GetKey("left"))
		{
			this.progress -= Time.deltaTime * this.avgSpeed;
		}
		if (this.progress < 0f && this.currentPoint > 0)
		{
			this.currentPath[0] = this.waypoints[this.currentPoint - 1];
			this.currentPath[1] = this.waypoints[this.currentPoint];
			this.currentPoint--;
			this.progress = 100f;
			this.avgSpeed = this.speed / Vector3.Distance(this.currentPath[0].position, this.currentPath[1].position) * 100f;
		}
		else if (this.progress > 100f && this.currentPoint < this.waypoints.Length - 2)
		{
			this.currentPoint++;
			this.currentPath[0] = this.waypoints[this.currentPoint];
			this.currentPath[1] = this.waypoints[this.currentPoint + 1];
			this.progress = 0f;
			this.avgSpeed = this.speed / Vector3.Distance(this.currentPath[0].position, this.currentPath[1].position) * 100f;
		}
		else
		{
			if (this.progress <= 0f)
			{
				this.progress = 0f;
			}
			if (this.progress >= 100f)
			{
				this.progress = 100f;
			}
		}
		this.PointOnPath(this.progress / 100f);
	}

	private void PointOnPath(float number)
	{
		base.transform.position = iTween.PointOnPath(this.currentPath, number) + new Vector3(0f, this.sizeToAdd, 0f);
	}

	public PathManager pathContainer;

	public float speed = 10f;

	public float sizeToAdd = 1f;

	private Transform[] waypoints;

	public int currentPoint;

	private Transform[] currentPath = new Transform[2];

	public float progress;

	private float avgSpeed;
}
