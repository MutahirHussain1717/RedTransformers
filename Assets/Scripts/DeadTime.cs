using System;
using UnityEngine;

[Serializable]
public class DeadTime : MonoBehaviour
{
	public virtual void Awake()
	{
		UnityEngine.Object.Destroy(this.gameObject, this.deadTime);
	}

	public virtual void Main()
	{
	}

	public float deadTime;
}
