using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Exhaust")]
public class RCC_Exhaust : MonoBehaviour
{
	private void Start()
	{
		this.carController = base.GetComponentInParent<RCC_CarControllerV3>();
		this.particle = base.GetComponent<ParticleSystem>();
		this.emission = this.particle.emission;
		if (this.flame)
		{
			this.subEmission = this.flame.emission;
			this.flameLight = this.flame.GetComponentInChildren<Light>();
			this.flameSource = RCC_CreateAudioSource.NewAudioSource(base.gameObject, "Exhaust Flame AudioSource", 10f, 50f, 10f, RCC_Settings.Instance.exhaustFlameClips[0], false, false, false);
		}
	}

	private void Update()
	{
		if (!this.carController || !this.particle)
		{
			return;
		}
		if (this.carController.engineRunning)
		{
			if (this.carController.speed < 150f)
			{
				if (!this.emission.enabled)
				{
					this.emission.enabled = true;
				}
				if (this.carController._gasInput > 0.05f)
				{
					this.emissionRate.constantMax = 50f;
					this.emission.rate = this.emissionRate;
					this.particle.startSpeed = 5f;
					this.particle.startSize = 8f;
				}
				else
				{
					this.emissionRate.constantMax = 5f;
					this.emission.rate = this.emissionRate;
					this.particle.startSpeed = 0.5f;
					this.particle.startSize = 4f;
				}
			}
			else if (this.emission.enabled)
			{
				this.emission.enabled = false;
			}
			if (this.carController._gasInput >= 0.25f)
			{
				this.flameTime = 0f;
			}
			if ((this.carController.useExhaustFlame && this.carController.engineRPM >= 5000f && this.carController.engineRPM <= 5500f && this.carController._gasInput <= 0.25f && this.flameTime <= 0.5f) || this.carController._boostInput >= 1.5f)
			{
				this.flameTime += Time.deltaTime;
				this.subEmission.enabled = true;
				if (this.flameLight)
				{
					this.flameLight.intensity = this.flameSource.pitch * 3f * UnityEngine.Random.Range(0.25f, 1f);
				}
				if (this.carController._boostInput >= 1.5f && this.flame)
				{
					this.flame.startColor = this.boostFlameColor;
					this.flameLight.color = this.flame.startColor;
				}
				else
				{
					this.flame.startColor = this.flameColor;
					this.flameLight.color = this.flame.startColor;
				}
				if (!this.flameSource.isPlaying)
				{
					this.flameSource.clip = RCC_Settings.Instance.exhaustFlameClips[UnityEngine.Random.Range(0, RCC_Settings.Instance.exhaustFlameClips.Length)];
					this.flameSource.Play();
				}
			}
			else
			{
				this.subEmission.enabled = false;
				if (this.flameLight)
				{
					this.flameLight.intensity = 0f;
				}
				if (this.flameSource.isPlaying)
				{
					this.flameSource.Stop();
				}
			}
		}
		else
		{
			if (this.emission.enabled)
			{
				this.emission.enabled = false;
			}
			this.subEmission.enabled = false;
			if (this.flameLight)
			{
				this.flameLight.intensity = 0f;
			}
			if (this.flameSource.isPlaying)
			{
				this.flameSource.Stop();
			}
		}
	}

	private RCC_CarControllerV3 carController;

	private ParticleSystem particle;

	private ParticleSystem.EmissionModule emission;

	private ParticleSystem.MinMaxCurve emissionRate;

	public ParticleSystem flame;

	private ParticleSystem.EmissionModule subEmission;

	private ParticleSystem.MinMaxCurve subEmissionRate;

	private Light flameLight;

	public float flameTime;

	private AudioSource flameSource;

	public Color flameColor = Color.red;

	public Color boostFlameColor = Color.blue;
}
