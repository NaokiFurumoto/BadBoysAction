using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
/// <summary>
/// �X�^�[�g��ʂɊւ���Ή�
/// </summary>
public class StartScene : MonoBehaviour
{
    /// <summary>
    /// �^�b�v�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Button btnBG;

    [SerializeField]
    private GameObject startMenuView;


    void Awake()
    {
        btnBG.interactable = false;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        EnableTap().Forget();
    }

    /// <summary>
    /// �^�b�v�{�^���̗L��
    /// </summary>
    /// <returns></returns>
    private async UniTask EnableTap()
    {
        await UniTask.Delay(1000);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        await UniTask.Delay(2000);
        menu.SetActive(true);
        await UniTask.Delay(500);
        btnBG.interactable = true;
    }

    /// <summary>
    /// GameScene�ɑJ�ڂ���
    /// </summary>
    public void OnClickTapBG()
    {

        GoGameScene().Forget();
        
    }

    private async UniTask GoGameScene()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        await UniTask.Delay(1000);
        LoadScene.Load("GameScene");
    }



    /// <summary>
    /// �����L���O�\��
    /// </summary>
    public void OnClickRank()
    {
        Debug.Log("�J�n");
    }

    /// <summary>
    /// ���j���|�\��
    /// </summary>
    public void OnClickMenu()
    {
        menu.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���j���[��ʂ̊J��
    /// </summary>
    /// <param name="isActive"></param>
    public void StartMenuViewActivate(bool isActive)
    {
        startMenuView?.SetActive(isActive);
    }
}
