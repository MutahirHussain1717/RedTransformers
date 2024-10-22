using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private void Start()
	{
		this.playerDead = false;
		this.explode = true;
		this.currentHealth = this.maxHealth;
		if (base.GetComponent<AudioSource>())
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (this.healthProgress)
		{
			this.healthProgress.fillAmount = this.currentHealth / 100f;
		}
		if (this.currentHealth <= 0f && this.explode)
		{
			this.audioSource.PlayOneShot(this.deathSound);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particles, base.transform.position, base.transform.rotation);
			this.explode = false;
			this.playerDead = true;
		}
	}

	public float maxHealth;

	public float currentHealth;

	public Image healthProgress;

	private AudioSource audioSource;

	public AudioClip deathSound;

	public GameObject particles;

	private bool explode;

	public bool playerDead;
}
