using System;
using System.Collections;
using System.IO;
using EncryptStringSample;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyTextGetter : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void onloadprivacycontent()
	{
		this.filePath = Application.streamingAssetsPath + "/UserContent.txt";
		this.imagePath = Application.streamingAssetsPath + "/UserSplash.jpg";
		if (Application.platform == RuntimePlatform.Android)
		{
			base.StartCoroutine(this.privacytext_Path1());
			base.StartCoroutine(this.loadsplash_Image());
		}
		else
		{
			this.privacytext_Path();
			this.imagePath = "file:///" + Application.streamingAssetsPath + "/UserSplash.jpg";
			base.StartCoroutine(this.loadsplash_Image());
		}
	}

	private IEnumerator privacytext_Path1()
	{
		WWW www = new WWW(this.filePath);
		yield return www;
		if (www.error != null)
		{
			MonoBehaviour.print("filenotfound");
		}
		else
		{
			string s = www.text;
			TextReader textReader = new StringReader(s);
			string cipherText = textReader.ReadToEnd();
			s = StringCipher.Decrypt(cipherText, AGameUtils.Cipher_Passwords);
			PrivacyTextGetter.privacy_Content = s;
		}
		yield break;
	}

	private IEnumerator loadsplash_Image()
	{
		Texture2D text = new Texture2D(512, 512, TextureFormat.DXT1, false);
		WWW www = new WWW(this.imagePath);
		yield return www;
		if (www.error != null)
		{
			MonoBehaviour.print("filenotfound" + www.error);
		}
		else
		{
			www.LoadImageIntoTexture(text);
			if (this.G_loadingsplash.gameObject.GetComponent(typeof(Image)) != null)
			{
				this.G_loadingsplash.gameObject.GetComponent<Image>().enabled = true;
				this.tempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
				this.G_loadingsplash.gameObject.GetComponent<Image>().sprite = this.tempaddsprite;
			}
		}
		yield break;
	}

	private void privacytext_Path()
	{
		StreamReader streamReader = new StreamReader(this.filePath);
		string text = streamReader.ReadToEnd();
		string text2 = text.ToString();
		text2 = StringCipher.Decrypt(text, AGameUtils.Cipher_Passwords);
		PrivacyTextGetter.privacy_Content = text2;
	}

	public static string privacy_Content = string.Empty;

	private string filePath = string.Empty;

	private string imagePath = string.Empty;

	public GameObject G_loadingsplash;

	private Sprite tempaddsprite;
}
