using System;
using UnityEngine;
using UnityEngine.UI;

public class bl_MiniMapItem : MonoBehaviour
{
	private void Start()
	{
		if (bl_MiniMap.MapUIRoot != null)
		{
			this.CreateIcon();
		}
		else
		{
			UnityEngine.Debug.Log("You need a MiniMap in scene for use MiniMap Items.");
		}
	}

	private void CreateIcon()
	{
		this.cacheItem = UnityEngine.Object.Instantiate<GameObject>(this.GraphicPrefab);
		this.RectRoot = bl_MiniMap.MapUIRoot;
		this.Graphic = this.cacheItem.GetComponent<Image>();
		if (this.Icon != null)
		{
			this.Graphic.sprite = this.Icon;
			this.Graphic.color = this.IconColor;
		}
		this.cacheItem.transform.SetParent(this.RectRoot.transform, false);
		this.Graphic.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		if (this.Target == null)
		{
			this.Target = base.GetComponent<Transform>();
		}
		this.StartEffect();
		bl_IconItem component = this.cacheItem.GetComponent<bl_IconItem>();
		component.GetInfoItem(this.InfoItem);
	}

	private void FixedUpdate()
	{
		if (this.Target == null)
		{
			return;
		}
		if (this.Graphic == null)
		{
			return;
		}
		RectTransform component = this.Graphic.GetComponent<RectTransform>();
		Vector3 position = this.TargetPosition + this.OffSet;
		Vector2 vector = bl_MiniMap.MiniMapCamera.WorldToViewportPoint(position);
		Vector2 anchoredPosition = new Vector2(vector.x * this.RectRoot.sizeDelta.x - this.RectRoot.sizeDelta.x * 0.5f, vector.y * this.RectRoot.sizeDelta.y - this.RectRoot.sizeDelta.y * 0.5f);
		if (this.OffScreen)
		{
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -(this.RectRoot.sizeDelta.x * 0.5f - this.BorderOffScreen), this.RectRoot.sizeDelta.x * 0.5f - this.BorderOffScreen);
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -(this.RectRoot.sizeDelta.y * 0.5f - this.BorderOffScreen), this.RectRoot.sizeDelta.y * 0.5f - this.BorderOffScreen);
		}
		float num = this.Size;
		if (this.m_miniMap.useCompassRotation)
		{
			Vector3 zero = Vector3.zero;
			Vector3 direction = this.Target.position - this.m_miniMap.TargetPosition;
			Vector3 vector2 = bl_MiniMap.MiniMapCamera.transform.InverseTransformDirection(direction);
			vector2.z = 0f;
			vector2 = vector2.normalized / 2f;
			float num2 = Mathf.Abs(anchoredPosition.x);
			float num3 = Mathf.Abs(0.5f + vector2.x * this.m_miniMap.CompassSize);
			if (num2 >= num3)
			{
				zero.x = 0.5f + vector2.x * this.m_miniMap.CompassSize;
				zero.y = 0.5f + vector2.y * this.m_miniMap.CompassSize;
				anchoredPosition = zero;
				num = this.OffScreenSize;
			}
			else
			{
				num = this.Size;
			}
		}
		else if (anchoredPosition.x == this.RectRoot.sizeDelta.x * 0.5f - this.BorderOffScreen || anchoredPosition.y == this.RectRoot.sizeDelta.y * 0.5f - this.BorderOffScreen || anchoredPosition.x == -(this.RectRoot.sizeDelta.x * 0.5f) - this.BorderOffScreen || -anchoredPosition.y == this.RectRoot.sizeDelta.y * 0.5f - this.BorderOffScreen)
		{
			num = this.OffScreenSize;
		}
		else
		{
			num = this.Size;
		}
		component.anchoredPosition = anchoredPosition;
		component.sizeDelta = Vector2.Lerp(component.sizeDelta, new Vector2(num, num), Time.deltaTime * 8f);
		Quaternion identity = Quaternion.identity;
		identity.x = this.Target.rotation.x;
		component.localRotation = identity;
	}

	private void StartEffect()
	{
		Animation component = this.Graphic.GetComponent<Animation>();
		if (this.m_Effect == ItemEffect.Pulsing)
		{
			component.Play("Pulsing");
		}
		else if (this.m_Effect == ItemEffect.Fade)
		{
			component.Play("Fade");
		}
	}

	public void DestroyItem(bool inmediate)
	{
		if (this.Graphic == null)
		{
			UnityEngine.Debug.Log("Graphic Item of " + base.name + " not exist in scene");
			return;
		}
		if (this.DeathIcon == null || inmediate)
		{
			this.Graphic.GetComponent<bl_IconItem>().DestroyIcon(inmediate);
		}
		else
		{
			this.Graphic.GetComponent<bl_IconItem>().DestroyIcon(inmediate, this.DeathIcon);
		}
	}

	public void HideItem()
	{
		if (this.cacheItem != null)
		{
			this.cacheItem.SetActive(false);
		}
		else
		{
			UnityEngine.Debug.Log("There is no item to disable.");
		}
	}

	public void ShowItem()
	{
		if (this.cacheItem != null)
		{
			this.cacheItem.SetActive(true);
		}
		else
		{
			UnityEngine.Debug.Log("There is no item to active.");
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			if (this.Target == null)
			{
				return Vector3.zero;
			}
			return new Vector3(this.Target.position.x, 0f, this.Target.position.z);
		}
	}

	private bl_MiniMap m_miniMap
	{
		get
		{
			if (this._minimap == null)
			{
				this._minimap = this.cacheItem.transform.root.GetComponentInChildren<bl_MiniMap>();
			}
			return this._minimap;
		}
	}

	[Header("Target")]
	public GameObject GraphicPrefab;

	public Transform Target;

	public Vector3 OffSet = Vector3.zero;

	[Space(5f)]
	[Header("Icon")]
	public Sprite Icon;

	public Sprite DeathIcon;

	public Color IconColor = new Color(1f, 1f, 1f, 0.9f);

	public float Size = 20f;

	[Header("Icon Button")]
	public string InfoItem = "Info Icon here";

	[Space(5f)]
	[Header("Settings")]
	public bool OffScreen = true;

	public float BorderOffScreen = 0.01f;

	public float OffScreenSize = 10f;

	public ItemEffect m_Effect = ItemEffect.None;

	private Image Graphic;

	private RectTransform RectRoot;

	private GameObject cacheItem;

	private bl_MiniMap _minimap;
}
