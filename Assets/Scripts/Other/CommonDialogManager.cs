using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using System.Linq;

/// <summary>
/// �ėp�_�C�A���O���Ǘ�����N���X
/// </summary>
public class CommonDialogManager : MonoBehaviour
{
    public static CommonDialogManager Instance;

    private static List<CommonDialog> dialogs = new List<CommonDialog>();

    public List<CommonDialog> Dialogs => dialogs;

    private void Awake()
    {
        Instance ??= this;
        dialogs.Clear();
    }

    /// <summary>
    /// ���X�g�ɒǉ�
    /// </summary>
    /// <param name="_dialog">�\���_�C�A���O</param>
    public void AddList(CommonDialog _dialog)
    {
        if (_dialog == null || dialogs.Contains(_dialog))
            return;

        dialogs.Add(_dialog);
    }

    /// <summary>
    /// �\�����̃_�C�A���O��S�폜
    /// </summary>
    public void DeleteDialogAll()
    {
        if (dialogs.Count == 0)
            return;

        foreach(var dialog in dialogs)
        {
            Destroy(dialog.gameObject);
        }

        dialogs.Clear();
    }

    /// <summary>
    /// �\�����̃_�C�A���O��1�폜
    /// </summary>
    public void DeleteOneDialog()
    {
        var length = dialogs.Count;
        if (length == 0)
            return;

        var last = dialogs.Last();
        dialogs.RemoveAt(length-1);
        Destroy(last.gameObject);
    }

    /// <summary>
    /// ���X�g�̃N���A�F�폜�͂��Ȃ�
    /// </summary>
    public void ListClear()
    {
        if (dialogs.Count == 0)
            return;

        dialogs.Clear();
    }

    /// <summary>
    /// ���ݕ\�����̃_�C�A���O�������擾
    /// </summary>
    /// <returns></returns>
    public List<CommonDialog> GetDisplayDialogs()
    {
        var root = GameObject.FindGameObjectWithTag("DialogRoot") as GameObject;
        List<CommonDialog> dialogs = root?.GetComponentsInChildren<CommonDialog>().ToList();

        return dialogs;
    }
}
