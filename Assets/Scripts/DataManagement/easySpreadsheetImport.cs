using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class easySpreadsheetImport : MonoBehaviour
{
    
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
}
