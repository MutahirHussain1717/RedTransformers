using System;
using UnityEngine;

[ExecuteInEditMode]
public class EasyJoystick : MonoBehaviour
{
	public static event EasyJoystick.JoystickMoveStartHandler On_JoystickMoveStart;

	public static event EasyJoystick.JoystickMoveHandler On_JoystickMove;

	public static event EasyJoystick.JoystickMoveEndHandler On_JoystickMoveEnd;

	public static event EasyJoystick.JoystickTouchStartHandler On_JoystickTouchStart;

	public static event EasyJoystick.JoystickTapHandler On_JoystickTap;

	public static event EasyJoystick.JoystickDoubleTapHandler On_JoystickDoubleTap;

	public static event EasyJoystick.JoystickTouchUpHandler On_JoystickTouchUp;

	public Vector2 JoystickAxis
	{
		get
		{
			return this.joystickAxis;
		}
	}

	public Vector2 JoystickTouch
	{
		get
		{
			return new Vector2(this.joystickTouch.x / this.zoneRadius, this.joystickTouch.y / this.zoneRadius);
		}
		set
		{
			float x = Mathf.Clamp(value.x, -1f, 1f) * this.zoneRadius;
			float y = Mathf.Clamp(value.y, -1f, 1f) * this.zoneRadius;
			this.joystickTouch = new Vector2(x, y);
		}
	}

	public Vector2 JoystickValue
	{
		get
		{
			return this.joystickValue;
		}
	}

