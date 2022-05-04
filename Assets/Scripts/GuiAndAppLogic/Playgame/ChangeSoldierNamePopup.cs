using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeSoldierNamePopup : MonoBehaviour
{
    [SerializeField] TMP_InputField soldierNameInput;

    PlaymodeWindow currentSoldierWindow;
    public void Init(PlaymodeWindow _PlaymodeWindow)
    {
        currentSoldierWindow = _PlaymodeWindow;
        RandomMaleName();
    }

    public void OnClickSetName()
    {
        if(soldierNameInput.text != "")
        {
            currentSoldierWindow.UpdateSoldierName(soldierNameInput.text);
            ClosePopup();
        }
        
        
    }
    public void RandomMaleName()
    {
        soldierNameInput.text = NVJOBNameGen.GiveAName(3);
    }
    public void RandomFemaleName()
    {
        soldierNameInput.text = NVJOBNameGen.GiveAName(1);
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
