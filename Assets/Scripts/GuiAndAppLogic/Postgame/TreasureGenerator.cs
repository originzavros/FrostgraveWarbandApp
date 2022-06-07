using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class TreasureGenerator : MonoBehaviour
{


    [BoxGroup("CoreBookTreasureTables")][SerializeField] TreasureTableScriptable MainTreasureTable;

    private List<MagicItemScriptable> allLesserPotionsCoreBook = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allGreaterPotionsCoreBook = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allMagicEquipmentCoreBook = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allArtifactsCoreBook = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allScrollsCoreBook = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allGrimoiresCoreBook = new List<MagicItemScriptable>();


 
    //generate lists so we don't have to do it later
    public void Init()
    {
        foreach(var item in LoadAssets.allMagicItemObjects)
        {
            if(item.itemType == MagicItemType.LesserPotion && item.itemBook == FrostgraveBook.Core)
            {
                allLesserPotionsCoreBook.Add(item);
            }
            else if(item.itemType == MagicItemType.GreaterPotion && item.itemBook == FrostgraveBook.Core)
            {
                allGreaterPotionsCoreBook.Add(item);
            }
            else if(item.itemType == MagicItemType.MagicEquipment && item.itemBook == FrostgraveBook.Core)
            {
                allMagicEquipmentCoreBook.Add(item);
            }
            else if(item.itemType == MagicItemType.Artifact && item.itemBook == FrostgraveBook.Core)
            {
                allArtifactsCoreBook.Add(item);
            }
            else if(item.itemType == MagicItemType.Scroll && item.itemBook == FrostgraveBook.Core)
            {
                allScrollsCoreBook.Add(item);
            }
            else if(item.itemType == MagicItemType.Grimoire && item.itemBook == FrostgraveBook.Core)
            {
                allGrimoiresCoreBook.Add(item);
            }
        }

    }

    public RuntimeTreasure GenerateTreasureCoreBook()
    {
        RuntimeTreasure generatedTreasure = new RuntimeTreasure();
        List<MagicItemScriptable> temp = new List<MagicItemScriptable>();
        TreasureTableEntryScriptable randomEntry = GetRandomEntryInTreasureTable(MainTreasureTable);
        if(randomEntry.variableGold)
        {
            generatedTreasure.goldAmount = randomEntry.variableGoldAmount * SpellRoller.RollDice();
        }
        else{
            generatedTreasure.goldAmount = randomEntry.goldAmount;
        }
        

        foreach(var item in randomEntry.entryList)
        {
            if(item == MagicItemType.LesserPotion || item == MagicItemType.GreaterPotion) //potions have a secondary table
            {
                int result = SpellRoller.RollDice();
                if(result > 18)
                {
                    temp.Add(GetTreasureBasedOnType(MagicItemType.GreaterPotion));
                }
                else{
                    temp.Add(GetTreasureBasedOnType(MagicItemType.LesserPotion));
                }
            }
            else{
                temp.Add(GetTreasureBasedOnType(item));
            }
        }
        generatedTreasure.items = temp;
        return generatedTreasure;
    }

    private TreasureTableEntryScriptable GetRandomEntryInTreasureTable(TreasureTableScriptable table)
    {
        return table.treasureList[Random.Range(0, table.treasureList.Count)];
    }

    private List<MagicItemScriptable> GenerateTreasuresFromTableEntry(TreasureTableEntryScriptable entry)
    {
        List<MagicItemScriptable> temp = new List<MagicItemScriptable>();
        foreach(var item in entry.entryList)
        {
            temp.Add(GetTreasureBasedOnType(item));
        }
        return temp;
    }

    private MagicItemScriptable GetTreasureBasedOnType(MagicItemType itemType)
    {
        
        if(itemType == MagicItemType.LesserPotion)
        {
            return allLesserPotionsCoreBook[Random.Range(0, allLesserPotionsCoreBook.Count)];
        }
        else if(itemType == MagicItemType.GreaterPotion)
        {
            return allGreaterPotionsCoreBook[Random.Range(0, allGreaterPotionsCoreBook.Count)];
        }
        else if(itemType == MagicItemType.MagicEquipment)
        {
            return allMagicEquipmentCoreBook[Random.Range(0, allMagicEquipmentCoreBook.Count)];
        }
        else if(itemType == MagicItemType.Artifact)
        {
            return allArtifactsCoreBook[Random.Range(0, allArtifactsCoreBook.Count)];
        }
        else if(itemType == MagicItemType.Scroll)
        {
            return allScrollsCoreBook[Random.Range(0, allScrollsCoreBook.Count)];
        }
        else if(itemType == MagicItemType.Grimoire)
        {
            return allGrimoiresCoreBook[Random.Range(0, allGrimoiresCoreBook.Count)];
        }
        else{
            Debug.Log("Somehow got null when getting treasure by type");
            return null;
        }
    }

    public RuntimeTreasure GetRandomScroll()
    {
        RuntimeTreasure generatedTreasure = new RuntimeTreasure();
        generatedTreasure.items.Add(GetTreasureBasedOnType(MagicItemType.Scroll));
        return generatedTreasure;
    }

    public RuntimeTreasure GetRandomGrimoire()
    {
        RuntimeTreasure generatedTreasure = new RuntimeTreasure();
        generatedTreasure.items.Add(GetTreasureBasedOnType(MagicItemType.Grimoire));
        return generatedTreasure;
    }

}

//meant to temporarily hold treasure, its a mono so we can attach it to gui stuff temporarily for reference
public class RuntimeTreasure : MonoBehaviour
{
    public List<MagicItemScriptable> items = new List<MagicItemScriptable>();
    public int goldAmount = 0;
}


