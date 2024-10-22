using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using EncryptStringSample;
using UnityEngine;

namespace GssAdSdk
{
	public class TenlogixAds
	{
		public static bool areAllAdsRemoved()
		{
			if (PlayerPrefs.GetInt("mremoveAllAds", 0) == 1)
			{
				TenlogixAds.mRemoveAllAds = true;
			}
			return TenlogixAds.mRemoveAllAds;
		}

		public static void setConfig(bool debugMode, string mpackageName, string AppName, string AppBundleVersion, NetworkHandler networkHandObj, int screen_Orientation)
		{
			TenlogixAds.Current_ScreenOrientation = screen_Orientation;
			if (PlayerPrefs.GetInt("mremoveAllAds", 0) == 1)
			{
				TenlogixAds.mRemoveAllAds = true;
			}
			UnityEngine.Debug.Log("GSS SDK::" + TenlogixAds.GSS_SDK_Version);
			TenlogixAds.HaltEverything = !UtilsGssSdk.isInternetConnected();
			UtilsGssSdk.isDebugOn = debugMode;
			if (!TenlogixAds.tenlogixAdsSdk_initialized)
			{
			}
			if (!TenlogixAds.HaltEverything)
			{
				networkHandObj.seturl();
				TenlogixAds.packageName = mpackageName;
				networkHandObj.Package = mpackageName;
				AdIDs.AppName = AppName;
				AdIDs.AppBundleID = AppBundleVersion;
			}
			else
			{
				TenlogixAds.isadIDS_loadcounter = 1;
				UnityEngine.Debug.Log("GSS SDK::Internet not Available. Dont Expect any ads.");
			}
		}

		public static string getUniqueID()
		{
			string value = string.Concat(new object[]
			{
				string.Empty,
				DateTime.Now.Ticks / 10000L,
				string.Empty,
				DateTime.UtcNow.ToString("HH:mm dd MMMM,yyyy")
			});
			if (PlayerPrefs.GetString("getUniqueIDs", string.Empty).Equals(string.Empty))
			{
				PlayerPrefs.SetString("getUniqueIDs", value);
				return PlayerPrefs.GetString("getUniqueIDs", null);
			}
			return PlayerPrefs.GetString("getUniqueIDs", null);
		}

		public static void init(string data = null)
		{
			if (!TenlogixAds.HaltEverything)
			{
				if (data == null)
				{
					TenlogixAds.isadIDS_loadcounter = 1;
					TenlogixAds.readDataLocally();
				}
				else
				{
					TenlogixAds.isadIDS_loadcounter = 2;
					data = StringCipher.Decrypt(data, AGameUtils.Cipher_Passwords);
					TenlogixAds.setDataObj(data);
				}
			}
		}

		private static void setDataObj(string obj)
		{
			if (Tenlogiclocal.xmltype == XML.Global)
			{
				TenlogixAds.isDataRead = false;
			}
			if (!TenlogixAds.HaltEverything)
			{
				UtilsGssSdk.Log("Setting Data for Parsing.");
				if (!TenlogixAds.isDataRead)
				{
					TenlogixAds.dataObj = obj;
					TenlogixAds.isDataRead = true;
					TenlogixAds.ParseJson();
					UtilsGssSdk.Log("Successfully Read::::");
				}
				else
				{
					UtilsGssSdk.Log("Data Already Set...");
				}
			}
		}

		private static void readDataLocally()
		{
		}

		public static void temp_parsingxml(string obj)
		{
			TenlogixAds.dataObj = obj;
			TenlogixAds.ParseJson();
		}

		private static void ParseJson()
		{
			XmlReader reader = XmlReader.Create(new StringReader(TenlogixAds.dataObj));
			TenlogixAds.xmlObj = XElement.Load(reader);
			TenlogixAds.getIDs();
		}

