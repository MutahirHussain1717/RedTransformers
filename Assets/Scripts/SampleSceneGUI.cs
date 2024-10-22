using System;
using UnityEngine;

[Serializable]
public class SampleSceneGUI : MonoBehaviour
{
	public virtual void OnGUI()
	{
		GUI.skin = this.mySkin;
		GUI.Label(new Rect((float)70, (float)10, (float)100, (float)20), "test");
		if (GUI.Button(new Rect((float)10, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_Fire")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect01, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_FireRain")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect02, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_Ground")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect03, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_Ice")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect04, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_IceRain")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect05, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_Lightning")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect06, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)190, (float)40, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_LightningRain")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect07, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)10, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_Rain")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect08, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "ArrowFX_SkullRain")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect09, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "BarrierFX")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect10, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "ChargeFX_Normal")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect11, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "ChargeFX_Wind01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect12, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "ChargeFX_Wind02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect13, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)190, (float)70, (float)20, (float)20), new GUIContent(string.Empty, "CircleFX_Dark")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect14, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)10, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "CircleFX_Fire")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect15, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "CircleFX_Ice")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect16, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "CircleFX_Lightning")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect17, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "CircleFX_Line")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect18, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "CloudFlashFX")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect19, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "ExplosionFX")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect20, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)190, (float)100, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Dark01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect21, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)10, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Dark02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect22, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Dark03")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect23, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Fire01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect24, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Fire02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect25, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Ice01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect26, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Ice02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect27, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)190, (float)130, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Ice03")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect28, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)10, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Lightning01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect29, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Lightning02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect30, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Lightning03")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect31, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Line01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect32, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Line02")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect33, new Vector3((float)0, 0.3f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "GroundFX_Line03")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect34, new Vector3((float)0, (float)0, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)190, (float)160, (float)20, (float)20), new GUIContent(string.Empty, "HitFX_Dark")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect35, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)10, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "HitFX_Fire")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect36, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)40, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "HitFX_Ice")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect37, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)70, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "HitFX_Lightning")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect38, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)100, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "HitFX_Normal01")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect39, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)130, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "LightDustFX")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect40, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		if (GUI.Button(new Rect((float)160, (float)190, (float)20, (float)20), new GUIContent(string.Empty, "LineSphereFX")))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.effect41, new Vector3((float)0, 1.5f, (float)0), Quaternion.Euler((float)0, (float)0, (float)0));
		}
		GUI.Label(new Rect((float)10, (float)(Screen.height - 30), (float)200, (float)30), GUI.tooltip);
	}

	public virtual void Main()
	{
	}

	public GUISkin mySkin;

	public GameObject effect01;

	public GameObject effect02;

	public GameObject effect03;

	public GameObject effect04;

	public GameObject effect05;

	public GameObject effect06;

	public GameObject effect07;

	public GameObject effect08;

	public GameObject effect09;

	public GameObject effect10;

	public GameObject effect11;

	public GameObject effect12;

	public GameObject effect13;

	public GameObject effect14;

	public GameObject effect15;

	public GameObject effect16;

	public GameObject effect17;

	public GameObject effect18;

	public GameObject effect19;

	public GameObject effect20;

	public GameObject effect21;

	public GameObject effect22;

	public GameObject effect23;

	public GameObject effect24;

	public GameObject effect25;

	public GameObject effect26;

	public GameObject effect27;

	public GameObject effect28;

	public GameObject effect29;

	public GameObject effect30;

	public GameObject effect31;

	public GameObject effect32;

	public GameObject effect33;

	public GameObject effect34;

	public GameObject effect35;

	public GameObject effect36;

	public GameObject effect37;

	public GameObject effect38;

	public GameObject effect39;

	public GameObject effect40;

	public GameObject effect41;
}
