using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NCMB;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginView : ViewBase
{
    public enum LOGIN_TYPE
    {
        NONE,//���ݒ�
        NEW,//�V�K����o�^
        LOGIN,//���O�C��
        LOGOUT//���O�A�E�g
    }

    [SerializeField]
    private OptionView optionView;

    //���[�U�[���
    [SerializeField]
    private string id;
    [SerializeField]
    private string pw;
    [SerializeField]
    private string mail;

    //���O�C����
    [SerializeField]
    private LOGIN_TYPE loginType = LOGIN_TYPE.NONE;

    //�ؑ�
    [SerializeField]
    private List<LoginMenuBase> menuList = new List<LoginMenuBase>();


    [SerializeField]
    private UserAuth userAuth;

    private void Awake()
    {
        if (userAuth == null)
        {
            userAuth = FindObjectOfType<UserAuth>();
        }
    }

    protected override void OnEnable()
    {
        if(optionView == null)
        {
            optionView = GameObject.FindWithTag("OptionView")
                        .GetComponent<OptionView>();
        }
    }

    protected override void OnDisable()
    {
        //animator?.SetTrigger("close");
    }

    /// <summary>
    /// ���j���[�؂�ւ�
    /// </summary>
    /// <param name="type"></param>
    public void SwitchMenu(LOGIN_TYPE type)
    {
        if (menuList == null)
            return;

        menuList.ForEach(menu => menu.gameObject.SetActive(menu.MenuType == type ? true : false));
    }

     /// <summary>
    /// ����{�^��
    /// </summary>
    public void OnClickClose()
    {
        this.gameObject.SetActive(false);
        optionView.ChangeLayout();
    }
}

/// <summary>
/// ���O�C�����j���[
/// </summary>
public abstract class LoginMenuBase : MonoBehaviour
{
    [SerializeField]
    private LoginView.LOGIN_TYPE menuType;

    [SerializeField]
    protected UserAuth user;

    public LoginView.LOGIN_TYPE MenuType => menuType;

    [SerializeField]
    private Button btn_Close;

    protected string  id;
    protected string pass;

    public string Id => id;
    public string Pass => pass;

    public virtual void OnEndEdit_ID() { }
    public virtual void OnEndEdit_Pass() { }

    protected virtual void OnEnable()
    {
        //�e��GO�擾�H�H
    }

    protected virtual void OnDisable()
    {
    }

   
}

