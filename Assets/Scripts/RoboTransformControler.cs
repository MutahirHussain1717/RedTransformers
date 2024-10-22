using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoboTransformControler : MonoBehaviour
{
    public GameObject ObjectPP;
	private void Start()
	{
        if (Application.platform == RuntimePlatform.Android)
        {
            Instantiate(ObjectPP);
        }
		this.totalcarhit = 0;
		RoboTransformControler.carshitcount = 0;
		RoboTransformControler.flymode = false;
		this.check_objrotation = false;
		this.current_obj = this.players[0];
		this.flyuibtns[1].SetActive(false);
		this.Mapplayer.GetComponent<bl_MiniMap>().setmapplayer_obj(this.current_obj);
		this.Mapplayer.GetComponent<bl_MMCompass>().setmapplayer_obj(this.current_obj);
		this.levelselection();
		this.settext();
		base.Invoke("offplayerrcccanvas", 1f);
	}

	private void Update()
	{
		if (this.check_objrotation)
		{
		}
		if (RoboTransformControler.flymode)
		{
			this.current_obj.transform.localEulerAngles = new Vector3(this.p_joystick.GetComponent<EasyJoystick>().JoystickAxis.y * -30f, this.current_obj.transform.localEulerAngles.y, this.p_joystick.GetComponent<EasyJoystick>().JoystickAxis.x * -45f);
		}
	}

	public void settext()
	{
		this.totalstatementtext.text = "Shoot " + this.totalcarhit.ToString() + " cars";
		this.leveltext.text = "Level " + (GlobalScripts.CurrLevelIndex + 1).ToString();
		this.TotalCarstext.text = RoboTransformControler.carshitcount.ToString() + "/" + this.totalcarhit.ToString();
	}

	public GameObject getcurrent_objplayer()
	{
		return this.current_obj;
	}

	public void onflymode(bool flybool)
	{
		RoboTransformControler.flymode = flybool;
		if (RoboTransformControler.flymode)
		{
			this.off_alltranslate();
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ClearUI();
			this.p_joystick.SetActive(true);
			this.flyuibtns[0].SetActive(false);
			this.flyuibtns[1].SetActive(true);
			if (this.current_obj.GetComponent<Rigidbody>() != null)
			{
				this.current_obj.GetComponent<Rigidbody>().useGravity = false;
				this.current_obj.GetComponent<Rigidbody>().isKinematic = false;
			}
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
			if (this.current_obj.name == "bike")
			{
				this.playercanvas[1].SetActive(false);
				GameObject.FindWithTag("MainCamera").GetComponent<BikeCamera>().enabled = false;
				this.current_obj.GetComponent<BikeControl>().enabled = false;
				BikeflyContrloler.bike_trans_check = true;
				BikeflyContrloler.bike_trans_up_check = true;
				base.Invoke("off_playeruptranslate", 2f);
				GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[2].transform, 27f, 8f);
			}
			else if (this.current_obj.name == "car" || this.current_obj.name == "truck")
			{
				this.playercanvas[0].SetActive(false);
				this.current_obj.GetComponent<RCC_CarControllerV3>().enabled = false;
				Carflycontroler.car_trans_check = true;
				Carflycontroler.car_trans_up_check = true;
				base.Invoke("off_playeruptranslate", 2f);
				GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[1].transform, 27f, 6f);
			}
			else if (this.current_obj.name == "plane")
			{
				PlaneControler.plane_trans_check = true;
				PlaneControler.plane_trans_up_check = true;
				base.Invoke("off_playeruptranslate", 2f);
			}
			else if (this.current_obj.name == "robot")
			{
				Roboflycontroler.car_trans_check = true;
				Roboflycontroler.car_trans_up_check = true;
				base.Invoke("off_playeruptranslate", 2f);
			}
			else if (this.current_obj.name == "helicopter")
			{
				Helicoptercontroler.heli_trans_check = true;
				Helicoptercontroler.heli_trans_up_check = true;
				base.Invoke("off_playeruptranslate", 2f);
			}
		}
		else if (!RoboTransformControler.flymode)
		{
			this.flyuibtns[0].SetActive(true);
			this.flyuibtns[1].SetActive(false);
			this.off_alltranslate();
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
			this.p_joystick.SetActive(true);
			if (this.current_obj.GetComponent<Rigidbody>() != null)
			{
				this.current_obj.GetComponent<Rigidbody>().useGravity = true;
				this.current_obj.GetComponent<Rigidbody>().isKinematic = false;
			}
			if (this.current_obj.name == "bike")
			{
				this.playercanvas[1].SetActive(true);
				this.p_joystick.SetActive(false);
				GameObject.FindWithTag("MainCamera").GetComponent<BikeCamera>().enabled = true;
				GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
				this.current_obj.GetComponent<BikeControl>().enabled = true;
			}
			else if (this.current_obj.name == "car" || this.current_obj.name == "truck")
			{
				this.playercanvas[0].SetActive(true);
				this.p_joystick.SetActive(false);
				this.current_obj.GetComponent<RCC_CarControllerV3>().enabled = true;
				GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[1].transform, 27f, 6f);
			}
			else if (this.current_obj.name == "plane")
			{
				this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
				this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
			}
			else if (this.current_obj.name == "robot")
			{
				this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
				this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
			}
			else if (this.current_obj.name == "helicopter")
			{
				this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
				this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
			}
		}
	}

	public void off_playeruptranslate()
	{
		if (this.current_obj.name == "bike")
		{
			BikeflyContrloler.bike_trans_up_check = false;
		}
		else if (this.current_obj.name == "car" || this.current_obj.name == "truck")
		{
			Carflycontroler.car_trans_up_check = false;
		}
		else if (this.current_obj.name == "plane")
		{
			PlaneControler.plane_trans_up_check = false;
		}
		else if (this.current_obj.name == "robot")
		{
			Roboflycontroler.car_trans_up_check = false;
		}
		else if (this.current_obj.name == "helicopter")
		{
			Helicoptercontroler.heli_trans_up_check = false;
		}
		this.p_joystick.SetActive(true);
		this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.Translate;
		this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Y;
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
	}

	public void off_playertranslate()
	{
		if (this.current_obj.name == "bike")
		{
			BikeflyContrloler.bike_trans_check = false;
		}
		else if (this.current_obj.name == "car" || this.current_obj.name == "truck")
		{
			Carflycontroler.car_trans_check = false;
		}
		else if (this.current_obj.name == "plane")
		{
			PlaneControler.plane_trans_check = false;
		}
		else if (this.current_obj.name == "robot")
		{
			Roboflycontroler.car_trans_check = false;
		}
		else if (this.current_obj.name == "helicopter")
		{
			Helicoptercontroler.heli_trans_check = false;
		}
	}

	public void off_alltranslate()
	{
		Roboflycontroler.car_trans_check = false;
		Roboflycontroler.car_trans_up_check = false;
		PlaneControler.plane_trans_check = false;
		PlaneControler.plane_trans_up_check = false;
		Carflycontroler.car_trans_check = false;
		Carflycontroler.car_trans_up_check = false;
		BikeflyContrloler.bike_trans_check = false;
		BikeflyContrloler.bike_trans_up_check = false;
		Helicoptercontroler.heli_trans_check = false;
		Helicoptercontroler.heli_trans_up_check = false;
	}

	public void curr_P_flysetting()
	{
	}

	public void curr_P_nonflysetting()
	{
	}

	public void levelselection()
	{
		if (GlobalScripts.CurrLevelIndex == 0)
		{
			this.totalcarhit = 10;
		}
		else if (GlobalScripts.CurrLevelIndex == 1)
		{
			this.totalcarhit = 15;
		}
		else if (GlobalScripts.CurrLevelIndex == 2)
		{
			this.totalcarhit = 20;
		}
		else if (GlobalScripts.CurrLevelIndex == 3)
		{
			this.totalcarhit = 25;
		}
		else if (GlobalScripts.CurrLevelIndex == 4)
		{
			this.totalcarhit = 30;
		}
		else if (GlobalScripts.CurrLevelIndex == 5)
		{
			this.totalcarhit = 20;
		}
		else if (GlobalScripts.CurrLevelIndex == 6)
		{
			this.totalcarhit = 25;
		}
		else if (GlobalScripts.CurrLevelIndex == 7)
		{
			this.totalcarhit = 30;
		}
		else if (GlobalScripts.CurrLevelIndex == 8)
		{
			this.totalcarhit = 35;
		}
		else if (GlobalScripts.CurrLevelIndex == 9)
		{
			this.totalcarhit = 40;
		}
	}

	public void transform_robotoobj(string setobj)
	{
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ClearUI();
		this.resetpositon = this.current_obj.transform.position;
		this.resetrotation = this.current_obj.transform.eulerAngles;
		this.p_joystick.SetActive(false);
		this.current_obj.SetActive(false);
		this.Animrobot.SetActive(true);
		this.Animrobot.transform.position = this.resetpositon;
		if (setobj == "car" || setobj == "truck")
		{
			this.current_obj = this.players[1];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(false);
			this.playercanvas[0].SetActive(true);
		}
		else if (setobj == "bike")
		{
			this.current_obj = this.players[2];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(false);
			this.playercanvas[1].SetActive(true);
		}
		else if (setobj == "helicopter")
		{
			this.current_obj = this.players[3];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(false);
		}
		else if (setobj == "plane")
		{
			this.current_obj = this.players[4];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(false);
		}
		this.current_obj.transform.eulerAngles = this.resetrotation;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.playercams[1].transform, 13f, 4f);
		base.StartCoroutine("starttransformation", setobj);
	}

	public void start_transformationto_Obj(string setobj)
	{
		if ((setobj == "plane" || setobj == "helicopter") && GlobalScripts.CurrLevelIndex >= 0 && GlobalScripts.CurrLevelIndex <= 4)
		{
			this.UnlockHeliplane_text.SetActive(true);
			base.Invoke("off_heliplanetext", 2f);
		}
		else if (this.current_obj.name == "robot" && this.current_obj.name != setobj)
		{
			this.onflymode(false);
			base.Invoke("playtransformsound", 2f);
			base.Invoke("playtransformsound", 5f);
			base.Invoke("playtransformsound", 6f);
			this.transform_robotoobj(setobj);
		}
		else if (((this.current_obj.name == "car" || this.current_obj.name == "truck") && this.current_obj.name != setobj) || (this.current_obj.name == "bike" && this.current_obj.name != setobj) || (this.current_obj.name == "plane" && this.current_obj.name != setobj) || (this.current_obj.name == "helicopter" && this.current_obj.name != setobj) || (this.current_obj.name == "robot" && this.current_obj.name != setobj))
		{
			this.onflymode(false);
			base.Invoke("playtransformsound", 2f);
			this.transform_objto_obj(setobj);
		}
	}

	public void playtransformsound()
	{
		this.audio.PlayOneShot(this.transformsound);
	}

	public void off_heliplanetext()
	{
		this.UnlockHeliplane_text.SetActive(false);
	}

	public void transform_objto_obj(string setobj)
	{
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ClearUI();
		this.resetpositon = this.current_obj.transform.position;
		this.resetrotation = this.current_obj.transform.eulerAngles;
		this.players[0].transform.position = this.resetpositon;
		this.p_joystick.SetActive(false);
		this.current_obj.SetActive(true);
		if (this.current_obj.GetComponent<Rigidbody>() != null)
		{
			this.current_obj.GetComponent<Rigidbody>().isKinematic = true;
		}
		for (int i = 0; i < this.playercanvas.Length; i++)
		{
			this.playercanvas[i].SetActive(false);
		}
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
		GameObject.FindWithTag("MainCamera").GetComponent<BikeCamera>().enabled = false;
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.playercams[1].transform, 13f, 4f);
		base.StartCoroutine("starttransformation2", setobj);
	}

	public void start_transformto_Robo()
	{
	}

	public void onfire()
	{
		this.current_obj.transform.Find("rocketnozzle").gameObject.GetComponent<Rocketbombing>().onfirerocket();
		if (this.current_obj.name == "robot")
		{
			this.current_obj.GetComponent<Animator>().SetTrigger("fireplay");
		}
		base.Invoke("updatecarhittext", 6f);
	}

	public void onfirewithweaponsystem()
	{
		if (this.current_obj.name == "robot")
		{
			this.current_obj.GetComponent<Animator>().SetTrigger("fireplay");
		}
		base.Invoke("updatecarhittext", 6f);
	}

	public void updatecarhittext()
	{
		this.TotalCarstext.text = RoboTransformControler.carshitcount.ToString() + "/" + this.totalcarhit.ToString();
		if (RoboTransformControler.carshitcount >= this.totalcarhit)
		{
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().Dia_Success();
		}
	}

	public void offplayerrcccanvas()
	{
		this.playercanvas[0].SetActive(false);
	}

	private IEnumerator starttransformation(string setobj)
	{
		yield return new WaitForSeconds(0f);
		this.Animrobot.GetComponent<Tranformationplayer>().play_transformation();
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < this.transformeffects.Length; i++)
		{
			this.transformeffects[i].transform.position = this.resetpositon;
			this.transformeffects[i].transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 8f, this.resetpositon.z);
			this.transformeffects[i].SetActive(true);
		}
		yield return new WaitForSeconds(1.5f);
		this.Animrobot.GetComponent<Tranformationplayer>().off_transformation();
		this.Animrobot.SetActive(false);
		for (int j = 0; j < this.transformeffects.Length; j++)
		{
			this.transformeffects[j].SetActive(false);
		}
		if (setobj == "car" || setobj == "truck")
		{
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[1].transform, 5.41f, 1.79f);
			this.current_obj.SetActive(true);
			this.current_obj.GetComponent<RCC_CarControllerV3>().canControl = true;
		}
		else if (setobj == "bike")
		{
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
			GameObject.FindWithTag("MainCamera").GetComponent<BikeCamera>().enabled = true;
			this.current_obj.SetActive(true);
		}
		else if (setobj == "helicopter")
		{
			this.current_obj.SetActive(true);
			this.current_obj.transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 24f, this.resetpositon.z);
			this.p_joystick.SetActive(true);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[3].transform, 27.5f, 10f);
			this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
			this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
		}
		else if (setobj == "plane")
		{
			this.current_obj.SetActive(true);
			this.p_joystick.SetActive(true);
			this.current_obj.transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 34f, this.resetpositon.z);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[4].transform, 15f, 5f);
			this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
			this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
		}
		this.Mapplayer.GetComponent<bl_MiniMap>().setmapplayer_obj(this.current_obj);
		this.Mapplayer.GetComponent<bl_MMCompass>().setmapplayer_obj(this.current_obj);
		if (this.current_obj.GetComponent<Rigidbody>() != null)
		{
			this.current_obj.GetComponent<Rigidbody>().isKinematic = false;
		}
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
		yield break;
	}

	private IEnumerator starttransformation2(string setobj)
	{
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < this.transformeffects.Length; i++)
		{
			this.transformeffects[i].transform.position = this.resetpositon;
			this.transformeffects[i].transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 2f, this.resetpositon.z);
			this.transformeffects[i].SetActive(true);
		}
		yield return new WaitForSeconds(1.5f);
		for (int j = 0; j < this.transformeffects.Length; j++)
		{
			this.transformeffects[j].SetActive(false);
		}
		this.current_obj.SetActive(false);
		if (setobj == "car" || setobj == "truck")
		{
			this.current_obj = this.players[1];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(true);
			this.playercanvas[0].SetActive(true);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[1].transform, 5.41f, 1.79f);
			this.current_obj.GetComponent<RCC_CarControllerV3>().canControl = true;
		}
		else if (setobj == "bike")
		{
			this.current_obj = this.players[2];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(true);
			this.playercanvas[1].SetActive(true);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
			GameObject.FindWithTag("MainCamera").GetComponent<BikeCamera>().enabled = true;
			this.current_obj.SetActive(true);
		}
		else if (setobj == "helicopter")
		{
			this.current_obj = this.players[3];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(true);
			this.current_obj.transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 24f, this.resetpositon.z);
			this.p_joystick.SetActive(true);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[3].transform, 27.5f, 10f);
			this.p_joystick.GetComponent<EasyJoystick>().speed.y = 30f;
			this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
			this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
		}
		else if (setobj == "plane")
		{
			this.current_obj = this.players[4];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(true);
			this.p_joystick.SetActive(true);
			this.current_obj.transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 34f, this.resetpositon.z);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.players[4].transform, 15f, 5f);
			this.p_joystick.GetComponent<EasyJoystick>().speed.y = 50f;
			this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
			this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
		}
		else if (setobj == "robot")
		{
			this.current_obj = this.players[0];
			this.current_obj.transform.position = this.resetpositon;
			this.current_obj.SetActive(true);
			this.current_obj.transform.position = new Vector3(this.resetpositon.x, this.resetpositon.y + 2f, this.resetpositon.z);
			this.p_joystick.SetActive(true);
			GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().CameraSwitch(this.playercams[0].transform, 27.5f, 10f);
			this.p_joystick.GetComponent<EasyJoystick>().speed.y = 20f;
			this.p_joystick.GetComponent<EasyJoystick>().YTI = EasyJoystick.PropertiesInfluenced.TranslateLocal;
			this.p_joystick.GetComponent<EasyJoystick>().yAI = EasyJoystick.AxisInfluenced.Z;
		}
		this.current_obj.transform.eulerAngles = this.resetrotation;
		if (this.current_obj.GetComponent<Rigidbody>() != null)
		{
			this.current_obj.GetComponent<Rigidbody>().isKinematic = true;
		}
		this.Mapplayer.GetComponent<bl_MiniMap>().setmapplayer_obj(this.current_obj);
		this.Mapplayer.GetComponent<bl_MMCompass>().setmapplayer_obj(this.current_obj);
		GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs>().ShowUI();
		yield break;
	}

	public GameObject Animrobot;

	public GameObject Mapplayer;

	public GameObject UnlockHeliplane_text;

	public GameObject[] players;

	public GameObject[] playercams;

	public GameObject[] playercanvas;

	public GameObject[] transformeffects;

	public GameObject[] flyuibtns;

	public GameObject p_joystick;

	private GameObject current_obj;

	public static bool flymode;

	public static int carshitcount;

	private Vector3 resetpositon;

	private Vector3 resetrotation;

	private bool check_objrotation;

	public Text TotalCarstext;

	public Text leveltext;

	public Text totalstatementtext;

	private int totalcarhit;

	public AudioSource audio;

	public AudioClip transformsound;
}
