using System;
using UnityEngine;

public class RuntimeExample : MonoBehaviour
{
	private void OnGUI()
	{
		this.Example1();
		this.Example2();
		GUI.Label(new Rect(10f, (float)(Screen.height - 30), (float)Screen.width, 30f), "Example 3: UserInput - Press the left/right arrow key to move the capsule.");
	}

	private void Example1()
	{
		GUI.Label(new Rect(10f, 5f, 120f, 30f), "Example 1");
		if (!this.walkerObj1 && GUI.Button(new Rect(10f, 30f, 130f, 25f), "Instantiate Objects"))
		{
			this.walkerObj1 = UnityEngine.Object.Instantiate<GameObject>(this.walkerPrefab, this.position1.position, Quaternion.identity);
			this.walkerObj1.name = "Soldier@" + DateTime.Now.TimeOfDay;
			this.newPath1 = UnityEngine.Object.Instantiate<GameObject>(this.pathPrefab, this.position1.position, Quaternion.identity);
			this.newPath1.name = "RuntimePath@" + DateTime.Now.TimeOfDay;
			WaypointManager.AddPath(this.newPath1);
		}
		if (this.walkerObj1 && !this.walkeriM1 && GUI.Button(new Rect(140f, 30f, 130f, 25f), "Start Movement"))
		{
			this.walkeriM1 = this.walkerObj1.GetComponent<iMove>();
			this.walkeriM1.SetPath(WaypointManager.Paths[this.newPath1.name]);
		}
		if (this.newPath1 && GUI.Button(new Rect(10f, 30f, 130f, 25f), "Reposition Path"))
		{
			Transform transform = this.newPath1.transform;
			if (transform.position == this.position1.position)
			{
				transform.position = this.position2.position;
			}
			else
			{
				transform.position = this.position1.position;
			}
		}
		if (this.walkerObj1 && this.walkeriM1 && GUI.Button(new Rect(140f, 30f, 130f, 25f), "Reset Walker"))
		{
			this.walkeriM1.Reset();
			this.walkeriM1 = null;
		}
		if (this.walkerObj1 && this.walkeriM1 && GUI.Button(new Rect(270f, 30f, 100f, 25f), "Stop Walker"))
		{
			this.walkeriM1.Stop();
		}
		if (this.walkerObj1 && this.walkeriM1 && GUI.Button(new Rect(370f, 30f, 100f, 25f), "Continue Walk"))
		{
			this.walkeriM1.moveToPath = true;
			this.walkeriM1.StartMove();
		}
	}

	private void Example2()
	{
		GUI.Label(new Rect(10f, 60f, 120f, 30f), "Example 2");
		if (!this.walkerObj2 && GUI.Button(new Rect(10f, 85f, 130f, 25f), "Instantiate Objects"))
		{
			this.walkerObj2 = UnityEngine.Object.Instantiate<GameObject>(this.walkerPrefab, this.position3.position, Quaternion.identity);
			this.walkerObj2.name = "Soldier@" + DateTime.Now.TimeOfDay;
			this.newPath2 = UnityEngine.Object.Instantiate<GameObject>(this.pathPrefab, this.position3.position, Quaternion.identity);
			this.newPath2.name = "RuntimePath@" + DateTime.Now.TimeOfDay;
			WaypointManager.AddPath(this.newPath2);
		}
		if (this.walkerObj2 && !this.walkeriM2 && GUI.Button(new Rect(140f, 85f, 130f, 25f), "Start Movement"))
		{
			this.walkeriM2 = this.walkerObj2.GetComponent<iMove>();
			this.walkeriM2.SetPath(WaypointManager.Paths[this.newPath2.name]);
		}
		if (this.newPath1 && this.newPath2 && GUI.Button(new Rect(10f, 85f, 130f, 25f), "Switch Path"))
		{
			if (!this.walkeriM2)
			{
				this.walkeriM2 = this.walkerObj2.GetComponent<iMove>();
			}
			this.walkeriM2.moveToPath = true;
			if (this.walkeriM2.pathContainer == WaypointManager.Paths[this.newPath1.name])
			{
				this.walkeriM2.SetPath(WaypointManager.Paths[this.newPath2.name]);
			}
			else
			{
				this.walkeriM2.SetPath(WaypointManager.Paths[this.newPath1.name]);
			}
		}
	}

	public GameObject walkerPrefab;

	public GameObject pathPrefab;

	public Transform position1;

	public Transform position2;

	public Transform position3;

	private GameObject walkerObj1;

	private GameObject newPath1;

	private iMove walkeriM1;

	private GameObject walkerObj2;

	private GameObject newPath2;

	private iMove walkeriM2;
}
