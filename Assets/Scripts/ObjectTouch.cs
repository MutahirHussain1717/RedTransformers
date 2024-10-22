using System;
using System.Collections;
using UnityEngine;

public class ObjectTouch : MonoBehaviour
{
	private void OnEnable()
	{
		EasyTouch.On_TouchStart += this.On_TouchStart;
		EasyTouch.On_SimpleTap += this.On_SimpleTap;
		EasyTouch.On_LongTap += this.On_LongTap;
		EasyTouch.On_DragStart += this.On_DragStart;
		EasyTouch.On_Drag += this.On_Drag;
		EasyTouch.On_DragEnd += this.On_DragEnd;
		EasyTouch.On_PinchIn += this.On_PinchIn;
		EasyTouch.On_PinchOut += this.On_PinchOut;
	}

	private void OnDisable()
	{
		this.UnsubscribeEvent();
	}

	private void OnDestroy()
	{
		this.UnsubscribeEvent();
	}

	private void UnsubscribeEvent()
	{
		EasyTouch.On_TouchStart -= this.On_TouchStart;
		EasyTouch.On_SimpleTap -= this.On_SimpleTap;
		EasyTouch.On_LongTap -= this.On_LongTap;
		EasyTouch.On_DragStart -= this.On_DragStart;
		EasyTouch.On_Drag -= this.On_Drag;
		EasyTouch.On_DragEnd -= this.On_DragEnd;
		EasyTouch.On_PinchIn -= this.On_PinchIn;
		EasyTouch.On_PinchOut -= this.On_PinchOut;
	}

	private void Start()
	{
		this.cam = Camera.main;
	}

	private void FixedUpdate()
	{
		Vector2 vector = this.cam.WorldToScreenPoint(base.GetComponent<Rigidbody>().position);
		if (vector.x > (float)Screen.width || vector.y < 0f || vector.y > (float)Screen.height)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (vector.x < base.transform.localScale.x / 2f)
		{
			base.GetComponent<Rigidbody>().AddForce(base.GetComponent<Rigidbody>().velocity * -100f);
		}
	}

	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	private void On_SimpleTap(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			GameObject gameObject = null;
			base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.name == "ring")
					{
						gameObject = transform.gameObject;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (gameObject == null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Ring01"), base.transform.position, Quaternion.identity) as GameObject;
				gameObject2.transform.localScale = base.transform.localScale * 1.5f;
				gameObject2.AddComponent<SlowRotate>();
				gameObject2.GetComponent<Renderer>().material.SetColor("_TintColor", base.GetComponent<Renderer>().material.GetColor("_TintColor"));
				gameObject2.transform.parent = base.transform;
				gameObject2.name = "ring";
			}
			else
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	private void On_LongTap(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			GameObject gameObject = null;
			base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.name == "ring")
					{
						gameObject = transform.gameObject;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (gameObject != null)
			{
				gameObject.GetComponent<SlowRotate>().rotateSpeed *= 1.1f;
			}
		}
	}

	private void On_DragStart(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(8f, false);
			this.deltaPosition = touchToWordlPoint - base.GetComponent<Rigidbody>().position;
			base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
	}

	private void On_Drag(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			Vector3 touchToWordlPoint = gesture.GetTouchToWordlPoint(8f, false);
			base.GetComponent<Rigidbody>().position = touchToWordlPoint - this.deltaPosition;
		}
	}

	private void On_DragEnd(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			base.GetComponent<Rigidbody>().AddForce(gesture.deltaPosition * gesture.swipeLength / 10f);
		}
	}

	private void On_PinchIn(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			float num = Time.deltaTime * gesture.deltaPinch;
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(localScale.x - num, localScale.y - num, 1f);
		}
	}

	private void On_PinchOut(Gesture gesture)
	{
		if (gesture.pickObject == base.gameObject)
		{
			float num = Time.deltaTime * gesture.deltaPinch;
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(localScale.x + num, localScale.y + num, 1f);
		}
	}

	private Camera cam;

	private Vector3 deltaPosition;
}