	public bool DynamicJoystick
	{
		get
		{
			return this.dynamicJoystick;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.joystickIndex = -1;
				this.dynamicJoystick = value;
				if (this.dynamicJoystick)
				{
					this.virtualJoystick = false;
				}
				else
				{
					this.virtualJoystick = true;
					this.joystickCenter = this.joystickPositionOffset;
				}
			}
		}
	}

	public EasyJoystick.JoystickAnchor JoyAnchor
	{
		get
		{
			return this.joyAnchor;
		}
		set
		{
			this.joyAnchor = value;
			this.ComputeJoystickAnchor(this.joyAnchor);
		}
	}

	public Vector2 JoystickPositionOffset
	{
		get
		{
			return this.joystickPositionOffset;
		}
		set
		{
			this.joystickPositionOffset = value;
			this.joystickCenter = this.joystickPositionOffset;
			this.ComputeJoystickAnchor(this.joyAnchor);
		}
	}

	public float ZoneRadius
	{
		get
		{
			return this.zoneRadius;
		}
		set
		{
			this.zoneRadius = value;
			this.ComputeJoystickAnchor(this.joyAnchor);
		}
	}

	public float TouchSize
	{
		get
		{
			return this.touchSize;
		}
		set
		{
			this.touchSize = value;
			if (this.touchSize > this.zoneRadius / 2f && this.restrictArea)
			{
				this.touchSize = this.zoneRadius / 2f;
			}
			this.ComputeJoystickAnchor(this.joyAnchor);
		}
	}

	public bool RestrictArea
	{
		get
		{
			return this.restrictArea;
		}
		set
		{
			this.restrictArea = value;
			if (this.restrictArea)
			{
				this.touchSizeCoef = this.touchSize;
			}
			else
			{
				this.touchSizeCoef = 0f;
			}
			this.ComputeJoystickAnchor(this.joyAnchor);
		}
	}

	public EasyJoystick.InteractionType Interaction
	{
		get
		{
			return this.interaction;
		}
		set
		{
			this.interaction = value;
			if (this.interaction == EasyJoystick.InteractionType.Direct || this.interaction == EasyJoystick.InteractionType.Include)
			{
				this.useBroadcast = false;
			}
		}
	}

	public Transform XAxisTransform
	{
		get
		{
			return this.xAxisTransform;
		}
		set
		{
			this.xAxisTransform = value;
			if (this.xAxisTransform != null)
			{
				this.xAxisCharacterController = this.xAxisTransform.GetComponent<CharacterController>();
			}
			else
			{
				this.xAxisCharacterController = null;
				this.xAxisGravity = 0f;
			}
		}
	}

	public EasyJoystick.PropertiesInfluenced XTI
	{
		get
		{
			return this.xTI;
		}
		set
		{
			this.xTI = value;
			if (this.xTI != EasyJoystick.PropertiesInfluenced.RotateLocal)
			{
				this.enableXAutoStab = false;
				this.enableXClamp = false;
			}
		}
	}

	public float ThresholdX
	{
		get
		{
			return this.thresholdX;
		}
		set
		{
			if (value <= 0f)
			{
				this.thresholdX = value * -1f;
			}
			else
			{
				this.thresholdX = value;
			}
		}
	}

	public float StabSpeedX
	{
		get
		{
			return this.stabSpeedX;
		}
		set
		{
			if (value <= 0f)
			{
				this.stabSpeedX = value * -1f;
			}
			else
			{
				this.stabSpeedX = value;
			}
		}
	}

	public Transform YAxisTransform
	{
		get
		{
			return this.yAxisTransform;
		}
		set
		{
			this.yAxisTransform = value;
			if (this.yAxisTransform != null)
			{
				this.yAxisCharacterController = this.yAxisTransform.GetComponent<CharacterController>();
			}
			else
			{
				this.yAxisCharacterController = null;
				this.yAxisGravity = 0f;
			}
		}
	}

	public EasyJoystick.PropertiesInfluenced YTI
	{
		get
		{
			return this.yTI;
		}
		set
		{
			this.yTI = value;
			if (this.yTI != EasyJoystick.PropertiesInfluenced.RotateLocal)
			{
				this.enableYAutoStab = false;
				this.enableYClamp = false;
			}
		}
	}

	public float ThresholdY
	{
		get
		{
			return this.thresholdY;
		}
		set
		{
			if (value <= 0f)
			{
				this.thresholdY = value * -1f;
			}
			else
			{
				this.thresholdY = value;
			}
		}
	}

	public float StabSpeedY
	{
		get
		{
			return this.stabSpeedY;
		}
		set
		{
			if (value <= 0f)
			{
				this.stabSpeedY = value * -1f;
			}
			else
			{
				this.stabSpeedY = value;
			}
		}
	}

	public Vector2 Smoothing
	{
		get
		{
			return this.smoothing;
		}
		set
		{
			this.smoothing = value;
			if (this.smoothing.x < 0f)
			{
				this.smoothing.x = 0f;
			}
			if (this.smoothing.y < 0f)
			{
				this.smoothing.y = 0f;
			}
		}
	}

	public Vector2 Inertia
	{
		get
		{
			return this.inertia;
		}
		set
		{
			this.inertia = value;
			if (this.inertia.x <= 0f)
			{
				this.inertia.x = 1f;
			}
			if (this.inertia.y <= 0f)
			{
				this.inertia.y = 1f;
			}
		}
	}

	private void OnLevelWasLoaded()
	{
		this.joystickIndex = -1;
	}

	private void OnEnable()
	{
		EasyTouch.On_TouchStart += this.On_TouchStart;
		EasyTouch.On_TouchUp += this.On_TouchUp;
		EasyTouch.On_TouchDown += this.On_TouchDown;
		EasyTouch.On_SimpleTap += this.On_SimpleTap;
		EasyTouch.On_DoubleTap += this.On_DoubleTap;
	}

	private void OnDisable()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
		EasyTouch.On_TouchUp -= this.On_TouchUp;
		EasyTouch.On_TouchDown -= this.On_TouchDown;
		EasyTouch.On_SimpleTap -= this.On_SimpleTap;
		EasyTouch.On_DoubleTap -= this.On_DoubleTap;
		if (Application.isPlaying && EasyTouch.instance != null)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
		}
	}

	private void OnDestroy()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
		EasyTouch.On_TouchUp -= this.On_TouchUp;
		EasyTouch.On_TouchDown -= this.On_TouchDown;
		EasyTouch.On_SimpleTap -= this.On_SimpleTap;
		EasyTouch.On_DoubleTap -= this.On_DoubleTap;
		if (Application.isPlaying && EasyTouch.instance != null)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
		}
	}

	private void Start()
	{
		if (!this.dynamicJoystick)
		{
			this.joystickCenter = this.joystickPositionOffset;
			this.ComputeJoystickAnchor(this.joyAnchor);
			this.virtualJoystick = true;
		}
		else
		{
			this.virtualJoystick = false;
		}
		VirtualScreen.ComputeVirtualScreen();
		this.startXLocalAngle = this.GetStartAutoStabAngle(this.xAxisTransform, this.xAI);
		this.startYLocalAngle = this.GetStartAutoStabAngle(this.yAxisTransform, this.yAI);
		this.RestrictArea = this.restrictArea;
	}

	private void Update()
	{
		if (!this.useFixedUpdate && this.enable)
		{
			this.UpdateJoystick();
		}
	}

	private void FixedUpdate()
	{
		if (this.useFixedUpdate && this.enable)
		{
			this.UpdateJoystick();
		}
	}

	private void UpdateJoystick()
	{
		if (Application.isPlaying)
		{
			if (EasyTouch.GetTouchCount() == 0)
			{
				this.joystickIndex = -1;
				if (this.dynamicJoystick)
				{
					this.virtualJoystick = false;
				}
			}
			if (this.isActivated)
			{
				if (this.joystickIndex == -1 || (this.joystickAxis == Vector2.zero && this.joystickIndex > -1))
				{
					if (this.enableXAutoStab)
					{
						this.DoAutoStabilisation(this.xAxisTransform, this.xAI, this.thresholdX, this.stabSpeedX, this.startXLocalAngle);
					}
					if (this.enableYAutoStab)
					{
						this.DoAutoStabilisation(this.yAxisTransform, this.yAI, this.thresholdY, this.stabSpeedY, this.startYLocalAngle);
					}
				}
				if (!this.dynamicJoystick)
				{
					this.joystickCenter = this.joystickPositionOffset;
				}
				if (this.joystickIndex == -1)
				{
					if (!this.enableSmoothing)
					{
						this.joystickTouch = Vector2.zero;
					}
					else if ((double)this.joystickTouch.sqrMagnitude > 0.0001)
					{
						this.joystickTouch = new Vector2(this.joystickTouch.x - this.joystickTouch.x * this.smoothing.x * Time.deltaTime, this.joystickTouch.y - this.joystickTouch.y * this.smoothing.y * Time.deltaTime);
					}
					else
					{
						this.joystickTouch = Vector2.zero;
					}
				}
				Vector2 lhs = new Vector2(this.joystickAxis.x, this.joystickAxis.y);
				float num = this.ComputeDeadZone();
				this.joystickAxis = new Vector2(this.joystickTouch.x * num, this.joystickTouch.y * num);
				if (this.inverseXAxis)
				{
					this.joystickAxis.x = this.joystickAxis.x * -1f;
				}
				if (this.inverseYAxis)
				{
					this.joystickAxis.y = this.joystickAxis.y * -1f;
				}
				Vector2 a = new Vector2(this.speed.x * this.joystickAxis.x, this.speed.y * this.joystickAxis.y);
				if (this.enableInertia)
				{
					Vector2 b = a - this.joystickValue;
					b.x /= this.inertia.x;
					b.y /= this.inertia.y;
					this.joystickValue += b;
				}
				else
				{
					this.joystickValue = a;
				}
				if (lhs == Vector2.zero && this.joystickAxis != Vector2.zero && this.interaction != EasyJoystick.InteractionType.Direct && this.interaction != EasyJoystick.InteractionType.Include)
				{
					this.CreateEvent(EasyJoystick.MessageName.On_JoystickMoveStart);
				}
				this.UpdateGravity();
				if (this.joystickAxis != Vector2.zero)
				{
					this.sendEnd = false;
					EasyJoystick.InteractionType interactionType = this.interaction;
					if (interactionType != EasyJoystick.InteractionType.Direct)
					{
						if (interactionType != EasyJoystick.InteractionType.EventNotification)
						{
							if (interactionType == EasyJoystick.InteractionType.DirectAndEvent)
							{
								this.UpdateDirect();
								this.CreateEvent(EasyJoystick.MessageName.On_JoystickMove);
							}
						}
						else
						{
							this.CreateEvent(EasyJoystick.MessageName.On_JoystickMove);
						}
					}
					else
					{
						this.UpdateDirect();
					}
				}
				else if (!this.sendEnd)
				{
					this.CreateEvent(EasyJoystick.MessageName.On_JoystickMoveEnd);
					this.sendEnd = true;
				}
			}
			if (GameObject.FindGameObjectWithTag("Player"))
			{
				this.yAxisTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
				this.xAxisTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
				this.xAxisCharacterController = this.xAxisTransform.GetComponent<CharacterController>();
				this.yAxisCharacterController = this.yAxisTransform.GetComponent<CharacterController>();
			}
		}
	}

	private void OnGUI()
	{
		if (this.enable && this.visible)
		{
			GUI.depth = this.guiDepth;
			base.useGUILayout = this.isUseGuiLayout;
			if (this.dynamicJoystick && Application.isEditor && !Application.isPlaying)
			{
				switch (this.area)
				{
				case EasyJoystick.DynamicArea.FullScreen:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.MiddleCenter);
					break;
				case EasyJoystick.DynamicArea.Left:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.MiddleLeft);
					break;
				case EasyJoystick.DynamicArea.Right:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.MiddleRight);
					break;
				case EasyJoystick.DynamicArea.Top:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.UpperCenter);
					break;
				case EasyJoystick.DynamicArea.Bottom:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.LowerCenter);
					break;
				case EasyJoystick.DynamicArea.TopLeft:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.UpperLeft);
					break;
				case EasyJoystick.DynamicArea.TopRight:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.UpperRight);
					break;
				case EasyJoystick.DynamicArea.BottomLeft:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.LowerLeft);
					break;
				case EasyJoystick.DynamicArea.BottomRight:
					this.ComputeJoystickAnchor(EasyJoystick.JoystickAnchor.LowerRight);
					break;
				}
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				VirtualScreen.ComputeVirtualScreen();
				this.ComputeJoystickAnchor(this.joyAnchor);
			}
			VirtualScreen.SetGuiScaleMatrix();
			if ((this.showZone && this.areaTexture != null && !this.dynamicJoystick) || (this.showZone && this.dynamicJoystick && this.virtualJoystick && this.areaTexture != null) || (this.dynamicJoystick && Application.isEditor && !Application.isPlaying))
			{
				if (this.isActivated)
				{
					GUI.color = this.areaColor;
					if (Application.isPlaying && !this.dynamicJoystick)
					{
						EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
						EasyTouch.instance.reservedVirtualAreas.Add(this.areaRect);
					}
				}
				else
				{
					GUI.color = new Color(this.areaColor.r, this.areaColor.g, this.areaColor.b, 0.2f);
					if (Application.isPlaying && !this.dynamicJoystick)
					{
						EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
					}
				}
				if (this.showDebugRadius && Application.isEditor && this.selected && !Application.isPlaying)
				{
					GUI.Box(this.areaRect, string.Empty);
				}
				GUI.DrawTexture(this.areaRect, this.areaTexture, ScaleMode.StretchToFill, true);
			}
			if ((this.showTouch && this.touchTexture != null && !this.dynamicJoystick) || (this.showTouch && this.dynamicJoystick && this.virtualJoystick && this.touchTexture != null) || (this.dynamicJoystick && Application.isEditor && !Application.isPlaying))
			{
				if (this.isActivated)
				{
					GUI.color = this.touchColor;
				}
				else
				{
					GUI.color = new Color(this.touchColor.r, this.touchColor.g, this.touchColor.b, 0.2f);
				}
				GUI.DrawTexture(new Rect(this.anchorPosition.x + this.joystickCenter.x + (this.joystickTouch.x - this.touchSize), this.anchorPosition.y + this.joystickCenter.y - (this.joystickTouch.y + this.touchSize), this.touchSize * 2f, this.touchSize * 2f), this.touchTexture, ScaleMode.ScaleToFit, true);
			}
			if ((this.showDeadZone && this.deadTexture != null && !this.dynamicJoystick) || (this.showDeadZone && this.dynamicJoystick && this.virtualJoystick && this.deadTexture != null) || (this.dynamicJoystick && Application.isEditor && !Application.isPlaying))
			{
				GUI.DrawTexture(this.deadRect, this.deadTexture, ScaleMode.ScaleToFit, true);
			}
			GUI.color = Color.white;
		}
		else if (Application.isPlaying)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
		}
	}

	private void OnDrawGizmos()
	{
	}

	private void CreateEvent(EasyJoystick.MessageName message)
	{
		MovingJoystick movingJoystick = new MovingJoystick();
		movingJoystick.joystickName = base.gameObject.name;
		movingJoystick.joystickAxis = this.joystickAxis;
		movingJoystick.joystickValue = this.joystickValue;
		movingJoystick.fingerIndex = this.joystickIndex;
		movingJoystick.joystick = this;
		if (!this.useBroadcast)
		{
			switch (message)
			{
			case EasyJoystick.MessageName.On_JoystickMoveStart:
				if (EasyJoystick.On_JoystickMoveStart != null)
				{
					EasyJoystick.On_JoystickMoveStart(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickTouchStart:
				if (EasyJoystick.On_JoystickTouchStart != null)
				{
					EasyJoystick.On_JoystickTouchStart(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickTouchUp:
				if (EasyJoystick.On_JoystickTouchUp != null)
				{
					EasyJoystick.On_JoystickTouchUp(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickMove:
				if (EasyJoystick.On_JoystickMove != null)
				{
					EasyJoystick.On_JoystickMove(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickMoveEnd:
				if (EasyJoystick.On_JoystickMoveEnd != null)
				{
					EasyJoystick.On_JoystickMoveEnd(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickTap:
				if (EasyJoystick.On_JoystickTap != null)
				{
					EasyJoystick.On_JoystickTap(movingJoystick);
				}
				break;
			case EasyJoystick.MessageName.On_JoystickDoubleTap:
				if (EasyJoystick.On_JoystickDoubleTap != null)
				{
					EasyJoystick.On_JoystickDoubleTap(movingJoystick);
				}
				break;
			}
		}
		else if (this.useBroadcast)
		{
			if (this.receiverGameObject != null)
			{
				EasyJoystick.Broadcast broadcast = this.messageMode;
				if (broadcast != EasyJoystick.Broadcast.BroadcastMessage)
				{
					if (broadcast != EasyJoystick.Broadcast.SendMessage)
					{
						if (broadcast == EasyJoystick.Broadcast.SendMessageUpwards)
						{
							this.receiverGameObject.SendMessageUpwards(message.ToString(), movingJoystick, SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						this.receiverGameObject.SendMessage(message.ToString(), movingJoystick, SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					this.receiverGameObject.BroadcastMessage(message.ToString(), movingJoystick, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Joystick : " + base.gameObject.name + " : you must setup receiver gameobject");
			}
		}
	}

	private void UpdateDirect()
	{
		if (this.xAxisTransform != null)
		{
			Vector3 influencedAxis = this.GetInfluencedAxis(this.xAI);
			this.DoActionDirect(this.xAxisTransform, this.xTI, influencedAxis, this.joystickValue.x, this.xAxisCharacterController);
			if (this.enableXClamp && this.xTI == EasyJoystick.PropertiesInfluenced.RotateLocal)
			{
				this.DoAngleLimitation(this.xAxisTransform, this.xAI, this.clampXMin, this.clampXMax, this.startXLocalAngle);
			}
		}
		if (this.YAxisTransform != null)
		{
			Vector3 influencedAxis2 = this.GetInfluencedAxis(this.yAI);
			this.DoActionDirect(this.yAxisTransform, this.yTI, influencedAxis2, this.joystickValue.y, this.yAxisCharacterController);
			if (this.enableYClamp && this.yTI == EasyJoystick.PropertiesInfluenced.RotateLocal)
			{
				this.DoAngleLimitation(this.yAxisTransform, this.yAI, this.clampYMin, this.clampYMax, this.startYLocalAngle);
			}
		}
	}

	private void UpdateGravity()
	{
		if (this.joystickAxis == Vector2.zero)
		{
			if (this.xAxisCharacterController != null && this.xAxisGravity > 0f)
			{
				this.xAxisCharacterController.Move(Vector3.down * this.xAxisGravity * Time.deltaTime);
			}
			if (this.yAxisCharacterController != null && this.yAxisGravity > 0f)
			{
				this.yAxisCharacterController.Move(Vector3.down * this.yAxisGravity * Time.deltaTime);
			}
		}
	}

	private Vector3 GetInfluencedAxis(EasyJoystick.AxisInfluenced axisInfluenced)
	{
		Vector3 result = Vector3.zero;
		switch (axisInfluenced)
		{
		case EasyJoystick.AxisInfluenced.X:
			result = Vector3.right;
			break;
		case EasyJoystick.AxisInfluenced.Y:
			result = Vector3.up;
			break;
		case EasyJoystick.AxisInfluenced.Z:
			result = Vector3.forward;
			break;
		case EasyJoystick.AxisInfluenced.XYZ:
			result = Vector3.one;
			break;
		}
		return result;
	}

	private void DoActionDirect(Transform axisTransform, EasyJoystick.PropertiesInfluenced inlfuencedProperty, Vector3 axis, float sensibility, CharacterController charact)
	{
		switch (inlfuencedProperty)
		{
		case EasyJoystick.PropertiesInfluenced.Rotate:
			axisTransform.Rotate(axis * sensibility * Time.deltaTime, Space.World);
			break;
		case EasyJoystick.PropertiesInfluenced.RotateLocal:
			axisTransform.Rotate(axis * sensibility * Time.deltaTime, Space.Self);
			break;
		case EasyJoystick.PropertiesInfluenced.Translate:
			if (charact == null)
			{
				axisTransform.Translate(axis * sensibility * Time.deltaTime, Space.World);
			}
			else
			{
				charact.Move(new Vector3(axis.x, axis.y, axis.z)
				{
					y = -(this.yAxisGravity + this.xAxisGravity)
				} * sensibility * Time.deltaTime);
			}
			break;
		case EasyJoystick.PropertiesInfluenced.TranslateLocal:
			if (charact == null)
			{
				axisTransform.Translate(axis * sensibility * Time.deltaTime, Space.Self);
			}
			else
			{
				Vector3 a = charact.transform.TransformDirection(axis) * sensibility;
				a.y = -(this.yAxisGravity + this.xAxisGravity);
				charact.Move(a * Time.deltaTime);
			}
			break;
		case EasyJoystick.PropertiesInfluenced.Scale:
			axisTransform.localScale += axis * sensibility * Time.deltaTime;
			break;
		}
	}

	private void DoAngleLimitation(Transform axisTransform, EasyJoystick.AxisInfluenced axisInfluenced, float clampMin, float clampMax, float startAngle)
	{
		float num = 0f;
		if (axisInfluenced != EasyJoystick.AxisInfluenced.X)
		{
			if (axisInfluenced != EasyJoystick.AxisInfluenced.Y)
			{
				if (axisInfluenced == EasyJoystick.AxisInfluenced.Z)
				{
					num = axisTransform.localRotation.eulerAngles.z;
				}
			}
			else
			{
				num = axisTransform.localRotation.eulerAngles.y;
			}
		}
		else
		{
			num = axisTransform.localRotation.eulerAngles.x;
		}
		if (num <= 360f && num >= 180f)
		{
			num -= 360f;
		}
		num = Mathf.Clamp(num, -clampMax, clampMin);
		if (axisInfluenced != EasyJoystick.AxisInfluenced.X)
		{
			if (axisInfluenced != EasyJoystick.AxisInfluenced.Y)
			{
				if (axisInfluenced == EasyJoystick.AxisInfluenced.Z)
				{
					axisTransform.localEulerAngles = new Vector3(axisTransform.localEulerAngles.x, axisTransform.localEulerAngles.y, num);
				}
			}
			else
			{
				axisTransform.localEulerAngles = new Vector3(axisTransform.localEulerAngles.x, num, axisTransform.localEulerAngles.z);
			}
		}
		else
		{
			axisTransform.localEulerAngles = new Vector3(num, axisTransform.localEulerAngles.y, axisTransform.localEulerAngles.z);
		}
	}

	private void DoAutoStabilisation(Transform axisTransform, EasyJoystick.AxisInfluenced axisInfluenced, float threshold, float speed, float startAngle)
	{
		float num = 0f;
		if (axisInfluenced != EasyJoystick.AxisInfluenced.X)
		{
			if (axisInfluenced != EasyJoystick.AxisInfluenced.Y)
			{
				if (axisInfluenced == EasyJoystick.AxisInfluenced.Z)
				{
					num = axisTransform.localRotation.eulerAngles.z;
				}
			}
			else
			{
				num = axisTransform.localRotation.eulerAngles.y;
			}
		}
		else
		{
			num = axisTransform.localRotation.eulerAngles.x;
		}
		if (num <= 360f && num >= 180f)
		{
			num -= 360f;
		}
		if (num > startAngle - threshold || num < startAngle + threshold)
		{
			float num2 = 0f;
			Vector3 zero = Vector3.zero;
			if (num > startAngle - threshold)
			{
				num2 = num + speed / 100f * Mathf.Abs(num - startAngle) * Time.deltaTime * -1f;
			}
			if (num < startAngle + threshold)
			{
				num2 = num + speed / 100f * Mathf.Abs(num - startAngle) * Time.deltaTime;
			}
			if (axisInfluenced != EasyJoystick.AxisInfluenced.X)
			{
				if (axisInfluenced != EasyJoystick.AxisInfluenced.Y)
				{
					if (axisInfluenced == EasyJoystick.AxisInfluenced.Z)
					{
						zero = new Vector3(axisTransform.localRotation.eulerAngles.x, axisTransform.localRotation.eulerAngles.y, num2);
					}
				}
				else
				{
					zero = new Vector3(axisTransform.localRotation.eulerAngles.x, num2, axisTransform.localRotation.eulerAngles.z);
				}
			}
			else
			{
				zero = new Vector3(num2, axisTransform.localRotation.eulerAngles.y, axisTransform.localRotation.eulerAngles.z);
			}
			axisTransform.localRotation = Quaternion.Euler(zero);
		}
	}

	private float GetStartAutoStabAngle(Transform axisTransform, EasyJoystick.AxisInfluenced axisInfluenced)
	{
		float num = 0f;
		if (axisTransform != null)
		{
			if (axisInfluenced != EasyJoystick.AxisInfluenced.X)
			{
				if (axisInfluenced != EasyJoystick.AxisInfluenced.Y)
				{
					if (axisInfluenced == EasyJoystick.AxisInfluenced.Z)
					{
						num = axisTransform.localRotation.eulerAngles.z;
					}
				}
				else
				{
					num = axisTransform.localRotation.eulerAngles.y;
				}
			}
			else
			{
				num = axisTransform.localRotation.eulerAngles.x;
			}
			if (num <= 360f && num >= 180f)
			{
				num -= 360f;
			}
		}
		return num;
	}

	private float ComputeDeadZone()
	{
		float num = Mathf.Max(this.joystickTouch.magnitude, 0.1f);
		float result;
		if (this.restrictArea)
		{
			result = Mathf.Max(num - this.deadZone, 0f) / (this.zoneRadius - this.touchSize - this.deadZone) / num;
		}
		else
		{
			result = Mathf.Max(num - this.deadZone, 0f) / (this.zoneRadius - this.deadZone) / num;
		}
		return result;
	}

	private void ComputeJoystickAnchor(EasyJoystick.JoystickAnchor anchor)
	{
		float num = 0f;
		if (!this.restrictArea)
		{
			num = this.touchSize;
		}
		switch (anchor)
		{
		case EasyJoystick.JoystickAnchor.None:
			this.anchorPosition = Vector2.zero;
			break;
		case EasyJoystick.JoystickAnchor.UpperLeft:
			this.anchorPosition = new Vector2(this.zoneRadius + num, this.zoneRadius + num);
			break;
		case EasyJoystick.JoystickAnchor.UpperCenter:
			this.anchorPosition = new Vector2(VirtualScreen.width / 2f, this.zoneRadius + num);
			break;
		case EasyJoystick.JoystickAnchor.UpperRight:
			this.anchorPosition = new Vector2(VirtualScreen.width - this.zoneRadius - num, this.zoneRadius + num);
			break;
		case EasyJoystick.JoystickAnchor.MiddleLeft:
			this.anchorPosition = new Vector2(this.zoneRadius + num, VirtualScreen.height / 2f);
			break;
		case EasyJoystick.JoystickAnchor.MiddleCenter:
			this.anchorPosition = new Vector2(VirtualScreen.width / 2f, VirtualScreen.height / 2f);
			break;
		case EasyJoystick.JoystickAnchor.MiddleRight:
			this.anchorPosition = new Vector2(VirtualScreen.width - this.zoneRadius - num, VirtualScreen.height / 2f);
			break;
		case EasyJoystick.JoystickAnchor.LowerLeft:
			this.anchorPosition = new Vector2(this.zoneRadius + num, VirtualScreen.height - this.zoneRadius - num);
			break;
		case EasyJoystick.JoystickAnchor.LowerCenter:
			this.anchorPosition = new Vector2(VirtualScreen.width / 2f, VirtualScreen.height - this.zoneRadius - num);
			break;
		case EasyJoystick.JoystickAnchor.LowerRight:
			this.anchorPosition = new Vector2(VirtualScreen.width - this.zoneRadius - num, VirtualScreen.height - this.zoneRadius - num);
			break;
		}
		this.areaRect = new Rect(this.anchorPosition.x + this.joystickCenter.x - this.zoneRadius, this.anchorPosition.y + this.joystickCenter.y - this.zoneRadius, this.zoneRadius * 2f, this.zoneRadius * 2f);
		this.deadRect = new Rect(this.anchorPosition.x + this.joystickCenter.x - this.deadZone, this.anchorPosition.y + this.joystickCenter.y - this.deadZone, this.deadZone * 2f, this.deadZone * 2f);
	}

	private void On_TouchStart(Gesture gesture)
	{
		if (!this.visible)
		{
			return;
		}
		if (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated)
		{
			if (!this.dynamicJoystick)
			{
				Vector2 b = new Vector2((this.anchorPosition.x + this.joystickCenter.x) * VirtualScreen.xRatio, (VirtualScreen.height - this.anchorPosition.y - this.joystickCenter.y) * VirtualScreen.yRatio);
				if ((gesture.position - b).sqrMagnitude < this.zoneRadius * VirtualScreen.xRatio * (this.zoneRadius * VirtualScreen.xRatio))
				{
					this.joystickIndex = gesture.fingerIndex;
					this.CreateEvent(EasyJoystick.MessageName.On_JoystickTouchStart);
				}
			}
			else if (!this.virtualJoystick)
			{
				switch (this.area)
				{
				case EasyJoystick.DynamicArea.FullScreen:
					this.virtualJoystick = true;
					break;
				case EasyJoystick.DynamicArea.Left:
					if (gesture.position.x < (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.Right:
					if (gesture.position.x > (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.Top:
					if (gesture.position.y > (float)(Screen.height / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.Bottom:
					if (gesture.position.y < (float)(Screen.height / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.TopLeft:
					if (gesture.position.y > (float)(Screen.height / 2) && gesture.position.x < (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.TopRight:
					if (gesture.position.y > (float)(Screen.height / 2) && gesture.position.x > (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.BottomLeft:
					if (gesture.position.y < (float)(Screen.height / 2) && gesture.position.x < (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				case EasyJoystick.DynamicArea.BottomRight:
					if (gesture.position.y < (float)(Screen.height / 2) && gesture.position.x > (float)(Screen.width / 2))
					{
						this.virtualJoystick = true;
					}
					break;
				}
				if (this.virtualJoystick)
				{
					this.joystickCenter = new Vector2(gesture.position.x / VirtualScreen.xRatio, VirtualScreen.height - gesture.position.y / VirtualScreen.yRatio);
					this.JoyAnchor = EasyJoystick.JoystickAnchor.None;
					this.joystickIndex = gesture.fingerIndex;
				}
			}
		}
	}

	private void On_SimpleTap(Gesture gesture)
	{
		if (!this.visible)
		{
			return;
		}
		if (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated && gesture.fingerIndex == this.joystickIndex)
		{
			this.CreateEvent(EasyJoystick.MessageName.On_JoystickTap);
		}
	}

	private void On_DoubleTap(Gesture gesture)
	{
		if (!this.visible)
		{
			return;
		}
		if (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated && gesture.fingerIndex == this.joystickIndex)
		{
			this.CreateEvent(EasyJoystick.MessageName.On_JoystickDoubleTap);
		}
	}

	private void On_TouchDown(Gesture gesture)
	{
		if (!this.visible)
		{
			return;
		}
		if (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated)
		{
			Vector2 b = new Vector2((this.anchorPosition.x + this.joystickCenter.x) * VirtualScreen.xRatio, (VirtualScreen.height - (this.anchorPosition.y + this.joystickCenter.y)) * VirtualScreen.yRatio);
			if (gesture.fingerIndex == this.joystickIndex)
			{
				if (((gesture.position - b).sqrMagnitude < this.zoneRadius * VirtualScreen.xRatio * (this.zoneRadius * VirtualScreen.xRatio) && this.resetFingerExit) || !this.resetFingerExit)
				{
					this.joystickTouch = new Vector2(gesture.position.x, gesture.position.y) - b;
					this.joystickTouch = new Vector2(this.joystickTouch.x / VirtualScreen.xRatio, this.joystickTouch.y / VirtualScreen.yRatio);
					if (!this.enableXaxis)
					{
						this.joystickTouch.x = 0f;
					}
					if (!this.enableYaxis)
					{
						this.joystickTouch.y = 0f;
					}
					if ((this.joystickTouch / (this.zoneRadius - this.touchSizeCoef)).sqrMagnitude > 1f)
					{
						this.joystickTouch.Normalize();
						this.joystickTouch *= this.zoneRadius - this.touchSizeCoef;
					}
				}
				else
				{
					this.On_TouchUp(gesture);
				}
			}
		}
	}

	private void On_TouchUp(Gesture gesture)
	{
		if (!this.visible)
		{
			return;
		}
		if (gesture.fingerIndex == this.joystickIndex)
		{
			this.joystickIndex = -1;
			if (this.dynamicJoystick)
			{
				this.virtualJoystick = false;
			}
			this.CreateEvent(EasyJoystick.MessageName.On_JoystickTouchUp);
		}
	}

	public void On_Manual(Vector2 movement)
	{
		if (this.isActivated)
		{
			if (movement != Vector2.zero)
			{
				if (!this.virtualJoystick)
				{
					this.virtualJoystick = true;
					this.CreateEvent(EasyJoystick.MessageName.On_JoystickTouchStart);
				}
				this.joystickIndex = 0;
				this.joystickTouch.x = movement.x * (this.areaRect.width / 2f);
				this.joystickTouch.y = movement.y * (this.areaRect.height / 2f);
			}
			else if (this.virtualJoystick)
			{
				this.virtualJoystick = false;
				this.joystickIndex = -1;
				this.CreateEvent(EasyJoystick.MessageName.On_JoystickTouchUp);
			}
		}
	}

	private Vector2 joystickAxis;

	private Vector2 joystickTouch;

	private Vector2 joystickValue;

	public bool enable = true;

	public bool visible = true;

	public bool isActivated = true;

	public bool showDebugRadius;

	public bool selected;

	public bool useFixedUpdate;

	public bool isUseGuiLayout = true;

	[SerializeField]
	private bool dynamicJoystick;

	public EasyJoystick.DynamicArea area;

	[SerializeField]
	private EasyJoystick.JoystickAnchor joyAnchor = EasyJoystick.JoystickAnchor.LowerLeft;

	[SerializeField]
	private Vector2 joystickPositionOffset = Vector2.zero;

	[SerializeField]
	private float zoneRadius = 100f;

	[SerializeField]
	private float touchSize = 30f;

	public float deadZone = 20f;

	[SerializeField]
	private bool restrictArea;

	public bool resetFingerExit;

	[SerializeField]
	private EasyJoystick.InteractionType interaction;

	public bool useBroadcast;

	public EasyJoystick.Broadcast messageMode;

	public GameObject receiverGameObject;

	public Vector2 speed;

	public bool enableXaxis = true;

	[SerializeField]
	private Transform xAxisTransform;

	public CharacterController xAxisCharacterController;

	public float xAxisGravity;

	[SerializeField]
	private EasyJoystick.PropertiesInfluenced xTI;

	public EasyJoystick.AxisInfluenced xAI;

	public bool inverseXAxis;

	public bool enableXClamp;

	public float clampXMax;

	public float clampXMin;

	public bool enableXAutoStab;

	[SerializeField]
	private float thresholdX = 0.01f;

	[SerializeField]
	private float stabSpeedX = 20f;

	public bool enableYaxis = true;

	[SerializeField]
	private Transform yAxisTransform;

	public CharacterController yAxisCharacterController;

	public float yAxisGravity;

	[SerializeField]
	private EasyJoystick.PropertiesInfluenced yTI;

	public EasyJoystick.AxisInfluenced yAI;

	public bool inverseYAxis;

	public bool enableYClamp;

	public float clampYMax;

	public float clampYMin;

	public bool enableYAutoStab;

	[SerializeField]
	private float thresholdY = 0.01f;

	[SerializeField]
	private float stabSpeedY = 20f;

	public bool enableSmoothing;

	[SerializeField]
	public Vector2 smoothing = new Vector2(2f, 2f);

	public bool enableInertia;

	[SerializeField]
	public Vector2 inertia = new Vector2(100f, 100f);

	public int guiDepth;

	public bool showZone = true;

	public bool showTouch = true;

	public bool showDeadZone = true;

	public Texture areaTexture;

	public Color areaColor = Color.white;

	public Texture touchTexture;

	public Color touchColor = Color.white;

	public Texture deadTexture;

	public bool showProperties = true;

	public bool showInteraction;

	public bool showAppearance;

	public bool showPosition = true;

	private Vector2 joystickCenter;

	private Rect areaRect;

	private Rect deadRect;

	private Vector2 anchorPosition = Vector2.zero;

	private bool virtualJoystick = true;

	private int joystickIndex = -1;

	private float touchSizeCoef;

	private bool sendEnd = true;

	private float startXLocalAngle;

	private float startYLocalAngle;

	public delegate void JoystickMoveStartHandler(MovingJoystick move);

	public delegate void JoystickMoveHandler(MovingJoystick move);

	public delegate void JoystickMoveEndHandler(MovingJoystick move);

	public delegate void JoystickTouchStartHandler(MovingJoystick move);

	public delegate void JoystickTapHandler(MovingJoystick move);

	public delegate void JoystickDoubleTapHandler(MovingJoystick move);

	public delegate void JoystickTouchUpHandler(MovingJoystick move);

	public enum JoystickAnchor
	{
		None,
		UpperLeft,
		UpperCenter,
		UpperRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		LowerLeft,
		LowerCenter,
		LowerRight
	}

	public enum PropertiesInfluenced
	{
		Rotate,
		RotateLocal,
		Translate,
		TranslateLocal,
		Scale
	}

	public enum AxisInfluenced
	{
		X,
		Y,
		Z,
		XYZ
	}

	public enum DynamicArea
	{
		FullScreen,
		Left,
		Right,
		Top,
		Bottom,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public enum InteractionType
	{
		Direct,
		Include,
		EventNotification,
		DirectAndEvent
	}

	public enum Broadcast
	{
		SendMessage,
		SendMessageUpwards,
		BroadcastMessage
	}

	private enum MessageName
	{
		On_JoystickMoveStart,
		On_JoystickTouchStart,
		On_JoystickTouchUp,
		On_JoystickMove,
		On_JoystickMoveEnd,
		On_JoystickTap,
		On_JoystickDoubleTap
	}
}
