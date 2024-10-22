using System;
using UnityEngine;

public class GUIOptions : MonoBehaviour
{
	private void Start()
	{
		this.spawner = UnityEngine.Object.FindObjectOfType<TSTrafficSpawner>();
		this.totalCars = (float)this.spawner.TrafficCars.Length;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, (float)(Screen.height - 45), 350f, 25f), "Target Amount of cars: " + this.spawner.amount.ToString());
		GUI.Label(new Rect(10f, (float)(Screen.height - 25), 350f, 25f), "Actual Amount of cars on scene: " + (this.totalCars - (float)this.spawner.totalFarCars).ToString());
		this.spawner.amount = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10f, (float)(Screen.height - 55), 250f, 25f), (float)this.spawner.amount, 0f, this.totalCars));
		GUI.Label(new Rect(10f, (float)(Screen.height - 110), 350f, 25f), "Respawn Time " + this.spawner.respawnUpSideDownTime.ToString());
		this.spawner.respawnUpSideDownTime = GUI.HorizontalSlider(new Rect(120f, (float)(Screen.height - 110), 100f, 25f), this.spawner.respawnUpSideDownTime, 0f, 20f);
		this.spawner.respawnIfUpSideDown = GUI.Toggle(new Rect(10f, (float)(Screen.height - 90), 250f, 25f), this.spawner.respawnIfUpSideDown, "Auto Respawn upside down cars?");
	}

	private TSTrafficSpawner spawner;

	private float totalCars;
}
