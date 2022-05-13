using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TreasureSelectWindow : MonoBehaviour
{
    [SerializeField] GameObject bodyContents;

    [SerializeField] TextMeshProUGUI treasureTypeText;

    [SerializeField] GameObject itemButtonPrefab;
    [SerializeField] GameObject itemGroupPrefab;

    public void Init(string treasureType)
    {
        treasureTypeText.text = treasureType;
    }

    public void AddItem(MagicItemScriptable itemScriptable, string groupName, UnityEngine.Events.UnityAction descriptionPopupCall)
    {
        GameObject temp = Instantiate(itemButtonPrefab);
        temp.GetComponent<ItemButton>().Init(itemScriptable);
        temp.GetComponent<ItemButton>().SwitchCostToSell();
        temp.GetComponent<Button>().onClick.AddListener(descriptionPopupCall);
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                temp.transform.SetParent(child.transform);
            }
        } 
    }

    public void AddItemGroup(string groupName, RuntimeTreasure runtimeTreasure)
    {
        GameObject temp = Instantiate(itemGroupPrefab);
        temp.name = groupName;
        temp.AddComponent<RuntimeTreasure>();
        temp.GetComponent<RuntimeTreasure>().items = runtimeTreasure.items;
        temp.GetComponent<RuntimeTreasure>().goldAmount = runtimeTreasure.goldAmount;
        temp.transform.SetParent(bodyContents.transform);
    }

    public void AddObjectToGroup(GameObject go, string groupName)
    {
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                go.transform.SetParent(child.transform);
            }
        } 
    }

    public List<MagicItemScriptable> GetEachItemInGroup(string groupName)
    {
        List<MagicItemScriptable> temp = new List<MagicItemScriptable>();
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                foreach(Transform item in child.transform)
                {
                    if(TryGetComponent(out ItemButton itemButton)) //probably have blank gold objects that arent items
                    {
                        temp.Add(itemButton.GetItemReference());
                    }
                   
                }
            }
        }
        return temp;
    }

    public void RemoveGroup(string groupName)
    {
         foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }


}
