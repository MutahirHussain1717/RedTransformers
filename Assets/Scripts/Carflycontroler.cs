using System;
using UnityEngine;

public class Carflycontroler : MonoBehaviour
{
	private void Start()
	{
		Carflycontroler.car_trans_check = false;
		Carflycontroler.car_trans_up_check = false;
	}

	private void Update()
	{
		if (Carflycontroler.car_trans_check)
		{
			base.gameObject.transform.Translate(0f, 0f, 0.8f);
		}
		if (Carflycontroler.car_trans_up_check)
		{
			base.gameObject.transform.Translate(0f, 0.2f, 0f);
		}
	}

	public static bool car_trans_check;

	public static bool car_trans_up_check;
}
