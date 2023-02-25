using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCode", menuName = "Data/Create new Enemy Code")]
public class EnemyCode : ScriptableObject
{
    public agentType type;
    public List<CodeBlockStruct> code;
    public AnimatorController animatorController;
}
