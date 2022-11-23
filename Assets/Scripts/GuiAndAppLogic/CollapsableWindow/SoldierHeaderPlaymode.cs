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
    private PlaymodeWindow parentWindow;

    public void UpdateInfo(RuntimeSoldierData soldier, PlaymodeWindow _parentWindow)
    {
        parentWindow = _parentWindow;
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
            parentWindow.UpdateChangedHp(currentHp);
        }
    }
    public void OnClickHPDown()
    {
        if(currentHp > 0)
        {
            currentHp--;
            soldierHP.text = currentHp.ToString();
            parentWindow.UpdateChangedHp(currentHp);
        }
    }

    public void SetUpForVaultPanel()
    {
        hptracker.SetActive(false);
        hpUpButton.SetActive(false);
        hpDownButton.SetActive(false);
    }

    public void SetCurrentHP(int _hp)
    {
        currentHp = _hp;
        soldierHP.text = currentHp.ToString();
    }

    

}
