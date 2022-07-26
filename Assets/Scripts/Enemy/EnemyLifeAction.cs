using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
/// <summary>
/// 敵の体力に関するクラス
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
  
    /// <summary>
    /// テキストの変更
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int life)
    {
        life = Mathf.Clamp(life, 0, ENMAX_LIFEPOINT);
        textMesh.text = life.ToString();
    }

}
