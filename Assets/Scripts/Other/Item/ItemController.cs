using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
/// <summary>
/// �h���b�v�A�C�e���Ɋւ��鐧��
/// </summary>
public class ItemController : MonoBehaviour
{
    public static ItemController Instance;

    /// <summary>
    /// �h���b�v�A�C�e�����X�g
    /// </summary>
    [SerializeField]
    private List<DropItem> dropItemsList = new List<DropItem>();

    [SerializeField]
    private Transform dropRoot;

    [SerializeField]
    private Vector3 dropItemPos;

    /// <summary>
    /// �Q�[����ɔz�u���Ă���A�C�e�����X�g
    /// </summary>
    private static List<DropItem> dropedItems = new List<DropItem>();

    /// <summary>
    /// �h���b�v�A�C�e���^�C�v
    /// </summary>
    private DROPITEM_TYPE dropItemType;

    /// <summary>
    /// �I�����ꂽ�h���b�v�A�C�e��
    /// </summary>
    private DropItem selectDropItem;

    private void Awake()
    {
        InitializeThis();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void InitializeThis()
    {
        Instance ??= this;
        dropItemType = DROPITEM_TYPE.NONE;
        dropedItems.Clear();
    }

    /// <summary>
    /// �h���b�v�A�C�e���̐ݒ�
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
    /// �h���b�v�A�C�e���̔z�u�ʒu�̐ݒ�
    /// </summary>
    private void SetDropPosition()
    {
        var x = Random.Range(DROPITEM_POSX_MIN, DROPITEM_POSX_MAX);
        var y = Random.Range(DROPITEM_POSY_MIN, DROPITEM_POSY_MAX);

        dropItemPos = new Vector3(x, y, 0);
    }

    /// <summary>
    /// �A�C�e�����I
    /// </summary>
    public void DropItemLottery(Vector2 pos)
    {
        //�����_���Œ��I
        var hitValue = Random.Range(1, MUTEKI_DROPINDEX);
        var getValue = Random.Range(1, MUTEKI_DROPINDEX);
        if(hitValue == getValue)
        {
            //������
            SetDropItem(DROPITEM_TYPE.MUTEKI);
            dropItemPos = pos;
            CreateDropItem(false);
        }
    }

    /// <summary>
    /// �h���b�v�A�C�e���z�u
    /// </summary>
    /// <param name="isRandom">�����_���z�u���ǂ���</param>
    public void CreateDropItem(bool isRandom)
    {
        if (selectDropItem == null)
            return;

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
    /// �h���b�v�A�C�e�����ׂč폜
    /// </summary>
    public void RemoveDropItems()
    {
        if (dropedItems.Count <= 0)
            return;
        RemoveNullItem();
        dropedItems.ForEach(item => item?.Destroy());
    }

    /// <summary>
    /// Null�v�f�̍폜
    /// </summary>
    public void RemoveNullItem()
    {
        dropedItems.RemoveAll(item => item == null);
    }

    /// <summary>
    /// ���g���C����
    /// </summary>
    public void Retry()
    {
        RemoveDropItems();
        dropedItems.Clear();
    }
}
