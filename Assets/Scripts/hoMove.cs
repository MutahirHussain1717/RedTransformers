using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

[AddComponentMenu("Simple Waypoint System/hoMove")]
public class hoMove : MonoBehaviour
{
	internal void Start()
	{
		if (!this.anim)
		{
			this.anim = base.gameObject.GetComponentInChildren<Animation>();
		}
		if (this.onStart)
		{
			this.StartMove();
		}
	}

	internal void InitWaypoints()
	{
		this.wpPos = new Vector3[this.waypoints.Length];
		for (int i = 0; i < this.wpPos.Length; i++)
		{
			this.wpPos[i] = this.waypoints[i].position + new Vector3(0f, this.sizeToAdd, 0f);
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
		this.originSpeed = this.speed;
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
		base.StartCoroutine(this.Move());
	}

	internal IEnumerator Move()
	{
		if (this.moveToPath)
		{
			yield return base.StartCoroutine(this.MoveToPath());
		}
		else
		{
			this.InitWaypoints();
			base.transform.position = this.waypoints[this.currentPoint].position + new Vector3(0f, this.sizeToAdd, 0f);
			if (this.orientToPath && this.currentPoint < this.wpPos.Length - 1)
			{
				base.transform.LookAt(this.wpPos[this.currentPoint + 1]);
			}
		}
		if (this.looptype == hoMove.LoopType.random)
		{
			base.StartCoroutine(this.ReachedEnd());
		}
		else
		{
			this.CreateTween();
			base.StartCoroutine(this.NextWaypoint());
		}
		yield break;
	}

	internal IEnumerator MoveToPath()
	{
		int max = (this.waypoints.Length <= 4) ? this.waypoints.Length : 4;
		this.wpPos = new Vector3[max];
		for (int i = 1; i < max; i++)
		{
			this.wpPos[i] = this.waypoints[i - 1].position + new Vector3(0f, this.sizeToAdd, 0f);
		}
		this.wpPos[0] = base.transform.position;
		this.CreateTween();
		if (this.tween.isPaused)
		{
			this.tween.Play();
		}
		yield return base.StartCoroutine(this.tween.UsePartialPath(-1, 1).WaitForCompletion());
		this.moveToPath = false;
		this.tween.Kill();
		this.tween = null;
		this.InitWaypoints();
		yield break;
	}

	internal void CreateTween()
	{
		this.PlayWalk();
		this.plugPath = new PlugVector3Path(this.wpPos, true, this.pathtype);
		if (this.orientToPath || this.lockAxis != Axis.None)
		{
			this.plugPath.OrientToPath(this.lookAhead, this.lockAxis);
		}
		if (this.lockPosition != Axis.None)
		{
			this.plugPath.LockPosition(this.lockPosition);
		}
		if (this.closePath)
		{
			this.plugPath.ClosePath(true);
		}
		this.tParms = new TweenParms();
		if (this.local)
		{
			this.tParms.Prop("localPosition", this.plugPath);
		}
		else
		{
			this.tParms.Prop("position", this.plugPath);
		}
		this.tParms.AutoKill(false);
		this.tParms.Pause(true);
		this.tParms.Loops(1);
		if (this.timeValue == hoMove.TimeValue.speed)
		{
			this.tParms.SpeedBased();
			this.tParms.Ease(EaseType.Linear);
		}
		else
		{
			this.tParms.Ease(this.easetype);
		}
		this.tween = HOTween.To(base.transform, this.originSpeed, this.tParms);
		if (this.originSpeed != this.speed)
		{
			this.ChangeSpeed(this.speed);
		}
	}

	internal IEnumerator NextWaypoint()
	{
		for (int point = 0; point < this.wpPos.Length - 1; point++)
		{
			base.StartCoroutine(this.SendMessages());
			if (this.StopAtPoint[this.currentPoint] > 0f)
			{
				yield return base.StartCoroutine(this.WaitDelay());
			}
			while (this.waiting)
			{
				yield return null;
			}
			this.PlayWalk();
			this.tween.Play();
			yield return base.StartCoroutine(this.tween.UsePartialPath(point, point + 1).WaitForCompletion());
			if (this.repeat)
			{
				this.currentPoint--;
			}
			else if (this.looptype == hoMove.LoopType.random)
			{
				this.rndIndex++;
				this.currentPoint = this.rndArray[this.rndIndex];
			}
			else
			{
				this.currentPoint++;
			}
		}
		if (this.looptype != hoMove.LoopType.pingPong && this.looptype != hoMove.LoopType.random)
		{
			base.StartCoroutine(this.SendMessages());
			if (this.StopAtPoint[this.currentPoint] > 0f)
			{
				yield return base.StartCoroutine(this.WaitDelay());
			}
		}
		base.StartCoroutine(this.ReachedEnd());
		yield break;
	}

	internal IEnumerator WaitDelay()
	{
		this.tween.Pause();
		this.PlayIdle();
		float timer = Time.time + this.StopAtPoint[this.currentPoint];
		while (!this.waiting && Time.time < timer)
		{
			yield return null;
		}
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

	internal IEnumerator ReachedEnd()
	{
		switch (this.looptype)
		{
		case hoMove.LoopType.none:
			this.tween.Kill();
			this.tween = null;
			this.PlayIdle();
			yield break;
		case hoMove.LoopType.loop:
			if (this.closePath)
			{
				this.tween.Play();
				this.PlayWalk();
				yield return base.StartCoroutine(this.tween.UsePartialPath(this.currentPoint, -1).WaitForCompletion());
			}
			this.currentPoint = 0;
			break;
		case hoMove.LoopType.pingPong:
			this.tween.Kill();
			this.tween = null;
			if (!this.repeat)
			{
				this.repeat = true;
				for (int j = 0; j < this.wpPos.Length; j++)
				{
					this.wpPos[j] = this.waypoints[this.waypoints.Length - 1 - j].position + new Vector3(0f, this.sizeToAdd, 0f);
				}
			}
			else
			{
				this.InitWaypoints();
				this.repeat = false;
			}
			this.CreateTween();
			break;
		case hoMove.LoopType.random:
		{
			this.rndIndex = 0;
			this.InitWaypoints();
			if (this.tween != null)
			{
				this.tween.Kill();
				this.tween = null;
			}
			this.rndArray = new int[this.wpPos.Length];
			for (int k = 0; k < this.rndArray.Length; k++)
			{
				this.rndArray[k] = k;
			}
			int i = this.wpPos.Length;
			while (i > 1)
			{
				System.Random random = this.rand;
				int maxValue;
				i = (maxValue = i) - 1;
				int num = random.Next(maxValue);
				Vector3 vector = this.wpPos[i];
				this.wpPos[i] = this.wpPos[num];
				this.wpPos[num] = vector;
				int num2 = this.rndArray[i];
				this.rndArray[i] = this.rndArray[num];
				this.rndArray[num] = num2;
			}
			Vector3 first = this.wpPos[0];
			int rndFirst = this.rndArray[0];
			for (int l = 0; l < this.wpPos.Length; l++)
			{
				if (this.rndArray[l] == this.currentPoint)
				{
					this.rndArray[l] = rndFirst;
					this.wpPos[0] = this.wpPos[l];
					this.wpPos[l] = first;
				}
			}
			this.rndArray[0] = this.currentPoint;
			this.CreateTween();
			break;
		}
		}
		base.StartCoroutine(this.NextWaypoint());
		yield break;
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
		base.StopAllCoroutines();
		HOTween.Kill(base.transform);
		this.plugPath = null;
		this.tween = null;
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

	public void Pause()
	{
		this.waiting = true;
		HOTween.Pause(base.transform);
		this.PlayIdle();
	}

	public void Resume()
	{
		this.waiting = false;
		HOTween.Play(base.transform);
		this.PlayWalk();
	}

	public void ChangeSpeed(float value)
	{
		float timeScale;
		if (this.timeValue == hoMove.TimeValue.speed)
		{
			timeScale = value / this.originSpeed;
		}
		else
		{
			timeScale = this.originSpeed / value;
		}
		this.speed = value;
		this.tween.timeScale = timeScale;
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

	public PathType pathtype = PathType.Curved;

	public bool onStart;

	public bool moveToPath;

	public bool closePath;

	public bool orientToPath;

	public bool local;

	public float lookAhead;

	public float sizeToAdd;

	[HideInInspector]
	public float[] StopAtPoint;

	[HideInInspector]
	public List<MessageOptions> _messages = new List<MessageOptions>();

	public hoMove.TimeValue timeValue = hoMove.TimeValue.speed;

	public float speed = 5f;

	public EaseType easetype;

	public hoMove.LoopType looptype = hoMove.LoopType.loop;

	private Transform[] waypoints;

	[HideInInspector]
	public int currentPoint;

	private bool repeat;

	public Axis lockAxis;

	public Axis lockPosition;

	[HideInInspector]
	public Animation anim;

	public AnimationClip walkAnim;

	public AnimationClip idleAnim;

	public bool crossfade;

	public Tweener tween;

	private Vector3[] wpPos;

	private TweenParms tParms;

	private PlugVector3Path plugPath;

	private System.Random rand = new System.Random();

	private int[] rndArray;

	private int rndIndex;

	private bool waiting;

	private float originSpeed;

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
}
