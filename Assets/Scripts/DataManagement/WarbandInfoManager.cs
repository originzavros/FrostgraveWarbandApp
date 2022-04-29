using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WarbandInfoManager : MonoBehaviour
{
    
    [SerializeField] PlayerWarband playerWarband;
    private PlayerWizard playerWizard;

    public void Init(PlayerWarband _warband)
    {
        playerWarband = _warband;
        playerWizard = playerWarband.warbandWizard;
    }

    public void SaveCurrentWarband()
    {
        string key = playerWarband.warbandName;
        if(!LoadAssets.warbandNames.Contains(key))
        {
            LoadAssets.warbandNames.Add(key);
        }
        playerWarband.saveSoldierArray = playerWarband.warbandSoldiers.ToArray();
        ES3.Save(key, playerWarband);
        ES3.Save("warbandNames", LoadAssets.warbandNames);
    }

    public void LoadWarband(string _warbandName)
    {
        if(ES3.KeyExists(_warbandName))
        {
            PlayerWarband tempwarband = ES3.Load<PlayerWarband>(_warbandName);
            tempwarband.warbandSoldiers.Clear();
            foreach(var item in tempwarband.saveSoldierArray)
            {
                tempwarband.warbandSoldiers.Add(item);
            }
            Init(tempwarband);
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
