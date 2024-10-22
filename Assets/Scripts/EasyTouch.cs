using System;
using System.Collections.Generic;
using UnityEngine;

public class EasyTouch : MonoBehaviour
{
	public EasyTouch()
	{
		this.enable = true;
		this.useBroadcastMessage = false;
		this.enable2FingersGesture = true;
		this.enableTwist = true;
		this.enablePinch = true;
		this.autoSelect = false;
		this.StationnaryTolerance = 25f;
		this.longTapTime = 1f;
		this.swipeTolerance = 0.85f;
		this.minPinchLength = 0f;
		this.minTwistAngle = 1f;
	}

	public static event EasyTouch.TouchCancelHandler On_Cancel;

	public static event EasyTouch.Cancel2FingersHandler On_Cancel2Fingers;

	public static event EasyTouch.TouchStartHandler On_TouchStart;

	public static event EasyTouch.TouchDownHandler On_TouchDown;

	public static event EasyTouch.TouchUpHandler On_TouchUp;

	public static event EasyTouch.SimpleTapHandler On_SimpleTap;

	public static event EasyTouch.DoubleTapHandler On_DoubleTap;

	public static event EasyTouch.LongTapStartHandler On_LongTapStart;

	public static event EasyTouch.LongTapHandler On_LongTap;

	public static event EasyTouch.LongTapEndHandler On_LongTapEnd;

	public static event EasyTouch.DragStartHandler On_DragStart;

	public static event EasyTouch.DragHandler On_Drag;

	public static event EasyTouch.DragEndHandler On_DragEnd;

	public static event EasyTouch.SwipeStartHandler On_SwipeStart;

	public static event EasyTouch.SwipeHandler On_Swipe;

	public static event EasyTouch.SwipeEndHandler On_SwipeEnd;

	public static event EasyTouch.TouchStart2FingersHandler On_TouchStart2Fingers;

	public static event EasyTouch.TouchDown2FingersHandler On_TouchDown2Fingers;

	public static event EasyTouch.TouchUp2FingersHandler On_TouchUp2Fingers;

	public static event EasyTouch.SimpleTap2FingersHandler On_SimpleTap2Fingers;

	public static event EasyTouch.DoubleTap2FingersHandler On_DoubleTap2Fingers;

	public static event EasyTouch.LongTapStart2FingersHandler On_LongTapStart2Fingers;

	public static event EasyTouch.LongTap2FingersHandler On_LongTap2Fingers;

	public static event EasyTouch.LongTapEnd2FingersHandler On_LongTapEnd2Fingers;

	public static event EasyTouch.TwistHandler On_Twist;

	public static event EasyTouch.TwistEndHandler On_TwistEnd;

	public static event EasyTouch.PinchInHandler On_PinchIn;

	public static event EasyTouch.PinchOutHandler On_PinchOut;

	public static event EasyTouch.PinchEndHandler On_PinchEnd;

	public static event EasyTouch.DragStart2FingersHandler On_DragStart2Fingers;

	public static event EasyTouch.Drag2FingersHandler On_Drag2Fingers;

	public static event EasyTouch.DragEnd2FingersHandler On_DragEnd2Fingers;

	public static event EasyTouch.SwipeStart2FingersHandler On_SwipeStart2Fingers;

	public static event EasyTouch.Swipe2FingersHandler On_Swipe2Fingers;

	public static event EasyTouch.SwipeEnd2FingersHandler On_SwipeEnd2Fingers;

	public static event EasyTouch.EasyTouchIsReadyHandler On_EasyTouchIsReady;

	private void OnEnable()
	{
		if (Application.isPlaying && Application.isEditor)
		{
			this.InitEasyTouch();
		}
	}

	private void Start()
	{
		int num = this.touchCameras.FindIndex((ECamera c) => c.camera == Camera.main);
		if (num < 0)
		{
			this.touchCameras.Add(new ECamera(Camera.main, false));
		}
		this.InitEasyTouch();
		this.RaiseReadyEvent();
	}

	private void InitEasyTouch()
	{
		this.input = new EasyTouchInput();
		if (EasyTouch.instance == null)
		{
			EasyTouch.instance = this;
		}
	}

	private void OnDrawGizmos()
	{
	}

	private void Update()
	{
		if (this.enable && EasyTouch.instance == this)
		{
			int num = this.input.TouchCount();
			if (this.oldTouchCount == 2 && num != 2 && num > 0)
			{
				this.CreateGesture2Finger(EasyTouch.EventName.On_Cancel2Fingers, Vector2.zero, Vector2.zero, Vector2.zero, 0f, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, 0f);
			}
			this.UpdateTouches(true, num);
			this.oldPickObject2Finger = this.pickObject2Finger;
			if (this.enable2FingersGesture)
			{
				if (num == 2)
				{
					this.TwoFinger();
				}
				else
				{
					this.complexCurrentGesture = EasyTouch.GestureType.None;
					this.pickObject2Finger = null;
					this.twoFingerSwipeStart = false;
					this.twoFingerDragStart = false;
				}
			}
			for (int i = 0; i < 10; i++)
			{
				if (this.fingers[i] != null)
				{
					this.OneFinger(i);
				}
			}
			this.oldTouchCount = num;
		}
	}

