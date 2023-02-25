using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject ifCodeBlockPref;

    [SerializeField] private GameObject upgradeCanvas;

    [SerializeField] private Transform[] options;

    [SerializeField] private List<Upgrade> upgradeList;
    [SerializeField] private Dictionary<Rarity, List<Upgrade>> upgradeMap;

    private List<CodeBlockStruct>[] optionRewards;

    public static UpgradeManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        upgradeMap = new Dictionary<Rarity, List<Upgrade>>();
        foreach (Upgrade u in upgradeList)
        {
            if (!upgradeMap.ContainsKey(u.rarity))
            {
                upgradeMap.Add(u.rarity, new List<Upgrade>());
            }
            upgradeMap[u.rarity].Add(u);
            upgradeMap[u.rarity].Add(u);
        }
    }

    private void OnEnable()
    {
        GameManager.onRoundWin += OpenUpgrades;
    }

    private void OnDisable()
    {
        GameManager.onRoundWin -= OpenUpgrades;
    }

    private void ClearUpgrades()
    {
        foreach (Transform t in options)
        {
            foreach (Transform tt in t)
            {
                Destroy(tt.gameObject);
            }
        }
    }

    private void RollUpgrades()
    {
        optionRewards = new List<CodeBlockStruct>[3] { new List<CodeBlockStruct>(), new List<CodeBlockStruct>(), new List<CodeBlockStruct>()};

        for (int i = 0; i < 3; i++)
        {
            //ADD WHEN I HAVE MORE UPGRADES :)
            //float rarity = Random.Range(0f, 1f);

            float rarity = 1f;

            if (rarity > 0.2f)
            {
                optionRewards[i] = RollOption(Rarity.Common);
            }
            else if (rarity > 0.04f)
            {
                optionRewards[i] = RollOption(Rarity.Uncommon);
            }
            else
            {
                optionRewards[i] = RollOption(Rarity.Rare);
            }
        }
    }

    private List<CodeBlockStruct> RollOption(Rarity rarity)
    {
        List<CodeBlockStruct> optionRolled = new List<CodeBlockStruct>();

        int roll = Mathf.FloorToInt(Random.Range(0f, upgradeMap[rarity].Count));
        Upgrade rolledUpgrade = upgradeMap[rarity][roll];
        upgradeMap[rarity].RemoveAt(roll);

        foreach (UpgradePoolElement u in rolledUpgrade.elements)
        {
            int codeIndex = Mathf.FloorToInt(Random.Range(0f, u.upgrades.Count));
            UpgradeElement e = u.upgrades[codeIndex];

            int[] parameters = new int[2];
            int index = Mathf.FloorToInt(Random.Range(0, e.parameter1Range.Length));
            parameters[0] = e.parameter1Range[index];
            index = Mathf.FloorToInt(Random.Range(0, e.parameter2Range.Length));
            parameters[1] = e.parameter2Range[index];

            optionRolled.Add(new CodeBlockStruct(e.type, parameters, null));
        }

        return optionRolled;
    }

    private void ShowUpgrades()
    {
        int count = 0;
        foreach (List<CodeBlockStruct> c in optionRewards)
        {
            foreach(CodeBlockStruct cs in c)
            {
                CodeBlock code = CodeBlockManager.GetCodeFromStruct(cs);
                GameObject go;
                if (code.isCondition)
                {
                    go = Instantiate(ifCodeBlockPref, options[count]);
                }
                else
                {
                    go = Instantiate(codeBlockPref, options[count]);
                }
                go.GetComponent<UIUpgradeElement>().SetText(code.ShowSyntax());
            }
            count += 1;
        }
    }

    private void OpenUpgrades()
    {
        ClearUpgrades();

        RollUpgrades();

        ShowUpgrades();

        ToggleUpgradeCanvas(true);
    }

    private void ToggleUpgradeCanvas(bool toggle)
    {
        upgradeCanvas.SetActive(toggle);
    }

    public void ChooseUpgrade(int index)
    {
        foreach (CodeBlockStruct c in optionRewards[index])
        {
            CodeBlock code = CodeBlockManager.GetCodeFromStruct(c);
            UIDataManager.Instance.CreateCodeBlock(code);
        }

        upgradeCanvas.SetActive(false);
        GameManager.onNewRound?.Invoke();
    }
}
