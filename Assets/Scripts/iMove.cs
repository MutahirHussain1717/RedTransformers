using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Simple Waypoint System/iMove")]
public class iMove : MonoBehaviour
{
	internal void Start()
	{
		if (!this.anim)
		{
			this.anim = base.gameObject.GetComponentInChildren<Animation>();
		}
		this.startRot = base.transform.localEulerAngles;
		if (this.onStart)
		{
			this.StartMove();
		}
	}

	public void StartMove()
	{
		if (this.pathContainer == null)
		{
			UnityEngine.Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
			return;
		}
		this.waypoints = this.pathContainer.waypoints;
		if (this.StopAtPoint == null)
		{
			this.StopAtPoint = new float[this.waypoints.Length];
		}
		else if (this.StopAtPoint.Length < this.waypoints.Length)
		{
			float[] array = new float[this.StopAtPoint.Length];
			Array.Copy(this.StopAtPoint, array, this.StopAtPoint.Length);
			this.StopAtPoint = new float[this.waypoints.Length];
			Array.Copy(array, this.StopAtPoint, array.Length);
		}
		if (this._messages.Count > 0)
		{
			this.InitializeMessageOptions();
		}
		if (!this.moveToPath)
		{
			base.transform.position = this.waypoints[this.currentPoint].position + new Vector3(0f, this.sizeToAdd, 0f);
			base.StartCoroutine("NextWaypoint");
			return;
		}
		this.Move(this.currentPoint);
	}

