using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierEscapePopup : MonoBehaviour
{
    
    PlaymodeWindow currentSoldierWindow;
    public void Init(PlaymodeWindow _playmodeWindow)
    {
        currentSoldierWindow = _playmodeWindow;
    }

    public void OnClickEscape()
    {
        SetSoldierStatusWindow(SoldierStatus.escaped);
    }
    public void OnClickEscapeWithTreasure()
    {
        SetSoldierStatusWindow(SoldierStatus.escapedWithTreasure);
    }
    public void OnClickKO()
    {
        SetSoldierStatusWindow(SoldierStatus.knockout);
    }

    public void SetSoldierStatusWindow(SoldierStatus ss)
    {
        currentSoldierWindow.GetStoredSoldier().status = ss;
        currentSoldierWindow.gameObject.SetActive(false);
        ClosePopup();
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
