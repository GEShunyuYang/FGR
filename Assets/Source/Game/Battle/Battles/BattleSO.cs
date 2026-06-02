using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSO : ScriptableObject
{
    public UnitRuntime Player;
    public List<UnitRuntime> Enemies;
    // card
    public CardDeck CurrentCardDeck;
}
