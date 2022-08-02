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
    public void SetLifeText(int _life)
    {
        _life = Mathf.Clamp(_life, 0, ENMAX_LIFEPOINT);
        textMesh.text = _life.ToString();
    }

}
