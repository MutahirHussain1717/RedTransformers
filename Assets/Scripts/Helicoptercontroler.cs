using System;
using UnityEngine;

public class Helicoptercontroler : MonoBehaviour
{
	private void Start()
	{
		Helicoptercontroler.heli_trans_check = false;
		Helicoptercontroler.heli_trans_up_check = false;
	}

	private void Update()
	{
		if (Helicoptercontroler.heli_trans_check)
		{
			base.gameObject.transform.Translate(0f, 0f, 0.7f);
		}
		if (Helicoptercontroler.heli_trans_up_check)
		{
			base.gameObject.transform.Translate(0f, 0.2f, 0f);
		}
	}

	public static bool heli_trans_check;

	public static bool heli_trans_up_check;
}
