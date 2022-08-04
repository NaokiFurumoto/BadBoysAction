using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
/// <summary>
/// ドロップアイテムに関する制御
/// </summary>
public class ItemController : MonoBehaviour
{
    /// <summary>
    /// ドロップアイテムリスト
    /// </summary>
    [SerializeField]
    private List<DropItem> dropItems = new List<DropItem>();

    [SerializeField]
    private Transform dropRoot;

    [SerializeField]
    private Vector3 dropItemPos;

    /// <summary>
    /// ドロップアイテムタイプ
    /// </summary>
    private DROPITEM_TYPE dropItemType;

    /// <summary>
    /// 選択されたドロップアイテム
    /// </summary>
    private DropItem selectDropItem;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        dropItemType = DROPITEM_TYPE.NONE;
    }

    /// <summary>
    /// ドロップアイテムの設定
    /// </summary>
    /// <param name="_itemType"></param>
    public void SetDropItem(DROPITEM_TYPE _itemType)
    {
        foreach(var item in dropItems)
        {
            if(item.Type == _itemType)
            {
                selectDropItem = item;
                return;
            }
        }
    }

    /// <summary>
    /// ドロップアイテムの配置位置の設定
    /// </summary>
    private void SetDropPosition()
    {
        var x = Random.Range(DROPITEM_POSX_MIN, DROPITEM_POSX_MAX);
        var y = Random.Range(DROPITEM_POSY_MIN, DROPITEM_POSY_MAX);

        dropItemPos = new Vector3(x, y, 0);
    }

    /// <summary>
    /// ドロップアイテム配置
    /// </summary>
    public void CreateDropItem()
    {
        if (selectDropItem == null)
            return;

        SetDropPosition();

        var obj = Instantiate(selectDropItem,dropItemPos,Quaternion.identity);
        obj.transform.SetParent(dropRoot);
    }
}
