using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandUIManager : MonoBehaviour
{
    
    [SerializeField] GameObject warbandListerUI;
    [SerializeField] GameObject warbandMainContentUI;
    [SerializeField] GameObject warbandHireSoldiersUI;
    [SerializeField] GameObject basicButtonPrefab;


   [SerializeField]  WarbandInfoManager warbandInfoManager;
   [SerializeField] WarbandLister warbandLister;
   [SerializeField] HireSoldiersManager hireSoldiersManager;


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
        warbandMainContentUI.SetActive(false);
        warbandHireSoldiersUI.SetActive(true);
        hireSoldiersManager.Init(warbandInfoManager.GetCurrentlyLoadedWarband());
    }
    public void OnClickVaultButton()
    {

    }
    public void OnClickViewWarbandButton()
    {

    }

    public void BackToMain()
    {
        warbandMainContentUI.SetActive(true);
        warbandListerUI.SetActive(false);
        warbandHireSoldiersUI.SetActive(false);
    }

    public void BackToWarbandMain()
    {
        warbandMainContentUI.SetActive(true);
        warbandHireSoldiersUI.SetActive(false);
    }

    public void SaveWarbandChanges()
    {
        warbandInfoManager.SaveCurrentWarband();
    }





}
