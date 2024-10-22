using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class bl_SnapShot : MonoBehaviour
{
	public static string FolderPath
	{
		get
		{
			return bl_SnapShot._folderPath;
		}
	}

	private string SnapshotName(int width, int height)
	{
		string loadedLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		return string.Format(this.GetFullFolderPath() + "MapSnapshot_{0}_{1}x{2}_{3}.png", new object[]
		{
			loadedLevelName,
			width,
			height,
			DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
		});
	}

	private string GetFullFolderPath()
	{
		return Application.dataPath + bl_SnapShot._folderPath;
	}

	[ContextMenu("Take Snap Shot")]
	public void TakeSnapshot()
	{
	}

	public int resWidth = 2048;

	public int resHeight = 2048;

	public int msaa = 1;

	[Space(7f)]
	[InspectorButton("TakeSnapshot")]
	public string m_TakeSnapShot = string.Empty;

	private static string _folderPath = "/UGUIMiniMap/Content/Art/SnapShots/";
}
