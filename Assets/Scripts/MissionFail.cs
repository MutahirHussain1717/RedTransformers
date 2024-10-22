using System;
using UnityEngine;

public class MissionFail : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			UnityEngine.Debug.Log("col");
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().Dia_Failed();
		}
	}
}
