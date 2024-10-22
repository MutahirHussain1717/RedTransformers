using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Advertisements.Instance.Initialize();
        Invoke("ShowAdLoop", 40f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ShowAdLoop()
    {
        Advertisements.Instance.ShowInterstitial(InterstitialClosed);
        Invoke("showAdLoop", 40f);
    }
    void showAdLoop()
    {
        Advertisements.Instance.ShowInterstitial(InterstitialClosed);
        Invoke("ShowAdLoop", 40f);
    }
    void InterstitialClosed() { }
}
