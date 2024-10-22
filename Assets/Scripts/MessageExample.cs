using System;
using System.Collections;
using Holoville.HOTween;
using UnityEngine;

public class MessageExample : MonoBehaviour
{
	private void Start()
	{
		this.iScript = base.GetComponent<iMove>();
		this.hoScript = base.GetComponent<hoMove>();
		this.thisObject = base.transform;
	}

	private void PositionObject(UnityEngine.Object point)
	{
		GameObject gameObject = (GameObject)point;
		HOTween.To(this.thisObject, 1f, new TweenParms().Prop("position", gameObject.transform.position + new Vector3(0f, 10f, 0f)).Ease(EaseType.Linear).Loops(2, LoopType.Yoyo));
	}

	private void RotateObject(Vector3 rot)
	{
		HOTween.To(this.thisObject, 1f, new TweenParms().Prop("rotation", rot).Ease(EaseType.Linear).Loops(2, LoopType.Yoyo));
	}

	private void UpdatePoints()
	{
		this.hoScript.Stop();
		this.hoScript.currentPoint = 0;
		this.hoScript.moveToPath = true;
		this.hoScript.StartMove();
	}

	private IEnumerator StopAndResume(float seconds)
	{
		this.iScript.Stop();
		yield return new WaitForSeconds(seconds);
		this.iScript.StartMove();
		yield break;
	}

	private IEnumerator PauseAndResume(float seconds)
	{
		this.hoScript.Pause();
		yield return new WaitForSeconds(seconds);
		this.hoScript.Resume();
		yield break;
	}

	private void PrintProgress()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			base.gameObject.name,
			": I'm now at waypoint ",
			this.hoScript.currentPoint + 1,
			"."
		}));
	}

	private void PrintText(string text)
	{
		UnityEngine.Debug.Log(text);
	}

	private void Method1()
	{
	}

	private iMove iScript;

	private hoMove hoScript;

	private Transform thisObject;
}
