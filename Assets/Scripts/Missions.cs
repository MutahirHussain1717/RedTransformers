using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
	private void Start()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3)
		{
			this.mission1 = "Destroy and eliminate 2 enemy tanks and 2 enemy helicopters.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 4)
		{
			this.mission1 = "Destroy and eliminate 2 enemy towers and 3 enemy tanks.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 5)
		{
			this.mission1 = "Destroy and eliminate 3 enemy towers and 2 enemy trucks parked in the enemy base.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 6)
		{
			this.mission1 = "Destroy and eliminate 4 warships in the sea.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 7)
		{
			this.mission1 = "Destroy and eliminate 3 enemy towers, 2 enemy trucks, 2 enemy tanks in the enemy base.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 8)
		{
			this.mission1 = "Destroy and eliminate 3 enemy towers, 3 enemy tanks and 3 riflemen.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 9)
		{
			this.mission1 = "Destroy and eliminate 3 enemy towers and 4 enemy jets.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 10)
		{
			this.mission1 = "Destroy and eliminate 6 warships and 5 riflemen.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 11)
		{
			this.mission1 = "Destroy and eliminate 3 warships, 3 helicopters, 4 tanks and 5 riflemen.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
		else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 12)
		{
			this.mission1 = "Destroy and eliminate 2 warships, 3 helicopters, 2 jets, 5 tanks, 3 trucks and 6 riflemen.";
			this.StartMissionMessage(0.1f, this.mission1);
		}
	}

	private void Update()
	{
	}

	public void StartMissionMessage(float showtime, string str)
	{
		base.StopAllCoroutines();
		this.missionTextMesh.gameObject.SetActive(true);
		this.isMissionTyping = false;
		base.StartCoroutine(this.MissionCorutine(showtime, str));
	}

	public void StartMissionMessage()
	{
		this.missionTextMesh.gameObject.SetActive(true);
		this.isMissionTyping = false;
		base.StartCoroutine(this.MissionCorutine2(this.mission2));
	}

	private IEnumerator MissionCorutine(float t, string str)
	{
		if (!this.isMissionTyping)
		{
			yield return new WaitForSeconds(t);
			this.audioSource.GetComponent<AudioSource>().PlayOneShot(this.clip);
			this.missionOne(str);
			base.StartCoroutine(this.MissionCorutine(t, str));
		}
		yield break;
	}

	private IEnumerator MissionCorutine2(string str)
	{
		if (!this.isMissionTyping)
		{
			yield return new WaitForSeconds(0.1f);
			this.audioSource.GetComponent<AudioSource>().PlayOneShot(this.clip);
			this.missionTwo(str);
			base.StartCoroutine(this.MissionCorutine2(str));
		}
		yield break;
	}

	public void missionOne(string st)
	{
		if (this.l < st.Length)
		{
			this.missionTextMesh.text = this.missionTextMesh.text + this.mission1[this.l];
			this.l++;
		}
		else
		{
			this.isMissionTyping = true;
			base.Invoke("DeActiveUIlable", 2f);
		}
	}

	public void missionTwo(string st)
	{
		if (this.l < st.Length)
		{
			this.missionTextMesh.text = this.missionTextMesh.text + this.mission2[this.l];
			this.l++;
		}
		else
		{
			this.isMissionTyping = true;
			base.Invoke("DeActiveUIlable", 5f);
		}
	}

	private void DeActiveUIlable()
	{
	}

	public AudioClip clip;

	private string mission1 = string.Empty;

	private string mission2 = "Criminal is Breaking Out his Gang Members";

	private int l;

	public Text missionTextMesh;

	public AudioSource audioSource;

	public bool isMissionTyping;
}
