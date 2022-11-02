using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
/// <summary>
/// スタート画面に関する対応
/// </summary>
public class StartScene : MonoBehaviour
{
    /// <summary>
    /// タップオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Button btnBG;


    void Awake()
    {
        btnBG.interactable = false;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnableTap");
    }

    /// <summary>
    /// タップボタンの有効
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        yield return new WaitForSeconds(1.0f);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        yield return new WaitForSeconds(2.0f);
        menu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        btnBG.interactable = true;
    }

    /// <summary>
    /// GameSceneに遷移する
    /// </summary>
    public void OnClickTapBG()
    {
        Debug.Log("開始");
    }

    
}
