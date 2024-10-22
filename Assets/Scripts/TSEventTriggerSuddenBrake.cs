using System;
using System.Collections;
using UnityEngine;

public class TSEventTriggerSuddenBrake : TSEventTrigger
{
	private void OnTriggerEnter()
	{
		if (!this.isTriggered)
		{
			base.StartCoroutine(this.CheckForPointsCarReference());
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.w1 = new WaitForSeconds(this.stopTime);
	}

	public override void InitializeMe()
	{
		this.spawnCarOnStartingPoint = false;
	}

	private IEnumerator CheckForPointsCarReference()
	{
		this.isTriggered = true;
		while (this.tAI == null)
		{
			if (this.manager.lanes[this.startingPoint.lane].points[this.startingPoint.point].carwhoReserved != null)
			{
				this.tAI = this.manager.lanes[this.startingPoint.lane].points[this.startingPoint.point].carwhoReserved;
				this.nav = this.tAI.GetComponent<TSNavigation>();
				break;
			}
			yield return null;
		}
		base.DisableCarAI();
		this.tAI.GetComponent<TSSimpleCar>().OnAIUpdate(0f, 1f, 0f, false);
		yield return this.w1;
		base.EnableCarAI();
		this.tAI = null;
		yield break;
	}

	public float stopTime = 10f;

	private WaitForSeconds w1;
}
