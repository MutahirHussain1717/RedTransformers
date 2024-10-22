using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
	private void Start()
	{
		this.checkfire = false;
	}

	private void Update()
	{
		if (this.checkfire)
		{
			this.fireweapon();
		}
	}

	public void onfireweapon(bool setcheckfire)
	{
		this.checkfire = setcheckfire;
	}

	public void fireweapon()
	{
		MonoBehaviour.print("firebutton");
		GameObject gameObject = base.GetComponent<RoboTransformControler>().getcurrent_objplayer();
		this.WeaponLists[this.CurrentWeapon].transform.position = gameObject.transform.Find("rocketnozzle").gameObject.transform.position;
		this.WeaponLists[this.CurrentWeapon].transform.eulerAngles = gameObject.transform.Find("rocketnozzle").gameObject.transform.eulerAngles;
		if (this.CurrentWeapon < this.WeaponLists.Length && this.WeaponLists[this.CurrentWeapon] != null)
		{
			this.WeaponLists[this.CurrentWeapon].gameObject.GetComponent<WeaponLauncher>().Shoot();
		}
	}

	public void changeweaponsystem()
	{
		this.CurrentWeapon++;
		if (this.CurrentWeapon == 4)
		{
			this.CurrentWeapon = 0;
		}
		GameObject gameObject = base.GetComponent<RoboTransformControler>().getcurrent_objplayer();
		if (gameObject.gameObject.name == "robo" || gameObject.gameObject.name == "robot")
		{
			for (int i = 0; i < this.roboweaponlist.Length; i++)
			{
				this.roboweaponlist[i].SetActive(false);
			}
			this.roboweaponlist[this.CurrentWeapon].SetActive(true);
		}
		this.weaponsbuttons.GetComponent<Image>().sprite = this.images[this.CurrentWeapon];
	}

	public void LaunchWeapon(int index)
	{
		this.CurrentWeapon = index;
		if (this.CurrentWeapon < this.WeaponLists.Length && this.WeaponLists[index] != null)
		{
			this.WeaponLists[index].gameObject.GetComponent<WeaponLauncher>().Shoot();
		}
	}

	public void LaunchWeapon()
	{
		if (this.CurrentWeapon < this.WeaponLists.Length && this.WeaponLists[this.CurrentWeapon] != null)
		{
			this.WeaponLists[this.CurrentWeapon].gameObject.GetComponent<WeaponLauncher>().Shoot();
		}
	}

	public GameObject[] WeaponLists;

	public GameObject[] roboweaponlist;

	public int CurrentWeapon;

	public Sprite[] images;

	public GameObject weaponsbuttons;

	private bool checkfire;
}
