using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class startmission : MonoBehaviour
{
	private void Start()
	{
		startmission.isMissionStarted = false;
	}

	private void Update()
	{
	}

	public void StartMission()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.btnsound);
		startmission.isMissionStarted = true;
		this.missionDialoge.SetActive(false);
		GameObject.FindWithTag("MainCamera").GetComponent<TimeController>().enabled = true;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().player_cams();
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.maincam.transform, 35f, 14f);
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
	}

	private IEnumerator missionstart()
	{
		if (GlobalScripts.CurrLevelIndex == 0)
		{
			this.statementtxt.text = "Plant the bomb on tank";
		}
		else if (GlobalScripts.CurrLevelIndex == 1)
		{
			this.statementtxt.text = "Park Two Cars on Truck";
		}
		else if (GlobalScripts.CurrLevelIndex == 2)
		{
			this.statementtxt.text = "Follow Arrow, Drive Truck to Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 3)
		{
			this.statementtxt.text = "Park Two Cars on Parking Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 4)
		{
			this.statementtxt.text = "Park Two Cars on Parking Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 5)
		{
			this.statementtxt.text = "Park Two Jeep on Truck";
		}
		else if (GlobalScripts.CurrLevelIndex == 6)
		{
			this.statementtxt.text = "Park Two Jeep on Truck";
		}
		else if (GlobalScripts.CurrLevelIndex == 7)
		{
			this.statementtxt.text = "Follow Arrow, Drive Truck to Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 8)
		{
			this.statementtxt.text = "Park Two Jeep on Parking Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 9)
		{
			this.statementtxt.text = "Park Two Jeep on Parking Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 10)
		{
			this.statementtxt.text = "Park Two Limo on Truck";
		}
		else if (GlobalScripts.CurrLevelIndex == 11)
		{
			this.statementtxt.text = "Park Two Limo on Truck";
		}
		else if (GlobalScripts.CurrLevelIndex == 12)
		{
			this.statementtxt.text = "Follow Arrow, Drive Truck to Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 13)
		{
			this.statementtxt.text = "Park Two Limo on Parking Destination";
		}
		else if (GlobalScripts.CurrLevelIndex == 14)
		{
			this.statementtxt.text = "Park Two Limo on Parking Destination";
		}
		yield return new WaitForSeconds(2.5f);
		this.missionDialoge.SetActive(true);
		yield break;
	}

	private IEnumerator mission()
	{
		yield return new WaitForSeconds(0f);
		GameObject.FindWithTag("MainCamera").GetComponent<TimeController>().enabled = true;
		startmission.again = false;
		yield break;
	}

	public GameObject[] counter;

	public GameObject maincam;

	public GameObject missionDialoge;

	public static bool again = true;

	public Text statementtxt;

	public AudioClip btnsound;

	private RaycastHit hit;

	public static bool isMissionStarted;

	private Ray ray;
}
