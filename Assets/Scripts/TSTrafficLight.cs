using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TSTrafficLight : MonoBehaviour
{
	private void Start()
	{
		this.trafficLightID = base.GetInstanceID();
		if (this.manager == null)
		{
			this.manager = (UnityEngine.Object.FindObjectOfType(typeof(TSMainManager)) as TSMainManager);
		}
		if (this.manager == null)
		{
			this.weHaveManager = false;
		}
		else
		{
			this.weHaveManager = true;
		}
		foreach (TSTrafficLight.TSLight tslight in this.lights)
		{
			if (tslight.enableDisableRenderer && tslight.lightMeshRenderer != null)
			{
				tslight.lightMeshRenderer.enabled = false;
			}
			if (tslight.lightGameObject != null)
			{
				tslight.lightGameObject.SetActive(false);
			}
		}
		if (this.lights.Count > 0 && this.weHaveManager)
		{
			base.StartCoroutine(this.PlayLights());
		}
		this.RegisterIntoLanes();
	}

	private void RegisterIntoLanes()
	{
	}

	private IEnumerator PlayLights()
	{
		for (;;)
		{
			if (this.lightToPlay >= this.lights.Count)
			{
				this.lightToPlay = 0;
			}
			if (this.lights[this.lightToPlay] != null)
			{
				if (!this.lights[this.lightToPlay].enableDisableRenderer)
				{
					if (this.lights[this.lightToPlay].lightMeshRenderer != null && this.lights[this.lightToPlay].lightMeshRenderer.material != null)
					{
						this.lights[this.lightToPlay].lightMeshRenderer.material.SetTexture(this.lights[this.lightToPlay].shaderTexturePropertyName, this.lights[this.lightToPlay].lightTexture);
					}
				}
				else if (this.lights[this.lightToPlay].lightMeshRenderer != null)
				{
					this.lights[this.lightToPlay].lightMeshRenderer.enabled = true;
				}
				if (this.lights[this.lightToPlay].lightGameObject != null)
				{
					this.lights[this.lightToPlay].lightGameObject.SetActive(true);
				}
				TSTrafficLight.LightType lightType = this.lights[this.lightToPlay].lightType;
				if (lightType != TSTrafficLight.LightType.Yellow)
				{
					if (lightType != TSTrafficLight.LightType.Red)
					{
						if (lightType == TSTrafficLight.LightType.Green)
						{
							for (int i = 0; i < this.pointsNormalLight.Count; i++)
							{
								this.ChangeReservation(this.pointsNormalLight[i], 0, -1);
							}
						}
					}
					else
					{
						for (int j = 0; j < this.pointsNormalLight.Count; j++)
						{
							this.ChangeReservation(this.pointsNormalLight[j], this.trafficLightID, this.trafficLightID);
						}
					}
				}
				else if (this.yellowLightsStopTraffic)
				{
					for (int k = 0; k < this.pointsNormalLight.Count; k++)
					{
						this.ChangeReservation(this.pointsNormalLight[k], this.trafficLightID, this.trafficLightID);
					}
				}
				yield return new WaitForSeconds(this.lights[this.lightToPlay].lightTime);
				if (this.lights[this.lightToPlay].enableDisableRenderer && this.lights[this.lightToPlay].lightMeshRenderer != null)
				{
					this.lights[this.lightToPlay].lightMeshRenderer.enabled = false;
				}
				if (this.lights[this.lightToPlay].lightGameObject != null)
				{
					this.lights[this.lightToPlay].lightGameObject.SetActive(false);
				}
				this.lightToPlay++;
			}
			else
			{
				this.lightToPlay++;
			}
		}
		yield break;
	}

	private IEnumerator UpdateRemaningGreenLightTime(List<TSTrafficLight.TSPointReference> point, float time)
	{
		for (int i = 0; i < point.Count; i++)
		{
			this.manager.lanes[point[i].lane].connectors[point[i].connector].remainingGreenLightTime = time;
		}
		while (this.manager.lanes[point[0].lane].connectors[point[0].connector].remainingGreenLightTime > 0f)
		{
			for (int j = 0; j < point.Count; j++)
			{
				this.manager.lanes[point[j].lane].connectors[point[j].connector].remainingGreenLightTime -= Time.deltaTime;
				if (this.manager.lanes[point[j].lane].connectors[point[j].connector].remainingGreenLightTime < 0f)
				{
					this.manager.lanes[point[j].lane].connectors[point[j].connector].remainingGreenLightTime = 0f;
				}
			}
			yield return null;
		}
		yield break;
	}

	private void ChangeReservation(TSTrafficLight.TSPointReference point, int ID, int lID)
	{
		if (point.connector == -1)
		{
			this.manager.lanes[point.lane].points[point.point].reservationID = ID;
		}
		else
		{
			this.manager.lanes[point.lane].connectors[point.connector].points[point.point].reservationID = ID;
			if (ID == 0 && lID == -1)
			{
				this.manager.lanes[point.lane].connectors[point.connector].connectorReservedByTrafficLight = false;
				this.manager.lanes[point.lane].connectors[point.connector].remainingGreenLightTime = -1f;
			}
			else
			{
				this.manager.lanes[point.lane].connectors[point.connector].connectorReservedByTrafficLight = true;
				this.manager.lanes[point.lane].connectors[point.connector].remainingGreenLightTime = -1f;
				this.manager.lanes[point.lane].connectors[point.connector].points[point.point].carwhoReserved = null;
			}
		}
	}

	[SerializeField]
	[XmlArray("Lights")]
	[XmlArrayItem("Light")]
	public List<TSTrafficLight.TSLight> lights = new List<TSTrafficLight.TSLight>();

	[SerializeField]
	public TSMainManager manager;

	private int lastLane;

	private int lastPoint;

	private int lastConnector;

	private int trafficLightID;

	[SerializeField]
	public List<TSTrafficLight.TSPointReference> pointsNormalLight = new List<TSTrafficLight.TSPointReference>();

	public bool enableLight = true;

	private bool weHaveManager;

	public bool yellowLightsStopTraffic = true;

	public float lightRange = 500f;

	public int lightToPlay;

	[Serializable]
	public class TSLight
	{
		[XmlElement("lightTimer")]
		public float lightTimer;

		[XmlElement("lightType")]
		public TSTrafficLight.LightType lightType;

		[XmlElement("lightTime")]
		public float lightTime = 20f;

		[XmlIgnore]
		public Texture2D lightTexture;

		[XmlIgnore]
		public MeshRenderer lightMeshRenderer;

		[XmlElement("enableDisableRenderer")]
		public bool enableDisableRenderer;

		[XmlIgnore]
		public GameObject lightGameObject;

		[XmlElement("shaderTexturePropertyName")]
		public string shaderTexturePropertyName = "_MainTex";
	}

	[Serializable]
	public class TSPointReference
	{
		[XmlElement("lane")]
		public int lane;

		[XmlElement("connector")]
		public int connector;

		[XmlElement("point")]
		public int point;
	}

	[Serializable]
	public enum LightType
	{
		Green,
		Red,
		Yellow,
		NoLights
	}
}
