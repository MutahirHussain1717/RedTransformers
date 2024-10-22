using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMenu : MonoBehaviour
{
	private void Start()
	{
		this.Element_Scale = 1.3f;
		this.Element_Margin = 36f;
		for (int i = 0; i < this.LevelThumbnails.Count; i++)
		{
			this.LevelThumbnails[i].GetComponent<RectTransform>().sizeDelta = new Vector2(this.Element_Width, this.Element_Height);
		}
		this.ScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(this.LevelThumbnails.Count + 2) * (this.Element_Width + 2f * this.Element_Margin), this.Element_Height);
		this.n = (float)(this.LevelThumbnails.Count - 1);
		this.ScrollSteps = 1f / this.n;
	}

	private void Update()
	{
		if (this.Enable_Show_ScrollBar)
		{
			this.HorizontalScrollBar.gameObject.SetActive(true);
		}
		else
		{
			this.HorizontalScrollBar.gameObject.SetActive(false);
		}
		if (!this.DesktopPlatform && UnityEngine.Input.touchCount == 0)
		{
			for (int i = 0; i < this.LevelThumbnails.Count; i++)
			{
				if (this.HorizontalScrollBar.GetComponent<Scrollbar>().value > this.ScrollSteps / 2f + (float)(i - 1) * this.ScrollSteps && this.HorizontalScrollBar.GetComponent<Scrollbar>().value <= Mathf.Clamp(this.ScrollSteps / 2f + (float)i * this.ScrollSteps, 0f, 1f))
				{
					this.HorizontalScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(this.HorizontalScrollBar.GetComponent<Scrollbar>().value, (float)i * this.ScrollSteps, 0.1f);
				}
			}
		}
		for (int j = 0; j < this.LevelThumbnails.Count; j++)
		{
			if (this.k > this.ScrollSteps / 2f + (float)(j - 1) * this.ScrollSteps && this.k <= Mathf.Clamp(this.ScrollSteps / 2f + (float)j * this.ScrollSteps, 0f, 1f))
			{
				this.k = Mathf.Lerp(this.k, (float)j * this.ScrollSteps, 0.1f);
			}
		}
		for (int k = 0; k < this.LevelThumbnails.Count; k++)
		{
			for (int l = 0; l < this.LevelThumbnails.Count; l++)
			{
				if (this.HorizontalScrollBar.GetComponent<Scrollbar>().value > this.ScrollSteps / 2f + (float)(k - 1) * this.ScrollSteps && this.HorizontalScrollBar.GetComponent<Scrollbar>().value <= Mathf.Clamp(this.ScrollSteps / 2f + (float)k * this.ScrollSteps, 0f, 1f))
				{
					if (l != k)
					{
						this.LevelThumbnails[l].transform.localScale = Vector2.Lerp(this.LevelThumbnails[l].transform.localScale, new Vector2(1f, 1f), this.Transition_Out);
					}
					if (l == k)
					{
						this.LevelThumbnails[k].transform.localScale = Vector2.Lerp(this.LevelThumbnails[k].transform.localScale, new Vector2(this.Element_Scale, this.Element_Scale), this.Transition_In);
						this.LevelThumbnails[k].gameObject.transform.SetAsLastSibling();
					}
				}
			}
		}
		if (this.ButtonClicked)
		{
			this.HorizontalScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(this.HorizontalScrollBar.GetComponent<Scrollbar>().value, this.k, 0.1f);
		}
	}

	public void NextButton()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.k = Mathf.Clamp(this.k + this.ScrollSteps, 0f, 1f);
		this.ButtonClicked = true;
	}

	public void PreviousButton()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.k = Mathf.Clamp(this.k - this.ScrollSteps, 0f, 1f);
		this.ButtonClicked = true;
	}

	public void ContentDrag()
	{
		this.ButtonClicked = false;
		this.k = Mathf.Clamp(this.HorizontalScrollBar.GetComponent<Scrollbar>().value, 0f, 1f);
	}

	public Canvas YourCanvas;

	public int SlidesInView;

	public AudioClip buttonSound;

	public bool Enable_Show_ScrollBar;

	public Scrollbar HorizontalScrollBar;

	public bool ShowButtons;

	public Sprite ButtonSprite;

	private float k;

	private bool ButtonClicked;

	public Sprite Background;

	public RectTransform ScrollContent;

	public List<GameObject> LevelThumbnails;

	public string SlidesNamePrefix = "Button 0";

	public float Element_Width;

	public float Element_Height;

	public float Element_Margin;

	public float Element_Scale;

	public float Transition_In;

	public float Transition_Out;

	public Color PreviousSlideColor = new Color(1f, 1f, 1f, 255f);

	public Color ActiveSlideColor = new Color(1f, 1f, 1f, 255f);

	public Color NextSlideColor = new Color(1f, 1f, 1f, 255f);

	public bool DesktopPlatform;

	private float n;

	private float ScrollSteps;
}
