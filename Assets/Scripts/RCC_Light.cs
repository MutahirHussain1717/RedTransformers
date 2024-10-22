using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light")]
public class RCC_Light : MonoBehaviour
{
	public AudioClip indicatorClip
	{
		get
		{
			return RCC_Settings.Instance.indicatorClip;
		}
	}

	private void Start()
	{
		this.carController = base.GetComponentInParent<RCC_CarControllerV3>();
		this._light = base.GetComponent<Light>();
		this._light.enabled = true;
		if (RCC_Settings.Instance.useLightProjectorForLightingEffect)
		{
			this.projector = base.GetComponent<Projector>();
			if (this.projector == null)
			{
				this.projector = UnityEngine.Object.Instantiate<GameObject>(RCC_Settings.Instance.projector, base.transform.position, base.transform.rotation).GetComponent<Projector>();
				this.projector.transform.SetParent(base.transform, true);
			}
			this.projector.ignoreLayers = RCC_Settings.Instance.projectorIgnoreLayer;
			if (this.lightType != RCC_Light.LightType.HeadLight)
			{
				this.projector.transform.localRotation = Quaternion.Euler(20f, (base.transform.localPosition.z <= 0f) ? 180f : 0f, 0f);
			}
			Material material = new Material(this.projector.material);
			this.projector.material = material;
		}
		if (RCC_Settings.Instance.useLightsAsVertexLights)
		{
			this._light.renderMode = LightRenderMode.ForceVertex;
			this._light.cullingMask = 0;
		}
		else
		{
			this._light.renderMode = LightRenderMode.ForcePixel;
		}
		if (this.lightType == RCC_Light.LightType.Indicator)
		{
			if (!this.carController.transform.Find("All Audio Sources/Indicator Sound AudioSource"))
			{
				this.indicatorSound = RCC_CreateAudioSource.NewAudioSource(this.carController.gameObject, "Indicator Sound AudioSource", 3f, 10f, 1f, this.indicatorClip, false, false, false);
			}
			else
			{
				this.indicatorSound = this.carController.transform.Find("All Audio Sources/Indicator Sound AudioSource").GetComponent<AudioSource>();
			}
		}
	}

	private void Update()
	{
		if (RCC_Settings.Instance.useLightProjectorForLightingEffect)
		{
			this.Projectors();
		}
		switch (this.lightType)
		{
		case RCC_Light.LightType.HeadLight:
			if (!this.carController.lowBeamHeadLightsOn && !this.carController.highBeamHeadLightsOn)
			{
				this.Lighting(0f);
			}
			if (this.carController.lowBeamHeadLightsOn && !this.carController.highBeamHeadLightsOn)
			{
				this.Lighting(0.6f, 50f, 90f);
				base.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
			}
			else if (this.carController.highBeamHeadLightsOn)
			{
				this.Lighting(1f, 200f, 45f);
				base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			break;
		case RCC_Light.LightType.BrakeLight:
			this.Lighting(this.carController.lowBeamHeadLightsOn ? Mathf.Clamp(this.carController._brakeInput * 10f, 0.3f, 1f) : Mathf.Clamp01(this.carController._brakeInput * 10f));
			break;
		case RCC_Light.LightType.ReverseLight:
			this.Lighting((this.carController.direction != -1) ? 0f : 1f);
			break;
		case RCC_Light.LightType.Indicator:
			this.indicatorsOn = this.carController.indicatorsOn;
			this.Indicators();
			break;
		}
	}

	private void Lighting(float input)
	{
		this._light.intensity = input;
	}

	private void Lighting(float input, float range, float spotAngle)
	{
		this._light.intensity = input;
		this._light.range = range;
		this._light.spotAngle = spotAngle;
	}

	private void Indicators()
	{
		switch (this.indicatorsOn)
		{
		case RCC_CarControllerV3.IndicatorsOn.Off:
			this._light.intensity = 0f;
			this.carController.indicatorTimer = 0f;
			break;
		case RCC_CarControllerV3.IndicatorsOn.Right:
			if (base.transform.localPosition.x < 0f)
			{
				this._light.intensity = 0f;
			}
			else
			{
				if (this.carController.indicatorTimer >= 0.5f)
				{
					this._light.intensity = 0f;
					if (this.indicatorSound.isPlaying)
					{
						this.indicatorSound.Stop();
					}
				}
				else
				{
					this._light.intensity = 1f;
					if (!this.indicatorSound.isPlaying && this.carController.indicatorTimer <= 0.05f)
					{
						this.indicatorSound.Play();
					}
				}
				if (this.carController.indicatorTimer >= 1f)
				{
					this.carController.indicatorTimer = 0f;
				}
			}
			break;
		case RCC_CarControllerV3.IndicatorsOn.Left:
			if (base.transform.localPosition.x > 0f)
			{
				this._light.intensity = 0f;
			}
			else
			{
				if (this.carController.indicatorTimer >= 0.5f)
				{
					this._light.intensity = 0f;
					if (this.indicatorSound.isPlaying)
					{
						this.indicatorSound.Stop();
					}
				}
				else
				{
					this._light.intensity = 1f;
					if (!this.indicatorSound.isPlaying && this.carController.indicatorTimer <= 0.05f)
					{
						this.indicatorSound.Play();
					}
				}
				if (this.carController.indicatorTimer >= 1f)
				{
					this.carController.indicatorTimer = 0f;
				}
			}
			break;
		case RCC_CarControllerV3.IndicatorsOn.All:
			if (this.carController.indicatorTimer >= 0.5f)
			{
				this._light.intensity = 0f;
				if (this.indicatorSound.isPlaying)
				{
					this.indicatorSound.Stop();
				}
			}
			else
			{
				this._light.intensity = 1f;
				if (!this.indicatorSound.isPlaying && this.carController.indicatorTimer <= 0.05f)
				{
					this.indicatorSound.Play();
				}
			}
			if (this.carController.indicatorTimer >= 1f)
			{
				this.carController.indicatorTimer = 0f;
			}
			break;
		}
	}

	private void Projectors()
	{
		if (!this._light.enabled)
		{
			this.projector.enabled = false;
			return;
		}
		this.projector.enabled = true;
		this.projector.material.color = this._light.color * this._light.intensity;
		this.projector.farClipPlane = Mathf.Lerp(10f, 40f, (this._light.range - 50f) / 150f);
		this.projector.fieldOfView = Mathf.Lerp(40f, 30f, (this._light.range - 50f) / 150f);
	}

	private RCC_CarControllerV3 carController;

	private Light _light;

	private Projector projector;

	public RCC_Light.LightType lightType;

	private RCC_CarControllerV3.IndicatorsOn indicatorsOn;

	private AudioSource indicatorSound;

	public enum LightType
	{
		HeadLight,
		BrakeLight,
		ReverseLight,
		Indicator
	}
}
