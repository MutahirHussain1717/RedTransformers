using System;
using Analytics;
using Facebook.Unity;
using GssAdSdk;
using UnityEngine;

namespace AudienceNetwork
{
	[RequireComponent(typeof(RectTransform))]
	public class NativeAdHandler : AdHandler
	{
		public void startImpressionValidation()
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			this.shouldCheckImpression = true;
		}

		public void stopImpressionValidation()
		{
			this.shouldCheckImpression = false;
		}

		private void OnGUI()
		{
			this.checkImpression();
		}

		private bool checkImpression()
		{
			float time = Time.time;
			float num = time - this.lastImpressionCheckTime;
			if (this.shouldCheckImpression && !this.impressionLogged && num > (float)this.checkViewabilityInterval)
			{
				this.lastImpressionCheckTime = time;
				GameObject gameObject = base.gameObject;
				Camera x = this.camera;
				if (x == null)
				{
					x = base.GetComponent<Camera>();
				}
				if (x == null)
				{
					x = Camera.main;
				}
				while (gameObject != null)
				{
					Canvas component = gameObject.GetComponent<Canvas>();
					if (component != null && component.renderMode == RenderMode.WorldSpace)
					{
						break;
					}
					if (!this.checkGameObjectViewability(x, gameObject))
					{
						if (this.validationCallback != null)
						{
							this.validationCallback(false);
						}
						return false;
					}
					gameObject = null;
				}
				if (this.validationCallback != null)
				{
					this.validationCallback(true);
				}
				this.impressionLogged = true;
			}
			return this.impressionLogged;
		}

		private bool logViewability(bool success, string message)
		{
			if (!success)
			{
				UnityEngine.Debug.Log("Viewability validation failed: " + message);
			}
			else
			{
				UnityEngine.Debug.Log("Viewability validation success! " + message);
			}
			return success;
		}

		public void SendAalatics(string message)
		{
			if (base.gameObject.GetComponent<NativeAdTest>() != null)
			{
				Analytics.MonoSingleton<Flurry>.Instance.LogEvent(message + base.gameObject.GetComponent<NativeAdTest>().Adplace.ToString());
				if (FB.IsInitialized)
				{
					FB.LogAppEvent(message + base.gameObject.GetComponent<NativeAdTest>().Adplace.ToString(), new float?(1f), null);
				}
			}
		}

