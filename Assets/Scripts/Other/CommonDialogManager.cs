using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using System.Linq;

/// <summary>
/// 汎用ダイアログを管理するクラス
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
    /// リストに追加
    /// </summary>
    /// <param name="_dialog">表示ダイアログ</param>
    public void AddList(CommonDialog _dialog)
    {
        if (_dialog == null || dialogs.Contains(_dialog))
            return;

        dialogs.Add(_dialog);
    }

    /// <summary>
    /// 表示中のダイアログを全削除
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
    /// 表示中のダイアログを1つ削除
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
    /// リストのクリア：削除はしない
    /// </summary>
    public void ListClear()
    {
        if (dialogs.Count == 0)
            return;

        dialogs.Clear();
    }

    /// <summary>
    /// 現在表示中のダイアログを強制取得
    /// </summary>
    /// <returns></returns>
    public List<CommonDialog> GetDisplayDialogs()
    {
        var root = GameObject.FindGameObjectWithTag("DialogRoot") as GameObject;
        List<CommonDialog> dialogs = root?.GetComponentsInChildren<CommonDialog>().ToList();

        return dialogs;
    }
}
