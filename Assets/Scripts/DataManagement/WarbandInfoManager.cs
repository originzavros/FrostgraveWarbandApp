using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandInfoManager : MonoBehaviour
{
    private PlayerWarband playerWarband;
    private PlayerWizard playerWizard;

    public void Init(PlayerWarband _warband)
    {
        playerWarband = _warband;
        playerWizard = playerWarband.warbandWizard;
    }

    public void SaveCurrentWarband()
    {
        string key = playerWarband.warbandName;
        LoadAssets.warbandNames.Add(key);
        ES3.Save(key, playerWarband);
        ES3.Save("warbandNames", LoadAssets.warbandNames);
    }

    public void LoadWarband(string _warbandName)
    {
        if(ES3.KeyExists(_warbandName))
        {
            Init(ES3.Load<PlayerWarband>(_warbandName));
        }
        else{
            Debug.Log("could not load warband");
        }
    }

    public PlayerWarband GetCurrentlyLoadedWarband()
    {
        return playerWarband;
    }

    
}
