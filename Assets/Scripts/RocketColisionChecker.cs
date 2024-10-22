using System;
using UnityEngine;

public class RocketColisionChecker : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision colinfo)
	{
		if (colinfo.gameObject.tag == "TrafficCar")
		{
			RoboTransformControler.carshitcount++;
			colinfo.gameObject.SetActive(false);
			UnityEngine.Object.Instantiate<GameObject>(this.bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dummyCar, colinfo.transform.position, colinfo.transform.rotation);
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			foreach (Transform transform in componentsInChildren)
			{
				transform.transform.parent = null;
				transform.transform.parent = null;
				transform.transform.parent = null;
				transform.transform.parent = null;
				transform.transform.parent = null;
				transform.transform.parent = null;
				transform.gameObject.AddComponent<Rigidbody>();
				transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(-10f, 10f)), ForceMode.Impulse);
				UnityEngine.Object.Destroy(transform.gameObject, 2f);
			}
		}
	}

	public void show_damgaecar(GameObject colinfo)
	{
		RoboTransformControler.carshitcount++;
		colinfo.gameObject.SetActive(false);
		UnityEngine.Object.Instantiate<GameObject>(this.bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dummyCar, colinfo.transform.position, colinfo.transform.rotation);
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			transform.transform.parent = null;
			transform.transform.parent = null;
			transform.transform.parent = null;
			transform.transform.parent = null;
			transform.transform.parent = null;
			transform.transform.parent = null;
			transform.gameObject.AddComponent<Rigidbody>();
			transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(-10f, 10f)), ForceMode.Impulse);
			UnityEngine.Object.Destroy(transform.gameObject, 2f);
		}
	}

	public GameObject bulletCarExplosion;

	public GameObject dummyCar;

	public GameObject bulletImpactFX;
}
