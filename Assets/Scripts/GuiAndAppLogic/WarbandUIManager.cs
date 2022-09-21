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
    [SerializeField] GameObject warbandPostgameUi;
    [SerializeField] GameObject warbandManageSoldiersUI;
    [SerializeField] GameObject basicButtonPrefab;
    [SerializeField] GameObject warbandxpGoldUI;
    [SerializeField] GameObject wizardViewerUI;

    [SerializeField] NavBox navBox;


   [SerializeField]  WarbandInfoManager warbandInfoManager;
   [SerializeField] WarbandLister warbandLister;
   [SerializeField] HireSoldiersManager hireSoldiersManager;
   [SerializeField] ShopVaultManager shopVaultManager;
   [SerializeField] PlayModeManager playModeManager;
   [SerializeField] PostGameManager postGameManager;
   [SerializeField] SoldierManager soldierManager;
   [SerializeField] warbandXPGoldEditor warbandxpg;
    [SerializeField] WizardViewer wizardViewer;



    public void Init()
    {
        warbandListerUI.SetActive(true);
        warbandMainContentUI.SetActive(false);
        warbandLister.PopulateListerWithWarbands();
        navBox.ChangeFragmentName(AppFragment.WarbandManagerMain);
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
        playModeManager.Init();

    }

    public void OnClickManageSoldiers()
    {
        DisableAllContentWindows();
        warbandManageSoldiersUI.SetActive(true);
        soldierManager.Init();
        soldierManager.OnClickViewWarband();
    }

    public void OnClickWarbandXPGoldEditorButton()
    {
        DisableAllContentWindows();
        warbandxpGoldUI.SetActive(true);
        warbandxpg.Init();
    }

    public void OnClickWizardViewer()
    {
        DisableAllContentWindows();
        wizardViewerUI.SetActive(true);
        wizardViewer.Init();
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
        warbandPostgameUi.SetActive(false);
        warbandManageSoldiersUI.SetActive(false);
        warbandxpGoldUI.SetActive(false);
        wizardViewerUI.SetActive(false);

    }

    public void SwitchToPostgameAndInit(RuntimeGameInfo gameInfo)
    {
        warbandPlayGameUI.SetActive(false);
        warbandPostgameUi.SetActive(true);
        postGameManager.Init(warbandInfoManager.GetCurrentlyLoadedWarband(), gameInfo);
    }





}
