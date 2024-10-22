using System;
using UnityEngine;

[Serializable]
public class ECamera
{
	public ECamera(Camera cam, bool gui)
	{
		this.camera = cam;
		this.guiCamera = gui;
	}

	public Camera camera;

	public bool guiCamera;
}
