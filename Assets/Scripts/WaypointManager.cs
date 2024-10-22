using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
	private void Awake()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				WaypointManager.AddPath(transform.gameObject);
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
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		HOTween.showPathGizmos = true;
	}

	public static void AddPath(GameObject path)
	{
		if (path.name.Contains("Clone"))
		{
			path.name = path.name.Replace("(Clone)", string.Empty);
		}
		if (WaypointManager.Paths.ContainsKey(path.name))
		{
			UnityEngine.Debug.LogWarning("Called AddPath() but Scene already contains Path " + path.name + ".");
			return;
		}
		PathManager componentInChildren = path.GetComponentInChildren<PathManager>();
		if (componentInChildren == null)
		{
			UnityEngine.Debug.LogWarning("Called AddPath() but Transform " + path.name + " has no PathManager attached.");
			return;
		}
		WaypointManager.Paths.Add(path.name, componentInChildren);
	}

	private void OnDestroy()
	{
		WaypointManager.Paths.Clear();
	}

	public static readonly Dictionary<string, PathManager> Paths = new Dictionary<string, PathManager>();
}
