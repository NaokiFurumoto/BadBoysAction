using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
/// <summary>
/// �C���Q�[��UI�Ɋւ��鑀��/�X�V�Ȃ�
/// </summary>
public class UiController : MonoBehaviour
{
    /// <summary>
    /// ���C�t�Ǘ�
    /// </summary>
    [SerializeField]
    private LifesManager lifesManager;

    /// <summary>
    /// �X�^�~�i�Ǘ�
    /// </summary>
    [SerializeField]
    private StaminasManager staminasManager;

    /// <summary>
    /// ���j���e�L�X�g�\��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_Kills;

    /// <summary>
    /// ���j��
    /// </summary>
    [SerializeField]
    private int killsNum;

    /// <summary>
    /// ������e�Ǘ��N���X
    /// </summary>
    private GeneratorManager generatorManager;

    /// <summary>
    /// ���g�̏�����
    /// </summary>
    private void Awake() { }

    #region
    public LifesManager LifesManager => lifesManager;
    public StaminasManager StaminasManager => staminasManager;
    #endregion

    /// <summary>
    /// �O��������
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //�J�n����0�ɂ���
        killsNum = 0;
        text_Kills.text = killsNum.ToString();
        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                      GetComponent<GeneratorManager>();
    }

    /// <summary>
    /// ���j���̐ݒ�
    /// </summary>
    /// <param name="_number"></param>
    public void SetTextKillsNumber()
    {
        if (text_Kills == null)
            return;
        killsNum++;
        text_Kills.text = killsNum.ToString();
        generatorManager?.ChangeGameLevel(killsNum);
        generatorManager?.ChangeUpdateGenerator();
    }

    /// <summary>
    /// ���j���̎擾
    /// </summary>
    public int GetTextKillsNumber()
    {
        int num;
        if (int.TryParse(text_Kills.text,out num))
            return num;

        return 0;
    }

    /// <summary>
    /// ���g���C�ݒ�
    /// </summary>
    public void RetryUI()
    {
        text_Kills.text = "0";
    }
}
