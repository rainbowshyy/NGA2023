using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIShopElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

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
            CodeBlock codeBlock = CodeBlockManager.GetCodeFromStruct(code.codeStruct);
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
        text.text = "Price: " + element.price + " <sprite index=10>";
    }
}
