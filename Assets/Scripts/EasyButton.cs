using System;
using UnityEngine;

[ExecuteInEditMode]
public class EasyButton : MonoBehaviour
{
	public static event EasyButton.ButtonDownHandler On_ButtonDown;

	public static event EasyButton.ButtonPressHandler On_ButtonPress;

	public static event EasyButton.ButtonUpHandler On_ButtonUp;

	public EasyButton.ButtonAnchor Anchor
	{
		get
		{
			return this.anchor;
		}
		set
		{
			this.anchor = value;
			this.ComputeButtonAnchor(this.anchor);
		}
	}

	public Vector2 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			this.ComputeButtonAnchor(this.anchor);
		}
	}

	public Vector2 Scale
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
			this.ComputeButtonAnchor(this.anchor);
		}
	}

	public Texture2D NormalTexture
	{
		get
		{
			return this.normalTexture;
		}
		set
		{
			this.normalTexture = value;
			if (this.normalTexture != null)
			{
				this.ComputeButtonAnchor(this.anchor);
				this.currentTexture = this.normalTexture;
			}
		}
	}

	public Texture2D ActiveTexture
	{
		get
		{
			return this.activeTexture;
		}
		set
		{
			this.activeTexture = value;
		}
	}

	private void OnEnable()
	{
		EasyTouch.On_TouchStart += this.On_TouchStart;
		EasyTouch.On_TouchDown += this.On_TouchDown;
		EasyTouch.On_TouchUp += this.On_TouchUp;
	}

	private void OnDisable()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
		EasyTouch.On_TouchDown -= this.On_TouchDown;
		EasyTouch.On_TouchUp -= this.On_TouchUp;
		if (Application.isPlaying && EasyTouch.instance != null)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
		}
	}

	private void OnDestroy()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
		EasyTouch.On_TouchDown -= this.On_TouchDown;
		EasyTouch.On_TouchUp -= this.On_TouchUp;
		if (Application.isPlaying && EasyTouch.instance != null)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
		}
	}

	private void Start()
	{
		this.currentTexture = this.normalTexture;
		this.currentColor = this.buttonNormalColor;
		this.buttonState = EasyButton.ButtonState.None;
		VirtualScreen.ComputeVirtualScreen();
		this.ComputeButtonAnchor(this.anchor);
	}

	private void OnGUI()
	{
		if (this.enable)
		{
			GUI.depth = this.guiDepth;
			base.useGUILayout = this.isUseGuiLayout;
			VirtualScreen.ComputeVirtualScreen();
			VirtualScreen.SetGuiScaleMatrix();
			if (this.normalTexture != null && this.activeTexture != null)
			{
				this.ComputeButtonAnchor(this.anchor);
				if (this.normalTexture != null)
				{
					if (Application.isEditor && !Application.isPlaying)
					{
						this.currentTexture = this.normalTexture;
					}
					if (this.showDebugArea && Application.isEditor && this.selected && !Application.isPlaying)
					{
						GUI.Box(this.buttonRect, string.Empty);
					}
					if (this.currentTexture != null)
					{
						if (this.isActivated)
						{
							GUI.color = this.currentColor;
							if (Application.isPlaying)
							{
								EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
								EasyTouch.instance.reservedVirtualAreas.Add(this.buttonRect);
							}
						}
						else
						{
							GUI.color = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, 0.2f);
							if (Application.isPlaying)
							{
								EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
							}
						}
						GUI.DrawTexture(this.buttonRect, this.currentTexture);
						GUI.color = Color.white;
					}
				}
			}
		}
		else if (Application.isPlaying)
		{
			EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
		}
	}

	private void Update()
	{
		if (this.buttonState == EasyButton.ButtonState.Up)
		{
			this.buttonState = EasyButton.ButtonState.None;
		}
		if (EasyTouch.GetTouchCount() == 0)
		{
			this.buttonFingerIndex = -1;
			this.currentTexture = this.normalTexture;
			this.currentColor = this.buttonNormalColor;
			this.buttonState = EasyButton.ButtonState.None;
		}
	}

	private void OnDrawGizmos()
	{
	}

	private void ComputeButtonAnchor(EasyButton.ButtonAnchor anchor)
	{
		if (this.normalTexture != null)
		{
			Vector2 vector = new Vector2((float)this.normalTexture.width * this.scale.x, (float)this.normalTexture.height * this.scale.y);
			Vector2 zero = Vector2.zero;
			switch (anchor)
			{
			case EasyButton.ButtonAnchor.UpperLeft:
				zero = new Vector2(0f, 0f);
				break;
			case EasyButton.ButtonAnchor.UpperCenter:
				zero = new Vector2(VirtualScreen.width / 2f - vector.x / 2f, this.offset.y);
				break;
			case EasyButton.ButtonAnchor.UpperRight:
				zero = new Vector2(VirtualScreen.width - vector.x, 0f);
				break;
			case EasyButton.ButtonAnchor.MiddleLeft:
				zero = new Vector2(0f, VirtualScreen.height / 2f - vector.y / 2f);
				break;
			case EasyButton.ButtonAnchor.MiddleCenter:
				zero = new Vector2(VirtualScreen.width / 2f - vector.x / 2f, VirtualScreen.height / 2f - vector.y / 2f);
				break;
			case EasyButton.ButtonAnchor.MiddleRight:
				zero = new Vector2(VirtualScreen.width - vector.x, VirtualScreen.height / 2f - vector.y / 2f);
				break;
			case EasyButton.ButtonAnchor.LowerLeft:
				zero = new Vector2(0f, VirtualScreen.height - vector.y);
				break;
			case EasyButton.ButtonAnchor.LowerCenter:
				zero = new Vector2(VirtualScreen.width / 2f - vector.x / 2f, VirtualScreen.height - vector.y);
				break;
			case EasyButton.ButtonAnchor.LowerRight:
				zero = new Vector2(VirtualScreen.width - vector.x, VirtualScreen.height - vector.y);
				break;
			}
			this.buttonRect = new Rect(zero.x + this.offset.x, zero.y + this.offset.y, vector.x, vector.y);
		}
	}

	private void RaiseEvent(EasyButton.MessageName msg)
	{
		if (this.interaction == EasyButton.InteractionType.Event)
		{
			if (!this.useBroadcast)
			{
				if (msg != EasyButton.MessageName.On_ButtonDown)
				{
					if (msg != EasyButton.MessageName.On_ButtonUp)
					{
						if (msg == EasyButton.MessageName.On_ButtonPress)
						{
							if (EasyButton.On_ButtonPress != null)
							{
								EasyButton.On_ButtonPress(base.gameObject.name);
							}
						}
					}
					else if (EasyButton.On_ButtonUp != null)
					{
						EasyButton.On_ButtonUp(base.gameObject.name);
					}
				}
				else if (EasyButton.On_ButtonDown != null)
				{
					EasyButton.On_ButtonDown(base.gameObject.name);
				}
			}
			else
			{
				string methodName = msg.ToString();
				if (msg == EasyButton.MessageName.On_ButtonDown && this.downMethodName != string.Empty && this.useSpecificalMethod)
				{
					methodName = this.downMethodName;
				}
				if (msg == EasyButton.MessageName.On_ButtonPress && this.pressMethodName != string.Empty && this.useSpecificalMethod)
				{
					methodName = this.pressMethodName;
				}
				if (msg == EasyButton.MessageName.On_ButtonUp && this.upMethodName != string.Empty && this.useSpecificalMethod)
				{
					methodName = this.upMethodName;
				}
				if (this.receiverGameObject != null)
				{
					EasyButton.Broadcast broadcast = this.messageMode;
					if (broadcast != EasyButton.Broadcast.BroadcastMessage)
					{
						if (broadcast != EasyButton.Broadcast.SendMessage)
						{
							if (broadcast == EasyButton.Broadcast.SendMessageUpwards)
							{
								this.receiverGameObject.SendMessageUpwards(methodName, base.name, SendMessageOptions.DontRequireReceiver);
							}
						}
						else
						{
							this.receiverGameObject.SendMessage(methodName, base.name, SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						this.receiverGameObject.BroadcastMessage(methodName, base.name, SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Button : " + base.gameObject.name + " : you must setup receiver gameobject");
				}
			}
		}
	}

	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) && this.enable && this.isActivated)
		{
			this.buttonFingerIndex = gesture.fingerIndex;
			this.currentTexture = this.activeTexture;
			this.currentColor = this.buttonActiveColor;
			this.buttonState = EasyButton.ButtonState.Down;
			this.frame = 0;
			this.RaiseEvent(EasyButton.MessageName.On_ButtonDown);
		}
	}

	private void On_TouchDown(Gesture gesture)
	{
		if (gesture.fingerIndex == this.buttonFingerIndex || (this.isSwipeIn && this.buttonState == EasyButton.ButtonState.None))
		{
			if (gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) && this.enable && this.isActivated)
			{
				this.currentTexture = this.activeTexture;
				this.currentColor = this.buttonActiveColor;
				this.frame++;
				if ((this.buttonState == EasyButton.ButtonState.Down || this.buttonState == EasyButton.ButtonState.Press) && this.frame >= 2)
				{
					this.RaiseEvent(EasyButton.MessageName.On_ButtonPress);
					this.buttonState = EasyButton.ButtonState.Press;
				}
				if (this.buttonState == EasyButton.ButtonState.None)
				{
					this.buttonFingerIndex = gesture.fingerIndex;
					this.buttonState = EasyButton.ButtonState.Down;
					this.frame = 0;
					this.RaiseEvent(EasyButton.MessageName.On_ButtonDown);
				}
			}
			else if ((this.isSwipeIn || !this.isSwipeIn) && !this.isSwipeOut && this.buttonState == EasyButton.ButtonState.Press)
			{
				this.buttonFingerIndex = -1;
				this.currentTexture = this.normalTexture;
				this.currentColor = this.buttonNormalColor;
				this.buttonState = EasyButton.ButtonState.None;
			}
			else if (this.isSwipeOut && this.buttonState == EasyButton.ButtonState.Press)
			{
				this.RaiseEvent(EasyButton.MessageName.On_ButtonPress);
				this.buttonState = EasyButton.ButtonState.Press;
			}
		}
	}

	private void On_TouchUp(Gesture gesture)
	{
		if (gesture.fingerIndex == this.buttonFingerIndex)
		{
			if ((gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) || (this.isSwipeOut && this.buttonState == EasyButton.ButtonState.Press)) && this.enable && this.isActivated)
			{
				this.RaiseEvent(EasyButton.MessageName.On_ButtonUp);
			}
			this.buttonState = EasyButton.ButtonState.Up;
			this.buttonFingerIndex = -1;
			this.currentTexture = this.normalTexture;
			this.currentColor = this.buttonNormalColor;
		}
	}

	public bool enable = true;

	public bool isActivated = true;

	public bool showDebugArea = true;

	public bool selected;

	public bool isUseGuiLayout = true;

	public EasyButton.ButtonState buttonState = EasyButton.ButtonState.None;

	[SerializeField]
	private EasyButton.ButtonAnchor anchor = EasyButton.ButtonAnchor.LowerRight;

	[SerializeField]
	private Vector2 offset = Vector2.zero;

	[SerializeField]
	private Vector2 scale = Vector2.one;

	public bool isSwipeIn;

	public bool isSwipeOut;

	public EasyButton.InteractionType interaction;

	public bool useBroadcast;

	public GameObject receiverGameObject;

	public EasyButton.Broadcast messageMode;

	public bool useSpecificalMethod;

	public string downMethodName;

	public string pressMethodName;

	public string upMethodName;

	public int guiDepth;

	[SerializeField]
	private Texture2D normalTexture;

	public Color buttonNormalColor = Color.white;

	[SerializeField]
	private Texture2D activeTexture;

	public Color buttonActiveColor = Color.white;

	public bool showInspectorProperties = true;

	public bool showInspectorPosition = true;

	public bool showInspectorEvent;

	public bool showInspectorTexture;

	private Rect buttonRect;

	private int buttonFingerIndex = -1;

	private Texture2D currentTexture;

	private Color currentColor;

	private int frame;

	public delegate void ButtonUpHandler(string buttonName);

	public delegate void ButtonPressHandler(string buttonName);

	public delegate void ButtonDownHandler(string buttonName);

	public enum ButtonAnchor
	{
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

	public enum Broadcast
	{
		SendMessage,
		SendMessageUpwards,
		BroadcastMessage
	}

	public enum ButtonState
	{
		Down,
		Press,
		Up,
		None
	}

	public enum InteractionType
	{
		Event,
		Include
	}

	private enum MessageName
	{
		On_ButtonDown,
		On_ButtonPress,
		On_ButtonUp
	}
}
