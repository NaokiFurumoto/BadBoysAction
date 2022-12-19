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
            {   //�o�b�N�O���E���h��
                //���̏�Ԃ��Z�[�u����
                uiController.SetIsBreak(true);
                SaveManager.Instance.GamePlaingSave();
                gameController.OnClickOptionButton();
                Debug.Log("�ꎞ��~");
            }
            else
            {
                //���A
                SaveManager.Instance.Load();
                uiController.SetIsBreak(false);
                Debug.Log("�o�b�N�O���E���h����̕��A");
            }
        }
    }

    /// <summary>
    /// �A�v���̒��f
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == GAMESCENENAME || SceneManager.GetActiveScene().name == RANKSCENENAME)
        {
            uiController.SetIsBreak(true);
            SaveManager.Instance.GamePlaingSave();
            Debug.Log("���f");
        }
    }
}
