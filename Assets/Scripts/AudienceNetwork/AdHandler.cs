using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceNetwork
{
	public class AdHandler : MonoBehaviour
	{
		public void executeOnMainThread(Action action)
		{
			AdHandler.executeOnMainThreadQueue.Enqueue(action);
		}

		private void Update()
		{
			while (AdHandler.executeOnMainThreadQueue.Count > 0)
			{
				AdHandler.executeOnMainThreadQueue.Dequeue()();
			}
		}

		public void removeFromParent()
		{
			UnityEngine.Object.Destroy(this);
		}

		private static readonly Queue<Action> executeOnMainThreadQueue = new Queue<Action>();
	}
}