		private static void getIDs()
		{
			foreach (XElement xelement in TenlogixAds.xmlObj.Elements("IDs"))
			{
				if (Tenlogiclocal.xmltype == XML.Local || Tenlogiclocal.xmltype == XML.Global)
				{
					AdIDs.AdmobBannerID = xelement.Attribute("AdmobBanner").Value;
					AdIDs.AdmobInterID = xelement.Attribute("AdmobInter").Value;
					AdIDs.LeadboltAppTrackerID = TenlogixAds.decryptID(xelement.Attribute("LeadboltAppTrackerID").Value);
					AdIDs.HeyZapPublisherKey = xelement.Attribute("HeyZapPublisherKey").Value;
					AdIDs.FlurryID = xelement.Attribute("FlurryID").Value;
					AdIDs.GoogleAnalyticsID = xelement.Attribute("GoogleAnalyticsID").Value;
					AdIDs.UnityAdID = xelement.Attribute("UnityAdID").Value;
					AdIDs.StartAppID = xelement.Attribute("StartApp").Value;
					AdIDs.FBAnalyticID = xelement.Attribute("FBAnalyticID").Value;
					AdIDs.FBNativeID = xelement.Attribute("FBNativeID").Value;
					AdIDs.FBInterID = xelement.Attribute("FBInterestialID").Value;
					AdIDs.FBBannerID = xelement.Attribute("FBBannerID").Value;
					AdIDs.AdmobNative = xelement.Attribute("ADmobnative").Value;
					XAttribute xattribute = xelement.Attribute("FBNativeLoadingID");
					if (xattribute != null)
					{
						AdIDs.FBNativeLoadingID = xelement.Attribute("FBNativeLoadingID").Value;
					}
					xattribute = xelement.Attribute("FBNativeExitID");
					if (xattribute != null)
					{
						AdIDs.FBNativeExitID = xelement.Attribute("FBNativeExitID").Value;
					}
					xattribute = xelement.Attribute("FBNativeFailID");
					if (xattribute != null)
					{
						AdIDs.FBNativeFailID = xelement.Attribute("FBNativeFailID").Value;
					}
					xattribute = xelement.Attribute("FBNativePauseID");
					if (xattribute != null)
					{
						AdIDs.FBNativePauseID = xelement.Attribute("FBNativePauseID").Value;
					}
					xattribute = xelement.Attribute("FBNativeSuccessID");
					if (xattribute != null)
					{
						AdIDs.FBNativeSucessID = xelement.Attribute("FBNativeSuccessID").Value;
					}
					xattribute = xelement.Attribute("FBNative_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativeID_All = xelement.Attribute("FBNative_AllID").Value;
					}
					else
					{
						AdIDs.FBNativeID_All = xelement.Attribute("FBNativeID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Loading_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativeLoadingID_All = xelement.Attribute("FBNative_Loading_AllID").Value;
					}
					else
					{
						AdIDs.FBNativeLoadingID_All = xelement.Attribute("FBNativeLoadingID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Exit_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativeExitID_All = xelement.Attribute("FBNative_Exit_AllID").Value;
					}
					else
					{
						AdIDs.FBNativeExitID_All = xelement.Attribute("FBNativeExitID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Fail_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativeFailID_All = xelement.Attribute("FBNative_Fail_AllID").Value;
					}
					else
					{
						AdIDs.FBNativeFailID_All = xelement.Attribute("FBNativeFailID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Pause_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativePauseID_All = xelement.Attribute("FBNative_Pause_AllID").Value;
					}
					else
					{
						AdIDs.FBNativePauseID_All = xelement.Attribute("FBNativePauseID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Success_AllID");
					if (xattribute != null)
					{
						AdIDs.FBNativeSucessID_All = xelement.Attribute("FBNative_Success_AllID").Value;
					}
					else
					{
						AdIDs.FBNativeSucessID_All = xelement.Attribute("FBNativeSuccessID").Value;
					}
					xattribute = xelement.Attribute("FBNative_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativeID_Custom = xelement.Attribute("FBNative_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativeID_Custom = xelement.Attribute("FBNativeID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Loading_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativeLoadingID_Custom = xelement.Attribute("FBNative_Loading_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativeLoadingID_Custom = xelement.Attribute("FBNativeLoadingID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Exit_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativeExitID_Custom = xelement.Attribute("FBNative_Exit_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativeExitID_Custom = xelement.Attribute("FBNativeExitID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Fail_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativeFailID_Custom = xelement.Attribute("FBNative_Fail_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativeFailID_Custom = xelement.Attribute("FBNativeFailID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Pause_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativePauseID_Custom = xelement.Attribute("FBNative_Pause_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativePauseID_Custom = xelement.Attribute("FBNativePauseID").Value;
					}
					xattribute = xelement.Attribute("FBNative_Success_CustomID");
					if (xattribute != null)
					{
						AdIDs.FBNativeSucessID_Custom = xelement.Attribute("FBNative_Success_CustomID").Value;
					}
					else
					{
						AdIDs.FBNativeSucessID_Custom = xelement.Attribute("FBNativeSuccessID").Value;
					}
					XAttribute xattribute2 = xelement.Attribute("FBLoadingFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBLoadingFill").Value.Contains("t") || xelement.Attribute("FBLoadingFill").Value.Contains("T"))
						{
							TenlogixAds.FBloadingFill = true;
						}
						else
						{
							TenlogixAds.FBloadingFill = false;
						}
					}
					else
					{
						TenlogixAds.FBloadingFill = false;
					}
					xattribute2 = xelement.Attribute("FBPanelFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBPanelFill").Value.Contains("t") || xelement.Attribute("FBPanelFill").Value.Contains("T"))
						{
							TenlogixAds.FBpanelFill = true;
						}
						else
						{
							TenlogixAds.FBpanelFill = false;
						}
					}
					else
					{
						TenlogixAds.FBpanelFill = false;
					}
					xattribute2 = xelement.Attribute("FBSplashFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBSplashFill").Value.Contains("t") || xelement.Attribute("FBSplashFill").Value.Contains("T"))
						{
							TenlogixAds.FBSplashFill = true;
						}
						else
						{
							TenlogixAds.FBSplashFill = false;
						}
					}
					else
					{
						TenlogixAds.FBSplashFill = false;
					}
					xattribute2 = xelement.Attribute("FBLevelFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBLevelFill").Value.Contains("t") || xelement.Attribute("FBLevelFill").Value.Contains("T"))
						{
							TenlogixAds.FBLevelFill = true;
						}
						else
						{
							TenlogixAds.FBLevelFill = false;
						}
					}
					else
					{
						TenlogixAds.FBLevelFill = false;
					}
					xattribute2 = xelement.Attribute("FBPauseFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBPauseFill").Value.Contains("t") || xelement.Attribute("FBPauseFill").Value.Contains("T"))
						{
							TenlogixAds.FBPauseFill = true;
						}
						else
						{
							TenlogixAds.FBPauseFill = false;
						}
					}
					else
					{
						TenlogixAds.FBPauseFill = false;
					}
					xattribute2 = xelement.Attribute("FBSucessFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBSucessFill").Value.Contains("t") || xelement.Attribute("FBSucessFill").Value.Contains("T"))
						{
							TenlogixAds.FBSucessFill = true;
						}
						else
						{
							TenlogixAds.FBSucessFill = false;
						}
					}
					else
					{
						TenlogixAds.FBSucessFill = false;
					}
					xattribute2 = xelement.Attribute("FBFailFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBFailFill").Value.Contains("t") || xelement.Attribute("FBFailFill").Value.Contains("T"))
						{
							TenlogixAds.FBFailFill = true;
						}
						else
						{
							TenlogixAds.FBFailFill = false;
						}
					}
					else
					{
						TenlogixAds.FBFailFill = false;
					}
					xattribute2 = xelement.Attribute("FBExitFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBExitFill").Value.Contains("t") || xelement.Attribute("FBExitFill").Value.Contains("T"))
						{
							TenlogixAds.FBExitFill = true;
						}
						else
						{
							TenlogixAds.FBExitFill = false;
						}
					}
					else
					{
						TenlogixAds.FBExitFill = false;
					}
					xattribute2 = xelement.Attribute("FBClickFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBClickFill").Value.Contains("t") || xelement.Attribute("FBClickFill").Value.Contains("T"))
						{
							TenlogixAds.FBClickFill = true;
						}
						else
						{
							TenlogixAds.FBClickFill = false;
						}
					}
					else
					{
						TenlogixAds.FBClickFill = false;
					}
					xattribute2 = xelement.Attribute("FBDelayFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBDelayFill").Value.Contains("t") || xelement.Attribute("FBDelayFill").Value.Contains("T"))
						{
							TenlogixAds.FBDelayFill = true;
						}
						else
						{
							TenlogixAds.FBDelayFill = false;
						}
					}
					else
					{
						TenlogixAds.FBDelayFill = true;
					}
					xattribute2 = xelement.Attribute("FBDelayPanelFill");
					if (xattribute2 != null)
					{
						if (xelement.Attribute("FBDelayPanelFill").Value.Contains("t") || xelement.Attribute("FBDelayPanelFill").Value.Contains("T"))
						{
							TenlogixAds.FBDelayPanelFill = true;
						}
						else
						{
							TenlogixAds.FBDelayPanelFill = false;
						}
					}
					else
					{
						TenlogixAds.FBDelayPanelFill = true;
					}
					if (xelement.Attribute("OverrideFlag") != null)
					{
						if (xelement.Attribute("OverrideFlag").Value != string.Empty)
						{
							UnityEngine.Debug.Log("new flag value" + xelement.Attribute("OverrideFlag").Value);
							if (xelement.Attribute("OverrideFlag").Value.Contains("t") || xelement.Attribute("OverrideFlag").Value.Contains("T"))
							{
								TenlogixAds.OverrideFlag = true;
							}
							else if (xelement.Attribute("OverrideFlag").Value.Contains("f") || xelement.Attribute("OverrideFlag").Value.Contains("F"))
							{
								TenlogixAds.OverrideFlag = false;
							}
						}
						else
						{
							UnityEngine.Debug.LogWarning("OverrideFlag attribute have no value");
							TenlogixAds.OverrideFlag = false;
						}
					}
					else
					{
						UnityEngine.Debug.LogWarning("OverrideFlag attribute not found");
						TenlogixAds.OverrideFlag = false;
					}
					if (xelement.Attribute("ADMobFill").Value.Contains("t") || xelement.Attribute("ADMobFill").Value.Contains("T"))
					{
						TenlogixAds.AdmobFill = true;
					}
					else
					{
						TenlogixAds.AdmobFill = false;
					}
				}
				if (xelement.Attribute("BackFillEnabled").Value.Contains("t") || xelement.Attribute("BackFillEnabled").Value.Contains("T"))
				{
					TenlogixAds.isBackFilledEnabled = true;
				}
				else if (xelement.Attribute("BackFillEnabled").Value.Contains("n") || xelement.Attribute("BackFillEnabled").Value.Contains("N"))
				{
					TenlogixAds.isBackFilledEnabled = false;
				}
				else if (Tenlogiclocal.xmltype == XML.Global && TenlogixAds.isBackFilledEnabled)
				{
					TenlogixAds.isBackFilledEnabled = true;
				}
				else
				{
					TenlogixAds.isBackFilledEnabled = false;
				}
				UtilsGssSdk.Log(string.Concat(new object[]
				{
					"ADMOB BANNER ID: ",
					AdIDs.AdmobBannerID,
					"\nADMOB INTER ID: ",
					AdIDs.AdmobInterID,
					"\nLEADBOLT APP TRACKER ID: ",
					AdIDs.LeadboltAppTrackerID,
					"\nHEYZAP PUBLISHER KEY: ",
					AdIDs.HeyZapPublisherKey,
					"\nFlurry ID: ",
					AdIDs.FlurryID,
					"\nGoogle Analytics ID: ",
					AdIDs.GoogleAnalyticsID,
					"\nUnity Ad ID: ",
					AdIDs.UnityAdID,
					"\nStartApp Publisher Ad ID: ",
					AdIDs.StartAppID,
					"\nBackfilling is: ",
					TenlogixAds.isBackFilledEnabled
				}));
				TenlogixAds.CrossPromoPackages = string.Concat(new string[]
				{
					xelement.Attribute("PackageA").Value,
					"*",
					xelement.Attribute("PackageB").Value,
					"*",
					xelement.Attribute("PackageC").Value,
					"*",
					xelement.Attribute("PackageD").Value,
					"*",
					xelement.Attribute("PackageE").Value
				});
				if ((Tenlogiclocal.xmltype == XML.Global && (xelement.Attribute("BackFillEnabled").Value.Contains("t") || xelement.Attribute("BackFillEnabled").Value.Contains("T"))) || (Tenlogiclocal.xmltype == XML.AGlobal && (xelement.Attribute("BackFillEnabled").Value.Contains("t") || xelement.Attribute("BackFillEnabled").Value.Contains("T"))))
				{
					TenlogixAds.PackageA = xelement.Attribute("PackageA").Value;
					TenlogixAds.PackageB = xelement.Attribute("PackageB").Value;
					TenlogixAds.PackageC = xelement.Attribute("PackageC").Value;
					TenlogixAds.PackageD = xelement.Attribute("PackageD").Value;
					TenlogixAds.PackageE = xelement.Attribute("PackageE").Value;
					TenlogixAds.PackageF = xelement.Attribute("PackageF").Value;
					TenlogixAds.PackageG = xelement.Attribute("PackageG").Value;
					TenlogixAds.PackageH = xelement.Attribute("PackageH").Value;
					TenlogixAds.PackageI = xelement.Attribute("PackageI").Value;
					TenlogixAds.PackageJ = xelement.Attribute("PackageJ").Value;
					TenlogixAds.GUR = xelement.Attribute("GICONUR").Value;
					if (Application.platform == RuntimePlatform.Android)
					{
						TenlogixAds.arlist.Clear();
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageA)))
						{
							UnityEngine.Debug.Log("apnotpresent");
							TenlogixAds.arlist.Add(TenlogixAds.PackageA);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageB)))
						{
							UnityEngine.Debug.Log("apnotpresent");
							TenlogixAds.arlist.Add(TenlogixAds.PackageB);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageC)))
						{
							UnityEngine.Debug.Log("apnotpresent");
							TenlogixAds.arlist.Add(TenlogixAds.PackageC);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageD)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageD);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageE)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageE);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageF)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageF);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageA)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageA);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageG)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageH);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageI)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageI);
						}
						if (!TenlogixAds.isapppresent(TenlogixAds.GetProductName(TenlogixAds.PackageJ)))
						{
							TenlogixAds.arlist.Add(TenlogixAds.PackageJ);
						}
						UnityEngine.Debug.Log("listcontains" + TenlogixAds.arlist.Count);
					}
					else
					{
						TenlogixAds.arrpackages[0] = TenlogixAds.PackageA;
						TenlogixAds.arrpackages[1] = TenlogixAds.PackageB;
						TenlogixAds.arrpackages[2] = TenlogixAds.PackageC;
						TenlogixAds.arrpackages[3] = TenlogixAds.PackageD;
						TenlogixAds.arrpackages[4] = TenlogixAds.PackageE;
						TenlogixAds.arrpackages[5] = TenlogixAds.PackageF;
						TenlogixAds.arrpackages[6] = TenlogixAds.PackageG;
						TenlogixAds.arrpackages[7] = TenlogixAds.PackageH;
						TenlogixAds.arrpackages[8] = TenlogixAds.PackageI;
						TenlogixAds.arrpackages[9] = TenlogixAds.PackageJ;
					}
				}
				if (Tenlogiclocal.xmltype == XML.Local || Tenlogiclocal.xmltype == XML.Global)
				{
					AGameUtils.MORE_APPS_DN = xelement.Attribute("DN").Value;
					AGameUtils.FLURRY_ID = xelement.Attribute("FlurryID").Value;
					TenlogixAds.UR = xelement.Attribute("UR").Value;
					TenlogixAds.AUR = xelement.Attribute("AUR").Value;
					MoPubAds._bannerAdUnitId = xelement.Attribute("BanneradID").Value;
					MoPubAds._interstitialOnSelectionId = xelement.Attribute("SelectionadID").Value;
					MoPubAds._interstitialOnGpEndId = xelement.Attribute("GpendadID").Value;
					MoPubAds._interstitialOnExit = xelement.Attribute("ExitadID").Value;
					MoPubAds._interstitialOnVideo = xelement.Attribute("VideoadID").Value;
				}
				if (Tenlogiclocal.xmltype == XML.Global)
				{
					UnityEngine.Debug.Log("Globalxml type complete");
				}
				if (Tenlogiclocal.xmltype == XML.Local)
				{
					Tenlogiclocal.xmltype = XML.AGlobal;
					UnityEngine.Debug.Log("localadscomplete");
				}
				else if (Tenlogiclocal.xmltype == XML.AGlobal && TenlogixAds.isadIDS_loadcounter != 0)
				{
					Tenlogiclocal.xmltype = XML.Global;
					TenlogixAds.isadIDS_loadcounter = 0;
					TenlogixAds.tenlogixAdsSdk_initialized = false;
					UnityEngine.Debug.Log("Acount type complete");
				}
				UtilsGssSdk.Log(string.Empty + TenlogixAds.CrossPromoPackages);
			}
			TenlogixAds.InitializeAllAdNetworks();
		}

		private static void InitializeAllAdNetworks()
		{
			PlayerPrefs.SetString("StartAppID", AdIDs.StartAppID);
		}

		private static void VideoAdRewardedEvent(int amount)
		{
			if (TenlogixAds.callbackObj != null)
			{
				TenlogixAds.callbackObj.incentivizeUsers();
				UtilsGssSdk.Log("The Video was successfully shown. User shall be rewarded.");
			}
			else
			{
				UtilsGssSdk.Log("CallbackObj is Null..User will not be rewarded.");
			}
		}

		private static string decryptID(string val)
		{
			if (val.Length < 5)
			{
				UtilsGssSdk.Log("Unable to Decrypt ID. Please verify the Encrypted IDs.");
				return string.Empty;
			}
			List<char> list = new List<char>();
			string text = string.Empty;
			char c = 'a';
			bool flag = val.Length % 2 == 0;
			char c2;
			if (flag)
			{
				int num = 1;
				int num2 = val.Length - 2;
				c2 = val[0];
				c = val[val.Length - 1];
				for (int i = num; i <= num2; i++)
				{
					list.Add(val[i]);
				}
			}
			else
			{
				int num = 1;
				int num2 = val.Length - 1;
				c2 = val[0];
				for (int j = num; j <= num2; j++)
				{
					list.Add(val[j]);
				}
			}
			for (int k = 0; k <= list.Count - 1; k += 2)
			{
				char value = list[k];
				list[k] = list[k + 1];
				list[k + 1] = value;
			}
			for (int l = 0; l <= list.Count - 1; l++)
			{
				text += list[l];
			}
			if (flag)
			{
				return c2 + text + c;
			}
			return c2 + text;
		}

		public static string getaddsurlpackage()
		{
			int num = UnityEngine.Random.Range(0, 9);
			return TenlogixAds.arrpackages[num];
		}

		public static bool isapppresent(string appname)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			bool result;
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				@static.Call<AndroidJavaObject>("getPackageManager", new object[0]).Call<AndroidJavaObject>("getPackageInfo", new object[]
				{
					appname,
					0
				});
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static string GetProductName(string getstringname)
		{
			string text = getstringname;
			if (text.Contains("/") && text.Contains("."))
			{
				int num = text.LastIndexOf("/", StringComparison.Ordinal) + 1;
				getstringname = text.Substring(num, text.Length - num);
				num = getstringname.LastIndexOf(".", StringComparison.Ordinal) + 1;
				return getstringname.Substring(0, getstringname.Length - (getstringname.Length - num + 1));
			}
			return text;
		}

		private static string GSS_SDK_Version = "Version 3.4 Updated on 30/9/2015";

		private static AndroidJavaClass exitClass;

		private static AndroidJavaObject exitClassObject;

		private static AndroidJavaObject gssPluginUtils = null;

		private static AndroidJavaObject activityContext = null;

		private static string dataObj;

		private static bool isDataRead;

		private static string packageName;

		private static ArrayList mAdData;

		public static string PackageA = string.Empty;

		public static string PackageB = string.Empty;

		public static string PackageC = string.Empty;

		public static string PackageD = string.Empty;

		public static string PackageE = string.Empty;

		public static string PackageF = string.Empty;

		public static string PackageG = string.Empty;

		public static string PackageH = string.Empty;

		public static string PackageI = string.Empty;

		public static string PackageJ = string.Empty;

		public static string DN = string.Empty;

		public static string UR = string.Empty;

		public static string AUR = string.Empty;

		public static string GUR = string.Empty;

		public static bool AdmobFill = false;

		public static bool FBloadingFill = false;

		public static bool FBpanelFill = false;

		public static bool FBSplashFill = false;

		public static bool FBLevelFill = false;

		public static bool FBPauseFill = false;

		public static bool FBSucessFill = false;

		public static bool FBFailFill = false;

		public static bool FBExitFill = false;

		public static bool FBClickFill = false;

		public static bool FBDelayFill = false;

		public static bool FBDelayPanelFill = false;

		public static string[] arrpackages = new string[10];

		public static List<string> arlist = new List<string>();

		public static int BannerTopLeftPosition = 1;

		public static int BannerTopRightPosition = 2;

		public static int BannerBottomLeftPosition = 3;

		public static int BannerBottomRightPosition = 4;

		private static bool bannerInitialized;

		private static bool HaltEverything;

		private static bool mRemoveAllAds;

		public static bool isBackFilledEnabled = false;

		public static bool OverrideFlag = false;

		public static AndroidJavaClass androidJC;

		public static AndroidJavaObject crossPromo_activity;

		public static AndroidJavaClass crossPromoClass;

		public static string CrossPromoPackages = null;

		public static int ScreenOrientation_Landscape = 1;

		public static int ScreenOrientation_Portrait = 2;

		public static int Current_ScreenOrientation = 1;

		public static int isadIDS_loadcounter = 0;

		public static bool tenlogixAdsSdk_initialized;

		private static XElement xmlObj;

		private static bool mLoadBannersbyDefault;

		private static string mFirstAdPlace;

		private static string mFirstInterAdPlace;

		public static bool callInterAfterDataRead;

		private static IuserInIcentivizedCallback callbackObj;

		private static bool areAnalyticsInitialized;
	}
}
