using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LoadAssets : MonoBehaviour
{
    [SerializeField] TreasureGenerator treasureGenerator;
    public static SpellScriptable[] spellObjects;
    //public static string[] wizardSchoolsForDisplay;
    public static string[] spellRangeTypeForDisplay;

    public static WizardSchoolScriptable[] wizardSchoolObjects;
    public static EquipmentScriptable[] allEquipmentObjects;
    public static SoldierScriptable[] allSoldierObjects;
    public static List<string> warbandNames;
    public static MonsterScriptable[] allMonsterObjects;
    public static MonsterKeywordScriptable[] allMonsterKeywordObjects;
    public static MagicItemScriptable[] allMagicItemObjects;

    void Start()
    {
        
    }

    void Awake()
    {
        spellObjects = Resources.LoadAll<SpellScriptable>("SpellsScriptables");
        wizardSchoolObjects = Resources.LoadAll<WizardSchoolScriptable>("WizardSchools");
        allEquipmentObjects = Resources.LoadAll<EquipmentScriptable>("StandardEquipment");
        allSoldierObjects = Resources.LoadAll<SoldierScriptable>("SoldierScriptables");
        allMonsterObjects =  Resources.LoadAll<MonsterScriptable>("MonsterScriptables");
        allMonsterKeywordObjects = Resources.LoadAll<MonsterKeywordScriptable>("MonsterKeywordScriptables");
        allMagicItemObjects = Resources.LoadAll<MagicItemScriptable>("ItemScriptables");



        warbandNames = ES3.Load("warbandNames", new List<string>());

        foreach(var item in warbandNames)
        {
            Debug.Log("warband name: " + item);
        }

        // wizardSchoolsForDisplay = new string[10];
        // for(int x=0;x<10;x++)
        // {
        //     WizardSchools tempEnum = (WizardSchools)x;
        //     wizardSchoolsForDisplay[x] = tempEnum.ToString();
        //     // Debug.Log("adding to options: " + tempEnum.ToString());
        // }

        spellRangeTypeForDisplay = new string[]{"Line of Sight", "Touch", "Self Only", "Area Effect", "Out of Game(A)", "Out of Game(B)", "Out of Game(B) OR Touch"};

        treasureGenerator.Init();
    }

    public static void LoadWarbandNames()
    {
        warbandNames = ES3.Load("warbandNames", new List<string>());
    }


}




