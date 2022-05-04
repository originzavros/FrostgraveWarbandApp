using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaymodeWindow : MonoBehaviour
{
    [SerializeField] DescriptionPanel descriptionPanel;
    [SerializeField] EquipmentList equipmentList;
    [SerializeField] StatCollapsablePanel statCollapsablePanel;
    [SerializeField] SoldierHeaderPlaymode soldierHeader;
    [SerializeField] GameObject bodyContents;
    [SerializeField] GameObject rollDiceButton;
    [SerializeField] GameObject statusButton;
    [SerializeField] GameObject deathEscapeButton;
    [SerializeField] GameObject editButton;
    [SerializeField] GameObject conditionPrefab;

    private RuntimeSoldierData storedSoldier;

    public void UpdatePanelInfo(RuntimeSoldierData soldier)
    {
        storedSoldier = soldier;
        descriptionPanel.UpdateDescription(soldier);
        equipmentList.UpdateEquipment(soldier);
        statCollapsablePanel.UpdateStats(soldier);
        soldierHeader.UpdateInfo(soldier);
    }

    public RuntimeSoldierData GetStoredSoldier()
    {
        return storedSoldier;
    }

    public void AddItemToContents(GameObject go)
    {
        // go.AddComponent<LayoutElement>();
        go.transform.SetParent(bodyContents.transform);
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

    public void AddStatus(string statusType, string statusTN = "14")
    {
        GameObject newStatusObject = Instantiate(conditionPrefab);
        newStatusObject.GetComponent<PlaymodeCondition>().UpdateCondition(statusType, statusTN);
        AddItemToContents(newStatusObject);
    }
    public void UpdateSoldierName(string name)
    {
        storedSoldier.soldierName = name;
        soldierHeader.UpdateSoldierName(name);
    }

    public void SoldierIsRemovedFromBoard()
    {

    }

    



    // public void SetHireEvent(UnityEngine.Events.UnityAction call)
    // {
    //     hireButton.GetComponent<Button>().onClick.AddListener(call);
    // }
}
