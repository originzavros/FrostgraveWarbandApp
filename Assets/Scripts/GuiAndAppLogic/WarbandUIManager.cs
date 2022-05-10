using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandUIManager : MonoBehaviour
{
    
    [SerializeField] GameObject warbandListerUI;
    [SerializeField] GameObject warbandMainContentUI;
    [SerializeField] GameObject warbandHireSoldiersUI;
    [SerializeField] GameObject warbandShopVaultUI;
    [SerializeField] GameObject warbandPlayGameUI;
    [SerializeField] GameObject basicButtonPrefab;

    [SerializeField] NavBox navBox;


   [SerializeField]  WarbandInfoManager warbandInfoManager;
   [SerializeField] WarbandLister warbandLister;
   [SerializeField] HireSoldiersManager hireSoldiersManager;
   [SerializeField] ShopVaultManager shopVaultManager;


    public void Init()
    {
        warbandListerUI.SetActive(true);
        warbandMainContentUI.SetActive(false);
        warbandLister.PopulateListerWithWarbands();
    }

    public void WarbandSelected(string name)
    {
        warbandInfoManager.LoadWarband(name);
        Debug.Log("Selected this warband:" + name);
        warbandListerUI.SetActive(false);
        warbandMainContentUI.SetActive(true);
    }

    public void OnClickHireSoldiersButton()
    {
        DisableAllContentWindows();
        warbandHireSoldiersUI.SetActive(true);
        hireSoldiersManager.Init(warbandInfoManager.GetCurrentlyLoadedWarband());
    }
    public void OnClickVaultButton()
    {
        DisableAllContentWindows();
        warbandShopVaultUI.SetActive(true);
        shopVaultManager.Init();

    }
    public void OnPlayGameButton()
    {
        DisableAllContentWindows();
        warbandPlayGameUI.SetActive(true);

    }

    public void BackToMain()
    {
        DisableAllContentWindows();
        warbandMainContentUI.SetActive(true);
        navBox.ChangeScreenName("Warband Manager");
    }

    public void BackToWarbandMain()
    {
        DisableAllContentWindows();
        warbandMainContentUI.SetActive(true);
    }

    public void SaveWarbandChanges()
    {
        warbandInfoManager.SaveCurrentWarband();
    }

    public void DisableAllContentWindows()
    {
        warbandMainContentUI.SetActive(false);
        warbandListerUI.SetActive(false);
        warbandHireSoldiersUI.SetActive(false);
        warbandPlayGameUI.SetActive(false);
        warbandShopVaultUI.SetActive(false);
    }





}
