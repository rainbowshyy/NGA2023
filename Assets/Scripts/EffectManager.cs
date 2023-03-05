using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypes { energyUp, damageRange, damageLaser, powerRange, coordCheck, healthUp, healthRange, powerLaser}

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject energyUpPref;
    [SerializeField] private GameObject healthUpPref;
    [SerializeField] private GameObject healthRange1;
    [SerializeField] private GameObject damageRange1Pref;
    [SerializeField] private GameObject damageRange2Pref;
    [SerializeField] private GameObject damageLaserPref;
    [SerializeField] private GameObject powerRange1Pref;
    [SerializeField] private GameObject powerRange2Pref;
    [SerializeField] private GameObject coordPref;
    [SerializeField] private GameObject powerLaserPref;

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

    public void CreateEffect(EffectTypes type, Vector2Int pos, int[] parameters, Vector2Int point, bool isTrue)
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
            case EffectTypes.powerRange:
                PowerRange(newPos, parameters[1]);
                break;
            case EffectTypes.coordCheck:
                CoordCheck(pos, parameters[0], parameters[1], isTrue);
                break;
            case EffectTypes.healthUp:
                HealthUp(newPos);
                break;
            case EffectTypes.healthRange:
                HealthRange(newPos, parameters[0]);
                break;
            case EffectTypes.powerLaser:
                PowerRaycast(pos, parameters[0], parameters[1], point);
                break;

        }
    }

    private void EnergyUp(Vector3 pos)
    {
        Instantiate(energyUpPref, pos + Vector3.up * 0.9f, Quaternion.identity, effectParent);
    }

    private void HealthUp(Vector3 pos)
    {
        Instantiate(healthUpPref, pos + Vector3.up * 0.9f, Quaternion.identity, effectParent);
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
    private void PowerRange(Vector3 pos, int range)
    {
        if (range == 1)
        {
            Instantiate(powerRange1Pref, pos, Quaternion.identity, effectParent);
        }
        else if (range == 2)
        {
            Instantiate(powerRange2Pref, pos, Quaternion.identity, effectParent);
        }
    }
    private void HealthRange(Vector3 pos, int range)
    {
        if (range == 1)
        {
            Instantiate(healthRange1, pos, Quaternion.identity, effectParent);
        }
    }

    private void DamageRaycast(Vector2Int pos, int xDir, int yDir, Vector2Int point)
    {
        var angle = Mathf.Atan2(yDir, xDir) * Mathf.Rad2Deg;
        GameObject go = Instantiate(damageLaserPref, GridVisualizer.Instance.GetWorldPos(pos), Quaternion.AngleAxis(angle + 180f, Vector3.forward), effectParent);
        go.GetComponent<SpriteRenderer>().size = new Vector2(new Vector2(pos.x - point.x, pos.y - point.y).magnitude, 1.5f);
    }

    private void PowerRaycast(Vector2Int pos, int xDir, int yDir, Vector2Int point)
    {
        var angle = Mathf.Atan2(yDir, xDir) * Mathf.Rad2Deg;
        GameObject go = Instantiate(powerLaserPref, GridVisualizer.Instance.GetWorldPos(pos), Quaternion.AngleAxis(angle + 180f, Vector3.forward), effectParent);
        go.GetComponent<SpriteRenderer>().size = new Vector2(new Vector2(pos.x - point.x, pos.y - point.y).magnitude, 1.5f);
    }

    private void CoordCheck(Vector2Int pos, int xDir, int yDir, bool isTrue)
    {
        Vector2Int point = new Vector2Int(0, 0);
        if (xDir == 0)
        {
            point.x = pos.x;
        }
        else
        {
            point.x = 7;
        }
        if (yDir == 0)
        {
            point.y = pos.y;
        }
        else
        {
            point.y = -1;
        }
        var angle = Mathf.Atan2(yDir, xDir) * Mathf.Rad2Deg;
        GameObject go = Instantiate(coordPref, GridVisualizer.Instance.GetWorldPos(pos), Quaternion.AngleAxis(angle + 180f, Vector3.forward), effectParent);
        go.transform.position += go.transform.right * 0.5f;
        go.GetComponent<SpriteRenderer>().size = new Vector2(new Vector2(pos.x - point.x, pos.y - point.y).magnitude + 0.5f, 1f);
        go.GetComponent<Animator>().SetBool("X", xDir == 0);
        go.GetComponent<Animator>().SetBool("True", isTrue);
    }
}
