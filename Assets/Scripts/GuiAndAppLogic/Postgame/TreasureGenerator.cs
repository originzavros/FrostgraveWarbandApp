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
    private List<MagicItemScriptable> allTheMazeOfMalcor = new List<MagicItemScriptable>();
    private List<MagicItemScriptable> allTheMazeOfMalcorScrolls = new List<MagicItemScriptable>();


 
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
            else if(item.itemBook == FrostgraveBook.TheMazeOfMalcor)
            {
                if(item.itemType == MagicItemType.Scroll)
                {
                    allTheMazeOfMalcorScrolls.Add(item);
                }
                else{
                    if(item.itemType != MagicItemType.BaseResource)
                    {
                        allTheMazeOfMalcor.Add(item);
                    }
                }
            }
        }

    }

    public RuntimeTreasure GenerateTreasureCoreBook()
    {
        RuntimeTreasure generatedTreasure = new RuntimeTreasure();
        List<MagicItemRuntime> temp = new List<MagicItemRuntime>();
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

    public RuntimeTreasure GenerateTreasureCampaign(FrostgraveBook book)
    {
        RuntimeTreasure generatedTreasure = new RuntimeTreasure();
        generatedTreasure.goldAmount = 0;
        if(book == FrostgraveBook.TheMazeOfMalcor)
        {
            generatedTreasure.items.Add(ConvertTreasure(allTheMazeOfMalcor[Random.Range(0, allTheMazeOfMalcor.Count)]));
            generatedTreasure.items.Add(ConvertTreasure(allTheMazeOfMalcorScrolls[Random.Range(0, allTheMazeOfMalcorScrolls.Count)]));
        }
        return generatedTreasure;
    }
    
    public MagicItemRuntime ConvertTreasure(MagicItemScriptable mis)
    {
        MagicItemRuntime treasureItem = new MagicItemRuntime();
        treasureItem.Init(mis);
        return treasureItem;
    }

    private TreasureTableEntryScriptable GetRandomEntryInTreasureTable(TreasureTableScriptable table)
    {
        return table.treasureList[Random.Range(0, table.treasureList.Count)];
    }

    private List<MagicItemRuntime> GenerateTreasuresFromTableEntry(TreasureTableEntryScriptable entry)
    {
        List<MagicItemRuntime> temp = new List<MagicItemRuntime>();
        foreach(var item in entry.entryList)
        {
            temp.Add(GetTreasureBasedOnType(item));
        }
        return temp;
    }

    private MagicItemRuntime GetTreasureBasedOnType(MagicItemType itemType)
    {
        MagicItemRuntime treasureItem = new MagicItemRuntime();
        if(itemType == MagicItemType.LesserPotion)
        {
            treasureItem.Init(allLesserPotionsCoreBook[Random.Range(0, allLesserPotionsCoreBook.Count)]);
        }
        else if(itemType == MagicItemType.GreaterPotion)
        {
            treasureItem.Init(allGreaterPotionsCoreBook[Random.Range(0, allGreaterPotionsCoreBook.Count)]);
        }
        else if(itemType == MagicItemType.MagicEquipment)
        {
            treasureItem.Init(allMagicEquipmentCoreBook[Random.Range(0, allMagicEquipmentCoreBook.Count)]);
        }
        else if(itemType == MagicItemType.Artifact)
        {
            treasureItem.Init(allArtifactsCoreBook[Random.Range(0, allArtifactsCoreBook.Count)]);
        }
        else if(itemType == MagicItemType.Scroll)
        {
            treasureItem.Init(allScrollsCoreBook[Random.Range(0, allScrollsCoreBook.Count)]);
        }
        else if(itemType == MagicItemType.Grimoire)
        {
            treasureItem.Init(allGrimoiresCoreBook[Random.Range(0, allGrimoiresCoreBook.Count)]);
        }
        else{
            Debug.Log("Somehow got null when getting treasure by type");
            return null;
        }
        return treasureItem;
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
    public List<MagicItemRuntime> items = new List<MagicItemRuntime>();
    public int goldAmount = 0;
}


