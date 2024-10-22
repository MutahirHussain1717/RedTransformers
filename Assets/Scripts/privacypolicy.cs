using System;
using UnityEngine;

public class privacypolicy : MonoBehaviour
{
	public void OnExitButtonClicked()
	{
		this.scrollview.SetActive(false);
		this.crossbt.SetActive(false);
		this.privacyButton.SetActive(true);
	}

	public void OnPrivacyButtonClicked()
	{
		this.scrollview.SetActive(true);
		this.crossbt.SetActive(true);
		this.privacyButton.SetActive(false);
	}

	public GameObject scrollview;

	public GameObject crossbt;

	public GameObject privacyButton;
}
