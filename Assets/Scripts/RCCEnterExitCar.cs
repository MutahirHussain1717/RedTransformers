using System;
using System.Collections;
using UnityEngine;

public class RCCEnterExitCar : MonoBehaviour
{
	private void Awake()
	{
		this.carController = base.GetComponent<RCC_CarControllerV3>();
		this.carCamera = UnityEngine.Object.FindObjectOfType<RCC_Camera>().gameObject;
		if (UnityEngine.Object.FindObjectOfType<RCC_DashboardInputs>())
		{
			this.dashboard = UnityEngine.Object.FindObjectOfType<RCC_DashboardInputs>().gameObject;
		}
		if (!this.getOutPosition)
		{
			GameObject gameObject = new GameObject("Get Out Position");
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = new Vector3(-3f, 0f, 0f);
			gameObject.transform.localRotation = Quaternion.identity;
			this.getOutPosition = gameObject.transform;
		}
	}

	private void Start()
	{
		if (this.dashboard)
		{
			base.StartCoroutine("DisableDashboard");
		}
	}

	private IEnumerator DisableDashboard()
	{
		yield return new WaitForEndOfFrame();
		this.dashboard.SetActive(false);
		yield break;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(RCC_Settings.Instance.enterExitVehicleKB) && this.opened && !this.temp)
		{
			this.GetOut();
			this.opened = false;
			this.temp = false;
		}
		if (this.isPlayerIn)
		{
			this.carController.canControl = true;
		}
		else
		{
			this.carController.canControl = false;
		}
	}

	private IEnumerator Act(GameObject fpsplayer)
	{
		this.player = fpsplayer;
		if (!this.opened && !this.temp)
		{
			this.GetIn();
			this.opened = true;
			this.temp = true;
			yield return new WaitForSeconds(this.waitTime);
			this.temp = false;
		}
		yield break;
	}

	private void GetIn()
	{
		this.isPlayerIn = true;
		this.carCamera.SetActive(true);
		if (this.carCamera.GetComponent<RCC_Camera>())
		{
			this.carCamera.GetComponent<RCC_Camera>().cameraSwitchCount = 10;
			this.carCamera.GetComponent<RCC_Camera>().ChangeCamera();
		}
		this.carCamera.transform.GetComponent<RCC_Camera>().SetPlayerCar(base.gameObject);
		this.player.transform.SetParent(base.transform);
		this.player.transform.localPosition = Vector3.zero;
		this.player.transform.localRotation = Quaternion.identity;
		this.player.SetActive(false);
		base.GetComponent<RCC_CarControllerV3>().canControl = true;
		if (this.dashboard)
		{
			this.dashboard.SetActive(true);
			this.dashboard.GetComponent<RCC_DashboardInputs>().GetVehicle(base.GetComponent<RCC_CarControllerV3>());
		}
		base.SendMessage("StartEngine");
	}

	private void GetOut()
	{
		this.isPlayerIn = false;
		this.player.transform.SetParent(null);
		this.player.transform.position = this.getOutPosition.position;
		this.player.transform.rotation = this.getOutPosition.rotation;
		this.carCamera.SetActive(false);
		this.player.SetActive(true);
		base.GetComponent<RCC_CarControllerV3>().canControl = false;
		base.GetComponent<RCC_CarControllerV3>().engineRunning = false;
		if (this.dashboard)
		{
			this.dashboard.SetActive(false);
		}
	}

	private RCC_CarControllerV3 carController;

	private GameObject carCamera;

	private GameObject player;

	private GameObject dashboard;

	public Transform getOutPosition;

	public bool isPlayerIn;

	private bool opened;

	private float waitTime = 1f;

	private bool temp;
}
