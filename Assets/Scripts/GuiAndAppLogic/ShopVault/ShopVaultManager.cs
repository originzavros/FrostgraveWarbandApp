using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ShopVaultManager : MonoBehaviour
{
    
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopSelectionPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopBuyPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopVaultPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopWarbandPanel;

    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject shopBuyContents;
    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject vaultContents;
    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject warbandContents;

    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] TextMeshProUGUI goldValueText;

    
    [BoxGroup("Popups")][SerializeField] ItemDescriptionPopup itemDescriptionPopup;
    [BoxGroup("Popups")][SerializeField] GameObject errorPopupContainer;

    [BoxGroup("Prefabs")] [SerializeField] GameObject buySellButtonContainerPrefab;
    [BoxGroup("Prefabs")] [SerializeField] GameObject itemSlotPrefab;
    [BoxGroup("Prefabs")] [SerializeField] GameObject collapsableWindowPlaymode;

    private PlayerWarband currentWarband;

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        currentWarband.warbandGold += 1000;
        UpdateGoldAmount(currentWarband.warbandGold);
        FillWarbandPanelWithSoldiers();
        OnClickShopTab();
    }

    public void OnClickShopTab()
    {
        DisableAllPanels();
        shopSelectionPanel.SetActive(true);
    }
    public void OnClickVaultTab()
    {
        DisableAllPanels();
        shopVaultPanel.SetActive(true);
    }
    public void OnClickWarbandTab()
    {
        DisableAllPanels();
        shopWarbandPanel.SetActive(true);
    }

    public void DisableAllPanels()
    {
        shopSelectionPanel.SetActive(false);
        shopVaultPanel.SetActive(false);
        shopWarbandPanel.SetActive(false);
    }

    public void UpdateGoldAmount(int newAmount)
    {
        goldValueText.text = "Gold: " + newAmount.ToString();
    }

    public bool CheckIfPurchaseable(int purchaseAmount)
    {
        if(purchaseAmount > currentWarband.warbandGold)
        {
            return false;
        }
        else{
            return true;
        }

    }

    public void PurchaseItem()
    {

    }

    public void FillShopBuyWithItems(string shopType)
    {
        if(shopType == "Potions")
        {
            foreach(var item in LoadAssets.allMagicItemObjects)
            {
                if(item.itemType == MagicItemType.LesserPotion || item.itemType == MagicItemType.GreaterPotion)
                {
                    if(item.itemPurchasePrice > 0) //filter out potions that you can only get as treasure/craft
                    {
                        InstanceItemButtonAndAttach(item, shopBuyContents, ItemContainerMode.buy);
                    }
                    
                }
            }
        }
        else if(shopType == "Grimoires"){
            
        }
        else if(shopType == "MagicEquipment"){
            
        }
        else if(shopType == "MagicItems"){
            
        }
        else if(shopType == "BaseResources"){
            
        }

        shopBuyPanel.gameObject.SetActive(true);
    }

    public void FillWarbandPanelWithSoldiers()
    {
        foreach(var item in currentWarband.warbandSoldiers)
        {
            CreateAndAttachPlaymodeSoldierContainer(item, warbandContents);
        }
        
    }

    public void InstanceItemButtonAndAttach(MagicItemScriptable itemScriptable, GameObject parentWindow, ItemContainerMode buymode)
    {
        GameObject temp = Instantiate(buySellButtonContainerPrefab);
        ItemButton newItemButton =  temp.GetComponentInChildren<ItemButton>();
        newItemButton.Init(itemScriptable);
        newItemButton.SetButtonEvent(delegate {AddItemInfoToItemPopup(newItemButton.GetItemReference());});

        SoldierHireWindow shw = temp.GetComponent<SoldierHireWindow>();
        shw.SetHireEvent(delegate {BuyItem(shw);});
        shw.SetFireEvent(delegate {SellItem(shw);});
        if(buymode == ItemContainerMode.buy){ shw.SwitchToHireMode();}
        else{
            shw.SwitchToFireMode();
            newItemButton.SwitchCostToSell();}

        temp.transform.SetParent(parentWindow.transform);

    }

     private void CreateAndAttachPlaymodeSoldierContainer(RuntimeSoldierData incoming, GameObject attachedTo)
    {
        GameObject temp = Instantiate(collapsableWindowPlaymode);
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        csw.UpdatePanelInfo(incoming);

        csw.SetUpForVaultPanel();
        csw.SetBodyPermaActive();
        if(incoming.soldierInventory.Count > 0)
        {
            foreach(var item in incoming.soldierInventory)
            {
                GameObject newItemSlot = Instantiate(itemSlotPrefab);
                ItemSlotSoldier iss = newItemSlot.GetComponent<ItemSlotSoldier>();
                iss.SetItem(item);
                iss.SetItemDescriptionButtonEvent(delegate { AddItemInfoToItemPopup(item);});
                iss.SetRemoveItemEvent(delegate { RemoveItemFromSoldier(csw, item);});
                iss.SetAddItemEvent( delegate { AddItemToSoldier(csw);});
                //csw.AddItemToContents()
            }
        }
        else{
            int count = 0;
            while(count < incoming.inventoryLimit)
            {

            }
        }
        
        temp.transform.SetParent(attachedTo.transform);
    }

    private void RemoveItemFromSoldier(PlaymodeWindow _playmodeWindow, MagicItemScriptable item)
    {
        RuntimeSoldierData rsd = _playmodeWindow.GetStoredSoldier();
        if(rsd.soldierInventory.Count > 0)
        {
            // int count = 0;
            foreach(var listitem in rsd.soldierInventory)
            {
                if(listitem == item)
                {
                    rsd.soldierInventory.Remove(listitem);
                    break; //only want to remove the first instance of that item
                }
                // count++;
            }
        }
    }
    private void AddItemToSoldier(PlaymodeWindow _playmodeWindow)
    {
        //need to launch item selection window
        RuntimeSoldierData rsd = _playmodeWindow.GetStoredSoldier();
        if(rsd.soldierInventory.Count < rsd.inventoryLimit)
        {

        }
        else{
            Debug.Log("tried to add item to soldier with inventory at limit");
        }
    }

    public void OnClickPotionShop()
    {
        FillShopBuyWithItems("Potions");
    }
    public void OnClickGrimoireShop()
    {

    }
    public void OnClickMagicEquipmentShop()
    {

    }
    public void OnClickMagicItemShop()
    {

    }
    public void OnClickBaseResourcesShop()
    {

    }

    // public void OnClickItemButtonForDescritptionPopup()
    // {

    // }

    public void AddItemInfoToItemPopup(MagicItemScriptable itemScriptable)
    {
        itemDescriptionPopup.gameObject.SetActive(true);
        itemDescriptionPopup.GetComponent<ItemDescriptionPopup>().Init(itemScriptable);
    }

    public void BuyItem(SoldierHireWindow shw)
    {
        if(CheckIfPurchaseable(shw.GetComponentInChildren<ItemButton>().GetItemReference().itemPurchasePrice))
        {
            InstanceItemButtonAndAttach(shw.GetComponentInChildren<ItemButton>().GetItemReference(), vaultContents, ItemContainerMode.sell);
            currentWarband.warbandGold -= shw.GetComponentInChildren<ItemButton>().GetItemReference().itemPurchasePrice;
            UpdateGoldAmount(currentWarband.warbandGold);
        }
        else{
            ErrorPopup("Not Enough Gold");
        }
    }

    public void SellItem(SoldierHireWindow shw)
    {
        currentWarband.warbandGold += shw.GetComponentInChildren<ItemButton>().GetItemReference().itemSalePrice;
        UpdateGoldAmount(currentWarband.warbandGold);
        Destroy(shw.gameObject);
    }

    public void ErrorPopup(string error)
    {
        errorPopupContainer.SetActive(true);
        errorPopupContainer.GetComponent<BasicPopup>().UpdatePopupText(error);
    }

    public enum ItemContainerMode{
        buy,
        sell,
        use
    }


}
