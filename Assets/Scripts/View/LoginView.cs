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
        NONE,//未設定
        NEW,//新規会員登録
        LOGIN,//ログイン
        LOGOUT//ログアウト
    }

    [SerializeField]
    private OptionView optionView;

    //ユーザー情報
    [SerializeField]
    private string id;
    [SerializeField]
    private string pw;
    [SerializeField]
    private string mail;

    //切替
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
    /// メニュー切り替え
    /// </summary>
    /// <param name="type"></param>
    public void SwitchMenu(LOGIN_TYPE type)
    {
        if (menuList == null)
            return;

        menuList.ForEach(menu => menu.gameObject.SetActive(menu.MenuType == type ? true : false));
    }

     /// <summary>
    /// 閉じるボタン
    /// </summary>
    public void OnClickClose()
    {
        SoundManager.Instance.PlayOneShot(AppSound.Instance.SE_MENU_CANCEL);
        this.gameObject.SetActive(false);
        optionView.ChangeLayout();
    }
}

/// <summary>
/// ログインメニュー
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
        //親のGO取得？？
    }

    protected virtual void OnDisable()
    {
    }

   
}

