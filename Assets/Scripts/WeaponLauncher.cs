using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponLauncher : WeaponBase
{
	private void Start()
	{
		if (!this.Owner)
		{
			this.Owner = base.transform.root.gameObject;
		}
		if (!this.audio)
		{
			this.audio = base.GetComponent<AudioSource>();
			if (!this.audio)
			{
				base.gameObject.AddComponent<AudioSource>();
			}
		}
	}

	private void Update()
	{
		if (this.TorqueObject)
		{
			this.TorqueObject.transform.Rotate(this.torqueTemp * Time.deltaTime);
			this.torqueTemp = Vector3.Lerp(this.torqueTemp, Vector3.zero, Time.deltaTime);
		}
		if (this.Seeker)
		{
			for (int i = 0; i < this.TargetTag.Length; i++)
			{
				if (GameObject.FindGameObjectsWithTag(this.TargetTag[i]).Length > 0)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag(this.TargetTag[i]);
					float num = 2.14748365E+09f;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j])
						{
							Vector3 normalized = (array[j].transform.position - base.transform.position).normalized;
							float num2 = Vector3.Dot(normalized, base.transform.forward);
							float num3 = Vector3.Distance(array[j].transform.position, base.transform.position);
							if (num2 >= this.AimDirection && this.DistanceLock > num3 && num > num3 && this.timetolockcount + this.TimeToLock < Time.time)
							{
								num = num3;
								this.target = array[j];
							}
						}
					}
				}
			}
		}
		if (this.target)
		{
			float num4 = Vector3.Distance(base.transform.position, this.target.transform.position);
			Vector3 normalized2 = (this.target.transform.position - base.transform.position).normalized;
			float num5 = Vector3.Dot(normalized2, base.transform.forward);
			if (num4 > this.DistanceLock || num5 <= this.AimDirection)
			{
				this.Unlock();
			}
		}
		if (this.Reloading)
		{
			this.ReloadingProcess = 1f / this.ReloadTime * (this.reloadTimeTemp + this.ReloadTime - Time.time);
			if (Time.time >= this.reloadTimeTemp + this.ReloadTime)
			{
				this.Reloading = false;
				if (this.SoundReloaded && this.audio)
				{
					this.audio.PlayOneShot(this.SoundReloaded);
				}
				this.Ammo = this.AmmoMax;
			}
		}
		else if (this.Ammo <= 0)
		{
			this.Unlock();
			this.Reloading = true;
			this.reloadTimeTemp = Time.time;
			if (this.SoundReloading && this.audio)
			{
				this.audio.PlayOneShot(this.SoundReloading);
			}
		}
	}

	private void DrawTargetLockon(Transform aimtarget, bool locked)
	{
		if (!this.ShowHUD)
		{
			return;
		}
		if (Camera.current)
		{
			Vector3 normalized = (aimtarget.position - Camera.current.GetComponent<Camera>().transform.position).normalized;
			float num = Vector3.Dot(normalized, Camera.current.GetComponent<Camera>().transform.forward);
			if (num > 0.5f)
			{
				Vector3 vector = Camera.current.GetComponent<Camera>().WorldToScreenPoint(aimtarget.transform.position);
				float f = Vector3.Distance(base.transform.position, aimtarget.transform.position);
				if (locked)
				{
					if (this.TargetLockedTexture)
					{
						GUI.DrawTexture(new Rect(vector.x - (float)(this.TargetLockedTexture.width / 2), (float)Screen.height - vector.y - (float)(this.TargetLockedTexture.height / 2), (float)this.TargetLockedTexture.width, (float)this.TargetLockedTexture.height), this.TargetLockedTexture);
					}
					GUI.Label(new Rect(vector.x + 40f, (float)Screen.height - vector.y, 200f, 30f), string.Concat(new object[]
					{
						aimtarget.name,
						" ",
						Mathf.Floor(f),
						"m."
					}));
				}
				else if (this.TargetLockOnTexture)
				{
					GUI.DrawTexture(new Rect(vector.x - (float)(this.TargetLockOnTexture.width / 2), (float)Screen.height - vector.y - (float)(this.TargetLockOnTexture.height / 2), (float)this.TargetLockOnTexture.width, (float)this.TargetLockOnTexture.height), this.TargetLockOnTexture);
				}
			}
		}
	}

	private void OnGUI()
	{
		if (this.Seeker)
		{
			if (this.target)
			{
				this.DrawTargetLockon(this.target.transform, true);
			}
			for (int i = 0; i < this.TargetTag.Length; i++)
			{
				if (GameObject.FindGameObjectsWithTag(this.TargetTag[i]).Length > 0)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag(this.TargetTag[i]);
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j])
						{
							Vector3 normalized = (array[j].transform.position - base.transform.position).normalized;
							float num = Vector3.Dot(normalized, base.transform.forward);
							if (num >= this.AimDirection)
							{
								float num2 = Vector3.Distance(array[j].transform.position, base.transform.position);
								if (this.DistanceLock > num2)
								{
									this.DrawTargetLockon(array[j].transform, false);
								}
							}
						}
					}
				}
			}
		}
	}

	private void Unlock()
	{
		this.timetolockcount = Time.time;
		this.target = null;
	}

	public void Shoot()
	{
		if (this.InfinityAmmo)
		{
			this.Ammo = 1;
		}
		if (this.Ammo > 0 && Time.time > this.nextFireTime + this.FireRate)
		{
			this.nextFireTime = Time.time;
			this.torqueTemp = this.TorqueSpeedAxis;
			this.Ammo--;
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			if (this.MissileOuter.Length > 0)
			{
				rotation = this.MissileOuter[this.currentOuter].transform.rotation;
				position = this.MissileOuter[this.currentOuter].transform.position;
			}
			if (this.MissileOuter.Length > 0)
			{
				this.currentOuter++;
				if (this.currentOuter >= this.MissileOuter.Length)
				{
					this.currentOuter = 0;
				}
			}
			if (this.Muzzle)
			{
				MonoBehaviour.print("firebuttonin");
				GameObject.FindWithTag("controls").GetComponent<RoboTransformControler>().onfirewithweaponsystem();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Muzzle, position, rotation);
				gameObject.transform.parent = base.transform;
				UnityEngine.Object.Destroy(gameObject, this.MuzzleLifeTime);
				if (this.MissileOuter.Length > 0)
				{
					gameObject.transform.parent = this.MissileOuter[this.currentOuter].transform;
				}
			}
			for (int i = 0; i < this.NumBullet; i++)
			{
				if (this.Missile)
				{
					Vector3 b = new Vector3(UnityEngine.Random.Range(-this.Spread, this.Spread), UnityEngine.Random.Range(-this.Spread, this.Spread), UnityEngine.Random.Range(-this.Spread, this.Spread)) / 100f;
					Vector3 vector = base.transform.forward + b;
					MonoBehaviour.print("firebuttonin2");
					GameObject.FindWithTag("controls").GetComponent<RoboTransformControler>().onfirewithweaponsystem();
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.Missile, position, rotation);
					if (gameObject2.GetComponent<DamageBase>())
					{
						gameObject2.GetComponent<DamageBase>().Owner = this.Owner;
					}
					if (gameObject2.GetComponent<WeaponBase>())
					{
						gameObject2.GetComponent<WeaponBase>().Owner = this.Owner;
						gameObject2.GetComponent<WeaponBase>().Target = this.target;
					}
					gameObject2.transform.forward = vector;
					if (this.RigidbodyProjectile && gameObject2.GetComponent<Rigidbody>())
					{
						if (this.Owner != null && this.Owner.GetComponent<Rigidbody>())
						{
							gameObject2.GetComponent<Rigidbody>().velocity = this.Owner.GetComponent<Rigidbody>().velocity;
						}
						gameObject2.GetComponent<Rigidbody>().AddForce(vector * this.ForceShoot);
					}
				}
			}
			if (this.Shell)
			{
				Transform transform = base.transform;
				if (this.ShellOuter.Length > 0)
				{
					transform = this.ShellOuter[this.currentOuter];
				}
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.Shell, transform.position, UnityEngine.Random.rotation);
				UnityEngine.Object.Destroy(gameObject3.gameObject, this.ShellLifeTime);
				if (gameObject3.GetComponent<Rigidbody>())
				{
					gameObject3.GetComponent<Rigidbody>().AddForce(transform.forward * (float)this.ShellOutForce);
				}
			}
			if (this.SoundGun.Length > 0 && this.audio)
			{
				this.audio.PlayOneShot(this.SoundGun[UnityEngine.Random.Range(0, this.SoundGun.Length)]);
			}
			this.nextFireTime += this.FireRate;
		}
	}

	public Transform[] MissileOuter;

	public GameObject Missile;

	public float FireRate = 0.1f;

	public float Spread = 1f;

	public float ForceShoot = 8000f;

	public int NumBullet = 1;

	public int Ammo = 10;

	public int AmmoMax = 10;

	public bool InfinityAmmo;

	public float ReloadTime = 1f;

	public bool ShowHUD = true;

	public Texture2D TargetLockOnTexture;

	public Texture2D TargetLockedTexture;

	public float DistanceLock = 200f;

	public float TimeToLock = 2f;

	public float AimDirection = 0.8f;

	public bool Seeker;

	public GameObject Shell;

	public float ShellLifeTime = 4f;

	public Transform[] ShellOuter;

	public int ShellOutForce = 300;

	public GameObject Muzzle;

	public float MuzzleLifeTime = 2f;

	public AudioClip[] SoundGun;

	public AudioClip SoundReloading;

	public AudioClip SoundReloaded;

	private float timetolockcount;

	private float nextFireTime;

	private GameObject target;

	private Vector3 torqueTemp;

	private float reloadTimeTemp;

	private AudioSource audio;

	[HideInInspector]
	public bool Reloading;

	[HideInInspector]
	public float ReloadingProcess;

	private int currentOuter;
}
