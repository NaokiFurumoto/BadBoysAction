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
    public static ItemController Instance;

    /// <summary>
    /// ドロップアイテムリスト
    /// </summary>
    [SerializeField]
    private List<DropItem> dropItemsList = new List<DropItem>();

    [SerializeField]
    private Transform dropRoot;

    [SerializeField]
    private Vector3 dropItemPos;

    /// <summary>
    /// ゲーム上に配置してあるアイテムリスト
    /// </summary>
    private static List<DropItem> dropedItems = new List<DropItem>();

    /// <summary>
    /// 選択されたドロップアイテム
    /// </summary>
    private DropItem selectDropItem;

    private void Awake()
    {
        InitializeThis();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void InitializeThis()
    {
        Instance ??= this;
        dropedItems.Clear();
    }

    /// <summary>
    /// ドロップアイテムの設定
    /// </summary>
    /// <param name="_itemType"></param>
    public void SetDropItem(DROPITEM_TYPE _itemType)
    {
        foreach(var item in dropItemsList)
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
    /// アイテム抽選
    /// </summary>
    public void DropItemLottery(Vector2 pos)
    {
        //ランダムで抽選
        var hitValue = Random.Range(1, MUTEKI_DROPINDEX);
        if(MUTEKI_DROPHITINDEX >= hitValue)
        {
            var choice = Random.Range(0, 2);
            var item = DROPITEM_TYPE.NONE;
            if(choice == 0)
            {
                item = DROPITEM_TYPE.HAOU;
            }
            else
            {
                item = DROPITEM_TYPE.HAOU;
            }

            //当たり
            SetDropItem(item);
            dropItemPos = pos;
            CreateDropItem(false);
        }
    }

    /// <summary>
    /// ドロップアイテム配置
    /// </summary>
    /// <param name="isRandom">ランダム配置かどうか</param>
    public void CreateDropItem(bool isRandom)
    {
        if (selectDropItem == null)
            return;

        //ランダム配置の場合
        if (isRandom)
        {
            SetDropPosition();
        }

        var obj = Instantiate(selectDropItem,dropItemPos,Quaternion.identity);
        obj.transform.SetParent(dropRoot);

        dropedItems.Add(obj.GetComponent<DropItem>());
        dropedItems.RemoveAll(item => item == null);
    }

    /// <summary>
    /// ドロップアイテムすべて削除
    /// </summary>
    public void RemoveDropItems()
    {
        if (dropedItems.Count <= 0)
            return;
        RemoveNullItem();
        dropedItems.ForEach(item => item?.Destroy());
    }

    /// <summary>
    /// Null要素の削除
    /// </summary>
    public void RemoveNullItem()
    {
        dropedItems.RemoveAll(item => item == null);
    }

    /// <summary>
    /// リトライ処理
    /// </summary>
    public void Retry()
    {
        RemoveDropItems();
        dropedItems.Clear();
    }
}
