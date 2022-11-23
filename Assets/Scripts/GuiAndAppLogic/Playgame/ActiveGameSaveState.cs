using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//currently this class does nothing
public class ActiveGameSaveState 
{
    public List<SoldierInfoSaveState> soldierStates = new List<SoldierInfoSaveState>();
}

//this also does nothing, added to soldiers as part of their data
public class SoldierInfoSaveState
{
    public List<StatusInfo> statusList = new List<StatusInfo>();
    public int soldierHealth;

}

//this is still used to track status effects
public class StatusInfo
{
    public string statusName;
    public string statusValue;
}
