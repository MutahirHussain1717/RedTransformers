using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Colors")]
public class RCC_DashboardColors : MonoBehaviour
{
	private void Awake()
	{
		if (this.huds == null || this.huds.Length < 1)
		{
			base.enabled = false;
		}
		if (this.hudColor_R && this.hudColor_G && this.hudColor_B)
		{
			this.hudColor_R.value = this.hudColor.r;
			this.hudColor_G.value = this.hudColor.g;
			this.hudColor_B.value = this.hudColor.b;
		}
	}

	private void Update()
	{
		if (this.hudColor_R && this.hudColor_G && this.hudColor_B)
		{
			this.hudColor = new Color(this.hudColor_R.value, this.hudColor_G.value, this.hudColor_B.value);
		}
		for (int i = 0; i < this.huds.Length; i++)
		{
			this.huds[i].color = new Color(this.hudColor.r, this.hudColor.g, this.hudColor.b, this.huds[i].color.a);
		}
	}

	public Image[] huds;

	public Color hudColor = Color.white;

	public Slider hudColor_R;

	public Slider hudColor_G;

	public Slider hudColor_B;
}
