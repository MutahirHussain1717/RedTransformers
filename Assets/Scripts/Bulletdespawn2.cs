using System;
using UnityEngine;

[Serializable]
public class Bulletdespawn2 : MonoBehaviour
{
	public virtual void Start()
	{
		this.player.PlayOneShot(this.explosionsound);
		UnityEngine.Object.Destroy(this.gameObject, (float)1);
	}

	public virtual void Update()
	{
	}

	public virtual void Main()
	{
	}

	public AudioClip explosionsound;

	public AudioSource player;
}
