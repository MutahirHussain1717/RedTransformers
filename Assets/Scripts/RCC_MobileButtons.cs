using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Mobile Buttons")]
public class RCC_MobileButtons : MonoBehaviour
{
	private void Start()
	{
		if (RCC_Settings.Instance.controllerType != RCC_Settings.ControllerType.Mobile)
		{
			if (this.gasButton)
			{
				this.gasButton.gameObject.SetActive(false);
			}
			if (this.leftButton)
			{
				this.leftButton.gameObject.SetActive(false);
			}
			if (this.rightButton)
			{
				this.rightButton.gameObject.SetActive(false);
			}
			if (this.brakeButton)
			{
				this.brakeButton.gameObject.SetActive(false);
			}
			if (this.steeringWheel)
			{
				this.steeringWheel.gameObject.SetActive(false);
			}
			if (this.handbrakeButton)
			{
				this.handbrakeButton.gameObject.SetActive(false);
			}
			if (this.NOSButton)
			{
				this.NOSButton.gameObject.SetActive(false);
			}
			if (this.gearButton)
			{
				this.gearButton.gameObject.SetActive(false);
			}
			base.enabled = false;
			return;
		}
		this.orgBrakeButtonPos = this.brakeButton.transform.position;
		this.GetVehicles();
	}

	public void GetVehicles()
	{
		this.carControllers = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
	}

	private void Update()
	{
		RCC_Settings.Instance.useAccelerometerForSteering = false;
		if (RCC_Settings.Instance.useSteeringWheelForSteering)
		{
			UnityEngine.Debug.Log("pannels off");
			if (RCC_Settings.Instance.useAccelerometerForSteering)
			{
				RCC_Settings.Instance.useAccelerometerForSteering = false;
			}
			if (!this.steeringWheel.gameObject.activeInHierarchy)
			{
				this.steeringWheel.gameObject.SetActive(true);
				this.brakeButton.transform.position = this.orgBrakeButtonPos;
			}
			if (this.leftButton.gameObject.activeInHierarchy)
			{
				this.leftButton.gameObject.SetActive(false);
			}
			if (this.rightButton.gameObject.activeInHierarchy)
			{
				this.rightButton.gameObject.SetActive(false);
			}
		}
		if (RCC_Settings.Instance.useAccelerometerForSteering)
		{
			UnityEngine.Debug.Log("pannels off2");
			if (RCC_Settings.Instance.useSteeringWheelForSteering)
			{
				RCC_Settings.Instance.useSteeringWheelForSteering = false;
			}
			this.brakeButton.transform.position = this.leftButton.transform.position;
			if (this.steeringWheel.gameObject.activeInHierarchy)
			{
				this.steeringWheel.gameObject.SetActive(false);
			}
			if (this.leftButton.gameObject.activeInHierarchy)
			{
				this.leftButton.gameObject.SetActive(false);
			}
			if (this.rightButton.gameObject.activeInHierarchy)
			{
				this.rightButton.gameObject.SetActive(false);
			}
		}
		if (!RCC_Settings.Instance.useAccelerometerForSteering && !RCC_Settings.Instance.useSteeringWheelForSteering)
		{
			if (this.steeringWheel && this.steeringWheel.gameObject.activeInHierarchy)
			{
				this.steeringWheel.gameObject.SetActive(false);
			}
			if (!this.leftButton.gameObject.activeInHierarchy)
			{
				this.brakeButton.transform.position = this.orgBrakeButtonPos;
				this.leftButton.gameObject.SetActive(true);
			}
			if (!this.rightButton.gameObject.activeInHierarchy)
			{
				this.rightButton.gameObject.SetActive(true);
			}
		}
		this.gasInput = this.GetInput(this.gasButton);
		this.brakeInput = this.GetInput(this.brakeButton);
		this.leftInput = this.GetInput(this.leftButton);
		this.rightInput = this.GetInput(this.rightButton);
		if (this.steeringWheel)
		{
			this.steeringWheelInput = this.steeringWheel.input;
		}
		if (RCC_Settings.Instance.useAccelerometerForSteering)
		{
			this.gyroInput = Input.acceleration.x * RCC_Settings.Instance.gyroSensitivity;
		}
		else
		{
			this.gyroInput = 0f;
		}
		this.handbrakeInput = this.GetInput(this.handbrakeButton);
		this.NOSInput = Mathf.Clamp(this.GetInput(this.NOSButton) * 2.5f, 1f, 2.5f);
		foreach (RCC_CarControllerV3 rcc_CarControllerV in this.carControllers)
		{
			if (rcc_CarControllerV.canControl)
			{
				rcc_CarControllerV.gasInput = this.gasInput;
				rcc_CarControllerV.brakeInput = this.brakeInput;
				rcc_CarControllerV.steerInput = -this.leftInput + this.rightInput + this.steeringWheelInput + this.gyroInput;
				rcc_CarControllerV.handbrakeInput = this.handbrakeInput;
				rcc_CarControllerV.boostInput = this.NOSInput;
			}
		}
	}

	private float GetInput(RCC_UIController button)
	{
		if (button == null)
		{
			return 0f;
		}
		return button.input;
	}

	public void ChangeCamera()
	{
		if (UnityEngine.Object.FindObjectOfType<RCC_Camera>())
		{
			UnityEngine.Object.FindObjectOfType<RCC_Camera>().ChangeCamera();
		}
	}

	public void ChangeController(int index)
	{
		if (index != 0)
		{
			if (index != 1)
			{
				if (index == 2)
				{
					RCC_Settings.Instance.useAccelerometerForSteering = false;
					RCC_Settings.Instance.useSteeringWheelForSteering = true;
				}
			}
			else
			{
				RCC_Settings.Instance.useAccelerometerForSteering = false;
				RCC_Settings.Instance.useSteeringWheelForSteering = true;
			}
		}
		else
		{
			RCC_Settings.Instance.useAccelerometerForSteering = false;
			RCC_Settings.Instance.useSteeringWheelForSteering = false;
		}
	}

	public RCC_CarControllerV3[] carControllers;

	public RCC_UIController gasButton;

	public RCC_UIController brakeButton;

	public RCC_UIController leftButton;

	public RCC_UIController rightButton;

	public RCC_UISteeringWheelController steeringWheel;

	public RCC_UIController handbrakeButton;

	public RCC_UIController NOSButton;

	public GameObject gearButton;

	private float gasInput;

	private float brakeInput;

	private float leftInput;

	private float rightInput;

	private float steeringWheelInput;

	private float handbrakeInput;

	private float NOSInput = 1f;

	private float gyroInput;

	private Vector3 orgBrakeButtonPos;
}
