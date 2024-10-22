using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Steering Wheel")]
public class RCC_UISteeringWheelController : MonoBehaviour
{
	private void Awake()
	{
		this.steeringWheelTexture = base.GetComponent<Image>();
	}

	private void Update()
	{
		if (!RCC_Settings.Instance.useSteeringWheelForSteering)
		{
			return;
		}
		if (!this.steeringWheelRect && this.steeringWheelTexture)
		{
			this.SteeringWheelInit();
		}
		this.SteeringWheelControlling();
		this.input = this.GetSteeringWheelInput();
	}

	private void SteeringWheelInit()
	{
		this.steeringWheelGameObject = this.steeringWheelTexture.gameObject;
		this.steeringWheelRect = this.steeringWheelTexture.rectTransform;
		this.steeringWheelCanvasGroup = this.steeringWheelTexture.GetComponent<CanvasGroup>();
		this.steeringWheelCenter = this.steeringWheelRect.position;
		this.SteeringWheelEventsInit();
	}

	private void SteeringWheelEventsInit()
	{
		this.eventTrigger = this.steeringWheelGameObject.GetComponent<EventTrigger>();
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			data.Use();
			this.steeringWheelPressed = true;
			this.steeringWheelTouchPos = pointerEventData.position;
			this.steeringWheelTempAngle = Vector2.Angle(Vector2.up, pointerEventData.position - this.steeringWheelCenter);
		});
		this.eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent,
			eventID = EventTriggerType.PointerDown
		});
		EventTrigger.TriggerEvent triggerEvent2 = new EventTrigger.TriggerEvent();
		triggerEvent2.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			data.Use();
			this.steeringWheelTouchPos = pointerEventData.position;
		});
		this.eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent2,
			eventID = EventTriggerType.Drag
		});
		EventTrigger.TriggerEvent triggerEvent3 = new EventTrigger.TriggerEvent();
		triggerEvent3.AddListener(delegate(BaseEventData data)
		{
			this.steeringWheelPressed = false;
		});
		this.eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent3,
			eventID = EventTriggerType.EndDrag
		});
	}

	public float GetSteeringWheelInput()
	{
		return Mathf.Round(this.steeringWheelAngle / this.steeringWheelMaximumsteerAngle * 100f) / 100f;
	}

	public bool isSteeringWheelPressed()
	{
		return this.steeringWheelPressed;
	}

	public void SteeringWheelControlling()
	{
		if (!this.steeringWheelCanvasGroup || !this.steeringWheelRect || !RCC_Settings.Instance.useSteeringWheelForSteering)
		{
			if (this.steeringWheelGameObject)
			{
				this.steeringWheelGameObject.SetActive(false);
			}
			return;
		}
		if (!this.steeringWheelGameObject.activeSelf)
		{
			this.steeringWheelGameObject.SetActive(true);
		}
		if (this.steeringWheelPressed)
		{
			this.steeringWheelNewAngle = Vector2.Angle(Vector2.up, this.steeringWheelTouchPos - this.steeringWheelCenter);
			if (Vector2.Distance(this.steeringWheelTouchPos, this.steeringWheelCenter) > this.steeringWheelCenterDeadZoneRadius)
			{
				if (this.steeringWheelTouchPos.x > this.steeringWheelCenter.x)
				{
					this.steeringWheelAngle += this.steeringWheelNewAngle - this.steeringWheelTempAngle;
				}
				else
				{
					this.steeringWheelAngle -= this.steeringWheelNewAngle - this.steeringWheelTempAngle;
				}
			}
			if (this.steeringWheelAngle > this.steeringWheelMaximumsteerAngle)
			{
				this.steeringWheelAngle = this.steeringWheelMaximumsteerAngle;
			}
			else if (this.steeringWheelAngle < -this.steeringWheelMaximumsteerAngle)
			{
				this.steeringWheelAngle = -this.steeringWheelMaximumsteerAngle;
			}
			this.steeringWheelTempAngle = this.steeringWheelNewAngle;
		}
		else if (!Mathf.Approximately(0f, this.steeringWheelAngle))
		{
			float num = this.steeringWheelResetPosSpeed;
			if (Mathf.Abs(num) > Mathf.Abs(this.steeringWheelAngle))
			{
				this.steeringWheelAngle = 0f;
				return;
			}
			if (this.steeringWheelAngle > 0f)
			{
				this.steeringWheelAngle -= num;
			}
			else
			{
				this.steeringWheelAngle += num;
			}
		}
		this.steeringWheelRect.eulerAngles = new Vector3(0f, 0f, -this.steeringWheelAngle);
	}

	private void OnDisable()
	{
		this.steeringWheelPressed = false;
		this.input = 0f;
	}

	private GameObject steeringWheelGameObject;

	private Image steeringWheelTexture;

	public float input;

	public float steeringWheelAngle;

	public float steeringWheelMaximumsteerAngle = 270f;

	public float steeringWheelResetPosSpeed = 20f;

	public float steeringWheelCenterDeadZoneRadius = 5f;

	private RectTransform steeringWheelRect;

	private CanvasGroup steeringWheelCanvasGroup;

	private float steeringWheelTempAngle;

	private float steeringWheelNewAngle;

	private bool steeringWheelPressed;

	private Vector2 steeringWheelCenter;

	private Vector2 steeringWheelTouchPos;

	private EventTrigger eventTrigger;
}
