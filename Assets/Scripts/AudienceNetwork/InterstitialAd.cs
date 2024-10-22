using System;
using UnityEngine;

namespace AudienceNetwork
{
	public sealed class InterstitialAd : IDisposable
	{
		public InterstitialAd(string placementId)
		{
			this.PlacementId = placementId;
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				this.uniqueId = InterstitialAdBridge.Instance.Create(placementId, this);
				InterstitialAdBridge.Instance.OnLoad(this.uniqueId, this.InterstitialAdDidLoad);
				InterstitialAdBridge.Instance.OnImpression(this.uniqueId, this.InterstitialAdWillLogImpression);
				InterstitialAdBridge.Instance.OnClick(this.uniqueId, this.InterstitialAdDidClick);
				InterstitialAdBridge.Instance.OnError(this.uniqueId, this.InterstitialAdDidFailWithError);
				InterstitialAdBridge.Instance.OnWillClose(this.uniqueId, this.InterstitialAdWillClose);
				InterstitialAdBridge.Instance.OnDidClose(this.uniqueId, this.InterstitialAdDidClose);
			}
		}

		public string PlacementId { get; private set; }

		public FBInterstitialAdBridgeCallback InterstitialAdDidLoad
		{
			internal get
			{
				return this.interstitialAdDidLoad;
			}
			set
			{
				this.interstitialAdDidLoad = value;
				InterstitialAdBridge.Instance.OnLoad(this.uniqueId, this.interstitialAdDidLoad);
			}
		}

		public FBInterstitialAdBridgeCallback InterstitialAdWillLogImpression
		{
			internal get
			{
				return this.interstitialAdWillLogImpression;
			}
			set
			{
				this.interstitialAdWillLogImpression = value;
				InterstitialAdBridge.Instance.OnImpression(this.uniqueId, this.interstitialAdWillLogImpression);
			}
		}

		public FBInterstitialAdBridgeErrorCallback InterstitialAdDidFailWithError
		{
			internal get
			{
				return this.interstitialAdDidFailWithError;
			}
			set
			{
				this.interstitialAdDidFailWithError = value;
				InterstitialAdBridge.Instance.OnError(this.uniqueId, this.interstitialAdDidFailWithError);
			}
		}

		public FBInterstitialAdBridgeCallback InterstitialAdDidClick
		{
			internal get
			{
				return this.interstitialAdDidClick;
			}
			set
			{
				this.interstitialAdDidClick = value;
				InterstitialAdBridge.Instance.OnClick(this.uniqueId, this.interstitialAdDidClick);
			}
		}

		public FBInterstitialAdBridgeCallback InterstitialAdWillClose
		{
			internal get
			{
				return this.interstitialAdWillClose;
			}
			set
			{
				this.interstitialAdWillClose = value;
				InterstitialAdBridge.Instance.OnWillClose(this.uniqueId, this.interstitialAdWillClose);
			}
		}

		public FBInterstitialAdBridgeCallback InterstitialAdDidClose
		{
			internal get
			{
				return this.interstitialAdDidClose;
			}
			set
			{
				this.interstitialAdDidClose = value;
				InterstitialAdBridge.Instance.OnDidClose(this.uniqueId, this.interstitialAdDidClose);
			}
		}

		~InterstitialAd()
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
				this.handler.removeFromParent();
			}
			UnityEngine.Debug.Log("Interstitial Ad Disposed.");
			InterstitialAdBridge.Instance.Release(this.uniqueId);
		}

		public override string ToString()
		{
			return string.Format("[InterstitialAd: PlacementId={0}, InterstitialAdDidLoad={1}, InterstitialAdWillLogImpression={2}, InterstitialAdDidFailWithError={3}, InterstitialAdDidClick={4}, InterstitialAdWillClose={5}, InterstitialAdDidClose={6}]", new object[]
			{
				this.PlacementId,
				this.InterstitialAdDidLoad,
				this.InterstitialAdWillLogImpression,
				this.InterstitialAdDidFailWithError,
				this.InterstitialAdDidClick,
				this.InterstitialAdWillClose,
				this.InterstitialAdDidClose
			});
		}

		public void Register(GameObject gameObject)
		{
			this.handler = gameObject.AddComponent<AdHandler>();
		}

		public void LoadAd()
		{
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				InterstitialAdBridge.Instance.Load(this.uniqueId);
			}
			else
			{
				this.InterstitialAdDidLoad();
			}
		}

		public bool IsValid()
		{
			return Application.platform == RuntimePlatform.OSXEditor || (this.isLoaded && InterstitialAdBridge.Instance.IsValid(this.uniqueId));
		}

		internal void loadAdFromData()
		{
			this.isLoaded = true;
		}

		public bool Show()
		{
			return InterstitialAdBridge.Instance.Show(this.uniqueId);
		}

		internal void executeOnMainThread(Action action)
		{
			if (this.handler)
			{
				this.handler.executeOnMainThread(action);
			}
		}

		public static implicit operator bool(InterstitialAd obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		private int uniqueId;

		private bool isLoaded;

		private AdHandler handler;

		public FBInterstitialAdBridgeCallback interstitialAdDidLoad;

		public FBInterstitialAdBridgeCallback interstitialAdWillLogImpression;

		public FBInterstitialAdBridgeErrorCallback interstitialAdDidFailWithError;

		public FBInterstitialAdBridgeCallback interstitialAdDidClick;

		public FBInterstitialAdBridgeCallback interstitialAdWillClose;

		public FBInterstitialAdBridgeCallback interstitialAdDidClose;
	}
}
