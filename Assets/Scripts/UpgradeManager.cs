using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject ifCodeBlockPref;

    [SerializeField] private GameObject upgradeCanvas;

    [SerializeField] private Transform[] options;

    private Rarity[] currentRarities;
    [SerializeField] private Color[] rarityColors;

    [SerializeField] private List<Upgrade> upgradeList;
    [SerializeField] private Dictionary<Rarity, List<Upgrade>> upgradeMap;

    private float rareOffset;

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

        FillUpgradePool();
        rareOffset = -0.05f;
        currentRarities = new Rarity[3];
    }

    private void OnEnable()
    {
        GameManager.onRoundEnd += OpenUpgrades;
    }

    private void OnDisable()
    {
        GameManager.onRoundEnd -= OpenUpgrades;
    }

    private void FillUpgradePool()
    {
        upgradeMap = new Dictionary<Rarity, List<Upgrade>>();
        foreach (Upgrade u in upgradeList)
        {
            if (!upgradeMap.ContainsKey(u.rarity))
            {
                upgradeMap.Add(u.rarity, new List<Upgrade>());
            }
            upgradeMap[u.rarity].Add(u);
        }
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
            if (GameManager.Instance.currentStage != Stages.Intro)
            {
                float rarity = Random.Range(0f, 1f); //Slay the spire rarity

                float[] rarityOdds = new float[2] { 0.4f, 0.03f };

                if (rareOffset < 0)
                {
                    rarityOdds[1] += rareOffset;
                    if (rarityOdds[1] < 0)
                    {
                        rarityOdds[0] += rarityOdds[1];
                    }
                }
                else if (rareOffset > 0)
                {
                    rarityOdds[0] += rareOffset;
                    rarityOdds[1] += rareOffset;
                }

                if (rarity > rarityOdds[0])
                {
                    optionRewards[i] = RollOption(Rarity.Common);
                    rareOffset += 0.01f;
                    ShowRarity(i, Rarity.Common);
                }
                else if (rarity > rarityOdds[1])
                {
                    optionRewards[i] = RollOption(Rarity.Uncommon);
                    ShowRarity(i, Rarity.Uncommon);
                }
                else
                {
                    optionRewards[i] = RollOption(Rarity.Rare);
                    rareOffset = -0.05f;
                    ShowRarity(i, Rarity.Rare);
                }
            }
            else
            {
                optionRewards[i] = RollOption(Rarity.Intro);
                ShowRarity(i, Rarity.Common);
            }
        }
    }

    private List<CodeBlockStruct> RollOption(Rarity rarity)
    {
        List<CodeBlockStruct> optionRolled = new List<CodeBlockStruct>();

        if (upgradeMap[rarity].Count == 0)
        {
            FillUpgradePool();
        }

        int roll = 0;
        if (rarity != Rarity.Intro)
        {
            roll = Mathf.FloorToInt(Random.Range(0f, upgradeMap[rarity].Count));
        }
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

            optionRolled.Add(new CodeBlockStruct(e.type, parameters, null, 0));
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

    private void ShowRarity(int index, Rarity rarity)
    {
        options[index].gameObject.GetComponent<Image>().color = rarityColors[((int)rarity)];
    }

    private void OpenUpgrades(bool win)
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