		private bool checkGameObjectViewability(Camera camera, GameObject gameObject)
		{
			if (gameObject == null)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject is null.Not Overridden");
					return this.logViewability(false, "GameObject is null.");
				}
				this.SendAalatics("GameObject is null.");
			}
			if (camera == null)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("Camera is null.Not Overridden");
					return this.logViewability(false, "Camera is null.");
				}
				this.SendAalatics("Camera is null.");
			}
			if (!gameObject.activeInHierarchy)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject is not active in hierarchy.Not Overridden");
					return this.logViewability(false, "GameObject is not active in hierarchy.");
				}
				this.SendAalatics("GameObject is not active in hierarchy.");
			}
			CanvasGroup[] components = gameObject.GetComponents<CanvasGroup>();
			foreach (CanvasGroup canvasGroup in components)
			{
				if (canvasGroup.alpha < this.minAlpha)
				{
					if (!TenlogixAds.OverrideFlag)
					{
						this.SendAalatics("GameObject has a CanvasGroup with less than the minimum alpha required.Not Overridden");
						return this.logViewability(false, "GameObject has a CanvasGroup with less than the minimum alpha required.");
					}
					this.SendAalatics("GameObject has a CanvasGroup with less than the minimum alpha required.");
				}
			}
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Vector3 position = rectTransform.position;
			float width = rectTransform.rect.width;
			float height = rectTransform.rect.height;
			Vector3 position2 = position;
			position2.x -= width / 2f;
			position2.y -= height / 2f;
			Vector3 position3 = position;
			position3.x += width / 2f;
			position3.y += height / 2f;
			Vector3 lowerLeft = camera.WorldToScreenPoint(position2);
			Vector3 upperRight = camera.WorldToScreenPoint(position3);
			float num = upperRight.x - lowerLeft.x;
			float num2 = upperRight.y - lowerLeft.y;
			Rect pixelRect = camera.pixelRect;
			Rect screen = new Rect(pixelRect.x * Screen.dpi, pixelRect.y * Screen.dpi, pixelRect.width * Screen.dpi, pixelRect.height * Screen.dpi);
			if (num <= 0f && num2 <= 0f)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject's height/width is less than or equal to zero.Not Overridden");
					return this.logViewability(false, "GameObject's height/width is less than or equal to zero.");
				}
				this.SendAalatics("GameObject's height/width is less than or equal to zero.");
			}
			if (!this.CheckScreenPosition(lowerLeft, upperRight, screen))
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("Not enough of the GameObject is inside the viewport.Not Overridden");
					return this.logViewability(false, "Not enough of the GameObject is inside the viewport.");
				}
				this.SendAalatics("Not enough of the GameObject is inside the viewport.");
			}
			if (num / width < (float)this.minViewabilityPercentage || num2 / height < (float)this.minViewabilityPercentage)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("The GameObject is too small to count as an impression.Not Overridden");
					return this.logViewability(false, "The GameObject is too small to count as an impression.");
				}
				this.SendAalatics("The GameObject is too small to count as an impression.");
			}
			Vector3 eulerAngles = rectTransform.eulerAngles;
			int num3 = Mathf.FloorToInt(eulerAngles.x);
			int num4 = Mathf.FloorToInt(eulerAngles.y);
			int num5 = Mathf.FloorToInt(eulerAngles.z);
			int num6 = 360 - this.maxRotation;
			int num7 = this.maxRotation;
			if (num3 < num6 && num3 > num7)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject is rotated too much. (x axis).Not Overridden");
					return this.logViewability(false, "GameObject is rotated too much. (x axis)");
				}
				this.SendAalatics("GameObject is rotated too much. (x axis)");
			}
			else if (num4 < num6 && num4 > num7)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject is rotated too much. (y axis).Not Overridden");
					return this.logViewability(false, "GameObject is rotated too much. (y axis)");
				}
				this.SendAalatics("GameObject is rotated too much. (y axis)");
			}
			else if (num5 < num6 && num5 > num7)
			{
				if (!TenlogixAds.OverrideFlag)
				{
					this.SendAalatics("GameObject is rotated too much. (z axis).Not Overridden");
					return this.logViewability(false, "GameObject is rotated too much. (z axis)");
				}
				this.SendAalatics("GameObject is rotated too much. (z axis)");
			}
			if (TenlogixAds.OverrideFlag)
			{
				this.SendAalatics("--------------- VALID IMPRESSION REGISTERED! ----------------------");
			}
			else
			{
				this.SendAalatics("--------------- VALID IMPRESSION REGISTERED! ----------------------.Not Overridden");
			}
			return this.logViewability(true, "--------------- VALID IMPRESSION REGISTERED! ----------------------");
		}

		private bool CheckScreenPosition(Vector3 lowerLeft, Vector3 upperRight, Rect screen)
		{
			float num = 0f;
			float num2 = 0f;
			if (lowerLeft.x < screen.xMin)
			{
				num += Mathf.Abs(lowerLeft.x - screen.xMin);
			}
			if (upperRight.x > screen.xMax)
			{
				num += Mathf.Abs(upperRight.x - screen.xMax);
			}
			float num3 = 1f - num / (upperRight.x - lowerLeft.x);
			if (num3 < (float)this.minViewabilityPercentage)
			{
				return false;
			}
			if (lowerLeft.y < screen.yMin)
			{
				num2 += Mathf.Abs(lowerLeft.y - screen.yMin);
			}
			if (upperRight.y > screen.yMax)
			{
				num2 += Mathf.Abs(upperRight.y - screen.yMax);
			}
			float num4 = 1f - num2 / (upperRight.y - lowerLeft.y);
			return num4 >= (float)this.minViewabilityPercentage;
		}

		public int minViewabilityPercentage;

		public float minAlpha;

		public int maxRotation;

		public int checkViewabilityInterval;

		public Camera camera;

		public FBNativeAdHandlerValidationCallback validationCallback;

		private float lastImpressionCheckTime;

		private bool impressionLogged;

		private bool shouldCheckImpression;
	}
}
