using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierHeaderPlaymode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soldierName;
    [SerializeField] TextMeshProUGUI soldierHP;
    [SerializeField] GameObject hptracker;
    [SerializeField] GameObject hpUpButton;
    [SerializeField] GameObject hpDownButton;

    private int maxHP = 10;
    private int currentHp;

    public void UpdateInfo(RuntimeSoldierData soldier)
    {
        soldierName.text = soldier.soldierName;
        maxHP = soldier.health;
        currentHp = maxHP;
        soldierHP.text = currentHp.ToString();
    }
    public void UpdateSoldierName(string name)
    {
        soldierName.text = name;
    }

    public int GetCurrentHP()
    {
        return currentHp;
    }

    public void OnClickHPUP()
    {
        if(currentHp < maxHP)
        {
            currentHp++;
            soldierHP.text = currentHp.ToString();
        }
    }
    public void OnClickHPDown()
    {
        if(currentHp > 0)
        {
            currentHp--;
            soldierHP.text = currentHp.ToString();
        }
    }

    public void SetUpForVaultPanel()
    {
        hptracker.SetActive(false);
        hpUpButton.SetActive(false);
        hpDownButton.SetActive(false);
    }
}
