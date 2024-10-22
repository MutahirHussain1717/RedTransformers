using System;
using UnityEngine;

public class RCCEnterExitPlayer : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject = UnityEngine.Object.FindObjectOfType<RCC_Camera>().gameObject;
		gameObject.SetActive(false);
	}

	private void Update()
	{
		Vector3 direction = base.transform.TransformDirection(Vector3.forward);
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, this.maxRayDistance))
		{
			if (raycastHit.transform.GetComponentInParent<RCC_CarControllerV3>())
			{
				this.showGui = true;
				if (UnityEngine.Input.GetKeyDown(RCC_Settings.Instance.enterExitVehicleKB))
				{
					raycastHit.transform.GetComponentInParent<RCC_CarControllerV3>().SendMessage("Act", base.GetComponentInParent<CharacterController>().gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this.showGui = false;
			}
		}
		else
		{
			this.showGui = false;
		}
	}

	private void OnGUI()
	{
		if (this.showGui)
		{
			GUI.Label(new Rect((float)Screen.width - (float)Screen.width / 1.7f, (float)Screen.height - (float)Screen.height / 1.2f, 800f, 100f), "Press ''" + RCC_Settings.Instance.enterExitVehicleKB.ToString() + "'' key to Get In");
		}
	}

	public float maxRayDistance = 2f;

	private bool showGui;
}
