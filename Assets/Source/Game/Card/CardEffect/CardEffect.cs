using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void BuildActions(CardEffectContext context, BattleActionQueue queue);
}

public class CardEffectContext
{
    public CardInstance Card { get; private set; }
    public Unit Caster { get; private set; }
    public Unit Target { get; private set; }
    public Board Board { get; private set; }
    public RuntimeBattleState BattleState { get; private set; }
    public CardManager CardManager { get; private set; }

    public CardEffectContext(
        CardInstance card,
        Unit caster,
        Unit target,
        Board board,
        RuntimeBattleState battleState,
        CardManager cardManager)
    {
        Card = card;
        Caster = caster;
        Target = target;
        Board = board;
        BattleState = battleState;
        CardManager = cardManager;
    }
}