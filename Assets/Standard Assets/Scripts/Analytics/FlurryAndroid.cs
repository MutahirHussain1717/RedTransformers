using System;
using System.Collections.Generic;
using UnityEngine;

namespace Analytics
{
    
	public static class FlurryAndroid
	{
        
		private static AndroidJavaClass FlurryAgent
		{
			get
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					return null;
				}
				if (FlurryAndroid.s_FlurryAgent == null)
				{
					FlurryAndroid.s_FlurryAgent = new AndroidJavaClass(FlurryAndroid.s_FlurryAgentClassName);
				}
				return FlurryAndroid.s_FlurryAgent;
			}
        
		}
        
		public static void Dispose()
		{
			if (FlurryAndroid.s_FlurryAgent != null)
			{
				FlurryAndroid.s_FlurryAgent.Dispose();
			}
			FlurryAndroid.s_FlurryAgent = null;
		}

		public static void Init(string apiKey)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(FlurryAndroid.s_UnityPlayerClassName))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>(FlurryAndroid.s_UnityPlayerActivityName))
				{
					FlurryAndroid.FlurryAgent.CallStatic("init", new object[]
					{
						@static,
						apiKey
					});
				}
			}
		}

		public static void OnStartSession()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(FlurryAndroid.s_UnityPlayerClassName))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>(FlurryAndroid.s_UnityPlayerActivityName))
				{
					FlurryAndroid.FlurryAgent.CallStatic("onStartSession", new object[]
					{
						@static
					});
				}
			}
		}

		public static void OnEndSession()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(FlurryAndroid.s_UnityPlayerClassName))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>(FlurryAndroid.s_UnityPlayerActivityName))
				{
					FlurryAndroid.FlurryAgent.CallStatic("onEndSession", new object[]
					{
						@static
					});
				}
			}
		}

		public static bool IsSessionActive()
		{
			return FlurryAndroid.FlurryAgent.CallStatic<bool>("isSessionActive", new object[0]);
		}

		public static string GetSessionId()
		{
			return FlurryAndroid.FlurryAgent.CallStatic<string>("getSessionId", new object[0]);
		}

		public static int GetAgentVersion()
		{
			return FlurryAndroid.FlurryAgent.CallStatic<int>("getAgentVersion", new object[0]);
		}

		public static string GetReleaseVersion()
		{
			return FlurryAndroid.FlurryAgent.CallStatic<string>("getReleaseVersion", new object[0]);
		}

		public static void SetLogEnabled(bool isEnabled)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setLogEnabled", new object[]
			{
				isEnabled
			});
		}

		public static void SetLogLevel(LogLevel logLevel)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setLogLevel", new object[]
			{
				(int)logLevel
			});
		}

		public static void SetVersionName(string versionName)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setVersionName", new object[]
			{
				versionName
			});
		}

		public static void SetReportLocation(bool reportLocation)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setReportLocation", new object[]
			{
				reportLocation
			});
		}

		public static void SetLocation(float lat, float lon)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setLocation", new object[]
			{
				lat,
				lon
			});
		}

		public static void ClearLocation()
		{
			FlurryAndroid.FlurryAgent.CallStatic("clearLocation", new object[0]);
		}

		public static void SetContinueSessionMillis(long millis)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setContinueSessionMillis", new object[]
			{
				millis
			});
		}

		public static void SetLogEvents(bool logEvents)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setLogEvents", new object[]
			{
				logEvents
			});
		}

		public static void SetCaptureUncaughtExceptions(bool isEnabled)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setCaptureUncaughtExceptions", new object[]
			{
				isEnabled
			});
		}

		public static void AddOrigin(string originName, string originVersion)
		{
			FlurryAndroid.FlurryAgent.CallStatic("addOrigin", new object[]
			{
				originName,
				originVersion
			});
		}

		public static void AddOrigin(string originName, string originVersion, Dictionary<string, string> originParameters)
		{
			using (AndroidJavaObject androidJavaObject = FlurryAndroid.DictionaryToJavaHashMap(originParameters))
			{
				FlurryAndroid.FlurryAgent.CallStatic("addOrigin", new object[]
				{
					originName,
					originVersion,
					androidJavaObject
				});
			}
		}

		public static void SetPulseEnabled(bool isEnabled)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setPulseEnabled", new object[]
			{
				isEnabled
			});
		}

		public static EventRecordStatus LogEvent(string eventId)
		{
            return new EventRecordStatus();
            
			return FlurryAndroid.JavaObjectToEventRecordStatus(FlurryAndroid.FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", new object[]
			{
				eventId
			}));
            
		}

		public static EventRecordStatus LogEvent(string eventId, Dictionary<string, string> parameters)
		{
			EventRecordStatus result;
			using (AndroidJavaObject androidJavaObject = FlurryAndroid.DictionaryToJavaHashMap(parameters))
			{
				result = FlurryAndroid.JavaObjectToEventRecordStatus(FlurryAndroid.FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", new object[]
				{
					eventId,
					androidJavaObject,
					false
				}));
			}
			return result;
		}

		public static EventRecordStatus LogEvent(string eventId, bool timed)
		{
			return FlurryAndroid.JavaObjectToEventRecordStatus(FlurryAndroid.FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", new object[]
			{
				eventId,
				timed
			}));
		}

		public static EventRecordStatus LogEvent(string eventId, Dictionary<string, string> parameters, bool timed)
		{
			EventRecordStatus result;
			using (AndroidJavaObject androidJavaObject = FlurryAndroid.DictionaryToJavaHashMap(parameters))
			{
				result = FlurryAndroid.JavaObjectToEventRecordStatus(FlurryAndroid.FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", new object[]
				{
					eventId,
					androidJavaObject,
					timed
				}));
			}
			return result;
		}

		public static void EndTimedEvent(string eventId)
		{
			FlurryAndroid.FlurryAgent.CallStatic("endTimedEvent", new object[]
			{
				eventId
			});
		}

		public static void EndTimedEvent(string eventId, Dictionary<string, string> parameters)
		{
			using (AndroidJavaObject androidJavaObject = FlurryAndroid.DictionaryToJavaHashMap(parameters))
			{
				FlurryAndroid.FlurryAgent.CallStatic("endTimedEvent", new object[]
				{
					eventId,
					androidJavaObject
				});
			}
		}

		public static void OnError(string errorId, string message, string errorClass)
		{
			FlurryAndroid.FlurryAgent.CallStatic("onError", new object[]
			{
				errorId,
				message,
				errorClass
			});
		}

		public static void OnPageView()
		{
			FlurryAndroid.FlurryAgent.CallStatic("onPageView", new object[0]);
		}

		public static void SetAge(int age)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setAge", new object[]
			{
				age
			});
		}

		public static void SetGender(byte gender)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setGender", new object[]
			{
				gender
			});
		}

		public static void SetUserId(string userId)
		{
			FlurryAndroid.FlurryAgent.CallStatic("setUserId", new object[]
			{
				userId
			});
		}

		private static AndroidJavaObject DictionaryToJavaHashMap(Dictionary<string, string> dictionary)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
				{
					keyValuePair.Key
				}))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Value
					}))
					{
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[]
						{
							androidJavaObject2,
							androidJavaObject3
						}));
					}
				}
			}
			return androidJavaObject;
		}

		private static EventRecordStatus JavaObjectToEventRecordStatus(AndroidJavaObject javaObject)
		{
			return (EventRecordStatus)javaObject.Call<int>("ordinal", new object[0]);
		}

		private static readonly string s_FlurryAgentClassName = "com.flurry.android.FlurryAgent";

		private static readonly string s_UnityPlayerClassName = "com.unity3d.player.UnityPlayer";

		private static readonly string s_UnityPlayerActivityName = "currentActivity";

		private static AndroidJavaClass s_FlurryAgent;
	}

}
