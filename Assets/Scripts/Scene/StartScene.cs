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


    void Awake()
    {
        btnBG.interactable = false;
        Time.timeScale = 1.0f;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
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

        yield return null;
        StartCoroutine("EnableTap");
    }


    /// <summary>
    /// �^�b�v�{�^���̗L��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
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
