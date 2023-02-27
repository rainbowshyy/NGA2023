using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypes { energyUp, damageRange, damageLaser}

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject energyUpPref;
    [SerializeField] private GameObject damageRange1Pref;
    [SerializeField] private GameObject damageRange2Pref;
    [SerializeField] private GameObject damageLaserPref;

    [SerializeField] private Transform effectParent;

    public static EffectManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreateEffect(EffectTypes type, Vector2Int pos, int[] parameters, Vector2Int point)
    {
        Vector3 newPos = GridVisualizer.Instance.GetWorldPos(pos);

        switch(type)
        {
            case EffectTypes.energyUp:
                EnergyUp(newPos);
                break;
            case EffectTypes.damageRange:
                DamageRange(newPos, parameters[0]);
                break;
            case EffectTypes.damageLaser:
                DamageRaycast(pos, parameters[0], parameters[1], point);
                break;
        }
    }

    private void EnergyUp(Vector3 pos)
    {
        Instantiate(energyUpPref, pos + Vector3.up * 0.9f, Quaternion.identity, effectParent);
    }

    private void DamageRange(Vector3 pos, int range)
    {
        if (range == 1)
        {
            Instantiate(damageRange1Pref, pos, Quaternion.identity, effectParent);
        }
        else if (range == 2)
        {
            Instantiate(damageRange2Pref, pos, Quaternion.identity, effectParent);
        }
    }

    private void DamageRaycast(Vector2Int pos, int xDir, int yDir, Vector2Int point)
    {
        var angle = Mathf.Atan2(yDir, xDir) * Mathf.Rad2Deg;
        GameObject go = Instantiate(damageLaserPref, GridVisualizer.Instance.GetWorldPos(pos), Quaternion.AngleAxis(angle + 180f, Vector3.forward), effectParent);
        go.GetComponent<SpriteRenderer>().size = new Vector2(new Vector2(pos.x - point.x, pos.y - point.y).magnitude, 1.5f);
    }
}
