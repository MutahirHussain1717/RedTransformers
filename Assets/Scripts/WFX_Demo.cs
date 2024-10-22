using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFX_Demo : MonoBehaviour
{
	private void OnMouseDown()
	{
		RaycastHit raycastHit = default(RaycastHit);
		if (base.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out raycastHit, 9999f))
		{
			GameObject gameObject = this.spawnParticle();
			if (!gameObject.name.StartsWith("WFX_MF"))
			{
				gameObject.transform.position = raycastHit.point + gameObject.transform.position;
			}
		}
	}

	public GameObject spawnParticle()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleExamples[this.exampleIndex]);
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
		this.SetActiveCrossVersions(gameObject, true);
		return gameObject;
	}

	private void SetActiveCrossVersions(GameObject obj, bool active)
	{
		obj.SetActive(active);
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			obj.transform.GetChild(i).gameObject.SetActive(active);
		}
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 20f, (float)(Screen.width - 10), 60f));
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Effect: " + this.ParticleExamples[this.exampleIndex].name, new GUILayoutOption[]
		{
			GUILayout.Width(280f)
		});
		if (GUILayout.Button("<", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.prevParticle();
		}
		if (GUILayout.Button(">", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.nextParticle();
		}
		GUILayout.FlexibleSpace();
		GUILayout.Label("Click on the ground to spawn the selected effect", new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button((!this.rotateCam) ? "Rotate Camera" : "Pause Camera", new GUILayoutOption[]
		{
			GUILayout.Width(110f)
		}))
		{
			this.rotateCam = !this.rotateCam;
		}
		if (GUILayout.Button((!base.GetComponent<Renderer>().enabled) ? "Show Ground" : "Hide Ground", new GUILayoutOption[]
		{
			GUILayout.Width(90f)
		}))
		{
			base.GetComponent<Renderer>().enabled = !base.GetComponent<Renderer>().enabled;
		}
		if (GUILayout.Button((!this.slowMo) ? "Slow Motion" : "Normal Speed", new GUILayoutOption[]
		{
			GUILayout.Width(100f)
		}))
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
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Ground texture: " + this.groundTextureStr, new GUILayoutOption[]
		{
			GUILayout.Width(160f)
		});
		if (GUILayout.Button("<", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.prevTexture();
		}
		if (GUILayout.Button(">", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.nextTexture();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (this.m4.GetComponent<Renderer>().enabled)
		{
			GUILayout.BeginArea(new Rect(5f, (float)(Screen.height - 100), (float)(Screen.width - 10), 90f));
			this.rotate_m4 = GUILayout.Toggle(this.rotate_m4, "AutoRotate Weapon", new GUILayoutOption[]
			{
				GUILayout.Width(250f)
			});
			GUI.enabled = !this.rotate_m4;
			float num = this.m4.transform.localEulerAngles.x;
			num = ((num <= 90f) ? num : (num - 180f));
			float num2 = this.m4.transform.localEulerAngles.y;
			float num3 = this.m4.transform.localEulerAngles.z;
			num = GUILayout.HorizontalSlider(num, 0f, 179f, new GUILayoutOption[]
			{
				GUILayout.Width(256f)
			});
			num2 = GUILayout.HorizontalSlider(num2, 0f, 359f, new GUILayoutOption[]
			{
				GUILayout.Width(256f)
			});
			num3 = GUILayout.HorizontalSlider(num3, 0f, 359f, new GUILayoutOption[]
			{
				GUILayout.Width(256f)
			});
			if (GUI.changed)
			{
				if (num > 90f)
				{
					num += 180f;
				}
				this.m4.transform.localEulerAngles = new Vector3(num, num2, num3);
				UnityEngine.Debug.Log(num);
			}
			GUILayout.EndArea();
		}
	}

	private IEnumerator RandomSpawnsCoroutine()
	{
		for (;;)
		{
			GameObject particles = this.spawnParticle();
			if (this.orderedSpawns)
			{
				particles.transform.position = base.transform.position + new Vector3(this.order, particles.transform.position.y, 0f);
				this.order -= this.step;
				if (this.order < -this.range)
				{
					this.order = this.range;
				}
			}
			else
			{
				particles.transform.position = base.transform.position + new Vector3(UnityEngine.Random.Range(-this.range, this.range), 0f, UnityEngine.Random.Range(-this.range, this.range)) + new Vector3(0f, particles.transform.position.y, 0f);
			}
			yield return new WaitForSeconds(float.Parse(this.randomSpawnsDelay));
		}
		yield break;
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
		if (this.rotateCam)
		{
			Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, this.cameraSpeed * Time.deltaTime);
		}
		if (this.rotate_m4)
		{
			this.m4.transform.Rotate(new Vector3(0f, 40f, 0f) * Time.deltaTime, Space.World);
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
							base.GetComponent<Renderer>().material = this.checker;
							this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.checkerWall;
							this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.checkerWall;
						}
					}
					else
					{
						base.GetComponent<Renderer>().material = this.metal;
						this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.metalWall;
						this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.metalWall;
					}
				}
				else
				{
					base.GetComponent<Renderer>().material = this.wood;
					this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.woodWall;
					this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.woodWall;
				}
			}
			else
			{
				base.GetComponent<Renderer>().material = this.concrete;
				this.walls.transform.GetChild(0).GetComponent<Renderer>().material = this.concreteWall;
				this.walls.transform.GetChild(1).GetComponent<Renderer>().material = this.concreteWall;
			}
		}
	}

	private void prevParticle()
	{
		this.exampleIndex--;
		if (this.exampleIndex < 0)
		{
			this.exampleIndex = this.ParticleExamples.Length - 1;
		}
		this.showHideStuff();
	}

	private void nextParticle()
	{
		this.exampleIndex++;
		if (this.exampleIndex >= this.ParticleExamples.Length)
		{
			this.exampleIndex = 0;
		}
		this.showHideStuff();
	}

	private void showHideStuff()
	{
		if (this.ParticleExamples[this.exampleIndex].name.StartsWith("WFX_MF Spr"))
		{
			this.m4.GetComponent<Renderer>().enabled = true;
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
			this.SetActiveCrossVersions(this.walls, true);
			Renderer[] componentsInChildren = this.bulletholes.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.enabled = true;
			}
		}
		else
		{
			this.SetActiveCrossVersions(this.walls, false);
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

	public float cameraSpeed = 10f;

	public bool orderedSpawns = true;

	public float step = 1f;

	public float range = 5f;

	private float order = -5f;

	public GameObject walls;

	public GameObject bulletholes;

	public GameObject[] ParticleExamples;

	private int exampleIndex;

	private string randomSpawnsDelay = "0.5";

	private bool randomSpawns;

	private bool slowMo;

	private bool rotateCam = true;

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

	public GameObject m4;

	public GameObject m4fps;

	private bool rotate_m4 = true;
}
