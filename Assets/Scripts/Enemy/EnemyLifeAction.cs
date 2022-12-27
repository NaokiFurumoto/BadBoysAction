using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
using System.Linq;
/// <summary>
/// 敵の体力に関するクラス
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private List<int> encountLife = new List<int>();
  
    /// <summary>
    /// テキストの変更
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int _life)
    {
        //値の制限
        _life = Mathf.Clamp(_life, 0, ENMAX_LIFEPOINT);
        textMesh.text = _life.ToString();
    }

    /// <summary>
    /// 割合の合計
    /// </summary>
    /// <returns></returns>
    private int TotalRatio()
    {
        return encountLife.Sum();
    }

    /// <summary>
    /// 設定された割合から体力を求める
    /// </summary>
    /// <returns></returns>
    public int SetCreateLife()
    {
        var totalRatio = TotalRatio();
        var createNum = Random.Range(1, totalRatio);

        var total = 0;
        for(int i = 0; i < encountLife.Count; i++)
        {
            total += encountLife[i];
            if(createNum <= total)
            {
                return i + 1;
            }
        }
        //例外
        return 1;
    }

    /// <summary>
    /// 表示切替
    /// </summary>
    /// <param name="isActive"></param>
    public void ChangeActiveLifeImage(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

}
