using System;
using UnityEngine;
using UnityEngine.UI;

public class RCC_Useless : MonoBehaviour
{
	private void Awake()
	{
		int value = 0;
		if (this.useless == RCC_Useless.Useless.Behavior)
		{
			switch (RCC_Settings.Instance.behaviorType)
			{
			case RCC_Settings.BehaviorType.Simulator:
				value = 0;
				break;
			case RCC_Settings.BehaviorType.Racing:
				value = 1;
				break;
			case RCC_Settings.BehaviorType.SemiArcade:
				value = 2;
				break;
			case RCC_Settings.BehaviorType.Drift:
				value = 3;
				break;
			case RCC_Settings.BehaviorType.Fun:
				value = 4;
				break;
			case RCC_Settings.BehaviorType.Custom:
				value = 5;
				break;
			}
		}
		else
		{
			if (!RCC_Settings.Instance.useAccelerometerForSteering && !RCC_Settings.Instance.useSteeringWheelForSteering)
			{
				value = 0;
			}
			if (RCC_Settings.Instance.useAccelerometerForSteering)
			{
				value = 1;
			}
			if (RCC_Settings.Instance.useSteeringWheelForSteering)
			{
				value = 2;
			}
		}
		base.GetComponent<Dropdown>().value = value;
		base.GetComponent<Dropdown>().RefreshShownValue();
	}

	public RCC_Useless.Useless useless;

	public enum Useless
	{
		Controller,
		Behavior
	}
}
