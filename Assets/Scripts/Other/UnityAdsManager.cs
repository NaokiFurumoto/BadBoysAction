using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour
{
    public static UnityAdsManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Advertisement.Initiallize(gameID, testMode);

        //DontDestroyオブジェクト
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
