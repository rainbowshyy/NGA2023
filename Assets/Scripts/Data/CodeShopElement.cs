using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeElement", menuName = "Data/Create new Code Shop Element")]
public class CodeShopElement : ShopElement
{
    public CodeBlockTypes codeBlockTypes;
    public Vector2Int[] parameters;
    public Vector2Int[] extraParameters;
    [HideInInspector]
    public CodeBlockStruct codeStruct;
}
