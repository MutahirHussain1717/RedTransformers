using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Camera Config")]
public class RCC_CameraConfig : MonoBehaviour
{
	private void Awake()
	{
		if (this.automatic)
		{
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = Quaternion.identity;
			this.combinedBounds = base.GetComponentInChildren<Renderer>().bounds;
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != base.GetComponent<Renderer>() && renderer.GetComponent<ParticleSystem>() == null)
				{
					this.combinedBounds.Encapsulate(renderer.bounds);
				}
			}
			base.transform.rotation = rotation;
			this.distance = this.combinedBounds.size.z * 1.1f;
			this.height = this.combinedBounds.size.z * 0.35f;
		}
	}

	public void SetCameraSettings()
	{
		RCC_Camera rcc_Camera = UnityEngine.Object.FindObjectOfType<RCC_Camera>();
		if (!rcc_Camera)
		{
			return;
		}
		rcc_Camera.distance = this.distance;
		rcc_Camera.height = this.height;
	}

	public bool automatic = true;

	private Bounds combinedBounds;

	public float distance = 10f;

	public float height = 5f;
}
