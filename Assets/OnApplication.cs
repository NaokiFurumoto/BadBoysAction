using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalValue;
using UnityEngine.SceneManagement;

public class OnApplication : MonoBehaviour
{
    [SerializeField]
    private UiController uiController;

    [SerializeField]
    private GameController gameController;

    private void Start() { InitializeThis(); }

    private void InitializeThis()
    {
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            if (gameController == null)
            {
                gameController = GameObject.FindGameObjectWithTag("GameController")
                                       .GetComponent<GameController>();
            }

            if (uiController == null)
            {
                uiController = GameObject.FindGameObjectWithTag("UI")
                                       .GetComponent<UiController>();
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            InitializeThis();

            if (pause)
            {   //バックグラウンドへ
                //今の状態をセーブする
                uiController.SetIsBreak(true);
                SaveManager.Instance.GamePlaingSave();
                gameController.OnClickOptionButton();
                Debug.Log("一時停止");
            }
            else
            {
                //復帰
                SaveManager.Instance.Load();
                uiController.SetIsBreak(false);
                Debug.Log("バックグラウンドからの復帰");
            }
        }
    }

    /// <summary>
    /// アプリの中断
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            uiController.SetIsBreak(true);
            SaveManager.Instance.GamePlaingSave();
            Debug.Log("中断");
        }
    }
}
