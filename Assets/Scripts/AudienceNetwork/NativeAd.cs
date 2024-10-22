using System;
using System.Collections;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AudienceNetwork
{
	public sealed class NativeAd : IDisposable
	{
		public NativeAd(string placementId)
		{
			this.PlacementId = placementId;
			this.uniqueId = NativeAdBridge.Instance.Create(placementId, this);
			NativeAdBridge.Instance.OnLoad(this.uniqueId, this.NativeAdDidLoad);
			NativeAdBridge.Instance.OnImpression(this.uniqueId, this.NativeAdWillLogImpression);
			NativeAdBridge.Instance.OnClick(this.uniqueId, this.NativeAdDidClick);
			NativeAdBridge.Instance.OnError(this.uniqueId, this.NativeAdDidFailWithError);
			NativeAdBridge.Instance.OnFinishedClick(this.uniqueId, this.NativeAdDidFinishHandlingClick);
		}

		public string PlacementId { get; private set; }

		public string Title { get; private set; }

		public string Subtitle { get; private set; }

		public string Body { get; private set; }

		public string CallToAction { get; private set; }

		public string SocialContext { get; private set; }

		public string IconImageURL { get; private set; }

		public string CoverImageURL { get; private set; }

		public string AdChoicesImageURL { get; private set; }

		public Sprite IconImage { get; private set; }

		public Sprite CoverImage { get; private set; }

		public Sprite AdChoicesImage { get; private set; }

		public string AdChoicesText { get; private set; }

		public string AdChoicesLinkURL { get; private set; }

		public FBNativeAdBridgeCallback NativeAdDidLoad
		{
			internal get
			{
				return this.nativeAdDidLoad;
			}
			set
			{
				this.nativeAdDidLoad = value;
				NativeAdBridge.Instance.OnLoad(this.uniqueId, this.nativeAdDidLoad);
			}
		}

		public FBNativeAdBridgeCallback NativeAdWillLogImpression
		{
			internal get
			{
				return this.nativeAdWillLogImpression;
			}
			set
			{
				this.nativeAdWillLogImpression = value;
				NativeAdBridge.Instance.OnImpression(this.uniqueId, this.nativeAdWillLogImpression);
			}
		}

		public FBNativeAdBridgeErrorCallback NativeAdDidFailWithError
		{
			internal get
			{
				return this.nativeAdDidFailWithError;
			}
			set
			{
				this.nativeAdDidFailWithError = value;
				NativeAdBridge.Instance.OnError(this.uniqueId, this.nativeAdDidFailWithError);
			}
		}

		public FBNativeAdBridgeCallback NativeAdDidClick
		{
			internal get
			{
				return this.nativeAdDidClick;
			}
			set
			{
				this.nativeAdDidClick = value;
				NativeAdBridge.Instance.OnClick(this.uniqueId, this.nativeAdDidClick);
			}
		}

		public FBNativeAdBridgeCallback NativeAdDidFinishHandlingClick
		{
			internal get
			{
				return this.nativeAdDidFinishHandlingClick;
			}
			set
			{
				this.nativeAdDidFinishHandlingClick = value;
				NativeAdBridge.Instance.OnFinishedClick(this.uniqueId, this.nativeAdDidFinishHandlingClick);
			}
		}

		~NativeAd()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool iAmBeingCalledFromDisposeAndNotFinalize)
		{
			if (this.handler)
			{
				this.handler.stopImpressionValidation();
				this.handler.removeFromParent();
			}
			UnityEngine.Debug.Log("Native Ad Disposed.");
			NativeAdBridge.Instance.Release(this.uniqueId);
		}

		public override string ToString()
		{
			return string.Format("[NativeAd: PlacementId={0}, Title={1}, Subtitle={2}, Body={3}, CallToAction={4}, SocialContext={5}, IconImageURL={6}, CoverImageURL={7}, IconImage={8}, CoverImage={9}, NativeAdDidLoad={10}, NativeAdWillLogImpression={11}, NativeAdDidFailWithError={12}, NativeAdDidClick={13}, NativeAdDidFinishHandlingClick={14}]", new object[]
			{
				this.PlacementId,
				this.Title,
				this.Subtitle,
				this.Body,
				this.CallToAction,
				this.SocialContext,
				this.IconImageURL,
				this.CoverImageURL,
				this.IconImage,
				this.CoverImage,
				this.NativeAdDidLoad,
				this.NativeAdWillLogImpression,
				this.NativeAdDidFailWithError,
				this.NativeAdDidClick,
				this.NativeAdDidFinishHandlingClick
			});
		}

		private static TextureFormat imageFormat()
		{
			return TextureFormat.RGBA32;
		}

		public IEnumerator LoadIconImage(string url)
		{
			Texture2D texture = new Texture2D(4, 4, NativeAd.imageFormat(), false);
			WWW www = new WWW(url);
			yield return www;
			if (www.error == null)
			{
				www.LoadImageIntoTexture(texture);
				if (texture)
				{
					this.IconImage = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f));
				}
			}
			yield break;
		}

		public IEnumerator LoadCoverImage(string url)
		{
			Texture2D texture = new Texture2D(4, 4, NativeAd.imageFormat(), false);
			WWW www = new WWW(url);
			yield return www;
			if (www.error == null)
			{
				www.LoadImageIntoTexture(texture);
				this.fbAd_isload = true;
				if (texture)
				{
					this.CoverImage = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f));
				}
			}
			yield break;
		}

		public IEnumerator LoadAdChoicesImage(string url)
		{
			Texture2D texture = new Texture2D(4, 4, NativeAd.imageFormat(), false);
			WWW www = new WWW(url);
			yield return www;
			if (www.error == null)
			{
				www.LoadImageIntoTexture(texture);
				if (texture)
				{
					this.AdChoicesImage = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f));
				}
			}
			yield break;
		}

		public void LoadAd()
		{
			NativeAdBridge.Instance.Load(this.uniqueId);
		}

		public bool IsValid()
		{
			return this.isLoaded && NativeAdBridge.Instance.IsValid(this.uniqueId);
		}

		private void RegisterGameObjectForManualImpression(GameObject gameObject)
		{
			this.createHandler(gameObject);
		}

		public void RegisterGameObjectForImpression(GameObject gameObject, Button[] clickableButtons)
		{
			this.RegisterGameObjectForImpression(gameObject, clickableButtons, Camera.main);
		}

		public void RegisterGameObjectForImpression(GameObject gameObject, Button[] clickableButtons, Camera camera)
		{
			foreach (Button button in clickableButtons)
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate()
				{
					AdLogger.Log("Native ad with unique id " + this.uniqueId + " clicked!");
					this.ExternalClick();
				});
			}
			if (this.handler)
			{
				this.handler.stopImpressionValidation();
				this.handler.removeFromParent();
				this.createHandler(camera, gameObject);
				this.handler.startImpressionValidation();
			}
			else
			{
				this.createHandler(camera, gameObject);
			}
		}

		private void createHandler(GameObject gameObject)
		{
			this.createHandler(null, gameObject);
		}

		private void createHandler(Camera camera, GameObject gameObject)
		{
			this.handler = gameObject.AddComponent<NativeAdHandler>();
			this.handler.camera = camera;
			this.handler.minAlpha = 0.9f;
			this.handler.maxRotation = 45;
			this.handler.checkViewabilityInterval = 1;
			this.handler.validationCallback = delegate(bool success)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Native ad viewability check for unique id ",
					this.uniqueId,
					" returned success? ",
					success
				}));
				if (FB.IsInitialized)
				{
					FB.LogAppEvent("Forcely Success native Ad-" + this.PlacementId.ToString(), new float?(1f), null);
				}
				if (success)
				{
					AdLogger.Log("Native ad with unique id " + this.uniqueId + " registered impression!");
					this.ExternalLogImpression();
					this.handler.stopImpressionValidation();
				}
			};
		}

		private void ManualLogImpression()
		{
			NativeAdBridge.Instance.ManualLogImpression(this.uniqueId);
		}

		private void ManualClick()
		{
			NativeAdBridge.Instance.ManualLogClick(this.uniqueId);
		}

		internal void ExternalLogImpression()
		{
			NativeAdBridge.Instance.ExternalLogImpression(this.uniqueId);
		}

		internal void ExternalClick()
		{
			NativeAdBridge.Instance.ExternalLogClick(this.uniqueId);
		}

		internal void loadAdFromData()
		{
			if (this.handler == null)
			{
				throw new InvalidOperationException("Native ad was loaded before it was registered. Ensure RegisterGameObjectForManualImpression () or RegisterGameObjectForImpression () are called.");
			}
			int num = this.uniqueId;
			this.Title = NativeAdBridge.Instance.GetTitle(num);
			this.Subtitle = NativeAdBridge.Instance.GetSubtitle(num);
			this.Body = NativeAdBridge.Instance.GetBody(num);
			this.CallToAction = NativeAdBridge.Instance.GetCallToAction(num);
			this.SocialContext = NativeAdBridge.Instance.GetSocialContext(num);
			this.CoverImageURL = NativeAdBridge.Instance.GetCoverImageURL(num);
			this.IconImageURL = NativeAdBridge.Instance.GetIconImageURL(num);
			this.AdChoicesImageURL = NativeAdBridge.Instance.GetAdChoicesImageURL(num);
			this.AdChoicesLinkURL = NativeAdBridge.Instance.GetAdChoicesLinkURL(num);
			this.isLoaded = true;
			this.minViewabilityPercentage = NativeAdBridge.Instance.GetMinViewabilityPercentage(num);
			this.handler.minViewabilityPercentage = this.minViewabilityPercentage;
			if (this.NativeAdDidLoad != null)
			{
				this.handler.executeOnMainThread(delegate
				{
					this.NativeAdDidLoad();
				});
			}
			this.handler.executeOnMainThread(delegate
			{
				this.handler.startImpressionValidation();
			});
		}

		internal void executeOnMainThread(Action action)
		{
			if (this.handler)
			{
				this.handler.executeOnMainThread(action);
			}
		}

		public static implicit operator bool(NativeAd obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		private int uniqueId;

		private bool isLoaded;

		public bool fbAd_isload;

		private int minViewabilityPercentage;

		internal const float MIN_ALPHA = 0.9f;

		internal const int MAX_ROTATION = 45;

		internal const int CHECK_VIEWABILITY_INTERVAL = 1;

		private NativeAdHandler handler;

		private FBNativeAdBridgeCallback nativeAdDidLoad;

		private FBNativeAdBridgeCallback nativeAdWillLogImpression;

		private FBNativeAdBridgeErrorCallback nativeAdDidFailWithError;

		private FBNativeAdBridgeCallback nativeAdDidClick;

		private FBNativeAdBridgeCallback nativeAdDidFinishHandlingClick;
	}
}
