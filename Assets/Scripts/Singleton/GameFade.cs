using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFade : MonoBehaviour
{
    /// <summary>
    /// ���[�h
    /// </summary>
    public enum Mode
    {
        IDLE,
        IN,
        OUT,
    }

    /// <summary>
    /// Fade�摜
    /// </summary>
    private Image fadeImage;

    /// <summary>
    /// ����
    /// </summary>
    private float time;

    /// <summary>
    /// 
    /// </summary>
    private float elaps;

    /// <summary>
    /// �ω��O��Color
    /// </summary>
    private Color toColor;

    /// <summary>
    /// �ω����Color
    /// </summary>
    private Color fromColor;

    /// <summary>
    /// �R�[���o�b�N
    /// </summary>
    private System.Action action;

    /// <summary>
    /// �\�����[�h
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
    /// ���ݐF����ToColor�Ƀt�F�[�h
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
    /// fromColor����toColor�փt�F�[�h
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="toColor"></param>
    /// <param name="fromColor"></param>
    /// <param name="time">�b�w��</param>
    /// <param name="action">�t�F�[�h�I�����̃R�[���o�b�N</param>
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
    /// �t�F�[�h����
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
