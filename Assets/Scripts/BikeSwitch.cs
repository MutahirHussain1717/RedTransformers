using System;
using UnityEngine;

public class BikeSwitch : MonoBehaviour
{
	public void CurrentBikeActive(int current)
	{
		int num = 0;
		foreach (Transform transform in this.Bikes)
		{
			if (current == num)
			{
				this.MyCamera.GetComponent<BikeCamera>().target = transform;
				this.MyCamera.GetComponent<BikeCamera>().Switch = 0;
				this.MyCamera.GetComponent<BikeCamera>().cameraSwitchView = transform.GetComponent<BikeControl>().bikeSetting.cameraSwitchView;
				this.MyCamera.GetComponent<BikeCamera>().BikerMan = transform.GetComponent<BikeControl>().bikeSetting.bikerMan;
				transform.GetComponent<BikeControl>().activeControl = true;
			}
			else
			{
				transform.GetComponent<BikeControl>().activeControl = false;
			}
			num++;
		}
	}

	public Transform[] Bikes;

	public Transform MyCamera;
}
