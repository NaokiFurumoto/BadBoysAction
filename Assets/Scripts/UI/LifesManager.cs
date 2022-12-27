using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
using System.Linq;

/// <summary>
/// 体力の管理
/// </summary>
public class LifesManager : MonoBehaviour
{
    /// <summary>
    /// 体力ステータス
    /// </summary>
    [SerializeField]
    private List<LifeStatus> lifeStatuses = new List<LifeStatus>();

    /// <summary>
    /// 体力所持数
    /// </summary>
    [SerializeField]
    private int haveLifeNumber;

    private PlayerStatusController playerStatusController;

    void Start()
    {
       Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        if (lifeStatuses == null)
        {
            var array = this.gameObject.GetComponentsInChildren<LifeStatus>();
            lifeStatuses.AddRange(array);
        }

        playerStatusController = GameObject.FindGameObjectWithTag("Player").
                                           GetComponent<PlayerStatusController>();
    }
   
    /// <summary>
    /// 体力表示設定
    /// </summary>
    /// <param name="_lifePoint"></param>
    public void SetLife(int _lifePoint)
    {
        haveLifeNumber = _lifePoint;

        foreach (var life in lifeStatuses)
        {
            if(life.Number <= _lifePoint)
            {
                life.ChangeLifeImage(true);
            }
            else
            {
                life.ChangeLifeImage(false);
            }
        }
        //プレイヤーのライフ更新
        playerStatusController.PlayerSetLife(_lifePoint);
    }

    /// <summary>
    /// ライフ数の取得
    /// </summary>
    /// <returns></returns>
    public int GetLifeNum()
    {
        return haveLifeNumber;
    }
}
