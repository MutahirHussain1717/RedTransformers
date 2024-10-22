using System;
using UnityEngine;

public class PlaneControler : MonoBehaviour
{
	private void Start()
	{
		PlaneControler.plane_trans_check = false;
		PlaneControler.plane_trans_up_check = false;
	}

	private void Update()
	{
		if (PlaneControler.plane_trans_check)
		{
			base.gameObject.transform.Translate(0f, 0f, 1f);
		}
		if (PlaneControler.plane_trans_up_check)
		{
			base.gameObject.transform.Translate(0f, 0.2f, 0f);
		}
	}

	public static bool plane_trans_check;

	public static bool plane_trans_up_check;
}
