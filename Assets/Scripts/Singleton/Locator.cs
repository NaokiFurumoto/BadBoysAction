using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Singleton�����g���₷���T�[�r�X��񋟂�����N���X
/// Interface�Ƒg�ݍ��킹��Ƌ���
/// </summary>

public static class Locator<T> where T : class
{
    public static T Instance { get; private set; }

    /// <summary>
    /// null�`�F�b�N
    /// </summary>
    /// <returns></returns>
    public static bool IsValid() => Instance != null;

    /// <summary>
    /// �O������ݒ肷��
    /// Locator<�N���X��>.Bind(this)
    /// Locator<�N���X��>.Bind(new �N���X��)
    /// </summary>
    /// <param name="instance"></param>
    public static void Bind(T instance) 
    {
        Instance = instance;
    }

    /// <summary>
    /// �N���A�F����
    /// </summary>
    /// <param name="instance"></param>
    public static void Unbind(T instance)
    {
        if(Instance == instance)
        {
            Instance = null;
        }
    }

    /// <summary>
    /// �N���A:����
    /// </summary>
    public static void Clear()
    {
        Instance = null;
    }


}
