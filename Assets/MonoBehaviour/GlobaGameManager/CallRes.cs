using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallRes : MonoBehaviour
{
    public GameObject GameManager;
    public static CallRes Instance;
    
    public void Call()
    {
        Instantiate(GameManager);
    }
}
