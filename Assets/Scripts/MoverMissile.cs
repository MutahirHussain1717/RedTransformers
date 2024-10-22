using System;
using UnityEngine;

public class MoverMissile : WeaponBase
{
	private void Start()
	{
		this.timeCount = Time.time;
		UnityEngine.Object.Destroy(base.gameObject, this.LifeTime);
	}

	private void FixedUpdate()
	{
		base.GetComponent<Rigidbody>().velocity = new Vector3(base.transform.forward.x * this.Speed * Time.fixedDeltaTime, base.transform.forward.y * this.Speed * Time.fixedDeltaTime, base.transform.forward.z * this.Speed * Time.fixedDeltaTime);
		base.GetComponent<Rigidbody>().velocity += new Vector3(UnityEngine.Random.Range(-this.Noise.x, this.Noise.x), UnityEngine.Random.Range(-this.Noise.y, this.Noise.y), UnityEngine.Random.Range(-this.Noise.z, this.Noise.z));
		if (this.Speed < this.SpeedMax)
		{
			this.Speed += this.SpeedMult * Time.fixedDeltaTime;
		}
	}

	private void Update()
	{
		if (Time.time >= this.timeCount + this.LifeTime - 0.5f && base.GetComponent<Damage>())
		{
			base.GetComponent<Damage>().Active();
		}
		if (this.Target)
		{
			Quaternion b = Quaternion.LookRotation(this.Target.transform.position - base.transform.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * this.Damping);
			Vector3 normalized = (this.Target.transform.position - base.transform.position).normalized;
			float num = Vector3.Dot(normalized, base.transform.forward);
			if (num < this.TargetLockDirection)
			{
				this.Target = null;
			}
		}
		if (this.Seeker)
		{
			if (this.timetorock > this.DurationLock)
			{
				if (!this.locked && !this.Target)
				{
					float num2 = 2.14748365E+09f;
					for (int i = 0; i < this.TargetTag.Length; i++)
					{
						if (GameObject.FindGameObjectsWithTag(this.TargetTag[i]).Length > 0)
						{
							GameObject[] array = GameObject.FindGameObjectsWithTag(this.TargetTag[i]);
							for (int j = 0; j < array.Length; j++)
							{
								if (array[j])
								{
									Vector3 normalized2 = (array[j].transform.position - base.transform.position).normalized;
									float num3 = Vector3.Dot(normalized2, base.transform.forward);
									float num4 = Vector3.Distance(array[j].transform.position, base.transform.position);
									if (num3 >= this.TargetLockDirection && (float)this.DistanceLock > num4)
									{
										if (num2 > num4)
										{
											num2 = num4;
											this.Target = array[j];
										}
										this.locked = true;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				this.timetorock++;
			}
			if (!this.Target)
			{
				this.locked = false;
			}
		}
	}

	public float Damping = 3f;

	public float Speed = 80f;

	public float SpeedMax = 80f;

	public float SpeedMult = 1f;

	public Vector3 Noise = new Vector3(20f, 20f, 20f);

	public float TargetLockDirection = 0.5f;

	public int DistanceLock = 70;

	public int DurationLock = 40;

	public bool Seeker;

	public float LifeTime = 5f;

	private bool locked;

	private int timetorock;

	private float timeCount;
}
