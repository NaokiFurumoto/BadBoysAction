using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFade : MonoBehaviour
{
    /// <summary>
    /// モード
    /// </summary>
    public enum Mode
    {
        IDLE,
        IN,
        OUT,
    }

    /// <summary>
    /// Fade画像
    /// </summary>
    private Image fadeImage;

    /// <summary>
    /// 時間
    /// </summary>
    private float time;

    /// <summary>
    /// 
    /// </summary>
    private float elaps;

    /// <summary>
    /// 変化前のColor
    /// </summary>
    private Color toColor;

    /// <summary>
    /// 変化後のColor
    /// </summary>
    private Color fromColor;

    /// <summary>
    /// コールバック
    /// </summary>
    private System.Action action;

    /// <summary>
    /// 表示モード
    /// </summary>
    private Mode mode = Mode.IDLE;

    private void Awake()
    {
        CreateImage();
        this.gameObject.SetActive(false);
    }


    void CreateImage()
    {
        fadeImage = this.GetComponent<Image>();
        fadeImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    /// <summary>
    /// 現在色からToColorにフェード
    /// </summary>
    /// <param name="_mode"></param>
    /// <param name="_toColor"></param>
    /// <param name="_fromColor"></param>
    /// <param name="_time"></param>
    /// <param name="_action"></param>
    public void Play(Mode _mode, Color _toColor, float _time = 1.0f, System.Action _action = null)
    {
        mode = _mode;
        this.gameObject.SetActive(true);
        time = _time;
        elaps = _time;
        toColor = _toColor;
        fromColor = fadeImage.color;
        action = _action;
    }

    /// <summary>
    /// fromColorからtoColorへフェード
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="toColor"></param>
    /// <param name="fromColor"></param>
    /// <param name="time">秒指定</param>
    /// <param name="action">フェード終了時のコールバック</param>
    public void Play(Mode _mode, Color _toColor, Color _fromColor, float _time = 1.0f, System.Action _action = null)
    {
        mode = _mode;
        this.gameObject.SetActive(true);
        time = _time;
        elaps = _time;
        toColor = _toColor;
        fadeImage.color = _fromColor;
        fromColor = _fromColor;
        action = _action;
    }


    /// <summary>
    /// フェード判定
    /// </summary>
    /// <returns></returns>
    public bool IsFading()
    {
        return mode == Mode.IN || mode == Mode.OUT;
    }

    private void Update()
    {
        switch (mode)
        {
            case Mode.IDLE:
                break;
            case Mode.IN:
                fadeImage.color = Color.Lerp(toColor, fromColor, elaps / time);
                elaps -= Time.unscaledDeltaTime;
                if(elaps < 0)
                {
                    fadeImage.color = toColor;
                    mode = Mode.IDLE;
                    action?.Invoke();
                }
                break;
            case Mode.OUT:
                fadeImage.color = Color.Lerp(toColor, fromColor, elaps / time);
                elaps -= Time.unscaledDeltaTime;
                if (elaps < 0.0f)
                {
                    fadeImage.color = toColor;
                    mode = Mode.IDLE;
                    action?.Invoke();
                }
                break;
            default:
                break;
        }
    }




}
