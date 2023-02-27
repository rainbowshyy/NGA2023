using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WhileBlock : ConditionBlock
{
    public WhileBlock(int[] param) : base(param) { isLoop = true; }
}