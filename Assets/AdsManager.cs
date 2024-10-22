using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    void Awake()
    {    
        DontDestroyOnLoad(this);
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        Advertisements.Instance.Initialize();
    }

    public void ShowInter()
    {
        Advertisements.Instance.ShowInterstitial();
    }


}
