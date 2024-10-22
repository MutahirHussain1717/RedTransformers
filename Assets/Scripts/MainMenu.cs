using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ObjectTer;
    public GameObject ObjectPP;
	private void Awake()
	{
	}
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Instantiate(ObjectPP);
        }
    }
	public void Play()
	{
        if (Application.platform == RuntimePlatform.Android) {
            
            Instantiate(ObjectTer);
        }
        else
        {
            AdsManager.Instance.ShowInter();
            base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
		
	}

	public void MoreApps()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		AGameUtils.moreAppsLink();
	}

	public void FeedBack()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		AGameUtils.rateUsLink();
	}

	public void Exit()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		MoPubAds.showAd(MoPubAds._interstitialOnExit);
		this.dialogexit.SetActive(true);
		this.play.SetActive(false);
		this.feedback.SetActive(false);
		this.exitbt.SetActive(false);
		this.playerobj2.SetActive(false);
		this.playerobj1.SetActive(false);
	}

	public void ButtonYes()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		Application.Quit();
	}

	public void ButtonNo()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
		this.dialogexit.SetActive(false);
		this.play.SetActive(true);
		this.feedback.SetActive(true);
		this.exitbt.SetActive(true);
		this.miniadicon.SetActive(true);
		this.playerobj2.SetActive(true);
		this.playerobj1.SetActive(true);
		MoPubAds.showBanner(MoPubAds._bannerAdUnitId);
	}


	private void Update()
	{
	}

	private AndroidJavaClass exitClass;

	private AndroidJavaObject exitClassObject;

	public GameObject dialogexit;

	public GameObject play;

	public GameObject moreapps;

	public GameObject feedback;

	public GameObject exitbt;

	public GameObject miniadicon;

	public GameObject playerobj1;

	public GameObject playerobj2;

	public static bool ads;

	public AudioClip buttonSound;

	public static bool mainmenufirst = true;

	public static bool adsalternative = true;

	private bool Exitadd;

	public Text gamename;
}
