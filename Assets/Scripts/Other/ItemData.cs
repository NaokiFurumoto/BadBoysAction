using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    LIFE,
}
[CreateAssetMenu(menuName = "MyScriptable/Create ItemData")]
public class ItemData : ScriptableObject
{

    [SerializeField]
    private ITEM_TYPE type;

    [SerializeField]
    private int amount;

    public ITEM_TYPE Type => type;
    public int Amount => amount;

}
