using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfRound : MonoBehaviour
{
    public static EndOfRound Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.onRoundEnd += RoundEnd;
    }

    private void OnDisable()
    {
        GameManager.onRoundEnd -= RoundEnd;
    }

    private void RoundEnd(bool win)
    {
        StartCoroutine(EndRoundAnim());
    }

    IEnumerator EndRoundAnim()
    {
        yield return new WaitForSeconds(1f);
        GameManager.onNewRound?.Invoke();
    }
}
