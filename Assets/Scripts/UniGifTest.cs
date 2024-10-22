using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UniGifTest : MonoBehaviour
{
	private void Start()
	{
		this.m_uniGifImage.SetGifFromUrl("https://i.makeagif.com/media/11-28-2015/4ze4qn.gif", true);
	}

	public void OnButtonClicked()
	{
		if (this.m_mutex || this.m_uniGifImage == null || string.IsNullOrEmpty(this.m_inputField.text))
		{
			return;
		}
		this.m_mutex = true;
		base.StartCoroutine(this.ViewGifCoroutine());
	}

	private IEnumerator ViewGifCoroutine()
	{
		yield return base.StartCoroutine(this.m_uniGifImage.SetGifFromUrlCoroutine(this.m_inputField.text, true));
		this.m_mutex = false;
		yield break;
	}

	[SerializeField]
	private InputField m_inputField;

	[SerializeField]
	private UniGifImage m_uniGifImage;

	private bool m_mutex;
}
