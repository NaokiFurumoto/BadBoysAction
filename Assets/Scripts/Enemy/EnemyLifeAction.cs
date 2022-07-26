using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
/// <summary>
/// �G�̗̑͂Ɋւ���N���X
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
  
    /// <summary>
    /// �e�L�X�g�̕ύX
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int life)
    {
        life = Mathf.Clamp(life, 0, ENMAX_LIFEPOINT);
        textMesh.text = life.ToString();
    }

}
