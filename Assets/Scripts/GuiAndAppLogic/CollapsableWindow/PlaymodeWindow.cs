using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaymodeWindow : MonoBehaviour
{
    [SerializeField] DescriptionPanel descriptionPanel;
    [SerializeField] SoldierTypeTextPanel soldierTypeTextPanel;
    [SerializeField] EquipmentList equipmentList;
    [SerializeField] StatCollapsablePanel statCollapsablePanel;
    [SerializeField] SoldierHeaderPlaymode soldierHeader;
    [SerializeField] GameObject soldierControlsPanel;
    [SerializeField] GameObject bodyContents;
    [SerializeField] GameObject rollDiceButton;
    [SerializeField] GameObject statusButton;
    [SerializeField] GameObject deathEscapeButton;
    [SerializeField] GameObject editButton;
    [SerializeField] GameObject conditionPrefab;

    private RuntimeSoldierData storedSoldier;
    //private SoldierInfoSaveState saveState = null; //not used anymore, data is tied directly to runtimesoldierdata

    public void UpdatePanelInfo(RuntimeSoldierData soldier)
    {
        storedSoldier = soldier;
        soldierTypeTextPanel.UpdateDescription(soldier);
        descriptionPanel.UpdateDescription(soldier);
        equipmentList.UpdateEquipment(soldier);
        statCollapsablePanel.UpdateStats(soldier);
        soldierHeader.UpdateInfo(soldier, this);
    }

    //public void SetSoldierState(SoldierInfoSaveState sis)
    //{
    //    //saveState = sis;
    //}
    
    public RuntimeSoldierData GetStoredSoldier()
    {
        return storedSoldier;
    }

    //public SoldierInfoSaveState GetSoldierSaveState()
    //{
    //    //return saveState;
    //}

    public void AddItemToContents(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(bodyContents.transform);
        /*
            there doesn't seem to be a clear answer on how to get the window to resize properly
            it works if the pane is switched off and on, but I just need this window to be redrawn
            maybe it's the parent's vertical layout/content size fitter issue?
        */
        // this.GetComponent<ContentSizeFitter>().enabled = false;
        // this.GetComponent<ContentSizeFitter>().enabled = true;
        // LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);
        // this.gameObject.SetActive(false);
        // this.gameObject.SetActive(true);

        //StartCoroutine(RefreshWindow());
    }

    public void SetRollDiceEvent(UnityEngine.Events.UnityAction call)
    {
        rollDiceButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetStatusEvent(UnityEngine.Events.UnityAction call)
    {
        statusButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetDeathEscapeEvent(UnityEngine.Events.UnityAction call)
    {
        deathEscapeButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetEditEvent(UnityEngine.Events.UnityAction call)
    {
        editButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void AddStatus(string statusType, string statusTN = "14", bool newStatus = false)
    {
        StatusInfo si = new StatusInfo();
        si.statusName = statusType;
        si.statusValue = statusTN;
        
        if(newStatus)
        {
            storedSoldier.conditions.Add(si);
        }

        GameObject newStatusObject = Instantiate(conditionPrefab);
        newStatusObject.GetComponent<PlaymodeCondition>().UpdateCondition(si, this);
        AddItemToContents(newStatusObject);
    }

    public void RemoveStatus(StatusInfo si)
    {
        for(int i = 0; i < storedSoldier.conditions.Count; i++)
        {
            if (storedSoldier.conditions[i].statusName == si.statusName)
            {
                if (storedSoldier.conditions[i].statusValue == si.statusValue)
                {
                    storedSoldier.conditions.RemoveAt(i);
                    break;
                }
            }
        }
    }


    public void UpdateSoldierName(string name)
    {
        storedSoldier.soldierName = name;
        soldierHeader.UpdateSoldierName(name);
    }

    public void SoldierIsRemovedFromBoard()
    {

    }

    public void SetUpForVaultPanel()
    {
        descriptionPanel.gameObject.SetActive(false);
        equipmentList.gameObject.SetActive(false);
        statCollapsablePanel.gameObject.SetActive(false);
        soldierHeader.SetUpForVaultPanel();
        soldierControlsPanel.SetActive(false);
    }

    public void SetBodyPermaActive()
    {
        // bodyContents.SetActive(true);
        this.GetComponent<CollapsableWindow>().SetBodyPermaActive();

    }

    public void SetWindowToManageMode()
    {
        rollDiceButton.SetActive(false);
        statusButton.SetActive(false);
        deathEscapeButton.SetActive(false);
    }

    public void SetWindowToNoButtons()
    {
        rollDiceButton.SetActive(false);
        statusButton.SetActive(false);
        deathEscapeButton.SetActive(false);
        editButton.SetActive(false);
    }

    //was attempting to resolve the resize issue, this doesn't seem to work and just makes it not be drawn at all
    public IEnumerator RefreshWindow()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(.01f);
        this.gameObject.SetActive(true);
        yield return null;
    }

    public void SetCurrentHP(int _hp)
    {
        soldierHeader.SetCurrentHP(_hp);
    }

    public void UpdateChangedHp(int _hp)
    {
        storedSoldier.activeHealth = _hp;
    }





    // public void SetHireEvent(UnityEngine.Events.UnityAction call)
    // {
    //     hireButton.GetComponent<Button>().onClick.AddListener(call);
    // }
}
