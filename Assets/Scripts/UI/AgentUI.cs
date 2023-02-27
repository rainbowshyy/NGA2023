using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentUI : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform rectTf;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Animator[] animator;

    private void Awake()
    {
        rectTf = GetComponent<RectTransform>();
    }

    public void SetPositionFromWorld(Vector3 pos)
    {
        rectTf.anchoredPosition = pos * canvas.scaleFactor * 64f + Vector3.down * 32f;
    }

    public void SetHPText(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void SetEnergyText(int energy)
    {
        energyText.text = energy.ToString();
    }

    public void EnergyAnimation()
    {
        animator[1].ResetTrigger("Activate");
        animator[1].SetTrigger("Activate");
    }
    public void HealthAnimation()
    {
        animator[0].ResetTrigger("Activate");
        animator[0].SetTrigger("Activate");
    }
}
