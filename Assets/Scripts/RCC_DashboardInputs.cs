using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Inputs")]
public class RCC_DashboardInputs : MonoBehaviour
{
	private void Update()
	{
		if (RCC_Settings.Instance.uiType == RCC_Settings.UIType.None)
		{
			base.gameObject.SetActive(false);
			base.enabled = false;
			return;
		}
		this.GetValues();
	}

	public void GetVehicle(RCC_CarControllerV3 rcc)
	{
		this.currentCarController = rcc;
		RCC_UIDashboardButton[] array = UnityEngine.Object.FindObjectsOfType<RCC_UIDashboardButton>();
		foreach (RCC_UIDashboardButton rcc_UIDashboardButton in array)
		{
			rcc_UIDashboardButton.Check();
		}
	}

	private void GetValues()
	{
		if (!this.currentCarController)
		{
			return;
		}
		if (!this.currentCarController.canControl || this.currentCarController.AIController)
		{
			return;
		}
		if (this.NOSGauge)
		{
			if (this.currentCarController.useNOS)
			{
				if (!this.NOSGauge.activeSelf)
				{
					this.NOSGauge.SetActive(true);
				}
			}
			else if (this.NOSGauge.activeSelf)
			{
				this.NOSGauge.SetActive(false);
			}
		}
		if (this.turboGauge)
		{
			if (this.currentCarController.useTurbo)
			{
				if (!this.turboGauge.activeSelf)
				{
					this.turboGauge.SetActive(true);
				}
			}
			else if (this.turboGauge.activeSelf)
			{
				this.turboGauge.SetActive(false);
			}
		}
		this.RPM = this.currentCarController.engineRPM;
		this.KMH = this.currentCarController.speed;
		this.direction = this.currentCarController.direction;
		this.Gear = (float)this.currentCarController.currentGear;
		this.NGear = this.currentCarController.changingGear;
		this.ABS = this.currentCarController.ABSAct;
		this.ESP = this.currentCarController.ESPAct;
		this.Park = (this.currentCarController.handbrakeInput > 0.1f);
		this.Headlights = (this.currentCarController.lowBeamHeadLightsOn || this.currentCarController.highBeamHeadLightsOn);
		this.indicators = this.currentCarController.indicatorsOn;
		if (this.RPMNeedle)
		{
			this.RPMNeedleRotation = this.currentCarController.engineRPM / 50f;
			this.RPMNeedle.transform.eulerAngles = new Vector3(this.RPMNeedle.transform.eulerAngles.x, this.RPMNeedle.transform.eulerAngles.y, -this.RPMNeedleRotation);
		}
		if (this.KMHNeedle)
		{
			if (RCC_Settings.Instance.units == RCC_Settings.Units.KMH)
			{
				this.KMHNeedleRotation = this.currentCarController.speed;
			}
			else
			{
				this.KMHNeedleRotation = this.currentCarController.speed * 0.62f;
			}
			this.KMHNeedle.transform.eulerAngles = new Vector3(this.KMHNeedle.transform.eulerAngles.x, this.KMHNeedle.transform.eulerAngles.y, -this.KMHNeedleRotation);
		}
		if (this.BoostNeedle)
		{
			this.BoostNeedleRotation = this.currentCarController.turboBoost / 30f * 270f;
			this.BoostNeedle.transform.eulerAngles = new Vector3(this.BoostNeedle.transform.eulerAngles.x, this.BoostNeedle.transform.eulerAngles.y, -this.BoostNeedleRotation);
		}
		if (this.NoSNeedle)
		{
			this.NoSNeedleRotation = this.currentCarController.NoS / 100f * 270f;
			this.NoSNeedle.transform.eulerAngles = new Vector3(this.NoSNeedle.transform.eulerAngles.x, this.NoSNeedle.transform.eulerAngles.y, -this.NoSNeedleRotation);
		}
	}

	public RCC_CarControllerV3 currentCarController;

	public GameObject RPMNeedle;

	public GameObject KMHNeedle;

	public GameObject turboGauge;

	public GameObject NOSGauge;

	public GameObject BoostNeedle;

	public GameObject NoSNeedle;

	private float RPMNeedleRotation;

	private float KMHNeedleRotation;

	private float BoostNeedleRotation;

	private float NoSNeedleRotation;

	public float RPM;

	public float KMH;

	internal int direction = 1;

	internal float Gear;

	internal bool NGear;

	internal bool ABS;

	internal bool ESP;

	internal bool Park;

	internal bool Headlights;

	internal RCC_CarControllerV3.IndicatorsOn indicators;
}
