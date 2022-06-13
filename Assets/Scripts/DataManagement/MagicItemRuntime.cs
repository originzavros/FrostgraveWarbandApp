using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItemRuntime
{
    public string itemName;
    public string itemDescription;
    public int itemPurchasePrice;
    public int itemSalePrice;
    public FrostgraveBook itemBook;
    public MagicItemType itemType;
    

    public void Init(MagicItemScriptable _item)
    {
        itemName = _item.itemName;
        itemDescription = _item.itemDescription;
        itemPurchasePrice = _item.itemPurchasePrice;
        itemSalePrice = _item.itemSalePrice;
        itemBook = _item.itemBook;
        itemType = _item.itemType;
    }
}
