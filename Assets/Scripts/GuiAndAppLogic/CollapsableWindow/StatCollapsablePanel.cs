using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatCollapsablePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moveStat;
    [SerializeField] TextMeshProUGUI fightStat;
    [SerializeField] TextMeshProUGUI shootStat;
    [SerializeField] TextMeshProUGUI armorStat;
    [SerializeField] TextMeshProUGUI willStat;
    [SerializeField] TextMeshProUGUI healthStat;


    public void UpdateStats( RuntimeSoldierData soldier)
    {
        moveStat.text = soldier.move.ToString();
        fightStat.text = soldier.fight.ToString();
        shootStat.text = soldier.shoot.ToString();
        armorStat.text = soldier.armor.ToString();
        willStat.text = soldier.will.ToString();
        healthStat.text = soldier.health.ToString();
    }

}
