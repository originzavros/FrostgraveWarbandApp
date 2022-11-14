using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKOPopup : MonoBehaviour
{
    PlaymodeWindow currentSoldierWindow;
    [SerializeReference] PlayModeManager PlayModeManager;
    public void Init(PlaymodeWindow _playmodeWindow)
    {
        currentSoldierWindow = _playmodeWindow;
    }

    public void OnClickPlayerSlayedMonster()
    {
        PlayModeManager.SlayMonsterEvent(true, currentSoldierWindow);
        ClosePopup();
    }
    public void OnClickOtherSlayedMonster()
    {
        PlayModeManager.SlayMonsterEvent(false, currentSoldierWindow);
        ClosePopup();
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
