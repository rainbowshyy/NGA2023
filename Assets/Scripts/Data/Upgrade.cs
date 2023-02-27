using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Rare, Intro}

[System.Serializable]
public struct UpgradePoolElement
{
    public List<UpgradeElement> upgrades;
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Data/Create new Upgrade")]
public class Upgrade : ScriptableObject
{
    public Rarity rarity;
    public List<UpgradePoolElement> elements;
}
