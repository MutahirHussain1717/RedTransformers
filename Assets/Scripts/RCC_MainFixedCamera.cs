using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Fixed System/Main Camera")]
public class RCC_MainFixedCamera : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.GetVariables());
	}

	private IEnumerator GetVariables()
	{
		yield return new WaitForSeconds(1f);
		this.childCams = base.GetComponentsInChildren<RCC_ChildFixedCamera>();
		this.rccCamera = UnityEngine.Object.FindObjectOfType<RCC_Camera>();
		foreach (RCC_ChildFixedCamera rcc_ChildFixedCamera in this.childCams)
		{
			rcc_ChildFixedCamera.enabled = false;
			rcc_ChildFixedCamera.player = this.player;
		}
		this.camPositions = new Transform[this.childCams.Length];
		this.childDistances = new float[this.childCams.Length];
		for (int j = 0; j < this.camPositions.Length; j++)
		{
			this.camPositions[j] = this.childCams[j].transform;
			this.childDistances[j] = this.childCams[j].distance;
		}
		yield break;
	}

	private void Act()
	{
		foreach (RCC_ChildFixedCamera rcc_ChildFixedCamera in this.childCams)
		{
			rcc_ChildFixedCamera.enabled = false;
			rcc_ChildFixedCamera.player = this.player;
		}
	}

	private void Update()
	{
		if (!this.player)
		{
			return;
		}
		if (this.canTrackNow)
		{
			this.Tracking();
		}
		foreach (RCC_ChildFixedCamera rcc_ChildFixedCamera in this.childCams)
		{
			if (rcc_ChildFixedCamera.player != this.player)
			{
				rcc_ChildFixedCamera.player = this.player;
			}
		}
	}

	private void Tracking()
	{
		bool flag = false;
		for (int i = 0; i < this.camPositions.Length; i++)
		{
			this.distance = Vector3.Distance(this.camPositions[i].position, this.player.transform.position);
			if (this.distance <= this.childDistances[i])
			{
				flag = true;
				if (!this.childCams[i].enabled)
				{
					this.childCams[i].enabled = true;
				}
				if (this.rccCamera.transform.parent != this.childCams[i].transform)
				{
					this.rccCamera.transform.SetParent(this.childCams[i].transform);
					this.rccCamera.transform.localPosition = Vector3.zero;
					this.rccCamera.transform.localRotation = Quaternion.identity;
				}
				this.rccCamera.targetFieldOfView = Mathf.Lerp(this.rccCamera.targetFieldOfView, Mathf.Lerp(this.maximumFOV, this.minimumFOV, Vector3.Distance(this.rccCamera.transform.position, this.player.transform.position) * 2f / this.childDistances[i]), Time.deltaTime * 3f);
			}
			else
			{
				if (this.childCams[i].enabled)
				{
					this.childCams[i].enabled = false;
				}
				if (this.rccCamera.transform.parent == this.childCams[i].transform)
				{
					this.canTrackNow = false;
					this.rccCamera.transform.SetParent(null);
					this.childCams[i].transform.rotation = Quaternion.identity;
					this.rccCamera.cameraSwitchCount = 10;
					this.rccCamera.ChangeCamera();
				}
			}
		}
		if (!flag && this.rccCamera.cameraSwitchCount == 3)
		{
			this.canTrackNow = false;
			this.rccCamera.transform.SetParent(null);
			this.rccCamera.cameraSwitchCount = 10;
			this.rccCamera.ChangeCamera();
		}
	}

	private void OnDrawGizmos()
	{
		if (this.drawGizmos)
		{
			this.childCams = base.GetComponentsInChildren<RCC_ChildFixedCamera>();
			this.camPositions = new Transform[this.childCams.Length];
			this.childDistances = new float[this.childCams.Length];
			for (int i = 0; i < this.camPositions.Length; i++)
			{
				this.camPositions[i] = this.childCams[i].transform;
				this.childDistances[i] = this.childCams[i].distance;
				Gizmos.matrix = this.camPositions[i].transform.localToWorldMatrix;
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawCube(Vector3.zero, new Vector3(this.childDistances[i] * 2f, this.childDistances[i] / 2f, this.childDistances[i] * 2f));
			}
		}
	}

	private Transform[] camPositions;

	private RCC_ChildFixedCamera[] childCams;

	private float[] childDistances;

	private RCC_Camera rccCamera;

	private float distance;

	internal Transform player;

	public float minimumFOV = 20f;

	public float maximumFOV = 60f;

	public bool canTrackNow;

	public bool drawGizmos = true;
}
