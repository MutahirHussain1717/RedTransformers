using System;
using UnityEngine;

[Serializable]
public class RCC_Settings : ScriptableObject
{
	public static RCC_Settings Instance
	{
		get
		{
			if (RCC_Settings.instance == null)
			{
				RCC_Settings.instance = (Resources.Load("RCCAssets/RCC_Settings") as RCC_Settings);
			}
			return RCC_Settings.instance;
		}
	}

	public static RCC_Settings instance;

	public int toolbarSelectedIndex;

	[Range(0.005f, 0.06f)]
	public float fixedTimeStep = 0.02f;

	[Range(0.5f, 20f)]
	public float maxAngularVelocity = 6f;

	public RCC_Settings.BehaviorType behaviorType;

	public bool useFixedWheelColliders = true;

	public RCC_Settings.ControllerType controllerType;

	public string verticalInput = "Vertical";

	public string horizontalInput = "Horizontal";

	public KeyCode handbrakeKB = KeyCode.Space;

	public KeyCode startEngineKB = KeyCode.I;

	public KeyCode lowBeamHeadlightsKB = KeyCode.L;

	public KeyCode highBeamHeadlightsKB = KeyCode.K;

	public KeyCode rightIndicatorKB = KeyCode.E;

	public KeyCode leftIndicatorKB = KeyCode.Q;

	public KeyCode hazardIndicatorKB = KeyCode.Z;

	public KeyCode shiftGearUp = KeyCode.LeftShift;

	public KeyCode shiftGearDown = KeyCode.LeftControl;

	public KeyCode boostKB = KeyCode.F;

	public KeyCode changeCameraKB = KeyCode.C;

	public KeyCode enterExitVehicleKB = KeyCode.E;

	public bool useAutomaticGear = true;

	public bool runEngineAtAwake = true;

	public bool autoReverse = true;

	public GameObject contactParticles;

	public RCC_Settings.Units units;

	public RCC_Settings.UIType uiType;

	public bool useTelemetry;

	public bool useAccelerometerForSteering;

	public bool useSteeringWheelForSteering;

	public float UIButtonSensitivity = 3f;

	public float UIButtonGravity = 5f;

	public float gyroSensitivity = 2f;

	public bool useLightsAsVertexLights = true;

	public bool useLightProjectorForLightingEffect;

	public GameObject exhaustGas;

	public LayerMask vehicleLayer;

	public RCC_Skidmarks skidmarksManager;

	public GameObject projector;

	public LayerMask projectorIgnoreLayer;

	public AudioClip[] gearShiftingClips;

	public AudioClip[] crashClips;

	public AudioClip reversingClip;

	public AudioClip windClip;

	public AudioClip brakeClip;

	public AudioClip indicatorClip;

	public AudioClip NOSClip;

	public AudioClip turboClip;

	public AudioClip[] blowoutClip;

	public AudioClip[] exhaustFlameClips;

	public bool useSharedAudioSources = true;

	[Range(0f, 1f)]
	public float maxGearShiftingSoundVolume = 0.25f;

	[Range(0f, 1f)]
	public float maxCrashSoundVolume = 1f;

	[Range(0f, 1f)]
	public float maxWindSoundVolume = 0.1f;

	[Range(0f, 1f)]
	public float maxBrakeSoundVolume = 0.1f;

	public bool foldGeneralSettings;

	public bool foldControllerSettings;

	public bool foldUISettings;

	public bool foldWheelPhysics;

	public bool foldSFX;

	public bool foldOptimization;

	public enum BehaviorType
	{
		Simulator,
		Racing,
		SemiArcade,
		Drift,
		Fun,
		Custom
	}

	public enum ControllerType
	{
		Keyboard,
		Mobile,
		Custom
	}

	public enum Units
	{
		KMH,
		MPH
	}

	public enum UIType
	{
		UI,
		NGUI,
		None
	}
}
