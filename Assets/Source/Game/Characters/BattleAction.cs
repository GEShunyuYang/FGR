using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAction
{
    public abstract IEnumerator Execute();
}

public class BattleActionQueue
{
    private readonly Queue<BattleAction> Actions = new Queue<BattleAction>();

    public bool HasActions => Actions.Count > 0;

    public void Enqueue(BattleAction action)
    {
        Actions.Enqueue(action);
    }

    public IEnumerator Execute()
    {
        while (Actions.Count > 0)
        {
            yield return Actions.Dequeue().Execute();
        }
    }
}

public class MoveAction : BattleAction
{
    private Unit Unit;
    private Vector2Int TargetCell;
    private Board Board;

    public MoveAction(Board board, Unit unit, Vector2Int targetCell)
    {
        this.Unit = unit;
        this.TargetCell = targetCell;
        this.Board = board;
    }

    public override IEnumerator Execute()
    {
        yield return Unit.MoveTo(Board, TargetCell);
    }
}

public class DamageAction : BattleAction
{
    private Unit target;
    private float damage;

    public DamageAction(Unit target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    public override IEnumerator Execute()
    {
        yield return target.TakeDamage(damage);
    }
}
