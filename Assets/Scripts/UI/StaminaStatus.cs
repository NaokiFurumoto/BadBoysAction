using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;

public class StaminaStatus : MonoBehaviour
{
    /// <summary>
    /// スタミナ番号
    /// </summary>
    [SerializeField]
    private int number;

    /// <summary>
    /// 回復判定
    /// </summary>
    [SerializeField]
    private bool isRecovery;

    /// <summary>
    /// ONイメージ
    /// </summary>
    [SerializeField]
    private GameObject image_On;

    #region プロパティ
    public int Number => number;
    public bool IsRecovery => isRecovery;
    #endregion


    //■関数
    //表示切替
    //回復時間計測：消費されたタイミングで


    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
   private void Initialize()
    {

    }


    /// <summary>
    /// イメージ切り替え
    /// </summary>
    /// <param name="_have"></param>
    public void ChangeStaminaImage(bool _have)
    {
        isRecovery = _have;
        image_On.SetActive(_have);
    }




}
