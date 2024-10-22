using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Start_game : MonoBehaviour
{
	private void Start()
	{
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
		base.StartCoroutine("displayfirstcam");
		this.stopstartanim();
	}

	private void Update()
	{
	}

	public void stopstartanim()
	{
		base.StopAllCoroutines();
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
		{
			this.statementtxt.text = "Go Fight with Gangster";
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3)
		{
			this.statementtxt.text = "Go Fight with Gangster";
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 4)
		{
			this.statementtxt.text = "Go Fight with Gangster";
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 5)
		{
			this.statementtxt.text = "Plant the bomb on the Army Jeep";
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 6)
		{
			this.statementtxt.text = "Plant the bomb on the Bridge";
		}
		this.dialogmission.SetActive(true);
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
		this.animationstopbt.SetActive(false);
		this.dialogmission.SetActive(false);
		GameObject.FindWithTag("MainCamera").GetComponent<TimeController>().enabled = true;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().player_cams();
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.maincam.transform, 27.5f, 10f);
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
		base.gameObject.SetActive(false);
	}

	private IEnumerator displayfirstcam()
	{
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.cam, 10f, 5f);
		yield return new WaitForSeconds(13f);
		base.StartCoroutine("displaysecondcam");
		yield break;
	}

	private IEnumerator displaysecondcam()
	{
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.cam1, 10f, 5f);
		yield return new WaitForSeconds(10f);
		this.stopstartanim();
		yield break;
	}

	private IEnumerator displaythirdcam()
	{
		this.audio.PlayOneShot(this.metalsound);
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.cam2, 9.51f, 2.87f);
		yield return new WaitForSeconds(2f);
		base.StartCoroutine("displayfourthcam");
		yield break;
	}

	private IEnumerator displayfourthcam()
	{
		this.audio.PlayOneShot(this.metalsound);
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.cam3, 9.51f, 2.87f);
		yield return new WaitForSeconds(2f);
		this.dialogmission.SetActive(true);
		this.animationstopbt.SetActive(false);
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
		this.audio.PlayOneShot(this.metalsound);
		yield break;
	}

	public GameObject dialogmission;

	public GameObject animationstopbt;

	public Transform cam;

	public Transform cam1;

	public Transform cam2;

	public Transform cam3;

	public Transform cam4;

	public Transform maincam;

	public AudioClip metalsound;

	public AudioSource audio;

	public Text statementtxt;
}
