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

    [SerializeField]
    private UserAuth user;

    [SerializeField]
    private GameObject txt_Login;

    [SerializeField]
    private GameObject txt_NotLogin;

    [SerializeField]
    private Button rankBtn;

    void Awake()
    {
        btnBG.interactable = false;
        Time.timeScale = 1.0f;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        FadeFilter.Instance.FadeIn(Color.black, 0.5f);
        //�����Ńf�[�^�̃��[�h
        var data = SaveManager.Instance.Load();
        if (data.IsBreak && data.StaminaNumber != 0)
        {
            //�퓬���I�I
            //���f���ꂽ�퓬�f�[�^������܂��B�ĊJ���܂����H
            //�ĊJ����A�ŏ�����
            //�_�C�A���O��\��
            var dialog = CommonDialog.ShowDialog
                (
                    LOADBATTLE_TITLE,
                    LOADBATTLE_DESC,
                    LOADBATTLE_YES,
                    LOADBATTLE_NO,
                    () => OnClickTapBG(),
                    () => StartCoroutine("InitDataStart")
                );
            yield break;
        }
        
        //���O�C�����Ă�����A���O�C�����̕\�����ă��O�C��
        var isLogin = data.IsLogin;
        if (isLogin)
        {
            var name = data.UserName;
            var pass = data.Passward;
            txt_Login.SetActive(true);
            txt_NotLogin.SetActive(false);

            if(!(String.IsNullOrEmpty(name)) && !(String.IsNullOrEmpty(pass)))
            {
                user.logIn(name, pass);
                user.IsLogin = true;
                user.IsSignUp = true;
            }

            rankBtn.interactable = true;
        }
        else
        {
            txt_Login.SetActive(false);
            txt_NotLogin.SetActive(true);
            rankBtn.interactable = false;
        }

        StartCoroutine("EnableTap");
    }


    /// <summary>
    /// �^�b�v�{�^���̗L��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        //yield return new WaitForSecondsRealtime(1.0f);
        //FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(2.0f);
        menu.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        btnBG.interactable = true;
    }

   
    /// <summary>
    /// GameScene�ɑJ�ڂ���
    /// </summary>
    public void OnClickTapBG()
    {
        StartCoroutine("GoGameScene");
    }


    private IEnumerator GoGameScene()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
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
        StartMenuViewActivate(true);
    }

    /// <summary>
    /// ���j���[��ʂ̊J��
    /// </summary>
    /// <param name="isActive"></param>
    public void StartMenuViewActivate(bool isActive)
    {
        startMenuView?.SetActive(isActive);
    }

    //�����f�[�^�ōX�V
    private IEnumerator InitDataStart()
    {
        var loadData = SaveManager.Instance.Load();
        loadData.IsBreak = false;

        SaveManager.Instance.Save(loadData);

        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        var dloadData = SaveManager.Instance.Load();
        LoadScene.Load("GameScene");
    }
}
