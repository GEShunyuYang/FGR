using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    public string CardID;

    public string CardName;

    public int BaseCost;

    [TextArea]
    public string Description;
}
