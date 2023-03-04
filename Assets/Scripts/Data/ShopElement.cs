using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum shopType { Movement, Power, Damage, Health, Agent, Condition}

public abstract class ShopElement : ScriptableObject
{
    public int price;
    public Stages stage;
    public shopType shopType;
}
