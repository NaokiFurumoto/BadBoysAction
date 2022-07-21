using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// 敵の体力に関するクラス
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    //アニメーション
    //テキスト
    [SerializeField]
    private TextMeshPro textMesh;
  
    /// <summary>
    /// テキストの変更
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int damage)
    {
        textMesh.text = damage.ToString();
    }

}
