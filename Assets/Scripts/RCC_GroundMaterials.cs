using System;
using UnityEngine;

[Serializable]
public class RCC_GroundMaterials : ScriptableObject
{
	public static RCC_GroundMaterials Instance
	{
		get
		{
			if (RCC_GroundMaterials.instance == null)
			{
				RCC_GroundMaterials.instance = (Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterials);
			}
			return RCC_GroundMaterials.instance;
		}
	}

	public static RCC_GroundMaterials instance;

	public RCC_GroundMaterials.GroundMaterialFrictions[] frictions;

	public bool useTerrainSplatMapForGroundFrictions;

	public PhysicMaterial terrainPhysicMaterial;

	public int[] terrainSplatMapIndex;

	[Serializable]
	public class GroundMaterialFrictions
	{
		public PhysicMaterial groundMaterial;

		public float forwardStiffness = 1f;

		public float sidewaysStiffness = 1f;

		public float slip = 0.25f;

		public float damp = 1f;

		public GameObject groundParticles;

		public AudioClip groundSound;
	}
}