	internal void Move(int point)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", this.waypoints[point].position + new Vector3(0f, this.sizeToAdd, 0f));
		hashtable.Add("easetype", this.easetype);
		hashtable.Add("orienttopath", this.orientToPath);
		hashtable.Add("oncomplete", "NextWaypoint");
		if (this.orientToPath)
		{
			hashtable.Add("looktime", this.smoothRotation);
			if (this.lockAxis != iMove.AxisLock.none)
			{
				hashtable.Add("onupdate", "LockAxis");
			}
		}
		if (this.timeValue == iMove.TimeValue.time)
		{
			hashtable.Add("time", this.speed);
		}
		else
		{
			hashtable.Add("speed", this.speed);
		}
		iTween.MoveTo(base.gameObject, hashtable);
		this.PlayWalk();
	}

	internal void LockAxis()
	{
		Transform transform = base.transform;
		Vector3 localEulerAngles = transform.localEulerAngles;
		iMove.AxisLock axisLock = this.lockAxis;
		if (axisLock != iMove.AxisLock.X)
		{
			if (axisLock != iMove.AxisLock.Y)
			{
				if (axisLock == iMove.AxisLock.Z)
				{
					transform.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, this.startRot.z);
				}
			}
			else
			{
				transform.localEulerAngles = new Vector3(localEulerAngles.x, this.startRot.y, localEulerAngles.z);
			}
		}
		else
		{
			transform.localEulerAngles = new Vector3(this.startRot.x, localEulerAngles.y, localEulerAngles.z);
		}
	}

	internal IEnumerator NextWaypoint()
	{
		base.StartCoroutine(this.SendMessages());
		if (this.StopAtPoint[this.currentPoint] > 0f)
		{
			this.PlayIdle();
			yield return new WaitForSeconds(this.StopAtPoint[this.currentPoint]);
		}
		switch (this.looptype)
		{
		case iMove.LoopType.none:
			if (this.currentPoint >= this.waypoints.Length - 1)
			{
				this.PlayIdle();
				yield break;
			}
			this.currentPoint++;
			break;
		case iMove.LoopType.loop:
			if (this.currentPoint == this.waypoints.Length - 1)
			{
				this.currentPoint = 0;
				this.StartMove();
				yield break;
			}
			this.currentPoint++;
			break;
		case iMove.LoopType.pingPong:
			if (this.currentPoint == this.waypoints.Length - 1)
			{
				this.repeat = true;
			}
			else if (this.currentPoint == 0)
			{
				this.repeat = false;
			}
			if (this.repeat)
			{
				this.currentPoint--;
			}
			else
			{
				this.currentPoint++;
			}
			break;
		case iMove.LoopType.random:
		{
			int num = this.currentPoint;
			do
			{
				this.currentPoint = UnityEngine.Random.Range(0, this.waypoints.Length);
			}
			while (num == this.currentPoint);
			break;
		}
		}
		this.Move(this.currentPoint);
		yield break;
	}

	internal IEnumerator SendMessages()
	{
		if (this._messages.Count != this.waypoints.Length)
		{
			yield break;
		}
		for (int i = 0; i < this._messages[this.currentPoint].message.Count; i++)
		{
			if (!(this._messages[this.currentPoint].message[i] == string.Empty))
			{
				MessageOptions messageOptions = this._messages[this.currentPoint];
				switch (messageOptions.type[i])
				{
				case MessageOptions.ValueType.None:
					base.SendMessage(messageOptions.message[i], SendMessageOptions.DontRequireReceiver);
					break;
				case MessageOptions.ValueType.Object:
					base.SendMessage(messageOptions.message[i], messageOptions.obj[i], SendMessageOptions.DontRequireReceiver);
					break;
				case MessageOptions.ValueType.Text:
					base.SendMessage(messageOptions.message[i], messageOptions.text[i], SendMessageOptions.DontRequireReceiver);
					break;
				case MessageOptions.ValueType.Numeric:
					base.SendMessage(messageOptions.message[i], messageOptions.num[i], SendMessageOptions.DontRequireReceiver);
					break;
				case MessageOptions.ValueType.Vector2:
					base.SendMessage(messageOptions.message[i], messageOptions.vect2[i], SendMessageOptions.DontRequireReceiver);
					break;
				case MessageOptions.ValueType.Vector3:
					base.SendMessage(messageOptions.message[i], messageOptions.vect3[i], SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}
		yield break;
	}

	internal void InitializeMessageOptions()
	{
		if (this._messages.Count < this.waypoints.Length)
		{
			for (int i = this._messages.Count; i < this.waypoints.Length; i++)
			{
				MessageOptions item = this.AddMessageToOption(new MessageOptions());
				this._messages.Add(item);
			}
		}
		else if (this._messages.Count > this.waypoints.Length)
		{
			for (int j = this._messages.Count - 1; j >= this.waypoints.Length; j--)
			{
				this._messages.RemoveAt(j);
			}
		}
	}

	internal MessageOptions AddMessageToOption(MessageOptions opt)
	{
		opt.message.Add(string.Empty);
		opt.type.Add(MessageOptions.ValueType.None);
		opt.obj.Add(null);
		opt.text.Add(null);
		opt.num.Add(0f);
		opt.vect2.Add(Vector2.zero);
		opt.vect3.Add(Vector3.zero);
		return opt;
	}

	internal void PlayIdle()
	{
		if (this.idleAnim)
		{
			if (this.crossfade)
			{
				this.anim.CrossFade(this.idleAnim.name, 0.2f);
			}
			else
			{
				this.anim.Play(this.idleAnim.name);
			}
		}
	}

	internal void PlayWalk()
	{
		if (this.walkAnim)
		{
			if (this.crossfade)
			{
				this.anim.CrossFade(this.walkAnim.name, 0.2f);
			}
			else
			{
				this.anim.Play(this.walkAnim.name);
			}
		}
	}

	public void SetPath(PathManager newPath)
	{
		this.Stop();
		this.pathContainer = newPath;
		this.waypoints = this.pathContainer.waypoints;
		this.currentPoint = 0;
		this.StartMove();
	}

	public void Stop()
	{
		base.StopCoroutine("NextWaypoint");
		iTween.Stop(base.gameObject);
		this.PlayIdle();
	}

	public void Reset()
	{
		this.Stop();
		this.currentPoint = 0;
		if (this.pathContainer)
		{
			base.transform.position = this.waypoints[this.currentPoint].position + new Vector3(0f, this.sizeToAdd, 0f);
		}
	}

	public void ChangeSpeed(float value)
	{
		this.Stop();
		this.speed = value;
		this.StartMove();
	}

	public MessageOptions GetMessageOption(int waypointID, int messageID)
	{
		this.InitializeMessageOptions();
		MessageOptions messageOptions = this._messages[waypointID];
		for (int i = messageOptions.message.Count; i <= messageID; i++)
		{
			this.AddMessageToOption(messageOptions);
		}
		return messageOptions;
	}

	public PathManager pathContainer;

	public int currentPoint;

	public bool onStart;

	public bool moveToPath;

	public bool orientToPath;

	public float smoothRotation;

	public float sizeToAdd;

	[HideInInspector]
	public float[] StopAtPoint;

	[HideInInspector]
	public List<MessageOptions> _messages = new List<MessageOptions>();

	public iMove.TimeValue timeValue = iMove.TimeValue.speed;

	public float speed = 5f;

	public iTween.EaseType easetype = iTween.EaseType.linear;

	public iMove.LoopType looptype = iMove.LoopType.loop;

	private Transform[] waypoints;

	private bool repeat;

	public iMove.AxisLock lockAxis;

	private Vector3 startRot;

	[HideInInspector]
	public Animation anim;

	public AnimationClip walkAnim;

	public AnimationClip idleAnim;

	public bool crossfade;

	public enum TimeValue
	{
		time,
		speed
	}

	public enum LoopType
	{
		none,
		loop,
		pingPong,
		random
	}

	public enum AxisLock
	{
		none,
		X,
		Y,
		Z
	}
}
