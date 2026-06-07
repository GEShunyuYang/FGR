using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Idle,
    Moving,
    Attacking,
    Dead
}

public class UnitConfig
{
    public EnemyType type;

    public UnitState DefaultState = UnitState.Idle;

    public int MoveRange;

    public float MaxHealth;
}

public class UnitRuntime
{
    public UnitConfig Config;

    public float CurrentHP;

    public Vector2Int GridPos;
}

public abstract class Unit : MonoBehaviour
{
    public Vector2Int GridPos;

    public int MoveRange => runtime.Config.MoveRange;

    protected UnitRuntime runtime;

    public UnitState State{get; private set;}

    public virtual void Init(Board board, UnitRuntime unitRuntime) { 
        runtime = unitRuntime;
        GridPos = runtime.GridPos;
        board.TryPlaceUnit(this, GridPos);
        transform.position = board.GridToWorld(runtime.GridPos); 
    }
    public IEnumerator MoveTo(Board board, Vector2Int targetCell)
    {
        State = UnitState.Moving;

        Vector3 start = transform.position;
        Vector2Int from = board.WorldToGrid(start);
        Vector3 target = board.GridToWorld(targetCell);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        GridPos = targetCell;
        State = UnitState.Idle;
        board.MoveUnit(this, from, targetCell);
    }

    public IEnumerator TakeDamage(float damage)
    {
        // tbd damage effect
        runtime.CurrentHP -= damage;
        if (runtime.CurrentHP <= 0)
        {
            State = UnitState.Dead;
        }
        yield return null;
    }

    public void TeleportTo(Board board, Vector2Int targetCell)
    {
        GridPos = targetCell;
        Vector2Int from = board.WorldToGrid(transform.position);
        transform.position = board.GridToWorld(targetCell);
        State = UnitState.Idle;
        board.MoveUnit(this, from, targetCell);
    }
}

