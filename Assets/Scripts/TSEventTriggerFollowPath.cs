using System;
using System.Collections;
using UnityEngine;

public class TSEventTriggerFollowPath : TSEventTrigger
{
	public override void Awake()
	{
		base.Awake();
		this.w = new WaitForSeconds(this.playerSensorTempDisableTime);
	}

	private void OnTriggerEnter()
	{
		if (this.disableCarUntilTriggeredByPlayer)
		{
			base.EnableCarAI();
			this.tAI.reservedForEventTrigger = false;
			if (this.disableCarPlayerSensor)
			{
				base.StartCoroutine(this.TemporaryDisablePlayerSensor());
			}
		}
	}

	public override void InitializeMe()
	{
		this.spawnCarOnStartingPoint = true;
	}

	private IEnumerator EnableCarSensorAtEndPoint()
	{
		while (this.tAI != base.Point(this.eventEndingPoint).carwhoReserved)
		{
			yield return null;
		}
		this.tAI.playerSensor.enabled = true;
		yield break;
	}

	private IEnumerator TemporaryDisablePlayerSensor()
	{
		yield return null;
		this.tAI.playerSensor.enabled = false;
		yield return this.w;
		this.tAI.playerSensor.enabled = true;
		yield break;
	}

	public override void SetCar(TSTrafficAI car)
	{
		base.SetCar(car);
		car.ignoreTrafficLight = true;
		if (this.disableCarUntilTriggeredByPlayer)
		{
			base.DisableCarAI();
		}
	}

	public bool disableCarUntilTriggeredByPlayer;

	public bool disableCarPlayerSensor;

	public bool endEventWithEndingPoint;

	public float playerSensorTempDisableTime = 10f;

	private WaitForSeconds w;
}
