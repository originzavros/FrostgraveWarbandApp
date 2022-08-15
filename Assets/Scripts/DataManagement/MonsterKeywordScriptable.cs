using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for any ability a model might have, originally for monsters but the expansions add some special stuff also.
[CreateAssetMenu(fileName = "New MonsterKeyword", menuName = "Assets/New MonsterKeyword")]
public class MonsterKeywordScriptable : ScriptableObject
{
    public string keywordName;
    public string keywordDescription;
}
