using System;
using UnityEngine;

public class WFX_Demo_Wall : MonoBehaviour
{
	private void OnMouseDown()
	{
		RaycastHit raycastHit = default(RaycastHit);
		if (base.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out raycastHit, 9999f))
		{
			GameObject gameObject = this.demo.spawnParticle();
			gameObject.transform.position = raycastHit.point;
			gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
		}
	}

	public WFX_Demo_New demo;
}
