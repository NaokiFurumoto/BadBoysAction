using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LogoWork");
    }

   private IEnumerator LogoWork()
    {
        yield return new WaitForSeconds(1.0f);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        yield return new WaitForSeconds(2.0f);
        FadeFilter.Instance.FadeOut(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("StartScene");
    }
}
