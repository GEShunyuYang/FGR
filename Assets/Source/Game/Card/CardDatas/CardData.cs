using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    public string CardName;

    public int BaseCost;

    [TextArea]
    public string Description;
}

public class CardRenderInfo
{
    public Rect UVRect;

    public int AtlasIndex;
}