using System;
using UnityEngine;

public class AILook : MonoBehaviour
{
	private void Start()
	{
		this.weapon = base.GetComponent<WeaponController>();
	}

	private void Update()
	{
		if (this.target)
		{
			Quaternion b = Quaternion.LookRotation(this.target.transform.position - base.transform.position);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 3f);
			Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
			float num = Vector3.Dot(normalized, base.transform.forward);
			if (num > 0.9f && this.weapon)
			{
				this.weapon.LaunchWeapon(this.indexWeapon);
			}
			if (Time.time > this.timeAIattack + 3f)
			{
				this.target = null;
			}
		}
		else
		{
			for (int i = 0; i < this.TargetTag.Length; i++)
			{
				if (GameObject.FindGameObjectsWithTag(this.TargetTag[i]).Length > 0)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag(this.TargetTag[i]);
					float num2 = 2.14748365E+09f;
					for (int j = 0; j < array.Length; j++)
					{
						float num3 = Vector3.Distance(array[j].transform.position, base.transform.position);
						if (num2 > num3)
						{
							num2 = num3;
							this.target = array[j];
							if (this.weapon)
							{
								this.indexWeapon = UnityEngine.Random.Range(0, this.weapon.WeaponLists.Length);
							}
							this.timeAIattack = Time.time;
						}
					}
				}
			}
		}
	}

	public string[] TargetTag = new string[]
	{
		"Enemy"
	};

	private int indexWeapon;

	private GameObject target;

	private WeaponController weapon;

	private float timeAIattack;
}
