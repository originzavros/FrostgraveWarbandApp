using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignSettingsManager : MonoBehaviour
{
    [SerializeField] NavBox navBox;
    [SerializeField] Toggle allSoldiersToggle;
    [SerializeField] Toggle mazeOfMalcorToggle;

    private bool mazeOfMalcor;
    private bool allSoldiers;

    private List<FrostgraveBook> enabledCampaigns = new List<FrostgraveBook>();
    Dictionary<string, bool> settings = new Dictionary<string,bool>();

    public void Init()
    {
        // Debug.Log("Init the settings");
        if(ES3.KeyExists("CampaignSettings"))
        {
            // Debug.Log("found campaignSettings");
            settings = ES3.Load<Dictionary<string, bool>>("CampaignSettings", new Dictionary<string,bool>());
            mazeOfMalcor = settings["MazeOfMalcor"];
            mazeOfMalcorToggle.isOn = mazeOfMalcor;
            // if(mazeOfMalcor){
            //     Debug.Log("added maze to campaigns");
            //     enabledCampaigns.Add(FrostgraveBook.TheMazeOfMalcor);}
            allSoldiers = settings["AllSoldiers"];
            allSoldiersToggle.isOn = allSoldiers;
        }

        // foreach(var item in settings)
        // {
        //     Debug.Log(item.Key + ": " + item.Value);
        // }
        
    }

    //Unity triggers active toggles, so if they go to this screen, need to clear bonus entries from list
    public void Activate()
    {
        enabledCampaigns.Clear();
    }

    public List<FrostgraveBook> GetEnabledCampaigns()
    {
        return enabledCampaigns;
    }

    public void OnClickBackButton()
    {
        navBox.OnClickNavHome();
    }

    public void OnToggleMazeOfMalcor()
    {
        // Debug.Log("toggled");
        mazeOfMalcor = mazeOfMalcorToggle.isOn;
        if(mazeOfMalcor == true)
        {
            enabledCampaigns.Add(FrostgraveBook.TheMazeOfMalcor);
        }
        else{
            enabledCampaigns.Remove(FrostgraveBook.TheMazeOfMalcor);
        }
        ChangeSetting("MazeOfMalcor", mazeOfMalcor);
        
    }

    public void OnToggleAllSoldiers()
    {
        allSoldiers = allSoldiersToggle.isOn;
        ChangeSetting("AllSoldiers", allSoldiers);
    }
    public bool GetAllSoldiersState()
    {
        return allSoldiers;
    }

    public void SaveSettings()
    {
        ES3.Save("CampaignSettings", settings);
    }

    public void ChangeSetting(string settingType, bool state)
    {
        // Debug.Log("Change state " + settingType + ": " + state);
        settings[settingType] = state;
        SaveSettings();
    }

    


}
