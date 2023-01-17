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

    private NewGenerateManager generatorRoot;

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

            if (generatorRoot == null)
            {
                generatorRoot = GameObject.FindGameObjectWithTag("GeneratorRoot")
                                       .GetComponent<NewGenerateManager>();
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        //広告再生中は中断復帰させない
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || 
            SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            InitializeThis();
            if (gameController.State == INGAME_STATE.RESULT || gameController.State == INGAME_STATE.ADS)
            {
                return;
            }

            if (pause)
            {   
                //バックグラウンドへ
                uiController.SetIsBreak(true);
                Debug.Log("一時停止");
                //停止中ならば何もしない
                if (gameController.State == INGAME_STATE.STOP)
                    return;

                //敵の生成を停止
                generatorRoot?.ChangeGeneratorState(GENERATOR_STATE.STOP);
                gameController?.OnClickOptionButton();
            }
            else
            {
                //復帰
                uiController.SetIsBreak(false);
                //gameController.State = INGAME_STATE.STOP;
                if (gameController.State == INGAME_STATE.PLAYING)
                {
                    gameController.OnClickOptionButton();
                }

                //敵の生成を再開
                if(generatorRoot == null)
                {
                    generatorRoot = generatorRoot = GameObject.FindGameObjectWithTag("GeneratorRoot")
                                       .GetComponent<NewGenerateManager>();
                }

                generatorRoot?.ChangeGeneratorState(GENERATOR_STATE.GENERATE);

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
