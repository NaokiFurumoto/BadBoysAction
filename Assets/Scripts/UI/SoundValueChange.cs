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
    /// �X���C�_�[�̍X�V���ɌĂ΂��
    /// </summary>
    public void SlidebarDrag()
    {

    }
}
