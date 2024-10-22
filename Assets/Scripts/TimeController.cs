using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		TimeController.isAttackMode = false;
		base.GetComponent<AudioSource>().loop = true;
		this.timeToCompleteLevel = (float)this.timeToCompleteLevels[GlobalScripts.CurrLevelIndex];
		this.checkIt = false;
		TimeController.isTimeOver = false;
		TimeController.isGamePaused = false;
	}

	private void Update()
	{
		int num = (int)Math.Abs(this.timeToCompleteLevel / 60f);
		int num2 = (int)this.timeToCompleteLevel % 60;
		if (this.timeToCompleteLevel >= 0f && !TimeController.isGamePaused)
		{
			this.timeToCompleteLevel -= Time.deltaTime;
		}
		if (this.timeToCompleteLevel <= (float)(this.timeToCompleteLevels[GlobalScripts.CurrLevelIndex] - 10))
		{
			TimeController.isAttackMode = true;
		}
		if (this.timeToCompleteLevel < 0f && !TimeController.isTimeOver)
		{
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().Dia_TimesUp();
			TimeController.isTimeOver = true;
		}
		this.timeTM.text = "0" + num.ToString() + ":";
		if (num2 < 10)
		{
			this.timeTM.text = this.timeTM.text + "0" + num2.ToString();
		}
		else
		{
			this.timeTM.text = this.timeTM.text + num2.ToString();
		}
	}

	public void Pasue()
	{
		base.GetComponent<GameDialogs>().Dia_Paused(true);
		TimeController.isGamePaused = true;
		this.buttonPaused.SetActive(false);
		this.GameControls.SetActive(false);
		GameObject.Find("Main Camera").GetComponent<GameDialogs>().ClearUI();
		if (this.zoomSprite)
		{
			this.zoomSprite.SetActive(false);
		}
		Time.timeScale = 0f;
	}

	private IEnumerator Delay(float t)
	{
		yield return new WaitForSeconds(t);
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().Dia_TimesUp();
		yield break;
	}

	public Text timeTM;

	public Text levelnum;

	private int[] timeToCompleteLevels = new int[]
	{
		400,
		400,
		350,
		300,
		300,
		300,
		300,
		300,
		300,
		300,
		200,
		200,
		20
	};

	public float timeToCompleteLevel;

	public static bool isTimeOver;

	public static bool isGamePaused;

	public GameObject buttonPaused;

	public GameObject zoomScroll;

	public GameObject zoomSprite;

	private bool checkIt;

	public static bool isAttackMode;

	public GameObject GameControls;
}
