using System;
using UnityEngine;

namespace StartApp
{
	public class StartAppWrapper : MonoBehaviour
	{
		public static void setVideoListener(StartAppWrapper.VideoListener listener)
		{
			StartAppWrapper.init();
			StartAppWrapper.wrapper.Call("setVideoListener", new object[]
			{
				new StartAppWrapper.ImplementationVideoListener(listener)
			});
		}

		public static void loadAd(StartAppWrapper.AdEventListener listener)
		{
			StartAppWrapper.loadAd(StartAppWrapper.AdMode.AUTOMATIC, listener);
		}

		public static void loadAd(StartAppWrapper.AdMode adMode)
		{
			StartAppWrapper.loadAd(adMode, null);
		}

		public static void loadAd(StartAppWrapper.AdMode adMode, StartAppWrapper.AdEventListener listener)
		{
			StartAppWrapper.loadAd(adMode, listener, false);
		}

		private static void loadAd(StartAppWrapper.AdMode adMode, StartAppWrapper.AdEventListener listener, bool is3D)
		{
			StartAppWrapper.init();
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Integer", new object[]
			{
				(int)adMode
			});
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.Boolean", new object[]
			{
				is3D
			});
			if (listener == null)
			{
				StartAppWrapper.wrapper.Call("loadAd", new object[]
				{
					androidJavaObject
				});
				return;
			}
			StartAppWrapper.wrapper.Call("loadAd", new object[]
			{
				androidJavaObject,
				new StartAppWrapper.ImplementationAdEventListener(listener)
			});
		}

		public static bool showAd(StartAppWrapper.AdDisplayListener listener)
		{
			StartAppWrapper.init();
			return StartAppWrapper.wrapper.Call<bool>("showAd", new object[]
			{
				new StartAppWrapper.ImplementationAdDisplayListener(listener)
			});
		}

		public static bool showAd(string adTag)
		{
			StartAppWrapper.init();
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.String", new object[]
			{
				adTag
			});
			return StartAppWrapper.wrapper.Call<bool>("showAd", new object[]
			{
				androidJavaObject
			});
		}

		public static bool showAd(string adTag, StartAppWrapper.AdDisplayListener listener)
		{
			if (adTag == null)
			{
				return StartAppWrapper.showAd(listener);
			}
			StartAppWrapper.init();
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.String", new object[]
			{
				adTag
			});
			return StartAppWrapper.wrapper.Call<bool>("showAd", new object[]
			{
				adTag,
				new StartAppWrapper.ImplementationAdDisplayListener(listener)
			});
		}

		public static bool onBackPressed(string gameObjectName)
		{
			StartAppWrapper.init();
			return StartAppWrapper.wrapper.Call<bool>("onBackPressed", new object[]
			{
				new StartAppWrapper.OnBackPressedAdDisplayListener(gameObjectName)
			});
		}

		public static void showSplash()
		{
			StartAppWrapper.init();
			StartAppWrapper.wrapper.Call("showSplash", new object[0]);
		}

		public static void showSplash(StartAppWrapper.SplashConfig splashConfig)
		{
			StartAppWrapper.init();
			StartAppWrapper.wrapper.Call("showSplash", new object[]
			{
				splashConfig.getJavaSplashConfig()
			});
		}

		public static bool checkIfBannerExists(StartAppWrapper.BannerPosition bannerPosition)
		{
			AndroidJavaObject bannerPositionObject = StartAppWrapper.getBannerPositionObject(bannerPosition);
			return StartAppWrapper.wrapper.Call<bool>("checkIfBannerExists", new object[]
			{
				bannerPositionObject
			});
		}

		private static AndroidJavaObject getBannerPositionObject(StartAppWrapper.BannerPosition bannerPosition)
		{
			int num = 1;
			if (bannerPosition != StartAppWrapper.BannerPosition.BOTTOM)
			{
				if (bannerPosition == StartAppWrapper.BannerPosition.TOP)
				{
					num = 2;
				}
			}
			else
			{
				num = 1;
			}
			return new AndroidJavaObject("java.lang.Integer", new object[]
			{
				num
			});
		}

		private static AndroidJavaObject getBannerTypeObject(StartAppWrapper.BannerType bannerType)
		{
			int num = 1;
			if (bannerType != StartAppWrapper.BannerType.AUTOMATIC)
			{
				if (bannerType != StartAppWrapper.BannerType.STANDARD)
				{
					if (bannerType == StartAppWrapper.BannerType.THREED)
					{
						num = 3;
					}
				}
				else
				{
					num = 2;
				}
			}
			else
			{
				num = 1;
			}
			return new AndroidJavaObject("java.lang.Integer", new object[]
			{
				num
			});
		}

		public static void addBanner()
		{
			StartAppWrapper.addBanner(StartAppWrapper.BannerType.AUTOMATIC, StartAppWrapper.BannerPosition.BOTTOM);
		}

		public static void addBanner(StartAppWrapper.BannerType bannerType, StartAppWrapper.BannerPosition bannerPosition)
		{
			StartAppWrapper.addBanner(bannerType, bannerPosition, null);
		}

		public static void addBanner(StartAppWrapper.BannerType bannerType, StartAppWrapper.BannerPosition bannerPosition, string adTag)
		{
			StartAppWrapper.init();
			AndroidJavaObject bannerPositionObject = StartAppWrapper.getBannerPositionObject(bannerPosition);
			AndroidJavaObject bannerTypeObject = StartAppWrapper.getBannerTypeObject(bannerType);
			if (adTag == null)
			{
				StartAppWrapper.wrapper.Call("addBanner", new AndroidJavaObject[]
				{
					bannerTypeObject,
					bannerPositionObject
				});
			}
			else
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.String", new object[]
				{
					adTag
				});
				StartAppWrapper.wrapper.Call("addBanner", new AndroidJavaObject[]
				{
					bannerTypeObject,
					bannerPositionObject,
					androidJavaObject
				});
			}
		}

		public static void removeBanner()
		{
			StartAppWrapper.removeBanner(StartAppWrapper.BannerPosition.BOTTOM);
		}

		public static void removeBanner(StartAppWrapper.BannerPosition bannerPosition)
		{
			StartAppWrapper.init();
			AndroidJavaObject bannerPositionObject = StartAppWrapper.getBannerPositionObject(bannerPosition);
			StartAppWrapper.wrapper.Call("removeBanner", new object[]
			{
				bannerPositionObject
			});
		}

		public static void disableReturnAds()
		{
			StartAppWrapper.enableReturnAds = false;
		}

		public static void loadAd()
		{
			StartAppWrapper.init();
			StartAppWrapper.wrapper.Call("loadAd", new object[0]);
		}

		public static bool showAd()
		{
			StartAppWrapper.init();
			return StartAppWrapper.wrapper.Call<bool>("showAd", new object[0]);
		}

		public static void init()
		{
			if (StartAppWrapper.wrapper == null)
			{
				StartAppWrapper.initWrapper();
				StartAppWrapper.initSdk();
			}
		}

		public static void init(string appId, bool enableReturnAds)
		{
			if (StartAppWrapper.wrapper == null)
			{
				StartAppWrapper.initWrapper();
				StartAppWrapper.initSdk(appId, enableReturnAds);
			}
		}

		public static void init(string accId, string appId, bool enableReturnAds)
		{
			if (StartAppWrapper.wrapper == null)
			{
				StartAppWrapper.initWrapper();
				StartAppWrapper.initSdk(accId, appId, enableReturnAds);
			}
		}

		private static void initWrapper()
		{
			StartAppWrapper.unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			StartAppWrapper.currentActivity = StartAppWrapper.unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			StartAppWrapper.wrapper = new AndroidJavaObject("com.startapp.android.unity.InAppWrapper", new object[]
			{
				StartAppWrapper.currentActivity
			});
		}

		private static void initSdk()
		{
			StartAppWrapper.applicationId = PlayerPrefs.GetString("StartAppID");
			StartAppWrapper.jAppId = new AndroidJavaObject("java.lang.String", new object[]
			{
				StartAppWrapper.applicationId
			});
			StartAppWrapper.jEnableReturnAds = new AndroidJavaObject("java.lang.Boolean", new object[]
			{
				StartAppWrapper.enableReturnAds
			});
			if (StartAppWrapper.isAccountIdUsed)
			{
				StartAppWrapper.initSdk(StartAppWrapper.accountId, StartAppWrapper.applicationId, StartAppWrapper.enableReturnAds);
			}
			else
			{
				StartAppWrapper.initSdk(StartAppWrapper.applicationId, StartAppWrapper.enableReturnAds);
			}
		}

		private static void initSdk(string appId, bool returnAds)
		{
			StartAppWrapper.initSdk(null, appId, returnAds);
		}

		private static void initSdk(string accId, string appId, bool returnAds)
		{
			StartAppWrapper.jAppId = new AndroidJavaObject("java.lang.String", new object[]
			{
				appId
			});
			StartAppWrapper.jEnableReturnAds = new AndroidJavaObject("java.lang.Boolean", new object[]
			{
				returnAds
			});
			if (accId == null)
			{
				StartAppWrapper.wrapper.Call("init", new object[]
				{
					StartAppWrapper.jAppId,
					StartAppWrapper.jEnableReturnAds
				});
			}
			else
			{
				StartAppWrapper.jAccId = new AndroidJavaObject("java.lang.String", new object[]
				{
					accId
				});
				StartAppWrapper.wrapper.Call("init", new object[]
				{
					StartAppWrapper.jAccId,
					StartAppWrapper.jAppId,
					StartAppWrapper.jEnableReturnAds
				});
			}
		}

		private static bool readDataFromTextFile()
		{
			bool result = false;
			int num = 0;
			TextAsset textAsset = (TextAsset)Resources.Load("StartAppData");
			string text = textAsset.ToString();
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'='
				});
				if (array2[0].ToLower().CompareTo("applicationid") == 0)
				{
					num++;
					StartAppWrapper.applicationId = array2[1].ToString().Trim();
				}
				if (array2[0].ToLower().CompareTo("accountid") == 0 || array2[0].ToLower().CompareTo("developerid") == 0)
				{
					StartAppWrapper.isAccountIdUsed = true;
					StartAppWrapper.accountId = array2[1].ToString().Trim();
				}
				if (array2[0].ToLower().CompareTo("returnads") == 0 && array2[1].ToLower().Equals("false"))
				{
					num++;
					StartAppWrapper.disableReturnAds();
				}
			}
			StartAppWrapper.removeSpecialCharacters();
			if ((StartAppWrapper.enableReturnAds && num == 1) || (!StartAppWrapper.enableReturnAds && num == 2))
			{
				UnityEngine.Debug.Log("Initialization successful");
				UnityEngine.Debug.Log("Application ID: " + StartAppWrapper.applicationId);
				if (StartAppWrapper.isAccountIdUsed)
				{
					UnityEngine.Debug.Log("Account ID: " + StartAppWrapper.accountId);
				}
				if (StartAppWrapper.enableReturnAds)
				{
					UnityEngine.Debug.Log("Return ads are enabled");
				}
				result = true;
			}
			return result;
		}

		private static void removeSpecialCharacters()
		{
			if (StartAppWrapper.applicationId != null && StartAppWrapper.applicationId.IndexOf("\"") != -1)
			{
				StartAppWrapper.applicationId = StartAppWrapper.applicationId.Replace("\"", string.Empty);
			}
			if (StartAppWrapper.isAccountIdUsed && StartAppWrapper.accountId != null && StartAppWrapper.accountId.IndexOf("\"") != -1)
			{
				StartAppWrapper.accountId = StartAppWrapper.accountId.Replace("\"", string.Empty);
			}
		}

		private static string accountId;

		private static string applicationId;

		private static bool enableReturnAds;

		private static bool isAccountIdUsed;

		private static AndroidJavaObject jAppId;

		private static AndroidJavaObject jAccId;

		private static AndroidJavaObject jEnableReturnAds;

		private static AndroidJavaClass unityClass;

		private static AndroidJavaObject currentActivity;

		private static AndroidJavaObject wrapper;

		public interface AdEventListener
		{
			void onReceiveAd();

			void onFailedToReceiveAd();
		}

		public interface AdDisplayListener
		{
			void adHidden();

			void adDisplayed();

			void adClicked();
		}

		public interface VideoListener
		{
			void onVideoCompleted();
		}

		public enum AdMode
		{
			AUTOMATIC = 1,
			FULLPAGE,
			OFFERWALL,
			REWARDED_VIDEO,
			[Obsolete]
			OVERLAY
		}

		public class SplashConfig
		{
			public SplashConfig()
			{
				StartAppWrapper.init();
				this.javaSplashConfig = new AndroidJavaObject("com.startapp.android.publish.ads.splash.SplashConfig", new object[0]);
			}

			public AndroidJavaObject getJavaSplashConfig()
			{
				return this.javaSplashConfig;
			}

			public StartAppWrapper.SplashConfig setAppName(string appName)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.String", new object[]
				{
					appName
				});
				StartAppWrapper.wrapper.Call<AndroidJavaObject>("setAppName", new object[]
				{
					this.getJavaSplashConfig(),
					androidJavaObject
				});
				return this;
			}

			public StartAppWrapper.SplashConfig setTheme(StartAppWrapper.SplashConfig.Theme theme)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Integer", new object[]
				{
					(int)theme
				});
				StartAppWrapper.wrapper.Call<AndroidJavaObject>("setTheme", new object[]
				{
					this.getJavaSplashConfig(),
					androidJavaObject
				});
				return this;
			}

			public StartAppWrapper.SplashConfig setLogo(string fileName)
			{
				byte[] array = null;
				Texture2D texture2D = Resources.Load(fileName) as Texture2D;
				if (texture2D != null)
				{
					array = texture2D.EncodeToPNG();
				}
				StartAppWrapper.wrapper.Call<AndroidJavaObject>("setLogo", new object[]
				{
					this.getJavaSplashConfig(),
					array
				});
				return this;
			}

			public StartAppWrapper.SplashConfig setOrientation(StartAppWrapper.SplashConfig.Orientation orientation)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Integer", new object[]
				{
					(int)orientation
				});
				StartAppWrapper.wrapper.Call<AndroidJavaObject>("setOrientation", new object[]
				{
					this.getJavaSplashConfig(),
					androidJavaObject
				});
				return this;
			}

			private AndroidJavaObject javaSplashConfig;

			public enum Theme
			{
				DEEP_BLUE = 1,
				SKY,
				ASHEN_SKY,
				BLAZE,
				GLOOMY,
				OCEAN
			}

			public enum Orientation
			{
				PORTRAIT = 1,
				LANDSCAPE,
				AUTO
			}
		}

		private class ImplementationAdEventListener : AndroidJavaProxy
		{
			public ImplementationAdEventListener(StartAppWrapper.AdEventListener listener) : base("com.startapp.android.publish.adsCommon.adListeners.AdEventListener")
			{
				this.listener = listener;
			}

			private void onReceiveAd(AndroidJavaObject ad)
			{
				if (this.listener != null)
				{
					this.listener.onReceiveAd();
				}
			}

			private void onFailedToReceiveAd(AndroidJavaObject ad)
			{
				if (this.listener != null)
				{
					this.listener.onFailedToReceiveAd();
				}
			}

			private int hashCode()
			{
				return this.listener.GetHashCode();
			}

			private bool equals(AndroidJavaObject o)
			{
				int num = o.Call<int>("hashCode", new object[0]);
				return num == this.listener.GetHashCode();
			}

			private string toString()
			{
				return "ImplementationAdEventListener: " + this.hashCode();
			}

			private StartAppWrapper.AdEventListener listener;
		}

		private class ImplementationAdDisplayListener : AndroidJavaProxy
		{
			public ImplementationAdDisplayListener(StartAppWrapper.AdDisplayListener listener) : base("com.startapp.android.publish.adsCommon.adListeners.AdDisplayListener")
			{
				this.listener = listener;
			}

			private void adHidden(AndroidJavaObject ad)
			{
				if (this.listener != null)
				{
					this.listener.adHidden();
				}
			}

			private void adDisplayed(AndroidJavaObject ad)
			{
				if (this.listener != null)
				{
					this.listener.adDisplayed();
				}
			}

			private void adClicked(AndroidJavaObject ad)
			{
				if (this.listener != null)
				{
					this.listener.adClicked();
				}
			}

			private int hashCode()
			{
				return this.listener.GetHashCode();
			}

			private bool equals(AndroidJavaObject o)
			{
				int num = o.Call<int>("hashCode", new object[0]);
				return num == this.listener.GetHashCode();
			}

			private string toString()
			{
				return "ImplementationAdDisplayListener: " + this.hashCode();
			}

			private StartAppWrapper.AdDisplayListener listener;
		}

		private class OnBackPressedAdDisplayListener : AndroidJavaProxy
		{
			public OnBackPressedAdDisplayListener(string gameObjectName) : base("com.startapp.android.publish.adsCommon.adListeners.AdDisplayListener")
			{
				this.gameObjectName = gameObjectName;
			}

			private void adHidden(AndroidJavaObject ad)
			{
				if (!this.clicked)
				{
					StartAppWrapper.init();
					Application.Quit();
				}
			}

			private void adDisplayed(AndroidJavaObject ad)
			{
			}

			private void adClicked(AndroidJavaObject ad)
			{
				this.clicked = true;
			}

			private void adNotDisplayed(AndroidJavaObject ad)
			{
			}

			private string gameObjectName;

			private bool clicked;
		}

		private class ImplementationVideoListener : AndroidJavaProxy
		{
			public ImplementationVideoListener(StartAppWrapper.VideoListener listener) : base("com.startapp.android.publish.adsCommon.VideoListener")
			{
				this.listener = listener;
			}

			private void onVideoCompleted()
			{
				if (this.listener != null)
				{
					this.listener.onVideoCompleted();
				}
			}

			private StartAppWrapper.VideoListener listener;
		}

		public enum BannerPosition
		{
			BOTTOM,
			TOP
		}

		public enum BannerType
		{
			AUTOMATIC,
			STANDARD,
			THREED
		}
	}
}
