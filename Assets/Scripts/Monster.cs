using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_TouchStart += this.On_TouchStart;
	}

	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject != null)
		{
			UnityEngine.Object.Destroy(gesture.pickObject);
			this.CreateMonster();
		}
	}

	private void CreateMonster()
	{
		if (this.enemy != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.enemy, Camera.main.ScreenToWorldPoint(new Vector3((float)UnityEngine.Random.Range(0, Screen.width), (float)Screen.height, Camera.main.nearClipPlane * 2f)), Quaternion.identity);
		}
	}

	public GameObject enemy;
}
