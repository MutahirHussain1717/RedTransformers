using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class NGUITools
{
	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	public static bool fileAccess
	{
		get
		{
			return Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.OSXPlayer;
		}
	}

	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				AudioListener[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (NGUITools.GetActive(array[i]))
						{
							NGUITools.mListener = array[i];
							break;
						}
					}
				}
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = (UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera);
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				AudioSource audioSource = NGUITools.mListener.GetComponent<AudioSource>();
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.priority = 50;
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	public static T[] FindActive<T>() where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera camera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.main;
		if (camera && (camera.cullingMask & num) != 0)
		{
			return camera;
		}
		Camera[] array = new Camera[Camera.allCamerasCount];
		int allCameras = Camera.GetAllCameras(array);
		for (int j = 0; j < allCameras; j++)
		{
			camera = array[j];
			if (camera && camera.enabled && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		return null;
	}

	public static void AddWidgetCollider(GameObject go)
	{
		NGUITools.AddWidgetCollider(go, false);
	}

	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
				return;
			}
			if (component != null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			UICamera uicamera = UICamera.FindCameraForLayer(go.layer);
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.World_2D || uicamera.eventType == UICamera.EventType.UI_2D))
			{
				boxCollider2D = go.AddComponent<BoxCollider2D>();
				boxCollider2D.isTrigger = true;
				UIWidget component2 = go.GetComponent<UIWidget>();
				if (component2 != null)
				{
					component2.autoResizeBoxCollider = true;
				}
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			UIWidget component3 = go.GetComponent<UIWidget>();
			if (component3 != null)
			{
				component3.autoResizeBoxCollider = true;
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
		}
	}

	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			BoxCollider component = go.GetComponent<BoxCollider>();
			if (component != null)
			{
				NGUITools.UpdateWidgetCollider(component, considerInactive);
				return;
			}
			BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
			if (component2 != null)
			{
				NGUITools.UpdateWidgetCollider(component2, considerInactive);
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector4 drawRegion = component.drawRegion;
				if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
				{
					Vector4 drawingDimensions = component.drawingDimensions;
					box.center = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
					box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
				}
				else
				{
					Vector3[] localCorners = component.localCorners;
					box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
					box.size = localCorners[2] - localCorners[0];
				}
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector3[] localCorners = component.localCorners;
				box.offset = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.offset = bounds.center;
				box.size = new Vector2(bounds.size.x, bounds.size.y);
			}
		}
	}

	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = int.MaxValue;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
			i++;
		}
		return num + 1;
	}

	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uiwidget = componentsInChildren[i];
				if (!(uiwidget.cachedGameObject != go) || (!(uiwidget.GetComponent<Collider>() != null) && !(uiwidget.GetComponent<Collider2D>() != null)))
				{
					num = Mathf.Max(num, uiwidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel uipanel = go.GetComponent<UIPanel>();
		if (uipanel != null)
		{
			foreach (UIPanel uipanel2 in go.GetComponentsInChildren<UIPanel>(true))
			{
				uipanel2.depth += adjustment;
			}
			return 1;
		}
		uipanel = NGUITools.FindInParents<UIPanel>(go);
		if (uipanel == null)
		{
			return 0;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uiwidget = componentsInChildren2[j];
			if (!(uiwidget.panel != uipanel))
			{
				uiwidget.depth += adjustment;
			}
			j++;
		}
		return 2;
	}

	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	public static void NormalizeWidgetDepths()
	{
		NGUITools.NormalizeWidgetDepths(NGUITools.FindActive<UIWidget>());
	}

	public static void NormalizeWidgetDepths(GameObject go)
	{
		NGUITools.NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int num = list.Length;
		if (num > 0)
		{
			if (NGUITools._003C_003Ef__mg_0024cache0 == null)
			{
				NGUITools._003C_003Ef__mg_0024cache0 = new Comparison<UIWidget>(UIWidget.FullCompareFunc);
			}
			Array.Sort<UIWidget>(list, NGUITools._003C_003Ef__mg_0024cache0);
			int num2 = 0;
			int depth = list[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uiwidget = list[i];
				if (uiwidget.depth == depth)
				{
					uiwidget.depth = num2;
				}
				else
				{
					depth = uiwidget.depth;
					num2 = (uiwidget.depth = num2 + 1);
				}
			}
		}
	}

	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			UIPanel[] array2 = array;
			if (NGUITools._003C_003Ef__mg_0024cache1 == null)
			{
				NGUITools._003C_003Ef__mg_0024cache1 = new Comparison<UIPanel>(UIPanel.CompareFunc);
			}
			Array.Sort<UIPanel>(array2, NGUITools._003C_003Ef__mg_0024cache1);
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uipanel = array[i];
				if (uipanel.depth == depth)
				{
					uipanel.depth = num2;
				}
				else
				{
					depth = uipanel.depth;
					num2 = (uipanel.depth = num2 + 1);
				}
			}
		}
	}

	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uiroot = (!(trans != null)) ? null : NGUITools.FindInParents<UIRoot>(trans.gameObject);
		if (uiroot == null && UIRoot.list.Count > 0)
		{
			foreach (UIRoot uiroot2 in UIRoot.list)
			{
				if (uiroot2.gameObject.layer == layer)
				{
					uiroot = uiroot2;
					break;
				}
			}
		}
		if (uiroot != null)
		{
			UICamera componentInChildren = uiroot.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.GetComponent<Camera>().orthographic == advanced3D)
			{
				trans = null;
				uiroot = null;
			}
		}
		if (uiroot == null)
		{
			GameObject gameObject = NGUITools.AddChild(null, false);
			uiroot = gameObject.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject.layer = layer;
			if (advanced3D)
			{
				gameObject.name = "UI Root (3D)";
				uiroot.scalingStyle = UIRoot.Scaling.Constrained;
			}
			else
			{
				gameObject.name = "UI Root";
				uiroot.scalingStyle = UIRoot.Scaling.Flexible;
			}
		}
		UIPanel uipanel = uiroot.GetComponentInChildren<UIPanel>();
		if (uipanel == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uiroot.gameObject.layer;
			foreach (Camera camera in array)
			{
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = NGUITools.AddChild<Camera>(uiroot.gameObject, false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = ((!flag) ? CameraClearFlags.Color : CameraClearFlags.Depth);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uipanel = uiroot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (NGUITools.IsChild(trans, uipanel.transform))
			{
				uipanel = trans.gameObject.AddComponent<UIPanel>();
			}
			else
			{
				trans.parent = uipanel.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				NGUITools.SetChildLayer(uipanel.cachedTransform, uipanel.cachedGameObject.layer);
			}
		}
		return uipanel;
	}

	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			NGUITools.SetChildLayer(child, layer);
		}
	}

	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent, undo);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int depth = NGUITools.CalculateNextDepth(go);
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static T AddWidget<T>(GameObject go, int depth) where T : UIWidget
	{
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uispriteData = (!(atlas != null)) ? null : atlas.GetSprite(spriteName);
		UISprite uisprite = NGUITools.AddWidget<UISprite>(go);
		uisprite.type = ((uispriteData != null && uispriteData.hasBorder) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple);
		uisprite.atlas = atlas;
		uisprite.spriteName = spriteName;
		return uisprite;
	}

	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		for (;;)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component == null)
		{
			Transform parent = go.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return component;
	}

	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)((object)null);
		}
		return trans.GetComponentInParent<T>();
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				if (obj is GameObject)
				{
					GameObject gameObject = obj as GameObject;
					gameObject.transform.parent = null;
				}
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, false);
	}

	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		if (compatibilityMode)
		{
			int i = 0;
			int childCount = t.childCount;
			while (i < childCount)
			{
				Transform child = t.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					return;
				}
				i++;
			}
			int j = 0;
			int childCount2 = t.childCount;
			while (j < childCount2)
			{
				Transform child2 = t.GetChild(j);
				NGUITools.Activate(child2, true);
				j++;
			}
		}
	}

	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.transform, compatibilityMode);
				NGUITools.CallCreatePanel(go.transform);
			}
			else
			{
				NGUITools.Deactivate(go.transform);
			}
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				Transform child = transform.GetChild(i);
				NGUITools.Activate(child);
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount2 = transform.childCount;
			while (j < childCount2)
			{
				Transform child2 = transform.GetChild(j);
				NGUITools.Deactivate(child2);
				j++;
			}
		}
	}

	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(Behaviour mb)
	{
		return mb && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(GameObject go)
	{
		return go && go.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			NGUITools.SetLayer(child.gameObject, layer);
			i++;
		}
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(path);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.orthographic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float y = -orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num3 = screenSize.x / screenSize.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			NGUITools.mSides[0] = rotation * new Vector3(num, 0f, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(0f, y2, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, 0f, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(0f, y, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float depth = Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f);
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.orthographic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float y = -orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num3 = screenSize.x / screenSize.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			NGUITools.mSides[0] = rotation * new Vector3(num, y, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(num, y2, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, y2, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(num2, y, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string text = obj.GetType().ToString();
		int num = text.LastIndexOf('/');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		return (!string.IsNullOrEmpty(method)) ? (text + "/" + method) : text;
	}

	public static void Execute<T>(GameObject go, string funcName) where T : Component
	{
		T[] components = go.GetComponents<T>();
		foreach (T t in components)
		{
			MethodInfo method = t.GetType().GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				method.Invoke(t, null);
			}
		}
	}

	public static void ExecuteAll<T>(GameObject root, string funcName) where T : Component
	{
		NGUITools.Execute<T>(root, funcName);
		Transform transform = root.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			NGUITools.ExecuteAll<T>(transform.GetChild(i).gameObject, funcName);
			i++;
		}
	}

	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		NGUITools.ExecuteAll<UIWidget>(root, "Start");
		NGUITools.ExecuteAll<UIPanel>(root, "Start");
		NGUITools.ExecuteAll<UIWidget>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "LateUpdate");
	}

	public static Vector2 screenSize
	{
		get
		{
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	private static AudioListener mListener;

	private static bool mLoaded = false;

	private static float mGlobalVolume = 1f;

	private static Vector3[] mSides = new Vector3[4];

	[CompilerGenerated]
	private static Comparison<UIWidget> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Comparison<UIPanel> _003C_003Ef__mg_0024cache1;
}
