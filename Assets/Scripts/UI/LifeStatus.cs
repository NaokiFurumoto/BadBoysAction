using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;

/// <summary>
/// ライフのステータスに関するクラス
/// </summary>
public class LifeStatus : MonoBehaviour
{
    /// <summary>
    /// ライフ番号：1-10
    /// </summary>
    [SerializeField]
    private int number;

    /// <summary>
    /// ONイメージ
    /// </summary>
    [SerializeField]
    private GameObject image_On;

    /// <summary>
    /// 取得されてる判定
    /// </summary>
    [SerializeField]
    private bool isHave;

    #region プロパティ
    public int Number => number;
    public bool IsHave => isHave;   
    #endregion

    /// <summary>
    /// ONイメージ切り替え
    /// </summary>
    /// <param name="_have"></param>
    public void ChangeLifeImage(bool _have)
    {
        isHave = _have;
        image_On.SetActive(_have);
    }

    private void Start()
    {
        Initialize();     
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        if(number == 1)
        {
            image_On.SetActive(true);
        }
        else
        {
            image_On.SetActive(false);
        }
    }
}
