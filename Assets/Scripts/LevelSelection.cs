using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject ObjectTer;
    
    private void Awake()
	{
		UnityEngine.Debug.Log("Levlssss" + PlayerPrefs.GetInt("LevelOpen"));
		if (PlayerPrefs.GetInt("LevelOpen") <= 0)
		{
			PlayerPrefs.SetInt("LevelOpen", 1);
		}
		if (PlayerPrefs.GetInt("LevelOpen") >= 2)
		{
			PlayerPrefs.SetInt("LevelOpen", 2);
		}
		this.levelOpen = PlayerPrefs.GetInt("LevelOpen");
		UnityEngine.Debug.Log(this.levelOpen);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
			if (Application.platform == RuntimePlatform.Android)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
			}
		}
		for (int i = 0; i < this.levelOpen; i++)
		{
			this.levelsContent[i].transform.GetComponent<Button>().interactable = true;
			this.levelsContent[i].transform.GetChild(0).gameObject.SetActive(false);
		}
	}

    public void OnBtnBack()
    {
        base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
    }

	public void ChangeScene(string sceneName)
	{
        if (Application.platform == RuntimePlatform.Android)
        {
            Instantiate(ObjectTer);
        }
        else
        {
            AdsManager.Instance.ShowInter();
            base.GetComponent<AudioSource>().PlayOneShot(this.buttonSound);
            this.OnLoading();
            LevelSelection.levelLoaded = sceneName;
            UnityEngine.SceneManagement.SceneManager.LoadScene(LevelSelection.levelLoaded);
        }
	}

	public void OnLoading()
	{
		this.loading.gameObject.SetActive(true);
		this.bg.gameObject.SetActive(false);
		this.scroll.gameObject.SetActive(false);
		this.btns.gameObject.SetActive(false);
	}

	public AudioClip buttonSound;

	public GameObject loading;

	public GameObject bg;

	public GameObject scroll;

	public GameObject btns;

	public GameObject[] levelsContent;

	public static string levelLoaded;

	public static int levelloadedNo;

	public int levelOpen;

	private AndroidJavaClass exitClass;

	private AndroidJavaObject exitClassObject;
}
