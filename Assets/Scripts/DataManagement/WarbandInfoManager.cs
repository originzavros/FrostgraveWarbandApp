using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WarbandInfoManager : MonoBehaviour
{
    
    private PlayerWarband playerWarband;
    private PlayerWizard playerWizard;

    private PlayerWarband activeGameWarband;
    private PlayerWarband lastGamePlayedWarband;

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
        // playerWizard.wizardProfilekey =  playerWarband.warbandName + "wizardkey";
        // ES3.Save(playerWizard.wizardProfilekey, playerWizard.playerWizardProfile);
        ES3.Save(key, playerWarband);
        ES3.Save("warbandNames", LoadAssets.warbandNames);
        SaveCurrentWarbandExport();
    }

    public void LoadWarband(string _warbandName)
    {
        if(ES3.KeyExists(_warbandName))
        {
            PlayerWarband tempwarband = new PlayerWarband();
            tempwarband.warbandWizard = new PlayerWizard();
            ES3.LoadInto<PlayerWarband>(_warbandName, tempwarband);

            // tempwarband.warbandWizard.playerWizardProfile = new RuntimeSoldierData();
            // ES3.LoadInto<RuntimeSoldierData>(tempwarband.warbandWizard.wizardProfilekey, tempwarband.warbandWizard.playerWizardProfile);
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

    // public void UpdateCurrentlyLoadedWarbandWithActiveGame(PlayerWarband _playerWarband)
    // {
        
    // }

    public void SaveActiveGame(PlayerWarband _playerWarband)
    {
        activeGameWarband = _playerWarband;
        string id = _playerWarband.warbandName + "ActiveGameSave";
        ES3.Save(id, activeGameWarband);
    }
    public PlayerWarband LoadActiveGame(string _warbandName)
    {
        string id = _warbandName + "ActiveGameSave";
        PlayerWarband tempwarband = new PlayerWarband();
        tempwarband.warbandName = "temp";
        if(ES3.KeyExists(id))
        {
            tempwarband.warbandWizard = new PlayerWizard();
            ES3.LoadInto<PlayerWarband>(id, tempwarband);

            // tempwarband.warbandWizard.playerWizardProfile = new RuntimeSoldierData();
            // ES3.LoadInto<RuntimeSoldierData>(tempwarband.warbandWizard.wizardProfilekey, tempwarband.warbandWizard.playerWizardProfile);
        }
        activeGameWarband = tempwarband;
        return tempwarband;
    }

    public void DeleteActiveGame(string _warbandName)
    {
        string id = _warbandName + "ActiveGameSave";
        if(ES3.KeyExists(id))
        {
            ES3.DeleteKey(id);
        }
    }


    public void SaveCurrentWarbandExport()
    {
        string key = playerWarband.warbandName;
        if(!LoadAssets.warbandNames.Contains(key))
        {
            LoadAssets.warbandNames.Add(key);
        }
        // playerWizard.wizardProfilekey =  playerWarband.warbandName + "wizardkey";
        // ES3.Save(playerWizard.wizardProfilekey, playerWizard.playerWizardProfile);

        var settings = new ES3Settings();
        settings.location = ES3.Location.PlayerPrefs;
        ES3.Save(key, playerWarband, settings);
        ES3.Save("warbandNames", LoadAssets.warbandNames, settings);
    }






    
}
