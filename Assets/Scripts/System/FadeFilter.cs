using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fade���
/// </summary>
public enum FADE_STATE
{
    NON,
    IN,//���邭�Ȃ�
    OUT//�Â��Ȃ�
}
public class FadeFilter : MonoBehaviour
{
    public static FadeFilter Instance = null;

    [SerializeField]
    private GameObject fadeFilterObject = null;

    [SerializeField]
    private FADE_STATE fadeState;

    // === �����p�����[�^�[ ================================
    private float startTime;
    private float fadeTime;
    private Color fadeColor;
    private SpriteRenderer sprite;
    private UnityAction fadeInCallback;
    private UnityAction fadeOutCallback;

    /// <summary>
    /// ���g������
    /// </summary>
    private void Awake()
    {
        Instance = this;
        fadeState = FADE_STATE.NON;
        sprite = fadeFilterObject.GetComponent<SpriteRenderer>();
        //sprite.enabled = false;
    }
    
    /// <summary>
    /// �ϐ��Z�b�g
    /// </summary>
    /// <param name="state">���</param>
    /// <param name="color">�F</param>
    /// <param name="time">�J�ڎ���</param>
    private void SetFadeAction(FADE_STATE state, Color color, float time, UnityAction callback = null)
    {
        fadeState = state;
        startTime = Time.unscaledTime;
        fadeTime = time;
        fadeColor = color;
        fadeInCallback = callback;
    }

    /// <summary>
    /// ���邭����
    /// </summary>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public void FadeIn(Color color, float time, UnityAction callback = null)
    {
        //sprite.enabled = true;
        SetFadeAction(FADE_STATE.IN, color, time, callback);
    }

    /// <summary>
    /// �Â�����
    /// </summary>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public void FadeOut(Color color, float time, UnityAction callback = null)
    {
        //sprite.enabled = true;
        SetFadeAction(FADE_STATE.OUT, color, time, callback);
    }

    /// <summary>
    /// Filter�̐ݒ�
    /// </summary>
    /// <param name="eneble"></param>
    /// <param name="color"></param>
    private void SetFadeFilterColor(bool enabled, Color color)
    {
        if (sprite)
        {
            sprite.enabled = enabled;
            sprite.color = color;
            fadeFilterObject.SetActive(enabled);
        }
    }

    private void Update()
    {
        switch (fadeState)
        {
            case FADE_STATE.NON :
                break;

            case FADE_STATE.IN :
                {
                    fadeColor.a = 1.0f - ((Time.unscaledTime - startTime) / fadeTime);
                    if(fadeColor.a > 1.0f || fadeColor.a < 0.0f)
                    {
                        fadeColor.a = 0.0f;
                        fadeState = FADE_STATE.NON;
                        SetFadeFilterColor(false, fadeColor);
                        fadeInCallback?.Invoke();
                        break;
                    }
                    SetFadeFilterColor(true, fadeColor);
                    break;
                }

            case FADE_STATE.OUT:
                fadeColor.a = (Time.unscaledTime - startTime) / fadeTime;
                if (fadeColor.a > 1.0f || fadeColor.a < 0.0f)
                {
                    fadeColor.a = 1.0f;
                    fadeState = FADE_STATE.NON;
                    fadeOutCallback?.Invoke();
                }
                SetFadeFilterColor(true, fadeColor);
                break;
        }
    }


}
