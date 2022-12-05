using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
using System.Linq;

/// <summary>
/// �̗͂̊Ǘ�
/// </summary>
public class LifesManager : MonoBehaviour
{
    /// <summary>
    /// �̗̓X�e�[�^�X
    /// </summary>
    [SerializeField]
    private List<LifeStatus> lifeStatuses = new List<LifeStatus>();

    /// <summary>
    /// �̗͏�����
    /// </summary>
    [SerializeField]
    private int haveLifeNumber;

    private PlayerStatusController playerStatusController;

    void Start()
    {
       Initialize();
    }

    /// <summary>
    /// ������
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
    /// �̗͕\���ݒ�
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
        //�v���C���[�̃��C�t�X�V
        playerStatusController.PlayerSetLife(_lifePoint);
    }

    /// <summary>
    /// ���C�t���̎擾
    /// </summary>
    /// <returns></returns>
    public int GetLifeNum()
    {
        return haveLifeNumber;
    }
}
