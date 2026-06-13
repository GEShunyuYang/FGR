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

    private HPBarView HPBar;

    private Animator Animator;

    public int MoveRange => runtime.Config.MoveRange;

    protected UnitRuntime runtime;

    public UnitState State{get; private set;}

    private bool attackHitNotified;

    public void OnAttackHit()
    {
        attackHitNotified = true;
    }

    public virtual void Init(Board board, UnitRuntime unitRuntime) { 
        runtime = unitRuntime;
        GridPos = runtime.GridPos;
        board.TryPlaceUnit(this, GridPos);
        transform.position = board.GridToWorld(runtime.GridPos);
        Animator = GetComponent<Animator>();
    }

    public void BindHealthBar(HPBarView view)
    {
        HPBar = view;
        HPBar.SetHealth(runtime.CurrentHP, runtime.Config.MaxHealth);
    }

    public IEnumerator MoveTo(Board board, Vector2Int targetCell)
    {
        State = UnitState.Moving;
        
        if (Animator != null)
        {
            Animator.SetBool("IsMoving", true);
        }

        Vector3 start = transform.position;
        Vector2Int from = board.WorldToGrid(start);
        Vector3 target = board.GridToWorld(targetCell);

        float moveSpeed = 2f;

        while (Vector3.Distance(transform.position, target) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;

        GridPos = targetCell;
        board.MoveUnit(this, from, targetCell);

        if (Animator != null)
        {
            Animator.SetBool("IsMoving", false);
        }

        State = UnitState.Idle;
    }

    public IEnumerator PlayAttackAnimation()
    {
        if (Animator == null)
        {
            Debug.LogWarning("Animator is null.");
            yield break;
        }

        attackHitNotified = false;

        State = UnitState.Attacking;

        Animator.ResetTrigger("Attack");
        Animator.SetTrigger("Attack");

        yield return WaitForAnimatorStateEnter("Attack");

        yield return new WaitUntil(() => attackHitNotified);
    }

    public IEnumerator WaitAttackEnd()
    {
        yield return WaitForAnimatorStateExit("Attack");
        State = UnitState.Idle;
    }

    public IEnumerator TakeDamage(float damage)
    {
        // tbd damage effect
        runtime.CurrentHP -= damage;
        if (runtime.CurrentHP <= 0)
        {
            State = UnitState.Dead;
        }
        HPBar.SetHealth(runtime.CurrentHP, runtime.Config.MaxHealth);
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

    private float FacingYawOffset = 0f;
    private float TurnSpeed = 720f;

    public IEnumerator FaceCell(Vector2Int targetCell)
    {
        Vector2Int delta = targetCell - GridPos;

        if (delta == Vector2Int.zero)
        {
            yield break;
        }

        Vector3 dir = new Vector3(delta.x, 0f, delta.y).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        targetRotation *= Quaternion.Euler(0f, FacingYawOffset, 0f);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                TurnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator WaitForAnimatorStateEnter(string stateName, int layer = 0)
    {
        while (true)
        {
            AnimatorStateInfo current = Animator.GetCurrentAnimatorStateInfo(layer);
            AnimatorStateInfo next = Animator.GetNextAnimatorStateInfo(layer);

            if (current.IsName(stateName) || next.IsName(stateName))
            {
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForAnimatorStateExit(string stateName, int layer = 0)
    {
        while (true)
        {
            AnimatorStateInfo current = Animator.GetCurrentAnimatorStateInfo(layer);

            if (!current.IsName(stateName) && !Animator.IsInTransition(layer))
            {
                yield break;
            }

            yield return null;
        }
    }
}

