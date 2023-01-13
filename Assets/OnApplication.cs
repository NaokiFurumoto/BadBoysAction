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
        //広告再生中は中断復帰させない
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            InitializeThis();

            if (gameController.State == INGAME_STATE.RESULT || gameController.State == INGAME_STATE.ADS)
            {
                return;
            }

            if (pause)
            {   //バックグラウンドへ
                uiController.SetIsBreak(true);
                //gameController.State = INGAME_STATE.PLAYING;
                gameController.OnClickOptionButton();
                Debug.Log("一時停止");
            }
            else
            {
                //復帰
                uiController.SetIsBreak(false);
                //gameController.State = INGAME_STATE.STOP;
                //gameController.OnClickOptionButton();
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
        if (gameController.State == INGAME_STATE.RESULT )
        {
            return;
        }

        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            uiController.SetIsBreak(true);
            SaveManager.Instance.GamePlaingSave();
            Debug.Log("中断");
        }
    }
}
