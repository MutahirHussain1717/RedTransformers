using System;
using UnityEngine;

public class TankHUD : MonoBehaviour
{
	private void Start()
	{
		this.weaponManager = base.GetComponent<WeaponController>();
	}

	private void Update()
	{
		if (!this.weaponManager)
		{
			return;
		}
		if (UnityEngine.Input.GetKey(KeyCode.Escape))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			this.weaponManager.CurrentWeapon--;
			if (this.weaponManager.CurrentWeapon < 0)
			{
				this.weaponManager.CurrentWeapon = this.weaponManager.WeaponLists.Length - 1;
			}
		}
		if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			this.weaponManager.CurrentWeapon++;
			if (this.weaponManager.CurrentWeapon >= this.weaponManager.WeaponLists.Length)
			{
				this.weaponManager.CurrentWeapon = 0;
			}
		}
		this.currentWeapon = this.weaponManager.CurrentWeapon;
	}

	private void OnGUI()
	{
		if (!this.weaponManager || this.currentWeapon > this.weaponManager.WeaponLists.Length)
		{
			return;
		}
		GUI.skin.label.fontSize = 15;
		GUI.Label(new Rect(20f, 20f, 300f, 30f), "Weapon Index " + this.currentWeapon);
		GUI.Label(new Rect(20f, 80f, 300f, 30f), "Esc back to mainmenu");
		GUI.Label(new Rect(20f, (float)(Screen.height - 50), 300f, 30f), "Scroll Mouse to Change weapons");
		GUI.Label(new Rect(20f, (float)(Screen.height - 70), 300f, 30f), "W A S D to Move");
		GUI.skin.label.fontSize = 25;
		GUI.Label(new Rect(20f, 40f, 300f, 50f), string.Empty + this.weaponManager.WeaponLists[this.currentWeapon].name);
	}

	private int currentWeapon;

	private WeaponController weaponManager;
}
