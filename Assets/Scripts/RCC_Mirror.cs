using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Mirror")]
public class RCC_Mirror : MonoBehaviour
{
	private void InvertCamera()
	{
		this.cam = base.GetComponent<Camera>();
		this.cam.ResetWorldToCameraMatrix();
		this.cam.ResetProjectionMatrix();
		this.cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
		this.carController = base.GetComponentInParent<RCC_CarControllerV3>();
	}

	private void OnPreRender()
	{
		GL.invertCulling = true;
	}

	private void OnPostRender()
	{
		GL.invertCulling = false;
	}

	private void Update()
	{
		if (!this.cam)
		{
			this.InvertCamera();
			return;
		}
		this.cam.enabled = this.carController.canControl;
	}

	private Camera cam;

	private RCC_CarControllerV3 carController;
}
