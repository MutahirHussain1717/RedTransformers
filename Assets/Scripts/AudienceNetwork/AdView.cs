using System;
using AudienceNetwork.Utility;
using UnityEngine;

namespace AudienceNetwork
{
	public sealed class AdView : IDisposable
	{
		public AdView(string placementId, AdSize size)
		{
			this.PlacementId = placementId;
			this.size = size;
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				this.uniqueId = AdViewBridge.Instance.Create(placementId, this, size);
				AdViewBridge.Instance.OnLoad(this.uniqueId, this.AdViewDidLoad);
				AdViewBridge.Instance.OnImpression(this.uniqueId, this.AdViewWillLogImpression);
				AdViewBridge.Instance.OnClick(this.uniqueId, this.AdViewDidClick);
				AdViewBridge.Instance.OnError(this.uniqueId, this.AdViewDidFailWithError);
				AdViewBridge.Instance.OnFinishedClick(this.uniqueId, this.AdViewDidFinishClick);
			}
		}

		public string PlacementId { get; private set; }

		public FBAdViewBridgeCallback AdViewDidLoad
		{
			internal get
			{
				return this.adViewDidLoad;
			}
			set
			{
				this.adViewDidLoad = value;
				AdViewBridge.Instance.OnLoad(this.uniqueId, this.adViewDidLoad);
			}
		}

		public FBAdViewBridgeCallback AdViewWillLogImpression
		{
			internal get
			{
				return this.adViewWillLogImpression;
			}
			set
			{
				this.adViewWillLogImpression = value;
				AdViewBridge.Instance.OnImpression(this.uniqueId, this.adViewWillLogImpression);
			}
		}

		public FBAdViewBridgeErrorCallback AdViewDidFailWithError
		{
			internal get
			{
				return this.adViewDidFailWithError;
			}
			set
			{
				this.adViewDidFailWithError = value;
				AdViewBridge.Instance.OnError(this.uniqueId, this.adViewDidFailWithError);
			}
		}

		public FBAdViewBridgeCallback AdViewDidClick
		{
			internal get
			{
				return this.adViewDidClick;
			}
			set
			{
				this.adViewDidClick = value;
				AdViewBridge.Instance.OnClick(this.uniqueId, this.adViewDidClick);
			}
		}

		public FBAdViewBridgeCallback AdViewDidFinishClick
		{
			internal get
			{
				return this.adViewDidFinishClick;
			}
			set
			{
				this.adViewDidFinishClick = value;
				AdViewBridge.Instance.OnFinishedClick(this.uniqueId, this.adViewDidFinishClick);
			}
		}

		~AdView()
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
			UnityEngine.Debug.Log("Banner Ad Disposed.");
			AdViewBridge.Instance.Release(this.uniqueId);
		}

		public override string ToString()
		{
			return string.Format("[AdView: PlacementId={0}, AdViewDidLoad={1}, AdViewWillLogImpression={2}, AdViewDidFailWithError={3}, AdViewDidClick={4}, adViewDidFinishClick={5}]", new object[]
			{
				this.PlacementId,
				this.AdViewDidLoad,
				this.AdViewWillLogImpression,
				this.AdViewDidFailWithError,
				this.AdViewDidClick,
				this.adViewDidFinishClick
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
				AdViewBridge.Instance.Load(this.uniqueId);
			}
			else
			{
				this.AdViewDidLoad();
			}
		}

		private double heightFromType(AdSize size)
		{
			if (size == AdSize.BANNER_HEIGHT_50)
			{
				return 50.0;
			}
			if (size == AdSize.BANNER_HEIGHT_90)
			{
				return 90.0;
			}
			if (size != AdSize.RECTANGLE_HEIGHT_250)
			{
				return 0.0;
			}
			return 250.0;
		}

		public bool Show(double y)
		{
			return AdViewBridge.Instance.Show(this.uniqueId, 0.0, y, AdUtility.width(), this.heightFromType(this.size));
		}

		public bool Show(double x, double y)
		{
			return AdViewBridge.Instance.Show(this.uniqueId, x, y, AdUtility.width(), this.heightFromType(this.size));
		}

		private bool Show(double x, double y, double width, double height)
		{
			return AdViewBridge.Instance.Show(this.uniqueId, x, y, width, height);
		}

		public void DisableAutoRefresh()
		{
			AdViewBridge.Instance.DisableAutoRefresh(this.uniqueId);
		}

		internal void executeOnMainThread(Action action)
		{
			if (this.handler)
			{
				this.handler.executeOnMainThread(action);
			}
		}

		public static implicit operator bool(AdView obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		private int uniqueId;

		private AdSize size;

		private AdHandler handler;

		public FBAdViewBridgeCallback adViewDidLoad;

		public FBAdViewBridgeCallback adViewWillLogImpression;

		public FBAdViewBridgeErrorCallback adViewDidFailWithError;

		public FBAdViewBridgeCallback adViewDidClick;

		public FBAdViewBridgeCallback adViewDidFinishClick;
	}
}
