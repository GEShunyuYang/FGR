using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    DEFAULT
}
public class Enemy : Unit
{
    [SerializeField] private EnemyAI Brain;

    public void BuildTurnActions(Unit player, Board board, BattleActionQueue queue)
    {
        Brain.BuildActions(this, player, board, queue);
    }
}
