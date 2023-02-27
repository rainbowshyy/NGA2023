using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject healthParticlePref;

    private void OnEnable()
    {
        GameManager.onNewHealth += SetHealth;
    }

    private void OnDisable()
    {
        GameManager.onNewHealth -= SetHealth;
    }

    private void SetHealth(int health)
    {
        int count = 0;
        foreach (Transform t in parent)
        {
            if (count > health - 1)
            {
                if (t.gameObject.GetComponent<CanvasGroup>().alpha == 1f)
                {
                    Instantiate(healthParticlePref, t.position, Quaternion.identity);
                }
                t.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            }
            else
            {
                t.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            }
            count++;
        }
    }
}
