using System;
using UnityEngine;

public class TSPlayerFinder : MonoBehaviour
{
	private void Start()
	{
		this.manager = UnityEngine.Object.FindObjectOfType<TSMainManager>();
		this.myTransform = base.transform;
		this.GetNearestPointBruteSearch();
	}

	private void Update()
	{
		this.FindNearestPoint();
	}

	private void FindNearestPoint()
	{
		float currentPDistance = (this.currentPoint.point - this.myTransform.position).sqrMagnitude;
		this.newPoint = this.currentPoint;
		for (int i = 0; i < this.currentPoint.nearbyPoints.Length; i++)
		{
			int lane = this.currentPoint.nearbyPoints[i].lane;
			int connector = this.currentPoint.nearbyPoints[i].connector;
			int pointIndex = this.currentPoint.nearbyPoints[i].pointIndex;
			currentPDistance = this.GetNewDistance(lane, connector, pointIndex, currentPDistance);
		}
		this.currentPoint = this.newPoint;
	}

	private float GetNewDistance(int lane, int connector, int pointindex, float currentPDistance)
	{
		TSPoints tspoints;
		if (connector == -1)
		{
			tspoints = this.manager.lanes[lane].points[pointindex];
		}
		else
		{
			tspoints = this.manager.lanes[lane].connectors[connector].points[pointindex];
		}
		float sqrMagnitude = (tspoints.point - this.myTransform.position).sqrMagnitude;
		if (sqrMagnitude < currentPDistance)
		{
			this.newPoint = tspoints;
			this.laneFound = lane;
			this.connectorFound = connector;
			this.pointFound = pointindex;
			return sqrMagnitude;
		}
		return currentPDistance;
	}

	private void GetNearestPointBruteSearch()
	{
		float num = float.MaxValue;
		for (int i = 0; i < this.manager.lanes.Length; i++)
		{
			for (int j = 0; j < this.manager.lanes[i].points.Length; j++)
			{
				float sqrMagnitude = (this.manager.lanes[i].points[j].point - this.myTransform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					this.laneFound = i;
					this.connectorFound = -1;
					this.pointFound = j;
					num = sqrMagnitude;
					this.currentPoint = this.manager.lanes[i].points[j];
				}
			}
			for (int k = 0; k < this.manager.lanes[i].connectors.Length; k++)
			{
				for (int l = 0; l < this.manager.lanes[i].connectors[k].points.Length; l++)
				{
					float sqrMagnitude2 = (this.manager.lanes[i].connectors[k].points[l].point - this.myTransform.position).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						this.laneFound = i;
						this.connectorFound = k;
						this.pointFound = l;
						num = sqrMagnitude2;
						this.currentPoint = this.manager.lanes[i].connectors[k].points[l];
					}
				}
			}
		}
	}

	private TSMainManager manager;

	public TSPoints currentPoint;

	public int laneFound;

	public int connectorFound;

	public int pointFound;

	private TSPoints newPoint;

	private Transform myTransform;
}
