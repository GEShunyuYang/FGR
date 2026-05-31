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

    public float MaxHealth;
}

public class UnitRuntime
{
    public UnitConfig Config;

    public int CurrentHP;

    public Vector2Int GridPos;
}

public abstract class Unit : MonoBehaviour
{
    public Vector2Int GridPos;

    public UnitState State{get; private set;}

    public virtual void Init(Board board, UnitRuntime unitRuntime) { 
        transform.position = board.GridToWorld(unitRuntime.GridPos); 
    }
    public IEnumerator MoveTo(Board board, Vector2Int targetCell)
    {
        State = UnitState.Moving;

        Vector3 start = transform.position;

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
    }
}

