using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class GeneratePirateOathStuff : MonoBehaviour
{
    [SerializeField] AbilityPanel panel1;
    [SerializeField] AbilityPanel panel2;

    private CrewAbilityScriptable[] allCrewAbilities;

    #if UNITY_EDITOR
    [Button] 
    public void GenerateAbilityScriptables()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("C:/Users/Nicholas Alaniz/Downloads/Crew Upgrades - Sheet1.csv");
        
        for(int row=1;row<sheet.RowCount;row++)
        {
            CrewAbilityScriptable ss = CrewAbilityScriptable.CreateInstance<CrewAbilityScriptable>();
            ss.abilityName = sheet.GetCell<string>(0,row);
            ss.abilityDescription = sheet.GetCell<string>(1,row);

            AssetDatabase.CreateAsset(ss, $"Assets/Resources/CrewAbilityScriptables/{ss.abilityName}.asset"); 
        }
        AssetDatabase.SaveAssets();
    }
    #endif

    void Start()
    {
        allCrewAbilities = Resources.LoadAll<CrewAbilityScriptable>("CrewAbilityScriptables");
    }

    public void updatePanels()
    {
        List<int> temp = new List<int>();
        for(int x = 0; x < allCrewAbilities.Length; x++)
        {
            temp.Add(x);
        }
        int remove = Random.Range(0, temp.Count);
        int first = temp[remove];
        temp.RemoveAt(first);
        remove = Random.Range(0, temp.Count);
        int second = temp[remove];

        panel1.UpdateInfo(allCrewAbilities[first]);
        panel2.UpdateInfo(allCrewAbilities[second]);
    }
}
