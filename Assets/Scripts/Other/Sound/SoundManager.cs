using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// サウンドマネージャー
/// AudioSource : ラジカセ
/// AudioClip：カセット
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private bool debugLog = false;

    [SerializeField]
    private bool dontDestroy = true;

    [SerializeField]
    private string soundFolder = "";

    public string SoundFolder { get { return soundFolder; } set { soundFolder = value; } }

    private void Awake()
    {
        if (dontDestroy)
        {
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// �O���[�v�쐬
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public bool CreateGroup(string _name)
    {
        GameObject go = new GameObject();
        go.name = SOUNDGROUP_NID + _name;
        go.transform.parent = transform;

        return false;
    }

    /// <summary>
    /// �O���[�v�I�u�W�F�N�g�̎擾
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public GameObject GetGroup(string _name)
    {
        return GameObject.Find(SOUNDGROUP_NID + _name);
    }

    /// <summary>
    /// AudioSouce��Load
    /// </summary>
    /// <param name="_groupName"></param>
    /// <param name="_fileName"></param>
    /// <returns></returns>
    [System.Obsolete]
    public AudioSource LoadResourcesSound(string _groupName, string _fileName)
    {
        GameObject goSound = transform.Find(SOUNDGROUP_NID + _groupName).gameObject;
        AudioSource audioSource = goSound.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        AudioClip audioClip = Resources.Load(soundFolder + _fileName, typeof(AudioClip)) as AudioClip;
        audioSource.clip = audioClip;

        return audioSource;
    }

    /// <summary>
    /// �w���AudioSouce�̎擾
    /// </summary>
    /// <param name="_groupName"></param>
    /// <param name="_soundName"></param>
    /// <returns></returns>
    [System.Obsolete]
    public AudioSource FindAudioSource(string _groupName, string _soundName)
    {
        GameObject goSound = transform.Find(SOUNDGROUP_NID + _groupName).gameObject;
        AudioSource[] audioSourceList = goSound.GetComponents<AudioSource>();

        foreach (AudioSource audioSouce in audioSourceList)
        {
            if (audioSouce.clip.name == _soundName)
            {
                return audioSouce;
            }
        }

        return null;
    }

    /// <summary>
    /// �O���[�v�擾
    /// </summary>
    /// <param name="_groupName"></param>
    /// <returns></returns>
    [System.Obsolete]

    public AudioSource[] FindAudioSource(string _groupName)
    {
        GameObject goSound = transform.Find(SOUNDGROUP_NID + _groupName).gameObject;
        return goSound.GetComponents<AudioSource>();
    }

    /// <summary>
    /// �Đ�
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <param name="_loop"></param>
    public void Play(AudioSource _audioSource, bool _loop)
    {
        _audioSource.loop = _loop;
        _audioSource.Play();
    }

    /// <summary>
    /// �w�肵�čĐ�
    /// ���̏d���͂ł��Ȃ�
    /// </summary>
    /// <param name="_groupName"></param>
    /// <param name="_soundName"></param>
    /// <param name="_loop"></param>
    [System.Obsolete]

    public void Play(string _groupName, string _soundName, bool _loop)
    {
        AudioSource audioSource = FindAudioSource(_groupName, _soundName);
        if (audioSource)
        {
            Play(audioSource, _loop);
        }
    }

    /// <summary>
    /// ���������čĐ�
    /// ���̏d���͂ł��Ȃ�
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <param name="_loop"></param>
    public void PlayDontOverride(AudioSource _audioSource, bool _loop)
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.loop = _loop;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// ���������čĐ�
    /// ���̏d���͂ł��Ȃ�
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <param name="_loop"></param>
    [System.Obsolete]
    public void PlayDontOverride(string groupName, string soundName, bool loop)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            PlayDontOverride(audioSource, loop);
        }
    }

    /// <summary>
    /// �Đ�
    /// ���̏d���\�FSE
    /// </summary>
    /// <param name="_audioSource"></param>
    public void PlayOneShot(AudioSource _audioSource)
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    /// <summary>
    /// �w��Đ��FSE
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="soundName"></param>
    [System.Obsolete]
    public void PlayOneShot(string groupName, string soundName)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            PlayOneShot(audioSource);
        }
    }

    /// <summary>
    /// ��~
    /// </summary>
    /// <param name="audioSource"></param>
    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    /// <summary>
    /// �w���~
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="soundName"></param>
    [System.Obsolete]

    public void Stop(string groupName, string soundName)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            Stop(audioSource);
        }
    }

    /// <summary>
    /// �w���~
    /// </summary>
    /// <param name="groupName"></param>
    [System.Obsolete]
    public void Stop(string groupName)
    {
        AudioSource[] audioSourceList = FindAudioSource(groupName);
        foreach (AudioSource audioSource in audioSourceList)
        {
            Stop(audioSource);
        }
    }

    /// <summary>
    /// �S�ẴT�E���h�X�g�b�v
    /// </summary>
    public void StopAllSound()
    {
        AudioSource[] audios = transform.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    /// <summary>
    /// �{�����[���擾
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <returns></returns>
    public float GetVolume(AudioSource _audioSource)
    {
        return _audioSource.volume;
    }

    /// <summary>
    /// �w��{�����[���擾
    /// </summary>
    /// <param name="_groupName"></param>
    /// <param name="_soundName"></param>
    /// <returns></returns>
    [System.Obsolete]
    public float GetVolume(string _groupName, string _soundName)
    {
        AudioSource audioSource = FindAudioSource(_groupName, _soundName);
        if (audioSource)
        {
            return GetVolume(audioSource);
        }

        return 0.0f;
    }

    /// <summary>
    /// �{�����[���ݒ�
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <param name="_vol"></param>
    public void SetVolume(AudioSource _audioSource, float _vol)
    {
        _audioSource.volume = _vol;
    }

    /// <summary>
    /// �w��{�����[���ݒ�
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="soundName"></param>
    /// <param name="vol"></param>
    [System.Obsolete]
    public void SetVolume(string groupName, string soundName, float vol)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            SetVolume(audioSource, vol);
        }
    }

    /// <summary>
    /// �w��O���[�v�ȉ��̃{�����[���ݒ�
    /// </summary>
    /// <param name="_groupName"></param>
    /// <param name="_vol"></param>
    public void SetVolume(string _groupName, float _vol)
    {
        GameObject go = GetGroup(_groupName);
        AudioSource[] audioSourceList = go.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSourceList)
        {
            SetVolume(audioSource, _vol);
        }
    }

    #region �t�F�[�h�N���X
    /// <summary>
    /// ���̃N���X���炾���A�N�Z�X�\
    /// </summary>
    class Fade
    {
        //�e�N���X��������̃A�N�Z�X�\
        public AudioSource fadeAudio;
        public float targetV;
        public float dir;
        public float time;
        public float vmin, vmax;

        public Fade(AudioSource _a, float _v, float _d, float _t)
        {
            fadeAudio = _a;
            targetV = _v;
            dir = _d;
            time = _t;
            if (dir < 0.0f)
            {
                vmin = _v;
                vmax = 1.0f;
            }
            else
            {
                vmin = 0.0f;
                vmax = _v;
            }
        }
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private List<Fade> fadeStackList = new List<Fade>();

    public void FadeInVolume(AudioSource _audioSource, float _v, float _t, bool _init)
    {
        if (_audioSource.volume < 1.0f && _audioSource.isPlaying)
        {
            //if (fadeStackList.Count > 0)����Ȃ��H�H
            if (fadeStackList.Count <= 0)
            {
                //�J��Ԃ����s
                InvokeRepeating("SoundFade", 0.0f, 0.02f);
            }

            if (_init)
            {
                _audioSource.volume = 0.0f;
            }

            fadeStackList.Add(new Fade(_audioSource, _v, +1.0f, _t));
        }
    }

    public void FadeOutVolume(AudioSource _audioSource, float _v, float _t, bool _init)
    {
        if (_audioSource.volume > 0.0f && _audioSource.isPlaying)
        {
            if (fadeStackList.Count <= 0)
            {
                InvokeRepeating("SoundFade", 0.0f, 0.02f);
            }
            if (_init)
            {
                _audioSource.volume = 1.0f;
            }

            fadeStackList.Add(new Fade(_audioSource, _v, -1.0f, _t));
        }
    }

    public void FadeOutVolumeGroup(string groupName, AudioSource playAudioSource, float v, float t, bool init)
    {
        GameObject go = GetGroup(groupName);
        AudioSource[] audioSourceList = go.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSourceList)
        {
            if (playAudioSource != audioSource)
            {
                FadeOutVolume(audioSource, v, t, init);
            }
        }
    }

    public void FadeOutVolumeGroup(string groupName, string soundName, float v, float t, bool init)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            FadeOutVolumeGroup(groupName, audioSource, v, t, init);
        }
    }

    public void FadeOutVolumeGroup(string groupName, float v, float t, bool init)
    {
        FadeOutVolumeGroup(groupName, (AudioSource)null, v, t, init);
    }


    private void SoundFade()
    {
        foreach (Fade fade in fadeStackList)
        {
            float v = fade.fadeAudio.volume + (1.0f * (0.02f / fade.time)) * fade.dir;
            SetVolume(fade.fadeAudio, v);
        }

        for (int i = 0; i < fadeStackList.Count; i++)
        {
            if (fadeStackList[i].fadeAudio.volume <= fadeStackList[i].vmin ||
                fadeStackList[i].fadeAudio.volume >= fadeStackList[i].vmax)
            {
                if (fadeStackList[i].fadeAudio.volume <= 0.0f)
                {
                    fadeStackList[i].fadeAudio.Stop();
                }
                fadeStackList.Remove(fadeStackList[i]);
            }
        }

        if (fadeStackList.Count <= 0)
        {
            CancelInvoke("SoundFade");
        }
    }

    /// <summary>
    /// �T�|�[�g�֐��F�C���X�^���X�擾
    /// </summary>
    /// <param name="_gameObjectName"></param>
    /// <returns></returns>
    public static SoundManager GetInstance(string _gameObjectName = "SoundManager")
    {
        GameObject go = GameObject.Find(_gameObjectName);
        if (go)
        {
            return go.GetComponent<SoundManager>();
        }

        return null;
    }
}
