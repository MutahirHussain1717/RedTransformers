using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Demo Manager")]
public class RCC_Demo : MonoBehaviour
{
	public void SelectVehicle(int index)
	{
		this.selectedCarIndex = index;
	}

	public void Spawn()
	{
		RCC_CarControllerV3[] array = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
		Vector3 vector = default(Vector3);
		Quaternion rotation = default(Quaternion);
		if (array != null && array.Length > 0)
		{
			foreach (RCC_CarControllerV3 rcc_CarControllerV in array)
			{
				if (rcc_CarControllerV.canControl)
				{
					vector = rcc_CarControllerV.transform.position;
					rotation = rcc_CarControllerV.transform.rotation;
					break;
				}
			}
		}
		if (vector == Vector3.zero && UnityEngine.Object.FindObjectOfType<RCC_Camera>())
		{
			vector = UnityEngine.Object.FindObjectOfType<RCC_Camera>().transform.position;
			rotation = UnityEngine.Object.FindObjectOfType<RCC_Camera>().transform.rotation;
		}
		rotation.x = 0f;
		rotation.z = 0f;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].canControl)
			{
				UnityEngine.Object.Destroy(array[j].gameObject);
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.selectableVehicles[this.selectedCarIndex].gameObject, vector + Vector3.up, rotation);
		gameObject.GetComponent<RCC_CarControllerV3>().canControl = true;
		if (UnityEngine.Object.FindObjectOfType<RCC_Camera>())
		{
			UnityEngine.Object.FindObjectOfType<RCC_Camera>().SetPlayerCar(gameObject);
		}
	}

	public void SelectBehavior(int index)
	{
		this.selectedBehaviorIndex = index;
	}

	public void InitBehavior()
	{
		switch (this.selectedBehaviorIndex)
		{
		case 0:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Simulator;
			this.RestartScene();
			break;
		case 1:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Racing;
			this.RestartScene();
			break;
		case 2:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.SemiArcade;
			this.RestartScene();
			break;
		case 3:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Drift;
			this.RestartScene();
			break;
		case 4:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Fun;
			this.RestartScene();
			break;
		case 5:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Custom;
			this.RestartScene();
			break;
		}
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public RCC_CarControllerV3[] selectableVehicles;

	public int selectedCarIndex;

	public int selectedBehaviorIndex;
}
