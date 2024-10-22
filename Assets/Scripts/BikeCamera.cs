using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BikeCamera : MonoBehaviour
{
	public void PoliceLightSwitch()
	{
		if (!this.target.gameObject.GetComponent<PoliceLights>())
		{
			return;
		}
		this.PLValue++;
		if (this.PLValue > 1)
		{
			this.PLValue = 0;
		}
		if (this.PLValue == 1)
		{
			this.target.gameObject.GetComponent<PoliceLights>().activeLight = true;
		}
		if (this.PLValue == 0)
		{
			this.target.gameObject.GetComponent<PoliceLights>().activeLight = false;
		}
	}

	public void CameraSwitch()
	{
		this.Switch++;
		if (this.Switch > this.cameraSwitchView.Count)
		{
			this.Switch = 0;
		}
	}

	public void BikeAccelForward(float amount)
	{
		this.bikeScript.accelFwd = amount;
	}

	public void BikeAccelBack(float amount)
	{
		this.bikeScript.accelBack = amount;
	}

	public void BikeSteer(float amount)
	{
		this.bikeScript.steerAmount = amount;
	}

	public void BikeHandBrake(bool HBrakeing)
	{
		this.bikeScript.brake = HBrakeing;
	}

	public void BikeShift(bool Shifting)
	{
		this.bikeScript.shift = Shifting;
	}

	public void RestBike()
	{
		if (this.restTime == 0f)
		{
			this.myRigidbody.AddForce(Vector3.up * 500000f);
			this.myRigidbody.MoveRotation(Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f));
			this.restTime = 2f;
		}
	}

	public void ShowBikeUI()
	{
		this.gearst = this.bikeScript.currentGear;
		this.BikeUI.speedText.text = ((int)this.bikeScript.speed).ToString();
		if (this.bikeScript.bikeSetting.automaticGear)
		{
			if (this.gearst > 0 && this.bikeScript.speed > 1f)
			{
				this.BikeUI.GearText.color = Color.green;
				this.BikeUI.GearText.text = this.gearst.ToString();
			}
			else if (this.bikeScript.speed > 1f)
			{
				this.BikeUI.GearText.color = Color.red;
				this.BikeUI.GearText.text = "R";
			}
			else
			{
				this.BikeUI.GearText.color = Color.white;
				this.BikeUI.GearText.text = "N";
			}
		}
		else if (this.bikeScript.NeutralGear)
		{
			this.BikeUI.GearText.color = Color.white;
			this.BikeUI.GearText.text = "N";
		}
		else if (this.bikeScript.currentGear != 0)
		{
			this.BikeUI.GearText.color = Color.green;
			this.BikeUI.GearText.text = this.gearst.ToString();
		}
		else
		{
			this.BikeUI.GearText.color = Color.red;
			this.BikeUI.GearText.text = "R";
		}
		this.thisAngle = this.bikeScript.motorRPM / 20f - 175f;
		this.thisAngle = Mathf.Clamp(this.thisAngle, -180f, 90f);
		this.BikeUI.tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0f, 0f, -this.thisAngle);
		this.BikeUI.barShiftGUI.rectTransform.localScale = new Vector3(this.bikeScript.powerShift / 100f, 1f, 1f);
	}

	private void Start()
	{
		this.bikeScript = this.target.GetComponent<BikeControl>();
		this.myRigidbody = this.target.GetComponent<Rigidbody>();
		this.cameraSwitchView = this.bikeScript.bikeSetting.cameraSwitchView;
		this.BikerMan = this.bikeScript.bikeSetting.bikerMan;
	}

	private void Update()
	{
		if (!this.target)
		{
			return;
		}
		this.bikeScript = this.target.GetComponent<BikeControl>();
		this.myRigidbody = this.target.GetComponent<Rigidbody>();
		if (UnityEngine.Input.GetKeyDown(KeyCode.G))
		{
			this.RestBike();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			this.PoliceLightSwitch();
		}
		if (this.restTime != 0f)
		{
			this.restTime = Mathf.MoveTowards(this.restTime, 0f, Time.deltaTime);
		}
		this.ShowBikeUI();
		base.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.bikeScript.speed / 10f + 60f, 60f, 90f);
		if (UnityEngine.Input.GetKeyDown(KeyCode.C))
		{
			this.Switch++;
			if (this.Switch > this.cameraSwitchView.Count)
			{
				this.Switch = 0;
			}
		}
		if (!this.bikeScript.crash)
		{
			if (this.Switch == 0)
			{
				float x = Mathf.SmoothDampAngle(base.transform.eulerAngles.x, this.target.eulerAngles.x + this.Angle, ref this.xVelocity, this.smooth);
				float y = Mathf.SmoothDampAngle(base.transform.eulerAngles.y, this.target.eulerAngles.y, ref this.yVelocity, this.smooth);
				base.transform.eulerAngles = new Vector3(x, y, 0f);
				Vector3 vector = base.transform.rotation * -Vector3.forward;
				float d = this.AdjustLineOfSight(this.target.position + new Vector3(0f, this.height, 0f), vector);
				base.transform.position = this.target.position + new Vector3(0f, this.height, 0f) + vector * d;
			}
			else
			{
				base.transform.position = this.cameraSwitchView[this.Switch - 1].position;
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.cameraSwitchView[this.Switch - 1].rotation, Time.deltaTime * 5f);
			}
		}
		else
		{
			Vector3 forward = this.BikerMan.position - base.transform.position;
			base.transform.rotation = Quaternion.LookRotation(forward);
		}
	}

	private float AdjustLineOfSight(Vector3 target, Vector3 direction)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(target, direction, out raycastHit, this.distance, this.lineOfSightMask.value))
		{
			return raycastHit.distance;
		}
		return this.distance;
	}

	public Transform target;

	public Transform BikerMan;

	public float smooth = 0.3f;

	public float distance = 5f;

	public float height = 1f;

	public float Angle = 20f;

	public List<Transform> cameraSwitchView;

	public BikeCamera.BikeUIClass BikeUI;

	public LayerMask lineOfSightMask = 0;

	private float yVelocity;

	private float xVelocity;

	[HideInInspector]
	public int Switch;

	private int gearst;

	private float thisAngle = -150f;

	private float restTime;

	private Rigidbody myRigidbody;

	private BikeControl bikeScript;

	private int PLValue;

	[Serializable]
	public class BikeUIClass
	{
		public Image tachometerNeedle;

		public Image barShiftGUI;

		public Text speedText;

		public Text GearText;
	}
}
