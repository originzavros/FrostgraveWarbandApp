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
    [SerializeField] GameObject basicButtonPrefab;

    /*
        Should be able to hold multiple treasure groups for the cases of treasure selection
        has controls to select a group(deleting the others)
        should have an optional button for requesting a reroll (central)
        each treasure group holds runtime treasure info
        has internal list of treasure groups
        when requested, offers runtime treasure of first treasure group( the only one left that matters), 
            if people don't make a selection just take top treasure.
    */
    private PostGameManager postGameManager;

    public void Init(string treasureType, PostGameManager postGameManagerReference)
    {
        treasureTypeText.text = treasureType;
        postGameManager = postGameManagerReference;
    }

    private void AddItem(MagicItemScriptable itemScriptable, string groupName)
    {
        GameObject temp = Instantiate(itemButtonPrefab);
        temp.GetComponent<ItemButton>().Init(itemScriptable);
        temp.GetComponent<ItemButton>().SwitchCostToSell();
        temp.GetComponent<Button>().onClick.AddListener(delegate {InternalPopupCall(itemScriptable);});
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                temp.transform.SetParent(child.transform);
            }
        } 
    }

    public void AddItemGroup(string groupName, RuntimeTreasure runtimeTreasure, TreasureSelectGroupType groupType)
    {
        GameObject temp = Instantiate(itemGroupPrefab);
        temp.name = groupName;

        temp.AddComponent<RuntimeTreasure>();
        temp.GetComponent<RuntimeTreasure>().items = runtimeTreasure.items;
        temp.GetComponent<RuntimeTreasure>().goldAmount = runtimeTreasure.goldAmount;

        temp.transform.SetParent(bodyContents.transform);

        GameObject goldObject = Instantiate(basicButtonPrefab);
        goldObject.GetComponentInChildren<TextMeshProUGUI>().text = runtimeTreasure.goldAmount.ToString() + " Gold";
        goldObject.GetComponent<Image>().color = Color.yellow;
        goldObject.transform.SetParent(temp.transform);

        foreach(var item in runtimeTreasure.items)
        {
            AddItem(item, groupName);
        }

        if(groupType == TreasureSelectGroupType.central)
        {
            GameObject rerollButton = Instantiate(basicButtonPrefab);
            rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll Central Treasure";
            rerollButton.transform.SetParent(temp.transform);
            rerollButton.GetComponent<Button>().onClick.AddListener(delegate {RerollGroupEvent(groupName);});
        }
        if(groupType == TreasureSelectGroupType.secret)
        {
            GameObject selectButton = Instantiate(basicButtonPrefab);
            selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Select Treasure";
            selectButton.transform.SetParent(temp.transform);
            selectButton.GetComponent<Button>().onClick.AddListener(delegate {SelectGroupEvent(groupName);});
        }

        //FFD760
        

        
    }

    private void AddObjectToGroup(GameObject go, string groupName)
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
                RuntimeTreasure rt = child.GetComponent<RuntimeTreasure>();
                temp = rt.items;
                // foreach(Transform item in child.transform)
                // {
                //     if(TryGetComponent(out ItemButton itemButton)) //probably have blank gold objects that arent items
                //     {
                //         temp.Add(itemButton.GetItemReference());
                //     }
                   
                // }
            }
        }
        return temp;
    }

    // public void RemoveGroup(string groupName)
    // {
    //     foreach(Transform child in bodyContents.transform)
    //     {
    //         if(child.name == groupName)
    //         {
    //             Destroy(child.gameObject);
    //             break;
    //         }
    //     }
    // }

    //when selected, delete all other groups
    public void SelectGroupEvent(string groupName)
    {
        GameObject[] allChildrenForDestruction = new GameObject[bodyContents.transform.childCount];
        int track = 0;
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name != groupName)
            {
                allChildrenForDestruction[track] = child.gameObject;
                track++;
            }
        }

        foreach(var item in allChildrenForDestruction)
        {
            Destroy(item);
        }
        
        // for(int i = bodyContents.transform.childCount; i > 0; i--)
        // {
        //     Transform child = bodyContents.transform.GetChild(i);
        //     if(child.name != groupName)
        //     {
        //         Destroy(child);
        //     }
        // }
    }

    //when pressed, reset the group with a new treasure roll, don't add reroll button though
    public void RerollGroupEvent(string groupName)
    {
        // for(int i = bodyContents.transform.childCount; i > 0; i--)
        // {
        //     Transform child = bodyContents.transform.GetChild(i);
        //     if(child.name == groupName)
        //     {
        //         Destroy(child);
        //     }
        // }
        

        GameObject[] allChildrenForDestruction = new GameObject[bodyContents.transform.childCount];
        int track = 0;
        foreach(Transform child in bodyContents.transform)
        {
            if(child.name == groupName)
            {
                allChildrenForDestruction[track] = child.gameObject;
                track++;
            }
        }

        foreach(var item in allChildrenForDestruction)
        {
            Destroy(item);
        }

        AddItemGroup("Rerolled treasure",postGameManager.RollTreasure(), TreasureSelectGroupType.normal);
        
    }

    public void InternalPopupCall(MagicItemScriptable magicItem)
    {
        postGameManager.ItemDescriptionPopupEvent(magicItem);
    }

    public RuntimeTreasure GetFinalTreasure()
    {
        return bodyContents.transform.GetChild(0).GetComponent<RuntimeTreasure>();
    }
}

public enum TreasureSelectGroupType{
    normal,
    central,
    secret
}
