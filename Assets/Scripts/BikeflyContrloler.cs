using System;
using UnityEngine;

public class BikeflyContrloler : MonoBehaviour
{
	private void Start()
	{
		BikeflyContrloler.bike_trans_check = false;
		BikeflyContrloler.bike_trans_up_check = false;
	}

	private void Update()
	{
		if (BikeflyContrloler.bike_trans_check)
		{
			base.gameObject.transform.Translate(0f, 0f, 0.7f);
		}
		if (BikeflyContrloler.bike_trans_up_check)
		{
			base.gameObject.transform.Translate(0f, 0.3f, 0f);
		}
	}

	public static bool bike_trans_check;

	public static bool bike_trans_up_check;
}
