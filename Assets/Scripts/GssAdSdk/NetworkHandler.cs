using System;
using System.Collections;
using UnityEngine;

namespace GssAdSdk
{
	public class NetworkHandler
	{
		public NetworkHandler(GSSNetworkHandlerDelegate monoBehaviour)
		{
			this.networkDelegate = monoBehaviour;
		}

		public void seturl()
		{
			if (Tenlogiclocal.xmltype == XML.Global)
			{
				this.url = string.Empty + TenlogixAds.UR;
			}
			else if (Tenlogiclocal.xmltype == XML.AGlobal)
			{
				this.url = string.Empty + TenlogixAds.AUR;
			}
		}

		public string Package
		{
			set
			{
				if (UtilsGssSdk.isInternetConnected())
				{
					this.SendRequest();
				}
				else
				{
					UnityEngine.Debug.Log("Internet Not Available...");
				}
			}
		}

		public void SendRequest()
		{
			WWW www = new WWW(this.url);
			UtilsGssSdk.Log("URL is: " + this.url);
			this.networkDelegate.StartCoroutine(this.WaitForRequest(www));
		}

		private IEnumerator WaitForRequest(WWW www)
		{
			yield return www;
			if (www.error == null)
			{
				if (www.text.Contains("site-info"))
				{
					UtilsGssSdk.Log("URL is: " + this.url);
					this.networkDelegate.NetworkCallFailure("No Ad File Found");
				}
				else
				{
					UtilsGssSdk.Log("WWW Ok!: " + www.text);
					this.networkDelegate.NetworkCallSuccess(www.text);
				}
			}
			else
			{
				UtilsGssSdk.Log("WWW Error: " + www.error);
				this.networkDelegate.NetworkCallFailure(www.error);
			}
			this.networkDelegate = null;
			yield break;
		}

		public static IEnumerator UntilDone(AsyncOperation op)
		{
			while (!op.isDone)
			{
				yield return op;
			}
			yield break;
		}

		private string url = string.Empty;

		private string package;

		private GSSNetworkHandlerDelegate networkDelegate;
	}
}
