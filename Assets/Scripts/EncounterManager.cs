using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    private Dictionary<Stages, List<Encounter>> encounterMap;

    public List<Encounter> currentPool;

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
        foreach (Encounter e in Resources.LoadAll<Encounter>("Encounters/"))
        {
            if (!encounterMap.ContainsKey(e.stage))
            {
                encounterMap.Add(e.stage, new List<Encounter>());
            }
            encounterMap[e.stage].Add(e);
        }

        currentPool = new List<Encounter>();
    }

    public void PopulatePool(int[] count)
    {
        foreach (Stages s in System.Enum.GetValues(typeof(Stages)))
        {
            List<Encounter> rolledPool = new List<Encounter>();

            foreach (Encounter e in encounterMap[s])
            {
                rolledPool.Add(e);
            }

            if (s != Stages.Intro && s != Stages.Intro2)
            {
                for (int i = 0; i < rolledPool.Count; i++)
                {
                    Encounter temp = rolledPool[i];
                    int randomIndex = Random.Range(i, rolledPool.Count);
                    rolledPool[i] = rolledPool[randomIndex];
                    rolledPool[randomIndex] = temp;
                }

                rolledPool.RemoveRange(count[(int)s - 2], rolledPool.Count - count[(int)s - 2]);
            }
            currentPool.AddRange(rolledPool);
        }
    }

    public bool NextInPool()
    {
        if (currentPool.Count == 0)
            return false;

        InitEncounter(currentPool[0]);
        currentPool.RemoveAt(0);
        return true;
    }

    private void InitEncounter(Encounter encounter)
    {
        AudioManager.onAudioEvent?.Invoke(audioEvent.EnemyIntensitySet, 0);
        if (encounter.stage != GameManager.Instance.currentStage)
        {
            GameManager.onNewStage?.Invoke(encounter.stage);
        }
        foreach (EncounterElement e in encounter.elements)
        {
            Vector2Int pos = e.positions[Mathf.FloorToInt(Random.Range(0, e.positions.Count))];
            if (e.type == agentType.Wall)
            {
                SpawningManager.Instance.SpawnWall(pos);
            }
            else
            {
                SpawningManager.Instance.SpawnEnemy(e.type, pos);
                AudioManager.onAudioEvent?.Invoke(audioEvent.EnemyIntensity, AgentManager.Instance.AgentMusicIntensityMap[e.type]);
            }
        }
    }
}
