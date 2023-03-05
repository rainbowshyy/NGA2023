using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeBlockTypes {
    [InspectorName("Movement/Move")]
    MoveBlock,
    [InspectorName("Power/Add")]
    EnergyBlock,
    [InspectorName("Misc/Wait")]
    WaitBlock,
    [InspectorName("Power/Add in Range")]
    EnergyInRange,
    [InspectorName("Conditions/If/Power greater")]
    EnergyGreaterThan,
    [InspectorName("Conditions/If/Power less")]
    EnergyLessThan,
    [InspectorName("Damage/Add in Range")]
    DamageInRange,
    [InspectorName("Conditions/If/X greater")]
    XGreaterThan,
    [InspectorName("Conditions/If/X less")]
    XLessThan,
    [InspectorName("Conditions/If/Y greater")]
    YGreaterThan,
    [InspectorName("Conditions/If/Y less")]
    YLessThan,
    [InspectorName("Conditions/While/Power greater")]
    EnergyGreaterWhile,
    [InspectorName("Conditions/While/Power less")]
    EnergyLessWhile,
    [InspectorName("Conditions/While/X greater")]
    XGreaterWhile,
    [InspectorName("Conditions/While/X less")]
    XLessWhile,
    [InspectorName("Conditions/While/Y greater")]
    YGreaterWhile,
    [InspectorName("Conditions/While/Y less")]
    YLessWhile,
    [InspectorName("Conditions/If/Space")]
    Space,
    [InspectorName("Conditions/While/Space")]
    SpaceWhile,
    [InspectorName("Damage/Add at Ray")]
    DamageRay,
    [InspectorName("Conditions/If/Enemy at Ray")]
    EnemyRay,
    [InspectorName("Conditions/While/Enemy at Ray")]
    EnemyRayWhile,
    [InspectorName("Health/Add in Range")]
    HealthRange,
    [InspectorName("Movement/Push")]
    PushBlock,
    [InspectorName("Power/Set")]
    EnergySetBlock,
    [InspectorName("Health/Add")]
    Health,
    [InspectorName("Health/Add power")]
    HealthPWR,
    [InspectorName("Misc/Create")]
    Create,
    [InspectorName("Power/Add at Ray")]
    EnergyRay,
    [InspectorName("Power/Add Power at Ray")]
    EnergyRayPower,
    [InspectorName("Power/Add Power in Range")]
    PowerRangePower
}

public abstract class CodeBlock
{
    public int[] parameters;
    public bool isCondition;
    public bool isLoop;
    public List<CodeBlock> scope;
    public int price;

    protected CodeBlock(int[] param)
    {
        parameters = param;
        isCondition = false;
        isLoop = false;
    }

    public abstract bool ReadyCode(CodeBlockAgent agent);
    public abstract bool RunCode(CodeBlockAgent agent);
    public abstract string ShowSyntax();
    public abstract void VisualCode(CodeBlockAgent agent);
    public abstract string ToolTip();
}
