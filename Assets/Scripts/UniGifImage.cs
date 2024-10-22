using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UniGifImage : MonoBehaviour
{
	public UniGifImage.State nowState { get; private set; }

	public int loopCount { get; private set; }

	public int width { get; private set; }

	public int height { get; private set; }

	private void Start()
	{
		if (this.m_rawImage == null)
		{
			this.m_rawImage = base.GetComponent<RawImage>();
		}
		if (this.m_loadOnStart)
		{
			this.SetGifFromUrl(this.m_loadOnStartUrl, true);
		}
		if (base.gameObject.GetComponent<RawImage>() != null)
		{
			base.gameObject.GetComponent<RawImage>().enabled = false;
		}
	}

	private void OnDestroy()
	{
		this.Clear();
	}

	private void OnGUI()
	{
		switch (this.nowState)
		{
		case UniGifImage.State.Playing:
			if (this.m_rawImage == null || this.m_gifTextureList == null || this.m_gifTextureList.Count <= 0)
			{
				return;
			}
			if (this.m_delayTime > Time.time)
			{
				return;
			}
			this.m_gifTextureIndex++;
			if (this.m_gifTextureIndex >= this.m_gifTextureList.Count)
			{
				this.m_gifTextureIndex = 0;
				if (this.loopCount > 0)
				{
					this.m_nowLoopCount++;
					if (this.m_nowLoopCount >= this.loopCount)
					{
						this.Stop();
						return;
					}
				}
			}
			this.m_rawImage.texture = this.m_gifTextureList[this.m_gifTextureIndex].m_texture2d;
			this.m_delayTime = Time.time + this.m_gifTextureList[this.m_gifTextureIndex].m_delaySec;
			break;
		}
	}

	public void SetGifFromUrl(string url, bool autoPlay = true)
	{
		base.StartCoroutine(this.SetGifFromUrlCoroutine(url, autoPlay));
	}

	public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
	{
		if (string.IsNullOrEmpty(url))
		{
			UnityEngine.Debug.LogError("URL is nothing.");
			yield break;
		}
		if (this.nowState == UniGifImage.State.Loading)
		{
			UnityEngine.Debug.LogWarning("Already loading.");
			yield break;
		}
		this.nowState = UniGifImage.State.Loading;
		string path;
		if (url.StartsWith("http"))
		{
			path = url;
		}
		else
		{
			path = Path.Combine("file:///" + Application.streamingAssetsPath, url);
		}
		using (WWW www = new WWW(path))
		{
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				UnityEngine.Debug.LogError("File load error.\n" + www.error);
				this.nowState = UniGifImage.State.None;
				yield break;
			}
			this.Clear();
			this.nowState = UniGifImage.State.Loading;
			yield return base.StartCoroutine(UniGif.GetTextureListCoroutine(www.bytes, delegate(List<UniGif.GifTexture> gifTexList, int loopCount, int width, int height)
			{
				if (gifTexList != null)
				{
					this.m_gifTextureList = gifTexList;
					this.loopCount = loopCount;
					this.width = width;
					this.height = height;
					this.nowState = UniGifImage.State.Ready;
					this.m_imgAspectCtrl.FixAspectRatio(width, height);
					if (this.m_rotateOnLoading)
					{
						this.transform.localEulerAngles = Vector3.zero;
					}
					if (autoPlay)
					{
						this.Play();
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Gif texture get error.");
					this.nowState = UniGifImage.State.None;
				}
			}, this.m_filterMode, this.m_wrapMode, this.m_outputDebugLog));
		}
		yield break;
	}

	public void Clear()
	{
		if (this.m_rawImage != null)
		{
			this.m_rawImage.texture = null;
		}
		if (this.m_gifTextureList != null)
		{
			for (int i = 0; i < this.m_gifTextureList.Count; i++)
			{
				if (this.m_gifTextureList[i] != null)
				{
					if (this.m_gifTextureList[i].m_texture2d != null)
					{
						UnityEngine.Object.Destroy(this.m_gifTextureList[i].m_texture2d);
						this.m_gifTextureList[i].m_texture2d = null;
					}
					this.m_gifTextureList[i] = null;
				}
			}
			this.m_gifTextureList.Clear();
			this.m_gifTextureList = null;
		}
		this.nowState = UniGifImage.State.None;
	}

	public void Play()
	{
		if (this.nowState != UniGifImage.State.Ready)
		{
			UnityEngine.Debug.LogWarning("State is not READY.");
			return;
		}
		if (this.m_rawImage == null || this.m_gifTextureList == null || this.m_gifTextureList.Count <= 0)
		{
			UnityEngine.Debug.LogError("Raw Image or GIF Texture is nothing.");
			return;
		}
		if (base.gameObject.GetComponent<RawImage>() != null)
		{
			base.gameObject.GetComponent<RawImage>().enabled = true;
		}
		this.nowState = UniGifImage.State.Playing;
		this.m_rawImage.texture = this.m_gifTextureList[0].m_texture2d;
		this.m_delayTime = Time.time + this.m_gifTextureList[0].m_delaySec;
		this.m_gifTextureIndex = 0;
		this.m_nowLoopCount = 0;
	}

	public void Stop()
	{
		if (this.nowState != UniGifImage.State.Playing && this.nowState != UniGifImage.State.Pause)
		{
			UnityEngine.Debug.LogWarning("State is not Playing and Pause.");
			return;
		}
		this.nowState = UniGifImage.State.Ready;
	}

	public void Pause()
	{
		if (this.nowState != UniGifImage.State.Playing)
		{
			UnityEngine.Debug.LogWarning("State is not Playing.");
			return;
		}
		this.nowState = UniGifImage.State.Pause;
	}

	public void Resume()
	{
		if (this.nowState != UniGifImage.State.Pause)
		{
			UnityEngine.Debug.LogWarning("State is not Pause.");
			return;
		}
		this.nowState = UniGifImage.State.Playing;
	}

	[SerializeField]
	private RawImage m_rawImage;

	[SerializeField]
	private UniGifImageAspectController m_imgAspectCtrl;

	[SerializeField]
	private FilterMode m_filterMode;

	[SerializeField]
	private TextureWrapMode m_wrapMode = TextureWrapMode.Clamp;

	[SerializeField]
	private bool m_loadOnStart;

	[SerializeField]
	private string m_loadOnStartUrl;

	[SerializeField]
	private bool m_rotateOnLoading;

	[SerializeField]
	private bool m_outputDebugLog;

	private List<UniGif.GifTexture> m_gifTextureList;

	private float m_delayTime;

	private int m_gifTextureIndex;

	private int m_nowLoopCount;

	public enum State
	{
		None,
		Loading,
		Ready,
		Playing,
		Pause
	}
}
