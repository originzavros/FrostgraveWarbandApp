using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class easySpreadsheetImport : MonoBehaviour
{
    #if UNITY_EDITOR
    [Button]
    public void GenerateSpells()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/Spell List - Sheet1.csv");
        
        for(int row=1;row<sheet.RowCount;row++)
        {
            SpellScriptable ss = ScriptableObject.CreateInstance<SpellScriptable>();
            ss.Name = sheet.GetCell<string>(0, row);
            ss.CastingNumber = sheet.GetCell<int>(1,row);
            ss.bookEdition = (FrostgraveBook)System.Enum.Parse(typeof(FrostgraveBook),sheet.GetCell<string>(2,row));
            ss.School = (WizardSchools)System.Enum.Parse(typeof(WizardSchools),sheet.GetCell<string>(3,row));
            ss.Restriction = sheet.GetCell<string>(4,row);
            ss.Description = sheet.GetCell<string>(5,row);

            AssetDatabase.CreateAsset(ss, $"Assets/Resources/SpellsScriptables/{ss.Name}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }

    [Button]
    public void GenerateStandardEquipment()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/StandardEquipment - Sheet1.csv");
        
        for(int row=1;row<sheet.RowCount;row++)
        {
            EquipmentScriptable es = ScriptableObject.CreateInstance<EquipmentScriptable>();
            es.equipmentName = sheet.GetCell<string>(0, row);
            es.bookEdition = (FrostgraveBook)System.Enum.Parse(typeof(FrostgraveBook),sheet.GetCell<string>(1,row));
            es.equipmentDescription = sheet.GetCell<string>(2,row);
            

            AssetDatabase.CreateAsset(es, $"Assets/Resources/StandardEquipment/{es.equipmentName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }
    [Button]
    public void GenerateAllMagicItems()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/MagicItems - Sheet1.csv");
        
        for(int row=1;row<sheet.RowCount;row++)
        {
            MagicItemScriptable es = ScriptableObject.CreateInstance<MagicItemScriptable>();
            es.itemName = sheet.GetCell<string>(0, row);
            es.itemBook = (FrostgraveBook)System.Enum.Parse(typeof(FrostgraveBook),sheet.GetCell<string>(1,row));
            es.itemType = (MagicItemType)System.Enum.Parse(typeof(MagicItemType),sheet.GetCell<string>(2,row));
            es.itemDescription = sheet.GetCell<string>(3,row);
            es.itemPurchasePrice = sheet.GetCell<int>(4,row);
            es.itemSalePrice = sheet.GetCell<int>(5,row);
            

            AssetDatabase.CreateAsset(es, $"Assets/Resources/ItemScriptables/{es.itemName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }

    [Button]
    public void GenerateHireSoldierScriptables()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/SoldierList - Sheet1.csv");

        for(int row=1;row<sheet.RowCount;row++)
        {
            SoldierScriptable ss = ScriptableObject.CreateInstance<SoldierScriptable>();
            ss.hiringName = sheet.GetCell<string>(0, row);
            ss.move = sheet.GetCell<int>(1, row);
            ss.fight = sheet.GetCell<int>(2, row);
            ss.shoot = sheet.GetCell<int>(3, row);
            ss.armor = sheet.GetCell<int>(4, row);
            ss.will = sheet.GetCell<int>(5, row);
            ss.health = sheet.GetCell<int>(6, row);
            ss.cost = sheet.GetCell<int>(7, row);
            ss.baseSoldierEquipment = ParseEquipment(sheet.GetCell<string>(8,row));
            ss.description = sheet.GetCell<string>(9, row);
            ss.soldierType = sheet.GetCell<string>(11, row);
            ss.bookEdition = (FrostgraveBook)System.Enum.Parse(typeof(FrostgraveBook),sheet.GetCell<string>(12,row));
          
            AssetDatabase.CreateAsset(ss, $"Assets/Resources/SoldierScriptables/{ss.hiringName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }

    public List<EquipmentScriptable> ParseEquipment(string equipstring)
    {
        EquipmentScriptable[] allEquipmentObjects = Resources.LoadAll<EquipmentScriptable>("StandardEquipment");
        List<EquipmentScriptable> temp = new List<EquipmentScriptable>();
        string[] words = equipstring.Split('.');

        foreach(var item in words)
        {
            foreach(EquipmentScriptable premadeEquipment in allEquipmentObjects)
            {
                if(item == premadeEquipment.equipmentName)
                {
                    temp.Add(premadeEquipment);
                    break;
                }
            }
        }
        return temp;
    }

    [Button]
    public void GenerateMonsters()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/Monsters - Sheet1.csv");

        for(int row=1;row<sheet.RowCount;row++)
        {
            MonsterScriptable ss = ScriptableObject.CreateInstance<MonsterScriptable>();
            ss.hiringName = sheet.GetCell<string>(0, row);
            ss.soldierName = sheet.GetCell<string>(0, row);
            ss.move = sheet.GetCell<int>(1, row);
            ss.fight = sheet.GetCell<int>(2, row);
            ss.shoot = sheet.GetCell<int>(3, row);
            ss.armor = sheet.GetCell<int>(4, row);
            ss.will = sheet.GetCell<int>(5, row);
            ss.health = sheet.GetCell<int>(6, row);
            ss.cost = sheet.GetCell<int>(7, row);
            // ss.baseSoldierEquipment = ParseEquipment(sheet.GetCell<string>(8,row));
            ss.description = sheet.GetCell<string>(9, row);
            ss.monsterKeywordList = ParseMonsterKeywords(sheet.GetCell<string>(10,row));
            ss.soldierType = sheet.GetCell<string>(11, row);
            ss.bookEdition = (FrostgraveBook)System.Enum.Parse(typeof(FrostgraveBook),sheet.GetCell<string>(12,row));



          
            AssetDatabase.CreateAsset(ss, $"Assets/Resources/MonsterScriptables/{ss.hiringName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }

    [Button]
    public void GenerateMonsterKeywords()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/MonsterKeywords - Sheet1.csv");

        for(int row=1;row<sheet.RowCount;row++)
        {
            MonsterKeywordScriptable ss = ScriptableObject.CreateInstance<MonsterKeywordScriptable>();
            ss.keywordName = sheet.GetCell<string>(0, row);
            ss.keywordDescription = sheet.GetCell<string>(1, row);

          
            AssetDatabase.CreateAsset(ss, $"Assets/Resources/MonsterKeywordScriptables/{ss.keywordName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }

    public List<MonsterKeywordScriptable> ParseMonsterKeywords(string keywordstring)
    {
        MonsterKeywordScriptable[] allMonsterKeywordObjects = Resources.LoadAll<MonsterKeywordScriptable>("MonsterKeywordScriptables");
        List<MonsterKeywordScriptable> temp = new List<MonsterKeywordScriptable>();
        string[] words = keywordstring.Split('.');

        foreach(var item in words)
        {
            foreach(MonsterKeywordScriptable mks in allMonsterKeywordObjects)
            {
                if(item == mks.keywordName)
                {
                    temp.Add(mks);
                    break;
                }
            }
        }
        return temp;
    }
    #endif
}
