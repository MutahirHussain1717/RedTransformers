using System;
using UnityEngine;
using UnityEngine.UI;

public class bl_MiniMap : MonoBehaviour
{
	private void Awake()
	{
		bl_MiniMap.MiniMapCamera = this.MMCamera;
		bl_MiniMap.MapUIRoot = this.MMUIRoot;
		this.DefaultRotationMode = this.DynamicRotation;
		this.DeafultMapRot = this.m_Transform.eulerAngles;
		this.DefaultRotationCircle = this.useCompassRotation;
		if (this.m_Type == bl_MiniMap.RenderType.Picture)
		{
			this.CreateMapPlane();
		}
		if (this.m_Mode == bl_MiniMap.RenderMode.Mode3D)
		{
			this.ConfigureCamera3D();
		}
		this.height = PlayerPrefs.GetFloat("MinimapCameraHeight", this.height);
	}

	private void CreateMapPlane()
	{
		string a = LayerMask.LayerToName(10);
		if (a != "MiniMap")
		{
			UnityEngine.Debug.LogError("MiniMap has not defined in Layer Mask list, please added this in place No. 10");
			this.MMUIRoot.gameObject.SetActive(false);
			base.enabled = false;
		}
		if (this.MapTexture == null)
		{
			UnityEngine.Debug.LogError("Map Texture not been allocated.");
			return;
		}
		Vector3 localPosition = this.WorldSpace.localPosition;
		Vector3 vector = this.WorldSpace.sizeDelta;
		this.MMCamera.cullingMask = 1024;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MapPlane);
		gameObject.transform.localPosition = localPosition;
		gameObject.transform.localScale = new Vector3(vector.x, 10f, vector.y) / 10f;
		gameObject.GetComponent<Renderer>().material = this.CreateMaterial();
	}

	private void ConfigureCamera3D()
	{
		Camera camera = (!(Camera.main != null)) ? Camera.current : Camera.main;
		if (camera == null)
		{
			UnityEngine.Debug.LogError("Not to have found a camera to configure");
			return;
		}
		camera.nearClipPlane = 0.015f;
		this.m_Canvas.planeDistance = 0.1f;
	}

	private void Update()
	{
		if (this.m_Target == null)
		{
			return;
		}
		if (this.MMCamera == null)
		{
			return;
		}
		this.Inputs();
		this.PositionControll();
		this.RotationControll();
		this.MapSize();
	}

	private void PositionControll()
	{
		Vector3 position = this.m_Transform.position;
		position.x = this.Target.position.x;
		position.z = this.Target.position.z;
		position.y = this.maxHeight + this.minHeight / 2f + this.Target.position.y * 2f;
		this.m_Transform.position = position;
	}

	private void RotationControll()
	{
		RectTransform component = this.PlayerIcon.GetComponent<RectTransform>();
		if (this.DynamicRotation)
		{
			Vector3 eulerAngles = this.m_Transform.eulerAngles;
			eulerAngles.y = this.Target.eulerAngles.y;
			if (this.SmoothRotation)
			{
				if (this.m_Mode == bl_MiniMap.RenderMode.Mode2D)
				{
					component.rotation = Quaternion.identity;
				}
				else
				{
					component.localRotation = Quaternion.identity;
				}
				if (this.m_Transform.eulerAngles.y != eulerAngles.y)
				{
					float num = eulerAngles.y - this.m_Transform.eulerAngles.y;
					if (num > 180f || num < -180f)
					{
						this.m_Transform.eulerAngles = eulerAngles;
					}
				}
				this.m_Transform.eulerAngles = Vector3.Lerp(base.transform.eulerAngles, eulerAngles, Time.deltaTime * this.LerpRotation);
			}
			else
			{
				this.m_Transform.eulerAngles = eulerAngles;
			}
		}
		else
		{
			this.m_Transform.eulerAngles = this.DeafultMapRot;
			if (this.m_Mode == bl_MiniMap.RenderMode.Mode2D)
			{
				Vector3 zero = Vector3.zero;
				zero.z = -this.Target.eulerAngles.y;
				component.eulerAngles = zero;
			}
			else
			{
				Vector3 localEulerAngles = this.Target.localEulerAngles;
				Vector3 zero2 = Vector3.zero;
				zero2.z = -localEulerAngles.y;
				component.localEulerAngles = zero2;
			}
		}
	}

	private void Inputs()
	{
		if (UnityEngine.Input.GetKeyDown(this.ToogleKey))
		{
			this.ToggleSize();
		}
		if (UnityEngine.Input.GetKeyDown(this.DecreaseHeightKey) && this.height < this.maxHeight)
		{
			this.ChangeHeight(true);
		}
		if (UnityEngine.Input.GetKeyDown(this.IncreaseHeightKey) && this.height > this.minHeight)
		{
			this.ChangeHeight(false);
		}
	}

	private void MapSize()
	{
		RectTransform mmuiroot = this.MMUIRoot;
		if (bl_MiniMap.isFullScreen)
		{
			if (this.DynamicRotation)
			{
				this.DynamicRotation = false;
				this.ResetMapRotation();
			}
			mmuiroot.sizeDelta = Vector2.Lerp(mmuiroot.sizeDelta, this.FullMapSize, Time.deltaTime * this.LerpTransition);
			mmuiroot.anchoredPosition = Vector3.Lerp(mmuiroot.anchoredPosition, this.FullMapPosition, Time.deltaTime * this.LerpTransition);
			mmuiroot.localEulerAngles = Vector3.Lerp(mmuiroot.localEulerAngles, this.FullMapRotation, Time.deltaTime * this.LerpTransition);
		}
		else
		{
			if (this.DynamicRotation != this.DefaultRotationMode)
			{
				this.DynamicRotation = this.DefaultRotationMode;
			}
			mmuiroot.sizeDelta = Vector2.Lerp(mmuiroot.sizeDelta, this.MiniMapSize, Time.deltaTime * this.LerpTransition);
			mmuiroot.anchoredPosition = Vector3.Lerp(mmuiroot.anchoredPosition, this.MiniMapPosition, Time.deltaTime * this.LerpTransition);
			mmuiroot.localEulerAngles = Vector3.Lerp(mmuiroot.localEulerAngles, this.MiniMapRotation, Time.deltaTime * this.LerpTransition);
		}
		this.MMCamera.orthographicSize = Mathf.Lerp(this.MMCamera.orthographicSize, this.height, Time.deltaTime * this.LerpHeight);
	}

	private void ToggleSize()
	{
		bl_MiniMap.isFullScreen = !bl_MiniMap.isFullScreen;
		if (bl_MiniMap.isFullScreen)
		{
			this.height = this.maxHeight;
			this.useCompassRotation = false;
			if (this.m_maskHelper)
			{
				this.m_maskHelper.OnChange(true);
			}
		}
		else
		{
			this.height = PlayerPrefs.GetFloat("MinimapCameraHeight", this.height);
			if (this.useCompassRotation != this.DefaultRotationCircle)
			{
				this.useCompassRotation = this.DefaultRotationCircle;
			}
			if (this.m_maskHelper)
			{
				this.m_maskHelper.OnChange(false);
			}
		}
	}

	public void ChangeHeight(bool b)
	{
		if (b)
		{
			if (this.height + (float)this.scrollSensitivity <= this.maxHeight)
			{
				this.height += (float)this.scrollSensitivity;
			}
			else
			{
				this.height = this.maxHeight;
			}
		}
		else if (this.height - (float)this.scrollSensitivity >= this.minHeight)
		{
			this.height -= (float)this.scrollSensitivity;
		}
		else
		{
			this.height = this.minHeight;
		}
		PlayerPrefs.SetFloat("MinimapCameraHeight", this.height);
	}

	public Material CreateMaterial()
	{
		Shader shader = Shader.Find("Legacy Shaders/VertexLit");
		Material material = new Material(shader);
		material.mainTexture = this.MapTexture;
		material.color = this.TintColor;
		material.SetColor("_SpecColor", this.SpecularColor);
		material.SetColor("_Emission", this.EmessiveColor);
		return material;
	}

	private void ResetMapRotation()
	{
		this.m_Transform.eulerAngles = new Vector3(90f, 0f, 0f);
	}

	public void RotationMap(bool mode)
	{
		if (bl_MiniMap.isFullScreen)
		{
			return;
		}
		this.DynamicRotation = mode;
		this.DefaultRotationMode = this.DynamicRotation;
	}

	public void ChangeMapSize(bool fullscreen)
	{
		bl_MiniMap.isFullScreen = fullscreen;
	}

	[ContextMenu("GetMiniMapRect")]
	private void GetMiniMapSize()
	{
		this.MiniMapSize = this.MMUIRoot.sizeDelta;
		this.MiniMapPosition = this.MMUIRoot.anchoredPosition;
		this.MiniMapRotation = this.MMUIRoot.eulerAngles;
	}

	[ContextMenu("GetFullMapRect")]
	private void GetFullMapSize()
	{
		this.FullMapSize = this.MMUIRoot.sizeDelta;
		this.FullMapPosition = this.MMUIRoot.anchoredPosition;
		this.FullMapRotation = this.MMUIRoot.eulerAngles;
	}

	public Transform Target
	{
		get
		{
			if (this.m_Target != null)
			{
				return this.m_Target.GetComponent<Transform>();
			}
			return base.GetComponent<Transform>();
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			Vector3 result = Vector3.zero;
			if (this.m_Target != null)
			{
				result = this.m_Target.transform.position;
			}
			return result;
		}
	}

	public void setmapplayer_obj(GameObject setplayer)
	{
		this.m_Target = setplayer;
	}

	private Transform m_Transform
	{
		get
		{
			if (this.t == null)
			{
				this.t = base.GetComponent<Transform>();
			}
			return this.t;
		}
	}

	private bl_MaskHelper m_maskHelper
	{
		get
		{
			if (this._maskHelper == null)
			{
				this._maskHelper = base.transform.root.GetComponentInChildren<bl_MaskHelper>();
			}
			return this._maskHelper;
		}
	}

	[Separator("General Settings")]
	public GameObject m_Target;

	[Tooltip("Keycode to toggle map size mode (world and mini map)")]
	public KeyCode ToogleKey = KeyCode.E;

	public Camera MMCamera;

	public bl_MiniMap.RenderType m_Type = bl_MiniMap.RenderType.Picture;

	public bl_MiniMap.RenderMode m_Mode;

	[Separator("UI")]
	public Canvas m_Canvas;

	public RectTransform MMUIRoot;

	public Image PlayerIcon;

	[Separator("Height")]
	[Tooltip("How much should we move for each small movement on the mouse wheel?")]
	public int scrollSensitivity = 3;

	[Tooltip("Maximum heights that the camera can reach.")]
	public float maxHeight = 80f;

	[Tooltip("Minimum heights that the camera can reach.")]
	public float minHeight = 5f;

	public KeyCode IncreaseHeightKey = KeyCode.KeypadPlus;

	public KeyCode DecreaseHeightKey = KeyCode.KeypadMinus;

	private float height = 30f;

	[Range(1f, 15f)]
	[Tooltip("Smooth speed to height change.")]
	public float LerpHeight = 8f;

	[Separator("Rotation")]
	[Tooltip("Compass rotation for circle maps, rotate icons around pivot.")]
	public bool useCompassRotation;

	[Range(25f, 500f)]
	[Tooltip("Size of Compass rotation diametre.")]
	public float CompassSize = 175f;

	[Tooltip("Should the minimap rotate with the player?")]
	public bool DynamicRotation = true;

	[Tooltip("this work only is dynamic rotation.")]
	public bool SmoothRotation = true;

	[Range(1f, 15f)]
	public float LerpRotation = 8f;

	[Separator("Map Rect")]
	[Tooltip("Position for MiniMap.")]
	public Vector3 MiniMapPosition = Vector2.zero;

	[Tooltip("Rotation for MiniMap. (3D Mode)")]
	public Vector3 MiniMapRotation = Vector3.zero;

	[Tooltip("Size of MiniMap.")]
	public Vector2 MiniMapSize = Vector2.zero;

	[Space(5f)]
	[Tooltip("Position for World Map.")]
	public Vector3 FullMapPosition = Vector2.zero;

	[Tooltip("Rotation for World Map.")]
	public Vector3 FullMapRotation = Vector3.zero;

	[Tooltip("Size of World Map.")]
	public Vector2 FullMapSize = Vector2.zero;

	[Space(5f)]
	[Tooltip("Smooth Speed for MiniMap World Map transition.")]
	public float LerpTransition = 7f;

	[Space(5f)]
	[InspectorButton("GetMiniMapSize")]
	public string GetMiniMapRect = string.Empty;

	[InspectorButton("GetFullMapSize")]
	public string GetWorldRect = string.Empty;

	[Separator("Picture Mode Settings")]
	[Tooltip("Texture for MiniMap renderer, you can take a snaphot from map.")]
	public Texture MapTexture;

	public Color TintColor = new Color(1f, 1f, 1f, 0.9f);

	public Color SpecularColor = new Color(1f, 1f, 1f, 0.9f);

	public Color EmessiveColor = new Color(0f, 0f, 0f, 0.9f);

	[Space(3f)]
	public GameObject MapPlane;

	public RectTransform WorldSpace;

	public static bool isFullScreen;

	public static Camera MiniMapCamera;

	public static RectTransform MapUIRoot;

	public const string MiniMapLayer = "MiniMap";

	private bool DefaultRotationMode;

	private Vector3 DeafultMapRot = Vector3.zero;

	private bool DefaultRotationCircle;

	private const string MMHeightKey = "MinimapCameraHeight";

	private Transform t;

	private bl_MaskHelper _maskHelper;

	[Serializable]
	public enum RenderType
	{
		RealTime,
		Picture
	}

	[Serializable]
	public enum RenderMode
	{
		Mode2D,
		Mode3D
	}
}
