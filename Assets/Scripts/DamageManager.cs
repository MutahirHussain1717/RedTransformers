using System;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
	private void Start()
	{
		this.checkonetime = true;
	}

	public void ApplyDamage(int damage)
	{
		if (this.HP < 0)
		{
			return;
		}
		if (this.HitSound.Length > 0)
		{
			AudioSource.PlayClipAtPoint(this.HitSound[UnityEngine.Random.Range(0, this.HitSound.Length)], base.transform.position);
		}
		this.HP -= damage;
		if (this.HP <= 0 && this.checkonetime)
		{
			this.Dead();
		}
	}

	private void OnEnable()
	{
		this.HP = 100;
		this.checkonetime = true;
	}

	private void Dead()
	{
		this.checkonetime = false;
		if (this.Effect)
		{
		}
		GameObject.FindWithTag("controls").GetComponent<RocketColisionChecker>().show_damgaecar(base.gameObject);
		base.gameObject.SetActive(false);
	}

	public AudioClip[] HitSound;

	public GameObject Effect;

	public int HP = 100;

	private bool checkonetime;
}
