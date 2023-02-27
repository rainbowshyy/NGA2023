using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stages { Intro, Intro2, Act1, Act2, Act3}

[System.Serializable]
public struct EncounterElement
{
    public agentType type;
    public List<Vector2Int> positions;
}

[CreateAssetMenu(fileName = "Encounter", menuName = "Data/Create new Encounter")]
public class Encounter : ScriptableObject
{
    public Stages stage;
    public List<EncounterElement> elements;
}