	private void UpdateTouches(bool realTouch, int touchCount)
	{
		Finger[] array = new Finger[10];
		this.fingers.CopyTo(array, 0);
		if (realTouch || this.enableRemote)
		{
			this.ResetTouches();
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = UnityEngine.Input.GetTouch(i);
				int num = 0;
				while (num < 10 && this.fingers[i] == null)
				{
					if (array[num] != null && array[num].fingerIndex == touch.fingerId)
					{
						this.fingers[i] = array[num];
					}
					num++;
				}
				if (this.fingers[i] == null)
				{
					this.fingers[i] = new Finger();
					this.fingers[i].fingerIndex = touch.fingerId;
					this.fingers[i].gesture = EasyTouch.GestureType.None;
					this.fingers[i].phase = TouchPhase.Began;
				}
				else
				{
					this.fingers[i].phase = touch.phase;
				}
				this.fingers[i].position = touch.position;
				this.fingers[i].deltaPosition = touch.deltaPosition;
				this.fingers[i].tapCount = touch.tapCount;
				this.fingers[i].deltaTime = touch.deltaTime;
				this.fingers[i].touchCount = touchCount;
			}
		}
		else
		{
			for (int j = 0; j < touchCount; j++)
			{
				this.fingers[j] = this.input.GetMouseTouch(j, this.fingers[j]);
				this.fingers[j].touchCount = touchCount;
			}
		}
	}

	private void ResetTouches()
	{
		for (int i = 0; i < 10; i++)
		{
			this.fingers[i] = null;
		}
	}

	private void OneFinger(int fingerIndex)
	{
		if (this.fingers[fingerIndex].gesture == EasyTouch.GestureType.None)
		{
			this.startTimeAction = Time.realtimeSinceStartup;
			this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Acquisition;
			this.fingers[fingerIndex].startPosition = this.fingers[fingerIndex].position;
			if (this.autoSelect)
			{
				this.GetPickeGameObject(ref this.fingers[fingerIndex], false);
			}
			this.CreateGesture(fingerIndex, EasyTouch.EventName.On_TouchStart, this.fingers[fingerIndex], 0f, EasyTouch.SwipeType.None, 0f, Vector2.zero);
		}
		float num = Time.realtimeSinceStartup - this.startTimeAction;
		if (this.fingers[fingerIndex].phase == TouchPhase.Canceled)
		{
			this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Cancel;
		}
		if (this.fingers[fingerIndex].phase != TouchPhase.Ended && this.fingers[fingerIndex].phase != TouchPhase.Canceled)
		{
			if (this.fingers[fingerIndex].phase == TouchPhase.Stationary && num >= this.longTapTime && this.fingers[fingerIndex].gesture == EasyTouch.GestureType.Acquisition)
			{
				this.fingers[fingerIndex].gesture = EasyTouch.GestureType.LongTap;
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_LongTapStart, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
			}
			if ((this.fingers[fingerIndex].gesture == EasyTouch.GestureType.Acquisition || this.fingers[fingerIndex].gesture == EasyTouch.GestureType.LongTap) && !this.FingerInTolerance(this.fingers[fingerIndex]))
			{
				if (this.fingers[fingerIndex].gesture == EasyTouch.GestureType.LongTap)
				{
					this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Cancel;
					this.CreateGesture(fingerIndex, EasyTouch.EventName.On_LongTapEnd, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
					this.fingers[fingerIndex].gesture = EasyTouch.GestureType.None;
				}
				else if (this.fingers[fingerIndex].pickedObject)
				{
					this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Drag;
					this.CreateGesture(fingerIndex, EasyTouch.EventName.On_DragStart, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
				}
				else
				{
					this.fingers[fingerIndex].gesture = EasyTouch.GestureType.Swipe;
					this.CreateGesture(fingerIndex, EasyTouch.EventName.On_SwipeStart, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
				}
			}
			EasyTouch.EventName eventName = EasyTouch.EventName.None;
			EasyTouch.GestureType gesture = this.fingers[fingerIndex].gesture;
			if (gesture != EasyTouch.GestureType.LongTap)
			{
				if (gesture != EasyTouch.GestureType.Drag)
				{
					if (gesture == EasyTouch.GestureType.Swipe)
					{
						eventName = EasyTouch.EventName.On_Swipe;
					}
				}
				else
				{
					eventName = EasyTouch.EventName.On_Drag;
				}
			}
			else
			{
				eventName = EasyTouch.EventName.On_LongTap;
			}
			EasyTouch.SwipeType swipe = EasyTouch.SwipeType.None;
			if (eventName != EasyTouch.EventName.None)
			{
				swipe = this.GetSwipe(new Vector2(0f, 0f), this.fingers[fingerIndex].deltaPosition);
				this.CreateGesture(fingerIndex, eventName, this.fingers[fingerIndex], num, swipe, 0f, this.fingers[fingerIndex].deltaPosition);
			}
			this.CreateGesture(fingerIndex, EasyTouch.EventName.On_TouchDown, this.fingers[fingerIndex], num, swipe, 0f, this.fingers[fingerIndex].deltaPosition);
		}
		else
		{
			bool flag = true;
			switch (this.fingers[fingerIndex].gesture)
			{
			case EasyTouch.GestureType.Drag:
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_DragEnd, this.fingers[fingerIndex], num, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].startPosition - this.fingers[fingerIndex].position).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
				break;
			case EasyTouch.GestureType.Swipe:
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_SwipeEnd, this.fingers[fingerIndex], num, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
				break;
			case EasyTouch.GestureType.LongTap:
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_LongTapEnd, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
				break;
			case EasyTouch.GestureType.Cancel:
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_Cancel, this.fingers[fingerIndex], 0f, EasyTouch.SwipeType.None, 0f, Vector2.zero);
				break;
			case EasyTouch.GestureType.Acquisition:
				if (this.FingerInTolerance(this.fingers[fingerIndex]))
				{
					if (this.fingers[fingerIndex].tapCount < 2)
					{
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_SimpleTap, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
					}
					else
					{
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_DoubleTap, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
					}
				}
				else
				{
					EasyTouch.SwipeType swipe2 = this.GetSwipe(new Vector2(0f, 0f), this.fingers[fingerIndex].deltaPosition);
					if (this.fingers[fingerIndex].pickedObject)
					{
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_DragStart, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_Drag, this.fingers[fingerIndex], num, swipe2, 0f, this.fingers[fingerIndex].deltaPosition);
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_DragEnd, this.fingers[fingerIndex], num, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].startPosition - this.fingers[fingerIndex].position).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
					}
					else
					{
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_SwipeStart, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_Swipe, this.fingers[fingerIndex], num, swipe2, 0f, this.fingers[fingerIndex].deltaPosition);
						this.CreateGesture(fingerIndex, EasyTouch.EventName.On_SwipeEnd, this.fingers[fingerIndex], num, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), (this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition).magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
					}
				}
				break;
			}
			if (flag)
			{
				this.CreateGesture(fingerIndex, EasyTouch.EventName.On_TouchUp, this.fingers[fingerIndex], num, EasyTouch.SwipeType.None, 0f, Vector2.zero);
				this.fingers[fingerIndex] = null;
			}
		}
	}

	private void CreateGesture(int touchIndex, EasyTouch.EventName message, Finger finger, float actionTime, EasyTouch.SwipeType swipe, float swipeLength, Vector2 swipeVector)
	{
		if (message == EasyTouch.EventName.On_TouchStart || message == EasyTouch.EventName.On_TouchUp)
		{
			this.isStartHoverNGUI = this.IsTouchHoverNGui(touchIndex);
		}
		if (!this.isStartHoverNGUI)
		{
			Gesture gesture = new Gesture();
			gesture.fingerIndex = finger.fingerIndex;
			gesture.touchCount = finger.touchCount;
			gesture.startPosition = finger.startPosition;
			gesture.position = finger.position;
			gesture.deltaPosition = finger.deltaPosition;
			gesture.actionTime = actionTime;
			gesture.deltaTime = finger.deltaTime;
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = 0f;
			gesture.twistAngle = 0f;
			gesture.pickObject = finger.pickedObject;
			gesture.otherReceiver = this.receiverObject;
			gesture.isHoverReservedArea = this.IsTouchReservedArea(touchIndex);
			gesture.pickCamera = finger.pickedCamera;
			gesture.isGuiCamera = finger.isGuiCamera;
			if (this.useBroadcastMessage)
			{
				this.SendGesture(message, gesture);
			}
			if (!this.useBroadcastMessage || this.isExtension)
			{
				this.RaiseEvent(message, gesture);
			}
		}
	}

	private void SendGesture(EasyTouch.EventName message, Gesture gesture)
	{
		if (this.useBroadcastMessage)
		{
			if (this.receiverObject != null && this.receiverObject != gesture.pickObject)
			{
				this.receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
			}
			if (gesture.pickObject)
			{
				gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				base.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void TwoFinger()
	{
		float num = 0f;
		bool flag = false;
		Vector2 zero = Vector2.zero;
		Vector2 vector = Vector2.zero;
		if (this.complexCurrentGesture == EasyTouch.GestureType.None)
		{
			this.twoFinger0 = this.GetTwoFinger(-1);
			this.twoFinger1 = this.GetTwoFinger(this.twoFinger0);
			this.startTimeAction = Time.realtimeSinceStartup;
			this.complexCurrentGesture = EasyTouch.GestureType.Tap;
			this.fingers[this.twoFinger0].complexStartPosition = this.fingers[this.twoFinger0].position;
			this.fingers[this.twoFinger1].complexStartPosition = this.fingers[this.twoFinger1].position;
			this.fingers[this.twoFinger0].oldPosition = this.fingers[this.twoFinger0].position;
			this.fingers[this.twoFinger1].oldPosition = this.fingers[this.twoFinger1].position;
			this.oldFingerDistance = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger0].position, this.fingers[this.twoFinger1].position));
			this.startPosition2Finger = new Vector2((this.fingers[this.twoFinger0].position.x + this.fingers[this.twoFinger1].position.x) / 2f, (this.fingers[this.twoFinger0].position.y + this.fingers[this.twoFinger1].position.y) / 2f);
			vector = Vector2.zero;
			if (this.autoSelect)
			{
				if (this.GetPickeGameObject(ref this.fingers[this.twoFinger0], true))
				{
					this.GetPickeGameObject(ref this.fingers[this.twoFinger1], true);
					if (this.fingers[this.twoFinger0].pickedObject != this.fingers[this.twoFinger1].pickedObject)
					{
						this.pickObject2Finger = null;
						this.fingers[this.twoFinger0].pickedObject = null;
						this.fingers[this.twoFinger1].pickedObject = null;
						this.fingers[this.twoFinger0].isGuiCamera = false;
						this.fingers[this.twoFinger1].isGuiCamera = false;
						this.fingers[this.twoFinger0].pickedCamera = null;
						this.fingers[this.twoFinger1].pickedCamera = null;
					}
					else
					{
						this.pickObject2Finger = this.fingers[this.twoFinger0].pickedObject;
					}
				}
				else
				{
					this.pickObject2Finger = null;
				}
			}
			this.CreateGesture2Finger(EasyTouch.EventName.On_TouchStart2Fingers, this.startPosition2Finger, this.startPosition2Finger, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, this.oldFingerDistance);
		}
		num = Time.realtimeSinceStartup - this.startTimeAction;
		zero = new Vector2((this.fingers[this.twoFinger0].position.x + this.fingers[this.twoFinger1].position.x) / 2f, (this.fingers[this.twoFinger0].position.y + this.fingers[this.twoFinger1].position.y) / 2f);
		vector = zero - this.oldStartPosition2Finger;
		float num2 = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger0].position, this.fingers[this.twoFinger1].position));
		if (this.fingers[this.twoFinger0].phase == TouchPhase.Canceled || this.fingers[this.twoFinger1].phase == TouchPhase.Canceled)
		{
			this.complexCurrentGesture = EasyTouch.GestureType.Cancel;
		}
		if (this.fingers[this.twoFinger0].phase != TouchPhase.Ended && this.fingers[this.twoFinger1].phase != TouchPhase.Ended && this.complexCurrentGesture != EasyTouch.GestureType.Cancel)
		{
			if (this.complexCurrentGesture == EasyTouch.GestureType.Tap && num >= this.longTapTime && this.FingerInTolerance(this.fingers[this.twoFinger0]) && this.FingerInTolerance(this.fingers[this.twoFinger1]))
			{
				this.complexCurrentGesture = EasyTouch.GestureType.LongTap;
				this.CreateGesture2Finger(EasyTouch.EventName.On_LongTapStart2Fingers, this.startPosition2Finger, zero, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, num2);
			}
			if (!this.FingerInTolerance(this.fingers[this.twoFinger0]) || !this.FingerInTolerance(this.fingers[this.twoFinger1]))
			{
				flag = true;
			}
			if (flag)
			{
				float num3 = Vector2.Dot(this.fingers[this.twoFinger0].deltaPosition.normalized, this.fingers[this.twoFinger1].deltaPosition.normalized);
				if (this.enablePinch && num2 != this.oldFingerDistance)
				{
					if (Mathf.Abs(num2 - this.oldFingerDistance) >= this.minPinchLength)
					{
						this.complexCurrentGesture = EasyTouch.GestureType.Pinch;
					}
					if (this.complexCurrentGesture == EasyTouch.GestureType.Pinch)
					{
						if (num2 < this.oldFingerDistance)
						{
							if (this.oldGesture != EasyTouch.GestureType.Pinch)
							{
								this.CreateStateEnd2Fingers(this.oldGesture, this.startPosition2Finger, zero, vector, num, false, num2);
								this.startTimeAction = Time.realtimeSinceStartup;
							}
							this.CreateGesture2Finger(EasyTouch.EventName.On_PinchIn, this.startPosition2Finger, zero, vector, num, this.GetSwipe(this.fingers[this.twoFinger0].complexStartPosition, this.fingers[this.twoFinger0].position), 0f, Vector2.zero, 0f, Mathf.Abs(num2 - this.oldFingerDistance), num2);
							this.complexCurrentGesture = EasyTouch.GestureType.Pinch;
						}
						else if (num2 > this.oldFingerDistance)
						{
							if (this.oldGesture != EasyTouch.GestureType.Pinch)
							{
								this.CreateStateEnd2Fingers(this.oldGesture, this.startPosition2Finger, zero, vector, num, false, num2);
								this.startTimeAction = Time.realtimeSinceStartup;
							}
							this.CreateGesture2Finger(EasyTouch.EventName.On_PinchOut, this.startPosition2Finger, zero, vector, num, this.GetSwipe(this.fingers[this.twoFinger0].complexStartPosition, this.fingers[this.twoFinger0].position), 0f, Vector2.zero, 0f, Mathf.Abs(num2 - this.oldFingerDistance), num2);
							this.complexCurrentGesture = EasyTouch.GestureType.Pinch;
						}
					}
				}
				if (this.enableTwist)
				{
					if (Mathf.Abs(this.TwistAngle()) > this.minTwistAngle)
					{
						if (this.complexCurrentGesture != EasyTouch.GestureType.Twist)
						{
							this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, vector, num, false, num2);
							this.startTimeAction = Time.realtimeSinceStartup;
						}
						this.complexCurrentGesture = EasyTouch.GestureType.Twist;
					}
					if (this.complexCurrentGesture == EasyTouch.GestureType.Twist)
					{
						this.CreateGesture2Finger(EasyTouch.EventName.On_Twist, this.startPosition2Finger, zero, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, this.TwistAngle(), 0f, num2);
					}
					this.fingers[this.twoFinger0].oldPosition = this.fingers[this.twoFinger0].position;
					this.fingers[this.twoFinger1].oldPosition = this.fingers[this.twoFinger1].position;
				}
				if (num3 > 0f)
				{
					if (this.pickObject2Finger && !this.twoFingerDragStart)
					{
						if (this.complexCurrentGesture != EasyTouch.GestureType.Tap)
						{
							this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, vector, num, false, num2);
							this.startTimeAction = Time.realtimeSinceStartup;
						}
						this.CreateGesture2Finger(EasyTouch.EventName.On_DragStart2Fingers, this.startPosition2Finger, zero, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, num2);
						this.twoFingerDragStart = true;
					}
					else if (!this.pickObject2Finger && !this.twoFingerSwipeStart)
					{
						if (this.complexCurrentGesture != EasyTouch.GestureType.Tap)
						{
							this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, vector, num, false, num2);
							this.startTimeAction = Time.realtimeSinceStartup;
						}
						this.CreateGesture2Finger(EasyTouch.EventName.On_SwipeStart2Fingers, this.startPosition2Finger, zero, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, num2);
						this.twoFingerSwipeStart = true;
					}
				}
				else if (num3 < 0f)
				{
					this.twoFingerDragStart = false;
					this.twoFingerSwipeStart = false;
				}
				if (this.twoFingerDragStart)
				{
					this.CreateGesture2Finger(EasyTouch.EventName.On_Drag2Fingers, this.startPosition2Finger, zero, vector, num, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, vector, 0f, 0f, num2);
				}
				if (this.twoFingerSwipeStart)
				{
					this.CreateGesture2Finger(EasyTouch.EventName.On_Swipe2Fingers, this.startPosition2Finger, zero, vector, num, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, vector, 0f, 0f, num2);
				}
			}
			else if (this.complexCurrentGesture == EasyTouch.GestureType.LongTap)
			{
				this.CreateGesture2Finger(EasyTouch.EventName.On_LongTap2Fingers, this.startPosition2Finger, zero, vector, num, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, num2);
			}
			this.CreateGesture2Finger(EasyTouch.EventName.On_TouchDown2Fingers, this.startPosition2Finger, zero, vector, num, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, vector, 0f, 0f, num2);
			this.oldFingerDistance = num2;
			this.oldStartPosition2Finger = zero;
			this.oldGesture = this.complexCurrentGesture;
		}
		else
		{
			this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, vector, num, true, num2);
			this.complexCurrentGesture = EasyTouch.GestureType.None;
			this.pickObject2Finger = null;
			this.twoFingerSwipeStart = false;
			this.twoFingerDragStart = false;
		}
	}

	private int GetTwoFinger(int index)
	{
		int num = index + 1;
		bool flag = false;
		while (num < 10 && !flag)
		{
			if (this.fingers[num] != null && num >= index)
			{
				flag = true;
			}
			num++;
		}
		return num - 1;
	}

	private void CreateStateEnd2Fingers(EasyTouch.GestureType gesture, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float time, bool realEnd, float fingerDistance)
	{
		switch (gesture)
		{
		case EasyTouch.GestureType.LongTap:
			this.CreateGesture2Finger(EasyTouch.EventName.On_LongTapEnd2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
			break;
		case EasyTouch.GestureType.Pinch:
			this.CreateGesture2Finger(EasyTouch.EventName.On_PinchEnd, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
			break;
		case EasyTouch.GestureType.Twist:
			this.CreateGesture2Finger(EasyTouch.EventName.On_TwistEnd, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
			break;
		default:
			if (gesture == EasyTouch.GestureType.Tap)
			{
				if (this.fingers[this.twoFinger0].tapCount < 2 && this.fingers[this.twoFinger1].tapCount < 2)
				{
					this.CreateGesture2Finger(EasyTouch.EventName.On_SimpleTap2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				}
				else
				{
					this.CreateGesture2Finger(EasyTouch.EventName.On_DoubleTap2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
				}
			}
			break;
		}
		if (realEnd)
		{
			if (this.twoFingerDragStart)
			{
				this.CreateGesture2Finger(EasyTouch.EventName.On_DragEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
			}
			if (this.twoFingerSwipeStart)
			{
				this.CreateGesture2Finger(EasyTouch.EventName.On_SwipeEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0f, 0f, fingerDistance);
			}
			this.CreateGesture2Finger(EasyTouch.EventName.On_TouchUp2Fingers, startPosition, position, deltaPosition, time, EasyTouch.SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
		}
	}

	private void CreateGesture2Finger(EasyTouch.EventName message, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float actionTime, EasyTouch.SwipeType swipe, float swipeLength, Vector2 swipeVector, float twist, float pinch, float twoDistance)
	{
		if (message == EasyTouch.EventName.On_TouchStart2Fingers)
		{
			this.isStartHoverNGUI = (this.IsTouchHoverNGui(this.twoFinger1) & this.IsTouchHoverNGui(this.twoFinger0));
		}
		if (!this.isStartHoverNGUI)
		{
			Gesture gesture = new Gesture();
			gesture.touchCount = 2;
			gesture.fingerIndex = -1;
			gesture.startPosition = startPosition;
			gesture.position = position;
			gesture.deltaPosition = deltaPosition;
			gesture.actionTime = actionTime;
			if (this.fingers[this.twoFinger0] != null)
			{
				gesture.deltaTime = this.fingers[this.twoFinger0].deltaTime;
			}
			else if (this.fingers[this.twoFinger1] != null)
			{
				gesture.deltaTime = this.fingers[this.twoFinger1].deltaTime;
			}
			else
			{
				gesture.deltaTime = 0f;
			}
			gesture.swipe = swipe;
			gesture.swipeLength = swipeLength;
			gesture.swipeVector = swipeVector;
			gesture.deltaPinch = pinch;
			gesture.twistAngle = twist;
			gesture.twoFingerDistance = twoDistance;
			if (this.fingers[this.twoFinger0] != null)
			{
				gesture.pickCamera = this.fingers[this.twoFinger0].pickedCamera;
				gesture.isGuiCamera = this.fingers[this.twoFinger0].isGuiCamera;
			}
			else if (this.fingers[this.twoFinger1] != null)
			{
				gesture.pickCamera = this.fingers[this.twoFinger1].pickedCamera;
				gesture.isGuiCamera = this.fingers[this.twoFinger1].isGuiCamera;
			}
			if (message != EasyTouch.EventName.On_Cancel2Fingers)
			{
				gesture.pickObject = this.pickObject2Finger;
			}
			else
			{
				gesture.pickObject = this.oldPickObject2Finger;
			}
			gesture.otherReceiver = this.receiverObject;
			if (this.fingers[this.twoFinger0] != null)
			{
				gesture.isHoverReservedArea = this.IsTouchReservedArea(this.fingers[this.twoFinger0].fingerIndex);
			}
			if (this.fingers[this.twoFinger1] != null)
			{
				gesture.isHoverReservedArea = (gesture.isHoverReservedArea || this.IsTouchReservedArea(this.fingers[this.twoFinger1].fingerIndex));
			}
			if (this.useBroadcastMessage)
			{
				this.SendGesture2Finger(message, gesture);
			}
			else
			{
				this.RaiseEvent(message, gesture);
			}
		}
	}

	private void SendGesture2Finger(EasyTouch.EventName message, Gesture gesture)
	{
		if (this.receiverObject != null && this.receiverObject != gesture.pickObject)
		{
			this.receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
		}
		if (gesture.pickObject != null)
		{
			gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			base.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void RaiseReadyEvent()
	{
		if (this.useBroadcastMessage)
		{
			if (this.receiverObject != null)
			{
				base.gameObject.SendMessage("On_EasyTouchIsReady", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (EasyTouch.On_EasyTouchIsReady != null)
		{
			EasyTouch.On_EasyTouchIsReady();
		}
	}

	private void RaiseEvent(EasyTouch.EventName evnt, Gesture gesture)
	{
		switch (evnt)
		{
		case EasyTouch.EventName.On_Cancel:
			if (EasyTouch.On_Cancel != null)
			{
				EasyTouch.On_Cancel(gesture);
			}
			break;
		case EasyTouch.EventName.On_Cancel2Fingers:
			if (EasyTouch.On_Cancel2Fingers != null)
			{
				EasyTouch.On_Cancel2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchStart:
			if (EasyTouch.On_TouchStart != null)
			{
				EasyTouch.On_TouchStart(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchDown:
			if (EasyTouch.On_TouchDown != null)
			{
				EasyTouch.On_TouchDown(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchUp:
			if (EasyTouch.On_TouchUp != null)
			{
				EasyTouch.On_TouchUp(gesture);
			}
			break;
		case EasyTouch.EventName.On_SimpleTap:
			if (EasyTouch.On_SimpleTap != null)
			{
				EasyTouch.On_SimpleTap(gesture);
			}
			break;
		case EasyTouch.EventName.On_DoubleTap:
			if (EasyTouch.On_DoubleTap != null)
			{
				EasyTouch.On_DoubleTap(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTapStart:
			if (EasyTouch.On_LongTapStart != null)
			{
				EasyTouch.On_LongTapStart(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTap:
			if (EasyTouch.On_LongTap != null)
			{
				EasyTouch.On_LongTap(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTapEnd:
			if (EasyTouch.On_LongTapEnd != null)
			{
				EasyTouch.On_LongTapEnd(gesture);
			}
			break;
		case EasyTouch.EventName.On_DragStart:
			if (EasyTouch.On_DragStart != null)
			{
				EasyTouch.On_DragStart(gesture);
			}
			break;
		case EasyTouch.EventName.On_Drag:
			if (EasyTouch.On_Drag != null)
			{
				EasyTouch.On_Drag(gesture);
			}
			break;
		case EasyTouch.EventName.On_DragEnd:
			if (EasyTouch.On_DragEnd != null)
			{
				EasyTouch.On_DragEnd(gesture);
			}
			break;
		case EasyTouch.EventName.On_SwipeStart:
			if (EasyTouch.On_SwipeStart != null)
			{
				EasyTouch.On_SwipeStart(gesture);
			}
			break;
		case EasyTouch.EventName.On_Swipe:
			if (EasyTouch.On_Swipe != null)
			{
				EasyTouch.On_Swipe(gesture);
			}
			break;
		case EasyTouch.EventName.On_SwipeEnd:
			if (EasyTouch.On_SwipeEnd != null)
			{
				EasyTouch.On_SwipeEnd(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchStart2Fingers:
			if (EasyTouch.On_TouchStart2Fingers != null)
			{
				EasyTouch.On_TouchStart2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchDown2Fingers:
			if (EasyTouch.On_TouchDown2Fingers != null)
			{
				EasyTouch.On_TouchDown2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_TouchUp2Fingers:
			if (EasyTouch.On_TouchUp2Fingers != null)
			{
				EasyTouch.On_TouchUp2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_SimpleTap2Fingers:
			if (EasyTouch.On_SimpleTap2Fingers != null)
			{
				EasyTouch.On_SimpleTap2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_DoubleTap2Fingers:
			if (EasyTouch.On_DoubleTap2Fingers != null)
			{
				EasyTouch.On_DoubleTap2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTapStart2Fingers:
			if (EasyTouch.On_LongTapStart2Fingers != null)
			{
				EasyTouch.On_LongTapStart2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTap2Fingers:
			if (EasyTouch.On_LongTap2Fingers != null)
			{
				EasyTouch.On_LongTap2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_LongTapEnd2Fingers:
			if (EasyTouch.On_LongTapEnd2Fingers != null)
			{
				EasyTouch.On_LongTapEnd2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_Twist:
			if (EasyTouch.On_Twist != null)
			{
				EasyTouch.On_Twist(gesture);
			}
			break;
		case EasyTouch.EventName.On_TwistEnd:
			if (EasyTouch.On_TwistEnd != null)
			{
				EasyTouch.On_TwistEnd(gesture);
			}
			break;
		case EasyTouch.EventName.On_PinchIn:
			if (EasyTouch.On_PinchIn != null)
			{
				EasyTouch.On_PinchIn(gesture);
			}
			break;
		case EasyTouch.EventName.On_PinchOut:
			if (EasyTouch.On_PinchOut != null)
			{
				EasyTouch.On_PinchOut(gesture);
			}
			break;
		case EasyTouch.EventName.On_PinchEnd:
			if (EasyTouch.On_PinchEnd != null)
			{
				EasyTouch.On_PinchEnd(gesture);
			}
			break;
		case EasyTouch.EventName.On_DragStart2Fingers:
			if (EasyTouch.On_DragStart2Fingers != null)
			{
				EasyTouch.On_DragStart2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_Drag2Fingers:
			if (EasyTouch.On_Drag2Fingers != null)
			{
				EasyTouch.On_Drag2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_DragEnd2Fingers:
			if (EasyTouch.On_DragEnd2Fingers != null)
			{
				EasyTouch.On_DragEnd2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_SwipeStart2Fingers:
			if (EasyTouch.On_SwipeStart2Fingers != null)
			{
				EasyTouch.On_SwipeStart2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_Swipe2Fingers:
			if (EasyTouch.On_Swipe2Fingers != null)
			{
				EasyTouch.On_Swipe2Fingers(gesture);
			}
			break;
		case EasyTouch.EventName.On_SwipeEnd2Fingers:
			if (EasyTouch.On_SwipeEnd2Fingers != null)
			{
				EasyTouch.On_SwipeEnd2Fingers(gesture);
			}
			break;
		}
	}

	private bool GetPickeGameObject(ref Finger finger, bool twoFinger = false)
	{
		finger.isGuiCamera = false;
		finger.pickedCamera = null;
		finger.pickedObject = null;
		if (this.touchCameras.Count > 0)
		{
			for (int i = 0; i < this.touchCameras.Count; i++)
			{
				if (this.touchCameras[i].camera != null && this.touchCameras[i].camera.enabled)
				{
					Vector2 v = Vector2.zero;
					if (!twoFinger)
					{
						v = finger.position;
					}
					else
					{
						v = finger.complexStartPosition;
					}
					Ray ray = this.touchCameras[i].camera.ScreenPointToRay(v);
					if (this.enable2D)
					{
						LayerMask mask = this.pickableLayers2D;
						RaycastHit2D[] array = new RaycastHit2D[1];
						if (Physics2D.GetRayIntersectionNonAlloc(ray, array, float.PositiveInfinity, mask) > 0)
						{
							finger.pickedCamera = this.touchCameras[i].camera;
							finger.isGuiCamera = this.touchCameras[i].guiCamera;
							finger.pickedObject = array[0].collider.gameObject;
							return true;
						}
					}
					LayerMask mask2 = this.pickableLayers;
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit, 3.40282347E+38f, mask2))
					{
						finger.pickedCamera = this.touchCameras[i].camera;
						finger.isGuiCamera = this.touchCameras[i].guiCamera;
						finger.pickedObject = raycastHit.collider.gameObject;
						return true;
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("No camera is assigned to EasyTouch");
		}
		return false;
	}

	private EasyTouch.SwipeType GetSwipe(Vector2 start, Vector2 end)
	{
		Vector2 normalized = (end - start).normalized;
		if (Mathf.Abs(normalized.y) > Mathf.Abs(normalized.x))
		{
			if (Vector2.Dot(normalized, Vector2.up) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeType.Up;
			}
			if (Vector2.Dot(normalized, -Vector2.up) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeType.Down;
			}
		}
		else
		{
			if (Vector2.Dot(normalized, Vector2.right) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeType.Right;
			}
			if (Vector2.Dot(normalized, -Vector2.right) >= this.swipeTolerance)
			{
				return EasyTouch.SwipeType.Left;
			}
		}
		return EasyTouch.SwipeType.Other;
	}

	private bool FingerInTolerance(Finger finger)
	{
		return (finger.position - finger.startPosition).sqrMagnitude <= this.StationnaryTolerance * this.StationnaryTolerance;
	}

	private float DeltaAngle(Vector2 start, Vector2 end)
	{
		float y = start.x * end.y - start.y * end.x;
		return Mathf.Atan2(y, Vector2.Dot(start, end));
	}

	private float TwistAngle()
	{
		Vector2 end = this.fingers[this.twoFinger0].position - this.fingers[this.twoFinger1].position;
		Vector2 start = this.fingers[this.twoFinger0].oldPosition - this.fingers[this.twoFinger1].oldPosition;
		return 57.29578f * this.DeltaAngle(start, end);
	}

	private bool IsTouchHoverNGui(int touchIndex)
	{
		bool flag = false;
		if (this.enabledNGuiMode)
		{
			LayerMask mask = this.nGUILayers;
			int num = 0;
			while (!flag && num < this.nGUICameras.Count)
			{
				Ray ray = this.nGUICameras[num].ScreenPointToRay(this.fingers[touchIndex].position);
				RaycastHit raycastHit;
				flag = Physics.Raycast(ray, out raycastHit, float.MaxValue, mask);
				num++;
			}
		}
		return flag;
	}

	private bool IsTouchReservedArea(int touchIndex)
	{
		bool flag = false;
		if (this.enableReservedArea && this.fingers[touchIndex] != null)
		{
			int num = 0;
			Rect realRect = new Rect(0f, 0f, 0f, 0f);
			while (!flag && num < this.reservedAreas.Count)
			{
				flag = this.reservedAreas[num].Contains(this.fingers[touchIndex].position);
				num++;
			}
			num = 0;
			while (!flag && num < this.reservedGuiAreas.Count)
			{
				realRect = new Rect(this.reservedGuiAreas[num].x, (float)Screen.height - this.reservedGuiAreas[num].y - this.reservedGuiAreas[num].height, this.reservedGuiAreas[num].width, this.reservedGuiAreas[num].height);
				flag = realRect.Contains(this.fingers[touchIndex].position);
				num++;
			}
			num = 0;
			while (!flag && num < this.reservedVirtualAreas.Count)
			{
				realRect = VirtualScreen.GetRealRect(this.reservedVirtualAreas[num]);
				realRect = new Rect(realRect.x, (float)Screen.height - realRect.y - realRect.height, realRect.width, realRect.height);
				flag = realRect.Contains(this.fingers[touchIndex].position);
				num++;
			}
		}
		return flag;
	}

	private Finger GetFinger(int finderId)
	{
		int num = 0;
		Finger finger = null;
		while (num < 10 && finger == null)
		{
			if (this.fingers[num] != null && this.fingers[num].fingerIndex == finderId)
			{
				finger = this.fingers[num];
			}
			num++;
		}
		return finger;
	}

	public static void SetEnabled(bool enable)
	{
		EasyTouch.instance.enable = enable;
		if (enable)
		{
			EasyTouch.instance.ResetTouches();
		}
	}

	public static bool GetEnabled()
	{
		return EasyTouch.instance.enable;
	}

	public static int GetTouchCount()
	{
		if (EasyTouch.instance != null)
		{
			return EasyTouch.instance.input.TouchCount();
		}
		return 0;
	}

	public static void SetCamera(Camera cam, bool guiCam = false)
	{
		EasyTouch.instance.touchCameras.Add(new ECamera(cam, guiCam));
	}

	public static Camera GetCamera(int index = 0)
	{
		if (index < EasyTouch.instance.touchCameras.Count)
		{
			return EasyTouch.instance.touchCameras[index].camera;
		}
		return null;
	}

	public static void SetEnable2FingersGesture(bool enable)
	{
		EasyTouch.instance.enable2FingersGesture = enable;
	}

	public static bool GetEnable2FingersGesture()
	{
		return EasyTouch.instance.enable2FingersGesture;
	}

	public static void SetEnableTwist(bool enable)
	{
		EasyTouch.instance.enableTwist = enable;
	}

	public static bool GetEnableTwist()
	{
		return EasyTouch.instance.enableTwist;
	}

	public static void SetEnablePinch(bool enable)
	{
		EasyTouch.instance.enablePinch = enable;
	}

	public static bool GetEnablePinch()
	{
		return EasyTouch.instance.enablePinch;
	}

	public static void SetEnableAutoSelect(bool enable)
	{
		EasyTouch.instance.autoSelect = enable;
	}

	public static bool GetEnableAutoSelect()
	{
		return EasyTouch.instance.autoSelect;
	}

	public static void SetOtherReceiverObject(GameObject receiver)
	{
		EasyTouch.instance.receiverObject = receiver;
	}

	public static GameObject GetOtherReceiverObject()
	{
		return EasyTouch.instance.receiverObject;
	}

	public static void SetStationnaryTolerance(float tolerance)
	{
		EasyTouch.instance.StationnaryTolerance = tolerance;
	}

	public static float GetStationnaryTolerance()
	{
		return EasyTouch.instance.StationnaryTolerance;
	}

	public static void SetlongTapTime(float time)
	{
		EasyTouch.instance.longTapTime = time;
	}

	public static float GetlongTapTime()
	{
		return EasyTouch.instance.longTapTime;
	}

	public static void SetSwipeTolerance(float tolerance)
	{
		EasyTouch.instance.swipeTolerance = tolerance;
	}

	public static float GetSwipeTolerance()
	{
		return EasyTouch.instance.swipeTolerance;
	}

	public static void SetMinPinchLength(float length)
	{
		EasyTouch.instance.minPinchLength = length;
	}

	public static float GetMinPinchLength()
	{
		return EasyTouch.instance.minPinchLength;
	}

	public static void SetMinTwistAngle(float angle)
	{
		EasyTouch.instance.minTwistAngle = angle;
	}

	public static float GetMinTwistAngle()
	{
		return EasyTouch.instance.minTwistAngle;
	}

	public static GameObject GetCurrentPickedObject(int fingerIndex)
	{
		Finger finger = EasyTouch.instance.GetFinger(fingerIndex);
		if (EasyTouch.instance.GetPickeGameObject(ref finger, false))
		{
			return finger.pickedObject;
		}
		return null;
	}

	public static bool IsRectUnderTouch(Rect rect, bool guiRect = false)
	{
		bool flag = false;
		for (int i = 0; i < 10; i++)
		{
			if (EasyTouch.instance.fingers[i] != null)
			{
				if (guiRect)
				{
					rect = new Rect(rect.x, (float)Screen.height - rect.y - rect.height, rect.width, rect.height);
				}
				flag = rect.Contains(EasyTouch.instance.fingers[i].position);
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	public static Vector2 GetFingerPosition(int fingerIndex)
	{
		if (EasyTouch.instance.fingers[fingerIndex] != null)
		{
			return EasyTouch.instance.GetFinger(fingerIndex).position;
		}
		return Vector2.zero;
	}

	public static bool GetIsReservedArea()
	{
		return EasyTouch.instance && EasyTouch.instance.enableReservedArea;
	}

	public static void SetIsReservedArea(bool enable)
	{
		EasyTouch.instance.enableReservedArea = enable;
	}

	public static void AddReservedArea(Rect rec)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.reservedAreas.Add(rec);
		}
	}

	public static void AddReservedGuiArea(Rect rec)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.reservedGuiAreas.Add(rec);
		}
	}

	public static void RemoveReservedArea(Rect rec)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.reservedAreas.Remove(rec);
		}
	}

	public static void RemoveReservedGuiArea(Rect rec)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.reservedGuiAreas.Remove(rec);
		}
	}

	public static void ResetTouch(int fingerIndex)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.GetFinger(fingerIndex).gesture = EasyTouch.GestureType.None;
		}
	}

	public static void SetPickableLayer(LayerMask mask)
	{
		if (EasyTouch.instance)
		{
			EasyTouch.instance.pickableLayers = mask;
		}
	}

	public static LayerMask GetPickableLayer()
	{
		return EasyTouch.instance.pickableLayers;
	}

	public static EasyTouch instance;

	public bool enable = true;

	public bool enableRemote;

	public bool useBroadcastMessage = true;

	public GameObject receiverObject;

	public bool isExtension;

	public bool enable2FingersGesture = true;

	public bool enableTwist = true;

	public bool enablePinch = true;

	public List<ECamera> touchCameras = new List<ECamera>();

	public bool autoSelect;

	public LayerMask pickableLayers;

	public bool enable2D;

	public LayerMask pickableLayers2D;

	public float StationnaryTolerance = 25f;

	public float longTapTime = 1f;

	public float swipeTolerance = 0.85f;

	public float minPinchLength;

	public float minTwistAngle = 1f;

	public bool enabledNGuiMode;

	public LayerMask nGUILayers;

	public List<Camera> nGUICameras = new List<Camera>();

	private bool isStartHoverNGUI;

	public List<Rect> reservedAreas = new List<Rect>();

	public List<Rect> reservedVirtualAreas = new List<Rect>();

	public List<Rect> reservedGuiAreas = new List<Rect>();

	public bool enableReservedArea = true;

	public KeyCode twistKey = KeyCode.LeftAlt;

	public KeyCode swipeKey = KeyCode.LeftControl;

	public bool showGeneral = true;

	public bool showSelect = true;

	public bool showGesture = true;

	public bool showTwoFinger = true;

	public bool showSecondFinger = true;

	private EasyTouchInput input;

	private EasyTouch.GestureType complexCurrentGesture = EasyTouch.GestureType.None;

	private EasyTouch.GestureType oldGesture = EasyTouch.GestureType.None;

	private float startTimeAction;

	private Finger[] fingers = new Finger[10];

	private GameObject pickObject2Finger;

	private GameObject oldPickObject2Finger;

	public Texture secondFingerTexture;

	private Vector2 startPosition2Finger;

	private int twoFinger0;

	private int twoFinger1;

	private Vector2 oldStartPosition2Finger;

	private float oldFingerDistance;

	private bool twoFingerDragStart;

	private bool twoFingerSwipeStart;

	private int oldTouchCount;

	public delegate void TouchCancelHandler(Gesture gesture);

	public delegate void Cancel2FingersHandler(Gesture gesture);

	public delegate void TouchStartHandler(Gesture gesture);

	public delegate void TouchDownHandler(Gesture gesture);

	public delegate void TouchUpHandler(Gesture gesture);

	public delegate void SimpleTapHandler(Gesture gesture);

	public delegate void DoubleTapHandler(Gesture gesture);

	public delegate void LongTapStartHandler(Gesture gesture);

	public delegate void LongTapHandler(Gesture gesture);

	public delegate void LongTapEndHandler(Gesture gesture);

	public delegate void DragStartHandler(Gesture gesture);

	public delegate void DragHandler(Gesture gesture);

	public delegate void DragEndHandler(Gesture gesture);

	public delegate void SwipeStartHandler(Gesture gesture);

	public delegate void SwipeHandler(Gesture gesture);

	public delegate void SwipeEndHandler(Gesture gesture);

	public delegate void TouchStart2FingersHandler(Gesture gesture);

	public delegate void TouchDown2FingersHandler(Gesture gesture);

	public delegate void TouchUp2FingersHandler(Gesture gesture);

	public delegate void SimpleTap2FingersHandler(Gesture gesture);

	public delegate void DoubleTap2FingersHandler(Gesture gesture);

	public delegate void LongTapStart2FingersHandler(Gesture gesture);

	public delegate void LongTap2FingersHandler(Gesture gesture);

	public delegate void LongTapEnd2FingersHandler(Gesture gesture);

	public delegate void TwistHandler(Gesture gesture);

	public delegate void TwistEndHandler(Gesture gesture);

	public delegate void PinchInHandler(Gesture gesture);

	public delegate void PinchOutHandler(Gesture gesture);

	public delegate void PinchEndHandler(Gesture gesture);

	public delegate void DragStart2FingersHandler(Gesture gesture);

	public delegate void Drag2FingersHandler(Gesture gesture);

	public delegate void DragEnd2FingersHandler(Gesture gesture);

	public delegate void SwipeStart2FingersHandler(Gesture gesture);

	public delegate void Swipe2FingersHandler(Gesture gesture);

	public delegate void SwipeEnd2FingersHandler(Gesture gesture);

	public delegate void EasyTouchIsReadyHandler();

	public enum GestureType
	{
		Tap,
		Drag,
		Swipe,
		None,
		LongTap,
		Pinch,
		Twist,
		Cancel,
		Acquisition
	}

	public enum SwipeType
	{
		None,
		Left,
		Right,
		Up,
		Down,
		Other
	}

	private enum EventName
	{
		None,
		On_Cancel,
		On_Cancel2Fingers,
		On_TouchStart,
		On_TouchDown,
		On_TouchUp,
		On_SimpleTap,
		On_DoubleTap,
		On_LongTapStart,
		On_LongTap,
		On_LongTapEnd,
		On_DragStart,
		On_Drag,
		On_DragEnd,
		On_SwipeStart,
		On_Swipe,
		On_SwipeEnd,
		On_TouchStart2Fingers,
		On_TouchDown2Fingers,
		On_TouchUp2Fingers,
		On_SimpleTap2Fingers,
		On_DoubleTap2Fingers,
		On_LongTapStart2Fingers,
		On_LongTap2Fingers,
		On_LongTapEnd2Fingers,
		On_Twist,
		On_TwistEnd,
		On_PinchIn,
		On_PinchOut,
		On_PinchEnd,
		On_DragStart2Fingers,
		On_Drag2Fingers,
		On_DragEnd2Fingers,
		On_SwipeStart2Fingers,
		On_Swipe2Fingers,
		On_SwipeEnd2Fingers,
		On_EasyTouchIsReady
	}
}
