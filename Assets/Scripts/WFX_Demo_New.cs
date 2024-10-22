using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WFX_Demo_New : MonoBehaviour
{
	private void Awake()
	{
		List<GameObject> list = new List<GameObject>();
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			list.Add(gameObject);
		}
		list.AddRange(this.AdditionalEffects);
		this.ParticleExamples = list.ToArray();
		this.defaultCamPosition = Camera.main.transform.position;
		this.defaultCamRotation = Camera.main.transform.rotation;
		base.StartCoroutine("CheckForDeletedParticles");
		this.UpdateUI();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.prevParticle();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.nextParticle();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Delete))
		{
			this.destroyParticles();
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit raycastHit = default(RaycastHit);
			if (this.groundCollider.Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out raycastHit, 9999f))
			{
				GameObject gameObject = this.spawnParticle();
				if (!gameObject.name.StartsWith("WFX_MF"))
				{
					gameObject.transform.position = raycastHit.point + gameObject.transform.position;
				}
			}
		}
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			Camera.main.transform.Translate(Vector3.forward * ((axis >= 0f) ? 1f : -1f), Space.Self);
		}
		if (Input.GetMouseButtonDown(2))
		{
			Camera.main.transform.position = this.defaultCamPosition;
			Camera.main.transform.rotation = this.defaultCamRotation;
		}
	}

	private void OnToggleGround()
	{
		this.groundRenderer.enabled = !this.groundRenderer.enabled;
	}

	private void OnToggleCamera()
	{
		CFX_Demo_RotateCamera.rotating = !CFX_Demo_RotateCamera.rotating;
	}

	private void OnToggleSlowMo()
	{
		this.slowMo = !this.slowMo;
		if (this.slowMo)
		{
			Time.timeScale = 0.33f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void OnPreviousEffect()
	{
		this.prevParticle();
	}

	private void OnNextEffect()
	{
		this.nextParticle();
	}

	private void UpdateUI()
	{
		this.EffectLabel.text = this.ParticleExamples[this.exampleIndex].name;
		this.EffectIndexLabel.text = string.Format("{0}/{1}", (this.exampleIndex + 1).ToString("00"), this.ParticleExamples.Length.ToString("00"));
	}

	public GameObject spawnParticle()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleExamples[this.exampleIndex]);
		gameObject.transform.position = new Vector3(0f, gameObject.transform.position.y, 0f);
		gameObject.SetActive(true);
		if (gameObject.name.StartsWith("WFX_MF"))
		{
			gameObject.transform.parent = this.ParticleExamples[this.exampleIndex].transform.parent;
			gameObject.transform.localPosition = this.ParticleExamples[this.exampleIndex].transform.localPosition;
			gameObject.transform.localRotation = this.ParticleExamples[this.exampleIndex].transform.localRotation;
		}
		else if (gameObject.name.Contains("Hole"))
		{
			gameObject.transform.parent = this.bulletholes.transform;
		}
		ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
		if (component != null && component.loop)
		{
			component.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
			component.gameObject.AddComponent<CFX_AutoDestructShuriken>();
		}
		this.onScreenParticles.Add(gameObject);
		return gameObject;
	}

	private IEnumerator CheckForDeletedParticles()
	{
		for (;;)
		{
			yield return new WaitForSeconds(5f);
			for (int i = this.onScreenParticles.Count - 1; i >= 0; i--)
			{
				if (this.onScreenParticles[i] == null)
				{
					this.onScreenParticles.RemoveAt(i);
				}
			}
		}
		yield break;
	}

	private void prevParticle()
	{
		this.exampleIndex--;
		if (this.exampleIndex < 0)
		{
			this.exampleIndex = this.ParticleExamples.Length - 1;
		}
		this.UpdateUI();
		this.showHideStuff();
	}

	private void nextParticle()
	{
		this.exampleIndex++;
		if (this.exampleIndex >= this.ParticleExamples.Length)
		{
			this.exampleIndex = 0;
		}
		this.UpdateUI();
		this.showHideStuff();
	}

	private void destroyParticles()
	{
		for (int i = this.onScreenParticles.Count - 1; i >= 0; i--)
		{
			if (this.onScreenParticles[i] != null)
			{
				UnityEngine.Object.Destroy(this.onScreenParticles[i]);
			}
			this.onScreenParticles.RemoveAt(i);
		}
	}

	private void prevTexture()
	{
		int num = this.groundTextures.IndexOf(this.groundTextureStr);
		num--;
		if (num < 0)
		{
			num = this.groundTextures.Count - 1;
		}
		this.groundTextureStr = this.groundTextures[num];
		this.selectMaterial();
	}

	private void nextTexture()
	{
		int num = this.groundTextures.IndexOf(this.groundTextureStr);
		num++;
		if (num >= this.groundTextures.Count)
		{
			num = 0;
		}
		this.groundTextureStr = this.groundTextures[num];
		this.selectMaterial();
	}

	private void selectMaterial()
	{
		string text = this.groundTextureStr;
		if (text != null)
		{
			if (!(text == "Concrete"))
			{
				if (!(text == "Wood"))
				{
					if (!(text == "Metal"))
					{
						if (text == "Checker")
						{
							this.ground.GetComponent<Renderer>().material = this.checker;
							this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.checkerWall;
							this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.checkerWall;
						}
					}
					else
					{
						this.ground.GetComponent<Renderer>().material = this.metal;
						this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.metalWall;
						this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.metalWall;
					}
				}
				else
				{
					this.ground.GetComponent<Renderer>().material = this.wood;
					this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.woodWall;
					this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.woodWall;
				}
			}
			else
			{
				this.ground.GetComponent<Renderer>().material = this.concrete;
				this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.concreteWall;
				this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.concreteWall;
			}
		}
	}

	private void showHideStuff()
	{
		if (this.ParticleExamples[this.exampleIndex].name.StartsWith("WFX_MF Spr"))
		{
			this.m4.GetComponent<Renderer>().enabled = true;
			Camera.main.transform.position = new Vector3(-2.482457f, 3.263842f, -0.004924395f);
			Camera.main.transform.eulerAngles = new Vector3(20f, 90f, 0f);
		}
		else
		{
			this.m4.GetComponent<Renderer>().enabled = false;
		}
		if (this.ParticleExamples[this.exampleIndex].name.StartsWith("WFX_MF FPS"))
		{
			this.m4fps.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			this.m4fps.GetComponent<Renderer>().enabled = false;
		}
		if (this.ParticleExamples[this.exampleIndex].name.StartsWith("WFX_BImpact"))
		{
			this.walls.SetActive(true);
			Renderer[] componentsInChildren = this.bulletholes.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.enabled = true;
			}
		}
		else
		{
			this.walls.SetActive(false);
			Renderer[] componentsInChildren2 = this.bulletholes.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer2 in componentsInChildren2)
			{
				renderer2.enabled = false;
			}
		}
		if (this.ParticleExamples[this.exampleIndex].name.Contains("Wood"))
		{
			this.groundTextureStr = "Wood";
			this.selectMaterial();
		}
		else if (this.ParticleExamples[this.exampleIndex].name.Contains("Concrete"))
		{
			this.groundTextureStr = "Concrete";
			this.selectMaterial();
		}
		else if (this.ParticleExamples[this.exampleIndex].name.Contains("Metal"))
		{
			this.groundTextureStr = "Metal";
			this.selectMaterial();
		}
		else if (this.ParticleExamples[this.exampleIndex].name.Contains("Dirt") || this.ParticleExamples[this.exampleIndex].name.Contains("Sand") || this.ParticleExamples[this.exampleIndex].name.Contains("SoftBody"))
		{
			this.groundTextureStr = "Checker";
			this.selectMaterial();
		}
		else if (this.ParticleExamples[this.exampleIndex].name == "WFX_Explosion")
		{
			this.groundTextureStr = "Checker";
			this.selectMaterial();
		}
	}

	public Text EffectLabel;

	public Text EffectIndexLabel;

	public Renderer groundRenderer;

	public Collider groundCollider;

	public GameObject[] AdditionalEffects;

	public GameObject ground;

	public GameObject walls;

	public GameObject bulletholes;

	public GameObject m4;

	public GameObject m4fps;

	public Material wood;

	public Material concrete;

	public Material metal;

	public Material checker;

	public Material woodWall;

	public Material concreteWall;

	public Material metalWall;

	public Material checkerWall;

	private string groundTextureStr = "Checker";

	private List<string> groundTextures = new List<string>(new string[]
	{
		"Concrete",
		"Wood",
		"Metal",
		"Checker"
	});

	private GameObject[] ParticleExamples;

	private int exampleIndex;

	private bool slowMo;

	private Vector3 defaultCamPosition;

	private Quaternion defaultCamRotation;

	private List<GameObject> onScreenParticles = new List<GameObject>();
}
