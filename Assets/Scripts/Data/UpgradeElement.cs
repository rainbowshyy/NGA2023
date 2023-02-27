using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeElement", menuName = "Data/Create new Upgrade Element")]
public class UpgradeElement : ScriptableObject
{
    public CodeBlockTypes type;
    public int[] parameter1Range;
    public int[] parameter2Range;
    public int[] parameter3Range;
}
