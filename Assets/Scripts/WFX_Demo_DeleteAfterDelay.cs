using System;
using UnityEngine;

public class WFX_Demo_DeleteAfterDelay : MonoBehaviour
{
	private void Update()
	{
		this.delay -= Time.deltaTime;
		if (this.delay < 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public float delay = 1f;
}
