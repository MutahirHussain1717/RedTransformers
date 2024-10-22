using System;
using System.Collections;
using System.IO;
using GssAdSdk;
using UnityEngine;
using UnityEngine.UI;

public class MiniADdIcon_script : MonoBehaviour
{
	private void Start()
	{
		this.checktransition = false;
		this.recursivecalls = 0;
		if (base.gameObject.transform.childCount > 0)
		{
			this.child = base.gameObject.transform.GetChild(0).gameObject;
			this.startMarker = new Vector3(base.gameObject.GetComponent<RectTransform>().localPosition.x, base.gameObject.GetComponent<RectTransform>().localPosition.y, base.gameObject.GetComponent<RectTransform>().localPosition.z);
			this.endMarker = new Vector3(base.gameObject.GetComponent<RectTransform>().rect.width * -1f, base.gameObject.GetComponent<RectTransform>().localPosition.y, base.gameObject.GetComponent<RectTransform>().localPosition.z);
		}
		else
		{
			this.switchadicon = false;
		}
		this.checkshowadd = true;
		this.checkurl = false;
		this.miniicon_url = string.Empty;
		this.url = string.Empty;
		this.tempaddpackage = string.Empty;
		this.switchtempaddpackage = string.Empty;
		if (base.gameObject.GetComponent(typeof(Image)) != null)
		{
			base.gameObject.GetComponent<Image>().enabled = false;
		}
		if (base.gameObject.GetComponent(typeof(SpriteRenderer)) != null)
		{
			base.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (base.gameObject.GetComponent(typeof(Image)) == null && base.gameObject.GetComponent(typeof(MeshFilter)) == null && base.gameObject.GetComponent(typeof(SpriteRenderer)) == null)
		{
			this.checkshowadd = false;
		}
		if (!TenlogixAds.isBackFilledEnabled)
		{
			base.gameObject.SetActive(false);
		}
	}

	private IEnumerator CallManageRoutine(float sec)
	{
		yield return new WaitForSecondsRealtime(sec);
		this.ManageswitchController();
		yield break;
	}

	private IEnumerator CallswitchRoutine(float sec)
	{
		yield return new WaitForSecondsRealtime(sec);
		this.switchicontransitionmanager();
		yield break;
	}

	private void OnGUI()
	{
		if (!this.checkurl)
		{
			if (string.IsNullOrEmpty(this.tempurl))
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					this.tempurl = string.Empty + TenlogixAds.getaddsurlpackage();
					if (this.switchadicon && TenlogixAds.isBackFilledEnabled)
					{
						int num = 0;
						foreach (string text in TenlogixAds.arrpackages)
						{
							if (text.Contains(this.switchtempaddpackage))
							{
								num++;
							}
						}
						num = 4;
						base.StartCoroutine(this.CallManageRoutine(2f));
						base.StartCoroutine(this.CallswitchRoutine((float)num));
					}
				}
				else if (TenlogixAds.arlist.Count > 0)
				{
					this.tempurl = string.Empty + TenlogixAds.arlist[UnityEngine.Random.Range(0, TenlogixAds.arlist.Count)];
					if (this.switchadicon && TenlogixAds.isBackFilledEnabled)
					{
						int num2 = 0;
						foreach (string text2 in TenlogixAds.arlist)
						{
							if (text2.Contains(this.tempurl))
							{
								num2++;
							}
						}
						num2 = 4;
						base.StartCoroutine(this.CallManageRoutine(2f));
						base.StartCoroutine(this.CallswitchRoutine((float)num2));
					}
				}
			}
			else
			{
				this.url = this.tempurl;
				this.tempaddpackage = this.GetProductName(this.tempurl);
				this.checkurl = true;
				this.miniicon_url = "market://details?id=" + this.tempaddpackage;
				if (this.checkshowadd && TenlogixAds.isBackFilledEnabled)
				{
					base.StartCoroutine("GetAddPackage");
				}
				else if (TenlogixAds.isBackFilledEnabled)
				{
					base.StartCoroutine("Temp_atbackend_GetAddPackage");
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
		}
		if (this.checktransition)
		{
			this.do_transition();
		}
	}

	private IEnumerator GetAddPackage()
	{
		Texture2D text = new Texture2D(512, 512, TextureFormat.DXT1, false);
		if (File.Exists(Application.persistentDataPath + "/" + this.tempaddpackage + ".png"))
		{
			byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + this.tempaddpackage + ".png");
			text.LoadImage(data);
			if (base.gameObject.GetComponent(typeof(MeshFilter)) != null)
			{
				base.GetComponent<MeshFilter>().mesh = this.quadmesh;
				Renderer component = base.GetComponent<Renderer>();
				component.material.mainTexture = text;
				component.material.shader = Shader.Find("Sprites/Default");
			}
			else if (base.gameObject.GetComponent(typeof(Image)) != null)
			{
				base.gameObject.GetComponent<Image>().enabled = true;
				this.tempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
				base.gameObject.GetComponent<Image>().sprite = this.tempaddsprite;
			}
			else if (base.gameObject.GetComponent(typeof(SpriteRenderer)) != null)
			{
				base.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				this.tempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
				base.gameObject.GetComponent<SpriteRenderer>().sprite = this.tempaddsprite;
				base.gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
			}
		}
		else
		{
			WWW www = new WWW(this.url);
			yield return www;
			if (www.error != null)
			{
				MonoBehaviour.print("this run");
				base.gameObject.SetActive(false);
			}
			else
			{
				www.LoadImageIntoTexture(text);
				File.WriteAllBytes(Application.persistentDataPath + "/" + this.tempaddpackage + ".png", www.bytes);
				if (base.gameObject.GetComponent(typeof(MeshFilter)) != null)
				{
					base.GetComponent<MeshFilter>().mesh = this.quadmesh;
					Renderer component2 = base.GetComponent<Renderer>();
					component2.material.mainTexture = text;
					component2.material.shader = Shader.Find("Sprites/Default");
				}
				else if (base.gameObject.GetComponent(typeof(Image)) != null)
				{
					base.gameObject.GetComponent<Image>().enabled = true;
					this.tempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
					base.gameObject.GetComponent<Image>().sprite = this.tempaddsprite;
				}
				else if (base.gameObject.GetComponent(typeof(SpriteRenderer)) != null)
				{
					base.gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.tempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
					base.gameObject.GetComponent<SpriteRenderer>().sprite = this.tempaddsprite;
					base.gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
				}
			}
		}
		yield break;
	}

