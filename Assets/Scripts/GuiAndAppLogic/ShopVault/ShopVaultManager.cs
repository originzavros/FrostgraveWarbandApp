using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;
// using UnityEditor;

public class ShopVaultManager : MonoBehaviour
{
    
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopSelectionPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopBuyPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopVaultPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject shopWarbandPanel;
    [BoxGroup("Gui Objects")][SerializeField] GameObject saveSettingsPanel;

    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject shopBuyContents;
    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject vaultContents;
    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject warbandContents;
    [BoxGroup("Gui Object Contents")] [SerializeField] GameObject itemSelectPopupContents;

    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] WarbandUIManager warbandUIManager;
    [SerializeField] TextMeshProUGUI goldValueText;
    [SerializeField] CampaignSettingsManager campaignSettingsManager;

    
    [BoxGroup("Popups")][SerializeField] ItemDescriptionPopup itemDescriptionPopup;
    [BoxGroup("Popups")][SerializeField] GameObject errorPopupContainer;
    [BoxGroup("Popups")][SerializeField] GameObject itemSelectPopup;
    [BoxGroup("Popups")][SerializeField] GameObject customItemPopup;

    [BoxGroup("Prefabs")] [SerializeField] GameObject buySellButtonContainerPrefab;
    [BoxGroup("Prefabs")] [SerializeField] GameObject itemSlotPrefab;
    [BoxGroup("Prefabs")] [SerializeField] GameObject collapsableWindowPlaymode;
    [BoxGroup("Prefabs")] [SerializeField] GameObject itemButtonPrefab;

    [BoxGroup("ExpansionButtons")][SerializeField] GameObject mazeOfMalcorButton;

    private PlayerWarband currentWarband;

    //These are here for convenience when adding items to soldiers
    //don't want to juggle chaining variable instances through UI objects
    private ItemSlotSoldier currentlySelectedSoldierSlot;
    private PlaymodeWindow currentlySelectedSoldierWindow;
    [SerializeField] int testGoldModifier = 0;


    public void Init()
    {
        // Debug.Log("Begin Init");
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        // Debug.Log("loaded warband");
        currentWarband.warbandGold += testGoldModifier;
        UpdateGoldAmount(currentWarband.warbandGold);
        // Debug.Log("updated gold");
        FillWarbandPanelWithSoldiers();
        // Debug.Log("filled warband panel with soldiers");
        OnClickShopTab();
        // Debug.Log("clicked shop tab");

        EnableButtonsBasedOnCampaigns();
    }

    #region tabsOnClick
    public void OnClickShopTab()
    {
        DisableAllPanels();
        shopSelectionPanel.SetActive(true);
    }
    public void OnClickVaultTab()
    {
        DisableAllPanels();
        shopVaultPanel.SetActive(true);
        UpdateVaultItems();
    }
    public void OnClickWarbandTab()
    {
        DisableAllPanels();
        shopWarbandPanel.SetActive(true);
    }
    public void OnClickSaveSettingsTab()
    {
        DisableAllPanels();
        saveSettingsPanel.SetActive(true);
    }

    public void DisableAllPanels()
    {
        shopSelectionPanel.SetActive(false);
        shopVaultPanel.SetActive(false);
        shopWarbandPanel.SetActive(false);
        saveSettingsPanel.SetActive(false);
        shopBuyPanel.SetActive(false);
    }
    #endregion

    private void UpdateGoldAmount(int newAmount)
    {
        goldValueText.text = "Gold: " + newAmount.ToString();
    }

    private void UpdateVaultItems()
    {
        ClearVaultPanel();
        foreach(var item in currentWarband.warbandVault)
        {
            InstanceItemContainerAndAttach(item, vaultContents, ItemContainerMode.sell);
        }
    }

    private void ClearVaultPanel()
    {
        foreach(Transform item in vaultContents.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private bool CheckIfPurchaseable(int purchaseAmount)
    {
        if(purchaseAmount > currentWarband.warbandGold)
        {
            return false;
        }
        else{
            return true;
        }

    }

    private void EnableButtonsBasedOnCampaigns()
    {
        mazeOfMalcorButton.SetActive(false); //make sure these are off if we go back and change settings.
        foreach(var book in campaignSettingsManager.GetEnabledCampaigns())
        {
            if(book == FrostgraveBook.TheMazeOfMalcor)
            {
                mazeOfMalcorButton.SetActive(true);
            }
        }
    }

    private void FillShopBuyWithItems(string shopType)
    {
        foreach(Transform child in shopBuyContents.transform)
        {
            Destroy(child.gameObject);
        }

        if(shopType == "BasicEquipment")
        {
            foreach(var item in LoadAssets.allEquipmentObjects)
            {
                MagicItemRuntime newItem = new MagicItemRuntime();
                newItem.Init(item);
                InstanceItemContainerAndAttach(newItem, shopBuyContents, ItemContainerMode.buy);
            }
        }

        if(shopType == "Black Market"){
                int totalItemsFound = 0;
                while(totalItemsFound < 4)
                {
                    bool usableItem = true;
                    MagicItemScriptable randomItem = LoadAssets.allMagicItemObjects[Random.Range(0,LoadAssets.allMagicItemObjects.Length)];
                    if(randomItem.itemPurchasePrice < 1){usableItem = false;}
                    if(randomItem.itemType == MagicItemType.Base || randomItem.itemType == MagicItemType.BaseResource){ usableItem = false;}
                    if(randomItem.itemName == "Crafted Scroll"){usableItem = false;}
                    if(usableItem)
                    {
                        MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(randomItem);
                        InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                        totalItemsFound++;
                    }
                }
        }

        if(shopType == "TheMazeOfMalcor")
        {
            foreach(var item in LoadAssets.allMagicItemObjects)
            {
                if(item.itemBook == FrostgraveBook.TheMazeOfMalcor)
                {
                    MagicItemRuntime temp = new MagicItemRuntime();
                    temp.Init(item);
                    InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                }
            }
        }
        else{
            foreach(var item in LoadAssets.allMagicItemObjects)
            {
                if(item.itemBook != FrostgraveBook.Core)
                {
                    // bool canContinue = false;
                    // foreach(var book in campaignSettingsManager.GetEnabledCampaigns())
                    // {
                    //     if(book == item.itemBook)
                    //     {
                    //         canContinue = true;
                    //     }
                    // }
                    // if(!canContinue)
                    // {
                        continue;//skips the rest of the loop for the current item
                    // }
                }

                if(shopType == "Potions")
                {
                    if(item.itemType == MagicItemType.LesserPotion || item.itemType == MagicItemType.GreaterPotion)
                    {
                        if(item.itemPurchasePrice > 0) //filter out potions that you can only get as treasure/craft
                        {
                            MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(item);
                            InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                        }
                        
                    }
                    
                }
                else if(shopType == "Grimoires"){ 
                    if(item.itemType == MagicItemType.Grimoire)
                    {
                        if(item.itemPurchasePrice > 0)
                        {
                            MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(item);
                            InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                        }
                    }
                    
                }
                else if(shopType == "MagicEquipment"){
                    if(item.itemType == MagicItemType.MagicEquipment)
                    {
                        if(item.itemPurchasePrice > 0)
                        {
                            MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(item);
                            InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                        }
                    } 
                }
                else if(shopType == "MagicItems"){
                    if(item.itemType == MagicItemType.Artifact)
                    {
                        if(item.itemPurchasePrice > 0)
                        {
                            MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(item);
                            InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                        }
                    }
                }
                else if(shopType == "BaseResources"){
                    if(item.itemType == MagicItemType.Base || item.itemType == MagicItemType.BaseResource)
                    {
                        MagicItemRuntime temp = new MagicItemRuntime();
                            temp.Init(item);
                        InstanceItemContainerAndAttach(temp, shopBuyContents, ItemContainerMode.buy);
                    }
                }
                // for(int i = 0; i < 4; i++)
                // {
                //     MagicItemScriptable randomItem = LoadAssets.allMagicItemObjects[Random.Range(0,LoadAssets.allMagicItemObjects.Length)];
                //     if(randomItem.itemPurchasePrice > 0 && randomItem.MagicItemType != MagicItemType.Base && randomItem.MagicItemType != MagicItemType.BaseResource)
                //     {
                //         InstanceItemContainerAndAttach(randomItem, shopBuyContents, ItemContainerMode.buy);
                //     }
                // }
            }
        }

        shopBuyPanel.gameObject.SetActive(true);
    }

    private void FillWarbandPanelWithSoldiers()
    {
        foreach(Transform child in warbandContents.transform)
        {
            Destroy(child.gameObject);
        }
        CreateAndAttachPlaymodeSoldierContainer(currentWarband.warbandWizard.playerWizardProfile, warbandContents);
        foreach(var item in currentWarband.warbandSoldiers)
        {
            CreateAndAttachPlaymodeSoldierContainer(item, warbandContents);
        }
    }

    private void InstanceItemContainerAndAttach(MagicItemRuntime itemScriptable, GameObject parentWindow, ItemContainerMode buymode)
    {
        GameObject temp = Instantiate(buySellButtonContainerPrefab);
        ItemButton newItemButton =  temp.GetComponentInChildren<ItemButton>();
        // MagicItemRuntime runtimeItem = new MagicItemRuntime();
        // runtimeItem.Init(itemScriptable);
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

    private void InstanceItemSelectButtonAndAttach(MagicItemRuntime itemScriptable, GameObject parentWindow)
    {
        GameObject temp = Instantiate(itemButtonPrefab);
        ItemButton newItemButton =  temp.GetComponent<ItemButton>();
        newItemButton.Init(itemScriptable);
        // newItemButton.SetButtonEvent(delegate {AddItemInfoToItemPopup(newItemButton.GetItemReference());});
        newItemButton.SetButtonEvent(delegate {AttachSelectedItemToSoldier(newItemButton);});

        temp.transform.SetParent(parentWindow.transform);
    }

     private void CreateAndAttachPlaymodeSoldierContainer(RuntimeSoldierData incoming, GameObject attachedTo)
    {
        GameObject temp = Instantiate(collapsableWindowPlaymode);
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        csw.UpdatePanelInfo(incoming);

        csw.SetUpForVaultPanel();
        csw.SetBodyPermaActive();

        //for each item in iventory below their item limit, add item, else add empty item slot
        int itemsOnSoldier = incoming.soldierInventory.Count;
        int currentItemSlotCount = 0;
        while(currentItemSlotCount < incoming.inventoryLimit)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab);
            ItemSlotSoldier iss = newItemSlot.GetComponent<ItemSlotSoldier>();

            iss.SetItemDescriptionButtonEvent(delegate { GetSoldierItemInfoAndAddToPopup(iss);});
            iss.SetRemoveItemEvent(delegate { RemoveItemFromSoldier(csw, iss);});
            iss.SetAddItemEvent( delegate { AddItemToSoldier(csw, iss);});

            if(currentItemSlotCount < itemsOnSoldier)
            {
                iss.SetItem(incoming.soldierInventory[currentItemSlotCount]);
                iss.SetItemToVaultMode();
            }
            csw.AddItemToContents(newItemSlot);
            currentItemSlotCount++;
        }

        temp.transform.SetParent(attachedTo.transform); 
    }

    private void RemoveItemFromSoldier(PlaymodeWindow _playmodeWindow, ItemSlotSoldier iss)
    {
        RuntimeSoldierData rsd = _playmodeWindow.GetStoredSoldier();
        if(rsd.soldierInventory.Count > 0)
        {
            foreach(var listitem in rsd.soldierInventory)
            {
                if(listitem == iss.GetStoredItem())
                {
                    currentWarband.warbandVault.Add(listitem);
                    rsd.soldierInventory.Remove(listitem);
                    break; //only want to remove the first instance of that item
                }
            }
        }
    }
    private void AddItemToSoldier(PlaymodeWindow _playmodeWindow, ItemSlotSoldier iss)
    {
        currentlySelectedSoldierSlot = iss;
        currentlySelectedSoldierWindow = _playmodeWindow;
        //need to launch item selection window
        RuntimeSoldierData rsd = _playmodeWindow.GetStoredSoldier();
        if(currentWarband.warbandVault.Count > 0)
        {
            if(rsd.soldierInventory.Count < rsd.inventoryLimit)
            {
                ActivateAndFillItemSelectPopup();
            }
            else{
                ErrorPopup("Can't add item, soldier inventory full");
                // Debug.Log("tried to add item to soldier with inventory at limit");
            }
        }
        else{
            iss.OnClickRemoveItem();
            ErrorPopup("Need items in vault to add to soldier");
        }
        
    }

    #region shopsOnClick
    public void OnClickPotionShop()
    {
        FillShopBuyWithItems("Potions");
    }
    public void OnClickGrimoireShop()
    {
        FillShopBuyWithItems("Grimoires");
    }
    public void OnClickMagicEquipmentShop()
    {
        FillShopBuyWithItems("MagicEquipment");
    }
    public void OnClickMagicItemShop()
    {
        FillShopBuyWithItems("MagicItems");
    }
    public void OnClickBaseResourcesShop()
    {
        FillShopBuyWithItems("BaseResources");
    }
    public void OnClickBlackMarketShop()
    {
        FillShopBuyWithItems("Black Market");
    }
    public void OnClickCustomItemShop()
    {
        customItemPopup.SetActive(true);
    }
    public void OnClickMazeOfMalcor()
    {
        FillShopBuyWithItems("TheMazeOfMalcor");
    }
    public void OnClickBasicEquipment()
    {
        FillShopBuyWithItems("BasicEquipment");
    }
    #endregion

    private void ActivateAndFillItemSelectPopup()
    {
        //clean up the popup window
        foreach(Transform child in itemSelectPopupContents.transform)
        {
            Destroy(child.gameObject);
        }
        itemSelectPopup.SetActive(true);
        foreach(var item in currentWarband.warbandVault)
        {
            InstanceItemSelectButtonAndAttach(item, itemSelectPopupContents);
        }
    }

    public void AttachSelectedItemToSoldier(ItemButton itemButton)
    {
        currentlySelectedSoldierSlot.SetItem(itemButton.GetItemReference());
        RemoveItemFromPlayerVault(itemButton.GetItemReference());
        currentlySelectedSoldierWindow.GetStoredSoldier().soldierInventory.Add(itemButton.GetItemReference());
        itemSelectPopup.SetActive(false);
    }
    private void GetSoldierItemInfoAndAddToPopup(ItemSlotSoldier iss)
    {
        AddItemInfoToItemPopup(iss.GetStoredItem());
    }
    public void AddItemInfoToItemPopup(MagicItemRuntime itemScriptable)
    {
        itemDescriptionPopup.gameObject.SetActive(true);
        itemDescriptionPopup.GetComponent<ItemDescriptionPopup>().Init(itemScriptable);
    }

    private void BuyItem(SoldierHireWindow shw)
    {
        if(shw.GetComponentInChildren<ItemButton>().GetItemReference().itemType == MagicItemType.Base)
        {
            foreach(var item in currentWarband.warbandVault)
            {
                if(item.itemType == MagicItemType.Base)
                {
                    ErrorPopup("Already Own a Base, Sell it to get a new one");
                    return;
                }
            }
        }

        if(CheckIfPurchaseable(shw.GetComponentInChildren<ItemButton>().GetItemReference().itemPurchasePrice))
        {
            // InstanceItemContainerAndAttach(shw.GetComponentInChildren<ItemButton>().GetItemReference(), vaultContents, ItemContainerMode.sell);
            currentWarband.warbandVault.Add(shw.GetComponentInChildren<ItemButton>().GetItemReference());
            currentWarband.warbandGold -= shw.GetComponentInChildren<ItemButton>().GetItemReference().itemPurchasePrice;
            UpdateGoldAmount(currentWarband.warbandGold);

        }
        else{
            ErrorPopup("Not Enough Gold");
        }
    }

    private void SellItem(SoldierHireWindow shw)
    {
        currentWarband.warbandGold += shw.GetComponentInChildren<ItemButton>().GetItemReference().itemSalePrice;
        UpdateGoldAmount(currentWarband.warbandGold);
        RemoveItemFromPlayerVault(shw.GetComponentInChildren<ItemButton>().GetItemReference());
        Destroy(shw.gameObject);
    }

    private void ErrorPopup(string error)
    {
        errorPopupContainer.SetActive(true);
        errorPopupContainer.GetComponent<BasicPopup>().UpdatePopupText(error);
    }

    public enum ItemContainerMode{
        buy,
        sell,
        use
    }

    private void RemoveItemFromPlayerVault(MagicItemRuntime itemScriptable)
    {
        foreach(var item in currentWarband.warbandVault)
        {
            if(item == itemScriptable)
            {
                currentWarband.warbandVault.Remove(item);
                break;
            }
        }
    }

    public void OnClickSaveAndExitButton()
    {
        SaveAllChangesToWarband();
        warbandUIManager.BackToWarbandMain();
    }

    public void OnClickCancelButton()
    {
        warbandUIManager.BackToWarbandMain();
    }

    private void SaveAllChangesToWarband()
    {
        // foreach(var item in currentWarband.warbandWizard.playerWizardProfile.soldierInventory)
        // {
        //     Debug.Log(item.itemName);
        // }
        // foreach(var soldier in currentWarband.warbandSoldiers)
        // {
        //     foreach(var item in soldier.soldierInventory)
        //     {
        //         Debug.Log(item.itemName);
        //     }
        // }
        warbandInfoManager.SaveCurrentWarband();
    }

    public void AddCustomItem(string itemName, string itemDescription)
    {
        MagicItemRuntime tempItem = new MagicItemRuntime();
        tempItem.itemName = itemName;
        tempItem.itemDescription = itemDescription;
        tempItem.itemType = MagicItemType.Custom;
        currentWarband.warbandVault.Add(tempItem);
        customItemPopup.SetActive(false);
    }

}
