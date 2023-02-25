using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] private List<Encounter> encounterList;

    private Dictionary<Stages, List<Encounter>> encounterMap;

    private List<Encounter> currentPool;

    public static EncounterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        encounterMap = new Dictionary<Stages, List<Encounter>>();
        foreach (Encounter e in encounterList)
        {
            if (!encounterMap.ContainsKey(e.stage))
            {
                encounterMap.Add(e.stage, new List<Encounter>());
            }
            encounterMap[e.stage].Add(e);
        }

        currentPool = new List<Encounter>();
    }

    public void PopulatePool(Stages stage)
    {
        currentPool.Clear();
        foreach (Encounter e in encounterMap[stage])
        {
            currentPool.Add(e);
        }
        if (stage != Stages.Intro)
        {
            for (int i = 0; i < currentPool.Count; i++)
            {
                Encounter temp = currentPool[i];
                int randomIndex = Random.Range(i, currentPool.Count);
                currentPool[i] = currentPool[randomIndex];
                currentPool[randomIndex] = temp;
            }
        }
    }

    public void NextInPool()
    {
        InitEncounter(currentPool[0]);
        currentPool.RemoveAt(0);
    }

    private void InitEncounter(Encounter encounter)
    {
        foreach (EncounterElement e in encounter.elements)
        {
            Vector2Int pos = e.positions[Mathf.FloorToInt(Random.Range(0, e.positions.Count))];
            SpawningManager.Instance.SpawnEnemy(e.type, pos);
        }
    }
}
