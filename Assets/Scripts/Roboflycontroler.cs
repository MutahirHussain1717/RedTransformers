using System;
using UnityEngine;

public class Roboflycontroler : MonoBehaviour
{
	private void Start()
	{
		Roboflycontroler.car_trans_check = false;
		Roboflycontroler.car_trans_up_check = false;
	}

	private void Update()
	{
		if (Roboflycontroler.car_trans_check)
		{
			base.gameObject.transform.Translate(0f, 0f, 0.001f);
		}
		if (Roboflycontroler.car_trans_up_check)
		{
			base.gameObject.transform.Translate(0f, 0.2f, 0f);
		}
	}

	public static bool car_trans_check;

	public static bool car_trans_up_check;
}
