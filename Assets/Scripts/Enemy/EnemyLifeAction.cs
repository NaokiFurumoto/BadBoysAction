using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// �G�̗̑͂Ɋւ���N���X
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    //�A�j���[�V����
    //�e�L�X�g
    [SerializeField]
    private TextMeshPro textMesh;
  
    /// <summary>
    /// �e�L�X�g�̕ύX
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int damage)
    {
        textMesh.text = damage.ToString();
    }

}