	private IEnumerator Temp_atbackend_GetAddPackage()
	{
		Texture2D text = new Texture2D(512, 512, TextureFormat.DXT1, false);
		if (!File.Exists(Application.persistentDataPath + "/" + this.tempaddpackage + ".png"))
		{
			WWW www = new WWW(this.url);
			yield return www;
			if (www.error != null)
			{
				UnityEngine.Debug.Log("Internet not connected-- couldnot find icon");
			}
			else
			{
				www.LoadImageIntoTexture(text);
				File.WriteAllBytes(Application.persistentDataPath + "/" + this.tempaddpackage + ".png", www.bytes);
			}
		}
		yield break;
	}

	private IEnumerator temporaryload_icon()
	{
		Texture2D text = new Texture2D(512, 512, TextureFormat.DXT1, false);
		if (File.Exists(Application.persistentDataPath + "/" + this.switchtempaddpackage + ".png"))
		{
			byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + this.switchtempaddpackage + ".png");
			text.LoadImage(data);
			if (base.gameObject.GetComponent(typeof(Image)) != null)
			{
				this.switchtempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
				this.child.GetComponent<Image>().sprite = this.switchtempaddsprite;
			}
			this.isNicon = true;
			if (this.isTransicon)
			{
				this.switchicontransitionmanager();
			}
		}
		else
		{
			WWW www = new WWW(this.switchurl);
			yield return www;
			if (www.error != null)
			{
				this.ManageswitchController();
			}
			else
			{
				www.LoadImageIntoTexture(text);
				File.WriteAllBytes(Application.persistentDataPath + "/" + this.switchtempaddpackage + ".png", www.bytes);
				if (base.gameObject.GetComponent(typeof(Image)) != null)
				{
					this.switchtempaddsprite = Sprite.Create(text, new Rect(0f, 0f, (float)text.width, (float)text.height), new Vector2(0.5f, 0.5f), 128f);
					this.child.GetComponent<Image>().sprite = this.switchtempaddsprite;
				}
				this.isNicon = true;
				if (this.isTransicon)
				{
					this.switchicontransitionmanager();
				}
			}
		}
		yield break;
	}

	private void OnEnable()
	{
		if (this.checkurl)
		{
			if (this.isNicon && TenlogixAds.isBackFilledEnabled)
			{
				this.switchicontransitionmanager();
			}
			else
			{
				UnityEngine.Debug.Log("NNNOt nexticonavailable");
				if (base.gameObject.GetComponent(typeof(Image)) != null)
				{
					base.gameObject.GetComponent<Image>().enabled = false;
				}
				this.tempurl = string.Empty;
				this.checkurl = false;
			}
		}
	}

	private void rearrangethings()
	{
		this.tempaddpackage = this.switchtempaddpackage;
		this.miniicon_url = "market://details?id=" + this.tempaddpackage;
		this.isNicon = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			if (TenlogixAds.isBackFilledEnabled)
			{
				int num = 0;
				foreach (string text in TenlogixAds.arlist)
				{
					if (text.Contains(this.switchtempaddpackage))
					{
						num++;
					}
				}
				num *= 2;
				base.StartCoroutine(this.CallswitchRoutine((float)num));
			}
		}
		else if (TenlogixAds.isBackFilledEnabled)
		{
			int num2 = 0;
			foreach (string text2 in TenlogixAds.arrpackages)
			{
				if (text2.Contains(this.switchtempaddpackage))
				{
					num2++;
				}
			}
			num2 *= 2;
			base.StartCoroutine(this.CallswitchRoutine((float)num2));
		}
		this.ManageswitchController();
	}

	private void do_transition()
	{
		float num = (Time.unscaledTime - this.startTime) * this.speed;
		float num2 = num / this.journeyLength;
		base.gameObject.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(this.startMarker, this.endMarker, num2);
		if (num2 > 1f)
		{
			this.checktransition = false;
			base.gameObject.GetComponent<RectTransform>().localPosition = this.startMarker;
			base.gameObject.GetComponent<Image>().sprite = this.child.GetComponent<Image>().sprite;
			this.rearrangethings();
		}
	}

	private void switchicontransitionmanager()
	{
		if (this.switchadicon && this.isNicon)
		{
			this.checktransition = true;
			this.startTime = Time.unscaledTime;
			this.isTransicon = false;
			this.journeyLength = Vector3.Distance(this.startMarker, this.endMarker);
		}
		else if (this.switchadicon && !this.isNicon)
		{
			this.isTransicon = true;
		}
	}

	private void ManageswitchController()
	{
		if (!this.isNicon && this.switchadicon)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				this.switchurl = string.Empty + TenlogixAds.getaddsurlpackage();
				this.recursivecalls++;
			}
			else
			{
				this.switchurl = string.Empty + TenlogixAds.arlist[UnityEngine.Random.Range(0, TenlogixAds.arlist.Count)];
				this.recursivecalls++;
			}
			if (this.GetProductName(this.switchurl) == this.tempaddpackage && this.recursivecalls < 13)
			{
				this.ManageswitchController();
			}
			else if (TenlogixAds.isapppresent(this.GetProductName(this.switchurl)) && this.recursivecalls < 13)
			{
				this.ManageswitchController();
			}
			else
			{
				this.recursivecalls = 0;
				this.switchtempaddpackage = this.GetProductName(this.switchurl);
				base.StartCoroutine("temporaryload_icon");
			}
		}
	}

	private string GetProductName(string getstringname)
	{
		string text = getstringname;
		if (text.Contains("/") && text.Contains("."))
		{
			int num = text.LastIndexOf("/", StringComparison.Ordinal) + 1;
			getstringname = text.Substring(num, text.Length - num);
			num = getstringname.LastIndexOf(".", StringComparison.Ordinal) + 1;
			return getstringname.Substring(0, getstringname.Length - (getstringname.Length - num + 1));
		}
		return text;
	}

	public void OnMiniaddicon()
	{
		Application.OpenURL(string.Empty + this.miniicon_url);
	}

	public void OnCrossbtn()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
			base.Invoke("OnthisObject", 11f);
		}
	}

	private void OnthisObject()
	{
		base.gameObject.SetActive(true);
	}

	public bool switchadicon;

	private string url = string.Empty;

	private string miniicon_url = string.Empty;

	private string tempaddpackage = string.Empty;

	public Mesh quadmesh;

	private string switchtempaddpackage = string.Empty;

	private string switchurl = string.Empty;

	private Sprite tempaddsprite;

	private Sprite switchtempaddsprite;

	private bool checkurl;

	private string tempurl = string.Empty;

	private bool checkshowadd;

	private bool isNicon;

	private bool isTransicon;

	private GameObject child;

	private float journeyLength;

	private float speed = 200f;

	private float startTime;

	private Vector3 startMarker;

	private Vector3 endMarker;

	private bool checktransition;

	private bool lerpiconcheck = true;

	private int recursivecalls;
}
