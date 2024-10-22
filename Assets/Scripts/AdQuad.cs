using System;
using AudienceNetwork;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AdQuad : MonoBehaviour
{
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		component.enabled = false;
		this.adRendered = false;
	}

	private void OnGUI()
	{
		NativeAd nativeAd = this.adManager.nativeAd;
		if (nativeAd && this.adManager.IsAdLoaded() && !this.adRendered)
		{
			Sprite sprite = null;
			if (this.useCoverImage)
			{
				sprite = nativeAd.CoverImage;
			}
			else if (this.useIconImage)
			{
				sprite = nativeAd.IconImage;
			}
			if (sprite)
			{
				MeshRenderer component = base.GetComponent<MeshRenderer>();
				Renderer component2 = base.GetComponent<Renderer>();
				component2.enabled = true;
				Texture2D texture = sprite.texture;
				Material material = new Material(Shader.Find("Sprites/Default"));
				material.color = Color.white;
				material.SetTexture("texture", texture);
				component.sharedMaterial = material;
				component2.material.mainTexture = texture;
				this.adRendered = true;
			}
		}
	}

	public AdManager adManager;

	public bool useIconImage;

	public bool useCoverImage;

	private bool adRendered;
}
