using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Displayer")]
[RequireComponent(typeof(RCC_DashboardInputs))]
public class RCC_UIDashboardDisplay : MonoBehaviour
{
	private void Start()
	{
		this.inputs = base.GetComponent<RCC_DashboardInputs>();
		base.StartCoroutine("LateDisplay");
	}

	private void OnEnable()
	{
		base.StopAllCoroutines();
		base.StartCoroutine("LateDisplay");
	}

	private IEnumerator LateDisplay()
	{
		for (;;)
		{
			yield return new WaitForSeconds(0.04f);
			if (this.RPMLabel)
			{
				this.RPMLabel.text = this.inputs.RPM.ToString("0");
			}
			if (this.KMHLabel)
			{
				if (RCC_Settings.Instance.units == RCC_Settings.Units.KMH)
				{
					this.KMHLabel.text = this.inputs.KMH.ToString("0") + "\nKMH";
				}
				else
				{
					this.KMHLabel.text = (this.inputs.KMH * 0.62f).ToString("0") + "\nMPH";
				}
			}
			if (this.GearLabel)
			{
				if (!this.inputs.NGear)
				{
					this.GearLabel.text = ((this.inputs.direction != 1) ? "R" : (this.inputs.Gear + 1f).ToString("0"));
				}
				else
				{
					this.GearLabel.text = "N";
				}
			}
			if (this.ABS)
			{
				this.ABS.color = ((!this.inputs.ABS) ? Color.white : Color.red);
			}
			if (this.ESP)
			{
				this.ESP.color = ((!this.inputs.ESP) ? Color.white : Color.red);
			}
			if (this.Park)
			{
				this.Park.color = ((!this.inputs.Park) ? Color.white : Color.red);
			}
			if (this.Headlights)
			{
				this.Headlights.color = ((!this.inputs.Headlights) ? Color.white : Color.green);
			}
			if (this.leftIndicator && this.rightIndicator)
			{
				switch (this.inputs.indicators)
				{
				case RCC_CarControllerV3.IndicatorsOn.Off:
					this.leftIndicator.color = new Color(0.5f, 0.25f, 0f);
					this.rightIndicator.color = new Color(0.5f, 0.25f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Right:
					this.leftIndicator.color = new Color(0.5f, 0.25f, 0f);
					this.rightIndicator.color = new Color(1f, 0.5f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Left:
					this.leftIndicator.color = new Color(1f, 0.5f, 0f);
					this.rightIndicator.color = new Color(0.5f, 0.25f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.All:
					this.leftIndicator.color = new Color(1f, 0.5f, 0f);
					this.rightIndicator.color = new Color(1f, 0.5f, 0f);
					break;
				}
			}
		}
		yield break;
	}

	private RCC_DashboardInputs inputs;

	public Text RPMLabel;

	public Text KMHLabel;

	public Text GearLabel;

	public Image ABS;

	public Image ESP;

	public Image Park;

	public Image Headlights;

	public Image leftIndicator;

	public Image rightIndicator;
}
