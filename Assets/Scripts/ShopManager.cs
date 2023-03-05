using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private UIShopElement[] slots;
    public Color[] rarityColors;

    private Dictionary<shopType, Dictionary<Stages, List<ShopElement>>> shopMap;

    private List<ShopElement> shop;

    public static Action<CodeBlock> onCodePickup;
    public static ShopManager Instance { get; private set; }

    private void OnEnable()
    {
        GameManager.onNewRound += DoShop;
        DragDropManager.onBeginDrag += HideElements;
        DragDropManager.onStopDrag += ShowElements;
    }

    private void OnDisable()
    {
        GameManager.onNewRound -= DoShop;
        DragDropManager.onBeginDrag -= HideElements;
        DragDropManager.onStopDrag -= ShowElements;
    }

    private void HideElements()
    {
        foreach (UIShopElement element in slots) 
        { 
            element.gameObject.SetActive(false);
        }
    }

    private void ShowElements()
    {
        foreach (UIShopElement element in slots)
        {
            element.gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        shopMap = new Dictionary<shopType, Dictionary<Stages, List<ShopElement>>>();
        foreach (ShopElement element in Resources.LoadAll<ShopElement>("ShopElements/"))
        {
            if (!shopMap.ContainsKey(element.shopType))
            {
                shopMap.Add(element.shopType, new Dictionary<Stages, List<ShopElement>>());
            }
            if (!shopMap[element.shopType].ContainsKey(element.stage))
            {
                shopMap[element.shopType].Add(element.stage, new List<ShopElement>());
            }
            shopMap[element.shopType][element.stage].Add(element);
        }
    }

    private void RollShop()
    {
        shop = new List<ShopElement>();

        List<shopType> typesRolled = RollShopTypes(false);
        foreach (shopType type in typesRolled)
        {
            shop.Add(RollShopElement(type));
        }
    }

    private List<shopType> RollShopTypes(bool noAgent)
    {
        List<shopType> types = new List<shopType>();
        foreach(shopType type in Enum.GetValues(typeof(shopType)))
        {
            if (!noAgent || type != shopType.Agent)
            {
                types.Add(type);
                if (type == shopType.Condition)
                {
                    types.Add(type);
                }
            }
        }

        while (types.Count > 4)
        {
            types.RemoveAt(Mathf.FloorToInt(UnityEngine.Random.Range(0, types.Count)));
        }

        for (int i = 0; i < types.Count; i++)
        {
            shopType temp = types[i];
            int randomIndex = UnityEngine.Random.Range(i, types.Count);
            types[i] = types[randomIndex];
            types[randomIndex] = temp;
        }

        return types;
    }

    private ShopElement RollShopElement(shopType type)
    {
        ShopElement element = null;
        Stages stageRolled;

        float roll = UnityEngine.Random.Range(0f, 1f);
        float[] odds = new float[2];
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Act3:
                odds[0] = 0.75f;
                odds[1] = 0.25f;
                break;
            case Stages.Act2:
                odds[0] = 0.5f;
                odds[1] = 0.1f;
                break;
            case Stages.Act1:
                odds[0] = 0.2f;
                odds[1] = 0.01f;
                break;
            case Stages.Intro2:
                odds[0] = 0.1f;
                odds[1] = 0f;
                break;
            case Stages.Intro:
                odds[0] = 0.01f;
                odds[1] = 0f;
                break;
        }

        if (roll < odds[1])
        {
            stageRolled = Stages.Act3;
        }
        else if (roll < odds[0])
        {
            stageRolled = Stages.Act2;
        }
        else
        {
            stageRolled = Stages.Act1;
        }

        element = GetRandomShopElement(type, stageRolled);

        if (element is CodeShopElement code)
        {
            Vector2Int parameters = code.parameters[Mathf.FloorToInt(UnityEngine.Random.Range(0, code.parameters.Length))];
            if (code.extraParameters != null && code.extraParameters.Length > 0)
            {
                Vector2Int extraParameters = code.extraParameters[Mathf.FloorToInt(UnityEngine.Random.Range(0, code.extraParameters.Length))];
                code.codeStruct = new CodeBlockStruct(code.codeBlockTypes, new int[4] { parameters.x, parameters.y, extraParameters.x, extraParameters.y }, null, code.price);
            }
            else
            {
                code.codeStruct = new CodeBlockStruct(code.codeBlockTypes, new int[2] { parameters.x, parameters.y }, null, code.price);
            }
        }

        return element;
    }

    private ShopElement GetRandomShopElement(shopType type, Stages stage)
    {
        ShopElement element = null;
        List<ShopElement> pool = shopMap[type][stage];
        bool rolled = false;
        while (!rolled)
        {
            if (pool.Count == 0)
            {
                List<shopType> newTypesPool = RollShopTypes(true);
                shopType newType = newTypesPool[Mathf.FloorToInt(UnityEngine.Random.Range(0f, newTypesPool.Count))];
                element = GetRandomShopElement(newType, stage);
                rolled = true;
            }
            else
            {
                int roll = Mathf.FloorToInt(UnityEngine.Random.Range(0f, pool.Count));
                element = pool[roll];
                rolled = true;
            }
        }

        return element;
    }

    private void ShowShop()
    {
        for (int i = 0; i < 4; i++)
        {
            slots[i].Display(shop[i]);
        }
    }

    private void DoShop()
    {
        RollShop();
        ShowShop();
    }

    public void Reroll()
    {
        if (GameManager.Instance.EnoughGold(1))
        {
            DoShop();
            GameManager.Instance.AddGold(-1);
        }
    }

    public void Buy(int id)
    {
        if (!GameManager.Instance.EnoughGold(shop[id].price))
        {
            return;
        }

        GameManager.Instance.AddGold(-shop[id].price);

        if (shop[id] is AgentShopElement agent)
        {
            PlayerDataManager.Instance.AddAgent(agent.type, true);
            shopMap[shop[id].shopType][shop[id].stage].Remove(shop[id]);
        }
        else if (shop[id] is CodeShopElement code)
        {
            CodeBlock block = CodeBlockManager.GetCodeFromStruct(code.codeStruct);
            block.price = shop[id].price;
            UIDataManager.Instance.CreateCodeBlock(block);
        }
        List<shopType> newTypesPool = RollShopTypes(true);
        shopType newType = newTypesPool[Mathf.FloorToInt(UnityEngine.Random.Range(0f, newTypesPool.Count))];
        shop[id] = RollShopElement(newType);
        slots[id].Display(shop[id]);
    }
}
