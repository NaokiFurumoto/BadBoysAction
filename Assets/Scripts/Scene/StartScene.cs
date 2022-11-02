using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
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


    void Awake()
    {
        btnBG.interactable = false;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnableTap");
    }

    /// <summary>
    /// �^�b�v�{�^���̗L��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        yield return new WaitForSeconds(1.0f);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        yield return new WaitForSeconds(2.0f);
        menu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        btnBG.interactable = true;
    }

    /// <summary>
    /// GameScene�ɑJ�ڂ���
    /// </summary>
    public void OnClickTapBG()
    {
        Debug.Log("�J�n");
    }

    
}
