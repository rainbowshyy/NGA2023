using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject conditionBlockPref;
    [SerializeField] private GameObject agentPref;

    public void Display(ShopElement element)
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        if (element is AgentShopElement agent)
        {
            GameObject go = Instantiate(agentPref, transform);
            go.GetComponent<UIAgentElement>().SetAnimator(agent.type);
        }
        else if (element is CodeShopElement code)
        {
            GameObject go;
            CodeBlock codeBlock = CodeBlockManager.GetCodeFromStruct(code.codeStruct, false);
            if (codeBlock.isCondition)
            {
                go = Instantiate(conditionBlockPref, transform);
            }
            else
            {
                go = Instantiate(codeBlockPref, transform);
            }
            go.GetComponent<UIUpgradeElement>().SetText(codeBlock.ShowSyntax());
            go.GetComponent<UIUpgradeElement>().SetToolTip(codeBlock.ToolTip());
        }
        text.text = element.price + " <sprite index=10>";

        switch (element.stage)
        {
            case Stages.Act1:
                image.color = ShopManager.Instance.rarityColors[0];
                break;
            case Stages.Act2:
                image.color = ShopManager.Instance.rarityColors[1];
                break;
            case Stages.Act3:
                image.color = ShopManager.Instance.rarityColors[2];
                break;
        }
    }
}
