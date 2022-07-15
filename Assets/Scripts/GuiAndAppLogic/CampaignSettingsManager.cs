using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignSettingsManager : MonoBehaviour
{
    [SerializeField] NavBox navBox;

    private bool mazeOfMalcor;
    private bool allSoldiers;

    private List<FrostgraveBook> enabledCampaigns = new List<FrostgraveBook>();

    public void Init()
    {
        // enabledCampaigns.Add(FrostgraveBook.Core);
    }

    public List<FrostgraveBook> GetEnabledCampaigns()
    {
        return enabledCampaigns;
    }

    public void OnClickBackButton()
    {
        navBox.OnClickNavHome();
    }

    public void OnToggleMazeOfMalcor(bool state)
    {
        mazeOfMalcor = state;
        if(mazeOfMalcor == true)
        {
            enabledCampaigns.Add(FrostgraveBook.TheMazeOfMalcor);
        }
        else{
            enabledCampaigns.Remove(FrostgraveBook.TheMazeOfMalcor);
        }
    }

    public void OnToggleAllSoldiers(bool state)
    {
        allSoldiers = state;
    }

    


}
