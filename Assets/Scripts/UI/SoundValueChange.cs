using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundValueChange : MonoBehaviour
{
    enum SOUNDTYPE { BGM, SE }

    [SerializeField]
    private SOUNDTYPE type;

    [SerializeField]
    private MenuObject_Sliderbar slider;

    /// <summary>
    /// スライダーの更新時に呼ばれる
    /// </summary>
    public void SlidebarDrag()
    {

    }
}
