using System;
using UnityEngine;

namespace AudienceNetwork
{
	public sealed class RewardedVideoAd : IDisposable
	{
		public RewardedVideoAd(string placementId)
		{
			this.PlacementId = placementId;
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				this.uniqueId = RewardedVideoAdBridge.Instance.Create(placementId, this);
				RewardedVideoAdBridge.Instance.OnLoad(this.uniqueId, this.RewardedVideoAdDidLoad);
				RewardedVideoAdBridge.Instance.OnImpression(this.uniqueId, this.RewardedVideoAdWillLogImpression);
				RewardedVideoAdBridge.Instance.OnClick(this.uniqueId, this.RewardedVideoAdDidClick);
				RewardedVideoAdBridge.Instance.OnError(this.uniqueId, this.RewardedVideoAdDidFailWithError);
				RewardedVideoAdBridge.Instance.OnWillClose(this.uniqueId, this.RewardedVideoAdWillClose);
				RewardedVideoAdBridge.Instance.OnDidClose(this.uniqueId, this.RewardedVideoAdDidClose);
				RewardedVideoAdBridge.Instance.OnComplete(this.uniqueId, this.RewardedVideoAdComplete);
				RewardedVideoAdBridge.Instance.OnDidSucceed(this.uniqueId, this.RewardedVideoAdDidSucceed);
				RewardedVideoAdBridge.Instance.OnDidFail(this.uniqueId, this.RewardedVideoAdDidFail);
			}
		}

		public string PlacementId { get; private set; }

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidLoad
		{
			internal get
			{
				return this.rewardedVideoAdDidLoad;
			}
			set
			{
				this.rewardedVideoAdDidLoad = value;
				RewardedVideoAdBridge.Instance.OnLoad(this.uniqueId, this.rewardedVideoAdDidLoad);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillLogImpression
		{
			internal get
			{
				return this.rewardedVideoAdWillLogImpression;
			}
			set
			{
				this.rewardedVideoAdWillLogImpression = value;
				RewardedVideoAdBridge.Instance.OnImpression(this.uniqueId, this.rewardedVideoAdWillLogImpression);
			}
		}

		public FBRewardedVideoAdBridgeErrorCallback RewardedVideoAdDidFailWithError
		{
			internal get
			{
				return this.rewardedVideoAdDidFailWithError;
			}
			set
			{
				this.rewardedVideoAdDidFailWithError = value;
				RewardedVideoAdBridge.Instance.OnError(this.uniqueId, this.rewardedVideoAdDidFailWithError);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClick
		{
			internal get
			{
				return this.rewardedVideoAdDidClick;
			}
			set
			{
				this.rewardedVideoAdDidClick = value;
				RewardedVideoAdBridge.Instance.OnClick(this.uniqueId, this.rewardedVideoAdDidClick);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillClose
		{
			internal get
			{
				return this.rewardedVideoAdWillClose;
			}
			set
			{
				this.rewardedVideoAdWillClose = value;
				RewardedVideoAdBridge.Instance.OnWillClose(this.uniqueId, this.rewardedVideoAdWillClose);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClose
		{
			internal get
			{
				return this.rewardedVideoAdDidClose;
			}
			set
			{
				this.rewardedVideoAdDidClose = value;
				RewardedVideoAdBridge.Instance.OnDidClose(this.uniqueId, this.rewardedVideoAdDidClose);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdComplete
		{
			internal get
			{
				return this.rewardedVideoAdComplete;
			}
			set
			{
				this.rewardedVideoAdComplete = value;
				RewardedVideoAdBridge.Instance.OnComplete(this.uniqueId, this.rewardedVideoAdComplete);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidSucceed
		{
			internal get
			{
				return this.rewardedVideoAdDidSucceed;
			}
			set
			{
				this.rewardedVideoAdDidSucceed = value;
				RewardedVideoAdBridge.Instance.OnDidSucceed(this.uniqueId, this.rewardedVideoAdDidSucceed);
			}
		}

		public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidFail
		{
			internal get
			{
				return this.rewardedVideoAdDidFail;
			}
			set
			{
				this.rewardedVideoAdDidFail = value;
				RewardedVideoAdBridge.Instance.OnDidFail(this.uniqueId, this.rewardedVideoAdDidFail);
			}
		}

		~RewardedVideoAd()
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
			UnityEngine.Debug.Log("RewardedVideo Ad Disposed.");
			RewardedVideoAdBridge.Instance.Release(this.uniqueId);
		}

		public override string ToString()
		{
			return string.Format("[RewardedVideoAd: PlacementId={0}, RewardedVideoAdDidLoad={1}, RewardedVideoAdWillLogImpression={2}, RewardedVideoAdDidFailWithError={3}, RewardedVideoAdDidClick={4}, RewardedVideoAdWillClose={5}, RewardedVideoAdDidClose={6}, RewardedVideoAdComplete={7}, RewardedVideoAdDidSucceed={8}, RewardedVideoAdDidFail={9}]", new object[]
			{
				this.PlacementId,
				this.RewardedVideoAdDidLoad,
				this.RewardedVideoAdWillLogImpression,
				this.RewardedVideoAdDidFailWithError,
				this.RewardedVideoAdDidClick,
				this.RewardedVideoAdWillClose,
				this.RewardedVideoAdDidClose,
				this.RewardedVideoAdComplete,
				this.RewardedVideoAdDidSucceed,
				this.RewardedVideoAdDidFail
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
				RewardedVideoAdBridge.Instance.Load(this.uniqueId);
			}
			else
			{
				this.RewardedVideoAdDidLoad();
			}
		}

		public bool IsValid()
		{
			return Application.platform == RuntimePlatform.OSXEditor || (this.isLoaded && RewardedVideoAdBridge.Instance.IsValid(this.uniqueId));
		}

		internal void loadAdFromData()
		{
			this.isLoaded = true;
		}

		public bool Show()
		{
			return RewardedVideoAdBridge.Instance.Show(this.uniqueId);
		}

		internal void executeOnMainThread(Action action)
		{
			if (this.handler)
			{
				this.handler.executeOnMainThread(action);
			}
		}

		public static implicit operator bool(RewardedVideoAd obj)
		{
			return !object.ReferenceEquals(obj, null);
		}

		private int uniqueId;

		private bool isLoaded;

		private AdHandler handler;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidLoad;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillLogImpression;

		public FBRewardedVideoAdBridgeErrorCallback rewardedVideoAdDidFailWithError;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClick;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillClose;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClose;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdComplete;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidSucceed;

		public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidFail;
	}
}
