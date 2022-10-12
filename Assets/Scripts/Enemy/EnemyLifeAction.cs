using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
using System.Linq;
/// <summary>
/// �G�̗̑͂Ɋւ���N���X
/// </summary>
public class EnemyLifeAction : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private List<int> encountLife = new List<int>();
  
    /// <summary>
    /// �e�L�X�g�̕ύX
    /// </summary>
    /// <param name="damage"></param>
    public void SetLifeText(int _life)
    {
        //�l�̐���
        _life = Mathf.Clamp(_life, 0, ENMAX_LIFEPOINT);
        textMesh.text = _life.ToString();
    }

    /// <summary>
    /// �����̍��v
    /// </summary>
    /// <returns></returns>
    private int TotalRatio()
    {
        return encountLife.Sum();
    }

    /// <summary>
    /// �ݒ肳�ꂽ��������̗͂����߂�
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
        //��O
        return 1;
    }

    /// <summary>
    /// �\���ؑ�
    /// </summary>
    /// <param name="isActive"></param>
    public void ChangeActiveLifeImage(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

}
