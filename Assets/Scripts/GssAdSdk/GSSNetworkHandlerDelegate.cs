using System;
using UnityEngine;

namespace GssAdSdk
{
	public abstract class GSSNetworkHandlerDelegate : MonoBehaviour
	{
		public abstract void NetworkCallFailure(string errorMsg);

		public abstract void NetworkCallSuccess(string data);
	}
}
