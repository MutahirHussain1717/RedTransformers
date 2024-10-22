using System;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light Emission")]
public class RCC_LightEmission : MonoBehaviour
{
	private void Start()
	{
		this.sharedLight = base.GetComponent<Light>();
		Material material = this.lightRenderer.materials[this.materialIndex];
		material.EnableKeyword("_EMISSION");
	}

	private void Update()
	{
		if (!this.sharedLight.enabled)
		{
			this.lightRenderer.materials[this.materialIndex].SetColor("_EmissionColor", Color.white * 0f);
			return;
		}
		if (!this.noTexture)
		{
			this.lightRenderer.materials[this.materialIndex].SetColor("_EmissionColor", Color.white * this.sharedLight.intensity);
		}
		else
		{
			this.lightRenderer.materials[this.materialIndex].SetColor("_EmissionColor", Color.red * this.sharedLight.intensity);
		}
	}

	private Light sharedLight;

	public Renderer lightRenderer;

	public int materialIndex;

	public bool noTexture;
}
