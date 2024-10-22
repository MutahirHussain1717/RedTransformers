using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Button")]
public class RCC_UIDashboardButton : MonoBehaviour
{
	private void Start()
	{
		if (base.GetComponentInChildren<Scrollbar>())
		{
			this.gearSlider = base.GetComponentInChildren<Scrollbar>();
			this.gearSlider.onValueChanged.AddListener(delegate(float A_1)
			{
				this.ChangeGear();
			});
		}
	}

	private void OnEnable()
	{
		this.Check();
	}

	public void OnClicked()
	{
		this.carControllers = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
		switch (this._buttonType)
		{
		case RCC_UIDashboardButton.ButtonType.Start:
			for (int i = 0; i < this.carControllers.Length; i++)
			{
				if (this.carControllers[i].canControl)
				{
					this.carControllers[i].KillOrStartEngine();
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.ABS:
			for (int j = 0; j < this.carControllers.Length; j++)
			{
				if (this.carControllers[j].canControl)
				{
					this.carControllers[j].ABS = !this.carControllers[j].ABS;
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.ESP:
			for (int k = 0; k < this.carControllers.Length; k++)
			{
				if (this.carControllers[k].canControl)
				{
					this.carControllers[k].ESP = !this.carControllers[k].ESP;
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.TCS:
			for (int l = 0; l < this.carControllers.Length; l++)
			{
				if (this.carControllers[l].canControl)
				{
					this.carControllers[l].TCS = !this.carControllers[l].TCS;
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.Headlights:
			for (int m = 0; m < this.carControllers.Length; m++)
			{
				if (this.carControllers[m].canControl)
				{
					if (!this.carControllers[m].highBeamHeadLightsOn && this.carControllers[m].lowBeamHeadLightsOn)
					{
						this.carControllers[m].highBeamHeadLightsOn = true;
						this.carControllers[m].lowBeamHeadLightsOn = true;
						break;
					}
					if (!this.carControllers[m].lowBeamHeadLightsOn)
					{
						this.carControllers[m].lowBeamHeadLightsOn = true;
					}
					if (this.carControllers[m].highBeamHeadLightsOn)
					{
						this.carControllers[m].lowBeamHeadLightsOn = false;
						this.carControllers[m].highBeamHeadLightsOn = false;
					}
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.LeftIndicator:
			for (int n = 0; n < this.carControllers.Length; n++)
			{
				if (this.carControllers[n].canControl)
				{
					if (this.carControllers[n].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Left)
					{
						this.carControllers[n].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Left;
					}
					else
					{
						this.carControllers[n].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
					}
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.RightIndicator:
			for (int num = 0; num < this.carControllers.Length; num++)
			{
				if (this.carControllers[num].canControl)
				{
					if (this.carControllers[num].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Right)
					{
						this.carControllers[num].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Right;
					}
					else
					{
						this.carControllers[num].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
					}
				}
			}
			break;
		}
		this.Check();
	}

	public void Check()
	{
		this.carControllers = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
		switch (this._buttonType)
		{
		case RCC_UIDashboardButton.ButtonType.ABS:
			for (int i = 0; i < this.carControllers.Length; i++)
			{
				if (this.carControllers[i].canControl && this.carControllers[i].ABS)
				{
					base.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if (this.carControllers[i].canControl)
				{
					base.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.ESP:
			for (int j = 0; j < this.carControllers.Length; j++)
			{
				if (this.carControllers[j].canControl && this.carControllers[j].ESP)
				{
					base.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if (this.carControllers[j].canControl)
				{
					base.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.TCS:
			for (int k = 0; k < this.carControllers.Length; k++)
			{
				if (this.carControllers[k].canControl && this.carControllers[k].TCS)
				{
					base.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if (this.carControllers[k].canControl)
				{
					base.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
				}
			}
			break;
		case RCC_UIDashboardButton.ButtonType.Headlights:
			for (int l = 0; l < this.carControllers.Length; l++)
			{
				if ((this.carControllers[l].canControl && this.carControllers[l].lowBeamHeadLightsOn) || this.carControllers[l].highBeamHeadLightsOn)
				{
					base.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if (this.carControllers[l].canControl)
				{
					base.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
				}
			}
			break;
		}
	}

	public void ChangeGear()
	{
		if (this.gearDirection == (int)this.gearSlider.value)
		{
			return;
		}
		this.gearDirection = (int)this.gearSlider.value;
		for (int i = 0; i < this.carControllers.Length; i++)
		{
			if (this.carControllers[i].canControl)
			{
				this.carControllers[i].semiAutomaticGear = true;
				if (this.gearDirection == 1)
				{
					this.carControllers[i].StartCoroutine("ChangingGear", -1);
				}
				else
				{
					this.carControllers[i].StartCoroutine("ChangingGear", 0);
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this._buttonType == RCC_UIDashboardButton.ButtonType.Gear)
		{
			this.carControllers = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
			foreach (RCC_CarControllerV3 rcc_CarControllerV in this.carControllers)
			{
				if (rcc_CarControllerV.canControl)
				{
					rcc_CarControllerV.semiAutomaticGear = false;
				}
			}
		}
	}

	public RCC_UIDashboardButton.ButtonType _buttonType;

	private Scrollbar gearSlider;

	private RCC_CarControllerV3[] carControllers;

	private int gearDirection;

	public enum ButtonType
	{
		Start,
		ABS,
		ESP,
		TCS,
		Headlights,
		LeftIndicator,
		RightIndicator,
		Gear
	}
}
