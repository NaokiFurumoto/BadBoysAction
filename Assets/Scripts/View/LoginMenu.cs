using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using NCMB;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginMenu : LoginMenuBase
{
    [SerializeField]
    private TMP_InputField inputFieldID;

    [SerializeField]
    private TMP_InputField inputFieldPASS;

    [SerializeField]
    private Button btn_Login;

    public override void OnEndEdit_ID()
    {
        id = inputFieldID.text;
    }

    public override void OnEndEdit_Pass()
    {
        pass = inputFieldPASS.text;
    }
}
