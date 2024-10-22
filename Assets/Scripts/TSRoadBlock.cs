using System;
using UnityEngine;

public class TSRoadBlock : MonoBehaviour
{
	private void Awake()
	{
		this.myID = base.GetInstanceID();
		if (this.manager == null)
		{
			this.manager = UnityEngine.Object.FindObjectOfType<TSMainManager>();
		}
	}

	private void OnEnable()
	{
		if (this.manager != null)
		{
			this.BlockPoints();
		}
	}

	private void OnDisable()
	{
		if (this.manager != null)
		{
			this.UnBlockPoints();
		}
	}

	public void BlockPoints()
	{
		for (int i = 0; i < this.blockingPoints.Length; i++)
		{
			this.SetPointReservationID(this.blockingPoints[i], this.myID);
		}
	}

	public void UnBlockPoints()
	{
		for (int i = 0; i < this.blockingPoints.Length; i++)
		{
			this.SetPointReservationID(this.blockingPoints[i], 0);
		}
	}

	private void SetPointReservationID(TSTrafficLight.TSPointReference point, int reservationID)
	{
		TSTrafficLight.TSPointReference tspointReference = new TSTrafficLight.TSPointReference();
		if (point.connector == -1)
		{
			this.manager.lanes[point.lane].points[point.point].reservationID = reservationID;
			this.manager.lanes[point.lane].points[point.point].carwhoReserved = null;
		}
		else
		{
			this.manager.lanes[point.lane].connectors[point.connector].points[point.point].reservationID = reservationID;
			this.manager.lanes[point.lane].connectors[point.connector].points[point.point].carwhoReserved = null;
			this.manager.lanes[point.lane].points[this.manager.lanes[point.lane].points.Length - 1].carwhoReserved = null;
			this.manager.lanes[point.lane].points[this.manager.lanes[point.lane].points.Length - 1].reservationID = reservationID;
			tspointReference.connector = -1;
			tspointReference.lane = point.lane;
			tspointReference.point = this.manager.lanes[point.lane].points.Length - 1;
		}
		this.SetRoadBlockAhead(point, reservationID != 0);
	}

	private void SetRoadBlockAhead(TSTrafficLight.TSPointReference point, bool setRoadBlock)
	{
		float num = 0f;
		int num2 = point.point;
		while (num < this.roadBlockAheadDistance && num2 >= 0)
		{
			this.manager.lanes[point.lane].points[num2].roadBlockAhead = setRoadBlock;
			num += this.manager.lanes[point.lane].points[num2].distanceToNextPoint;
			num2--;
		}
	}

	public TSTrafficLight.TSPointReference[] blockingPoints = new TSTrafficLight.TSPointReference[0];

	public TSMainManager manager;

	public float roadBlockAheadDistance = 40f;

	public float range = 10f;

	private int myID;
}
