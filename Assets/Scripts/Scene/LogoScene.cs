using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class LogoScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LogoWork().Forget();
    }

   private async UniTask LogoWork()
    {
        await UniTask.Delay(1000);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        await UniTask.Delay(2000);
        FadeFilter.Instance.FadeOut(Color.black, 2.0f);
        await UniTask.Delay(2000);
        LoadScene.Load("StartScene");
    }
}
