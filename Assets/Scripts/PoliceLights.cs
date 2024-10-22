using System;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{
	private void Awake()
	{
		if (this.policeAudioClips.Length > 0)
		{
			this.policeAudioSource.clip = this.policeAudioClips[UnityEngine.Random.Range(0, this.policeAudioClips.Length)];
			this.policeAudioSource.Play();
		}
	}

	private void Update()
	{
		if (!this.activeLight)
		{
			if (!this.policeAudioSource.mute)
			{
				this.policeAudioSource.clip = this.policeAudioClips[UnityEngine.Random.Range(0, this.policeAudioClips.Length)];
				this.policeAudioSource.mute = true;
			}
			foreach (Light light in this.RedLights)
			{
				light.enabled = false;
			}
			foreach (Light light2 in this.BlueLights)
			{
				light2.enabled = false;
			}
			return;
		}
		this.timer = Mathf.MoveTowards(this.timer, 0f, Time.deltaTime * this.time);
		if (this.timer == 0f)
		{
			this.lightNum++;
			if (this.lightNum > 12)
			{
				this.lightNum = 1;
			}
			this.timer = 1f;
		}
		if (this.policeAudioSource)
		{
			this.policeAudioSource.mute = false;
			if (!this.policeAudioSource.isPlaying)
			{
				this.policeAudioSource.Play();
			}
		}
		if (this.lightNum == 1 || this.lightNum == 3)
		{
			foreach (Light light3 in this.RedLights)
			{
				light3.enabled = true;
			}
			foreach (Light light4 in this.BlueLights)
			{
				light4.enabled = false;
			}
		}
		if (this.lightNum == 5 || this.lightNum == 7)
		{
			foreach (Light light5 in this.BlueLights)
			{
				light5.enabled = true;
			}
			foreach (Light light6 in this.RedLights)
			{
				light6.enabled = false;
			}
		}
		if (this.lightNum == 2 || this.lightNum == 4 || this.lightNum == 6 || this.lightNum == 8)
		{
			foreach (Light light7 in this.BlueLights)
			{
				light7.enabled = false;
			}
			foreach (Light light8 in this.RedLights)
			{
				light8.enabled = false;
			}
		}
	}

	public bool activeLight = true;

	public float time = 20f;

	public AudioSource policeAudioSource;

	public AudioClip[] policeAudioClips;

	public Light[] RedLights;

	public Light[] BlueLights;

	private float timer;

	private int lightNum;
}
