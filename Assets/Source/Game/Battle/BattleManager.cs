using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // chess board
    private Board Board;

    // cards
    private CardManager CurrentCardManager;

    // player
    [SerializeField] private Unit PlayerPrefab;
    private Unit CurrentPlayer;

    // enemies
    [SerializeField] List<Unit> EnemyPrefabs;
    private List<Unit> CurrentEnemies;

    // gameplay
    private RuntimeBattleState CurrentBattleState;
    private BattleActionQueue Queue; // animation sequence

    private void Awake()
    {
        if (!PlayerPrefab || EnemyPrefabs.Count == 0) Debug.LogError("Battle Manager is not set");

        CurrentEnemies = new List<Unit>();
        Board = FindFirstObjectByType<Board>();
        Queue = new BattleActionQueue();
    }

    public void Init(RuntimeBattleState RTBattleState, CardManager cardManager)
    {
        CurrentCardManager = cardManager;
        CurrentBattleState = RTBattleState;

        // chess board
        Board.Init();

        // player
        CurrentPlayer = Instantiate(PlayerPrefab);
        CurrentPlayer.Init(Board, CurrentBattleState.Player);

        // enemies
        foreach (UnitRuntime enemy in CurrentBattleState.Enemies)
        {
            Unit EnemyUnit;
            switch (enemy.Config.type)
            {
                default:
                    if (!EnemyPrefabs[0]) break;
                    EnemyUnit = Instantiate(EnemyPrefabs[0]);
                    EnemyUnit.Init(Board, enemy);
                    CurrentEnemies.Add(EnemyUnit);
                    break;
            }
        }
    }

    public void GameStart()
    {
        ChangeState(BattleState.PlayerTurnStart);
    }

    private void StartPlayerTurn()
    {
        // Settle Dots dmg

        CurrentBattleState.CurrentTurn++;

        CurrentCardManager.DrawRandomCards(2, Queue);

        if (Queue.HasActions)
        {
            ChangeState(BattleState.PlayerTakingActions);
        }
        else
        {
            ChangeState(BattleState.WaitingForPlayerInput);
        }
        //CurrentCardManager.DrawRandomCard(2);
        //Queue.Enqueue(new MoveAction(Board, CurrentPlayer, new Vector2Int(3, 4)));
        //ChangeState(BattleState.WaitingForPlayerInput);
    }

    private IEnumerator ExecuteActions()
    {
        yield return Queue.Execute();

        if (CheckBattleEnd())
        {
            ChangeState(BattleState.BattleEnd);
            
        } else if(CurrentBattleState.State == BattleState.EnemyTakingActions)
        {
            ChangeState(BattleState.EnemyTurnEnd);
        }
        else if(CurrentBattleState.State == BattleState.PlayerTakingActions)
        {
            ChangeState(BattleState.WaitingForPlayerInput);
        }
    }

    public void TryEndTurn()
    {
        if(CanChangeTo(BattleState.PlayerTurnEnd))
        {
            ChangeState(BattleState.PlayerTurnEnd);
        }
    }

    private void EndPlayerTurn()
    {

        // settle dots on player or something
        ChangeState(BattleState.EnemyTurnStart);
    }

    private void StartEnemyTurn()
    {
        Queue.Enqueue(new MoveAction(Board, CurrentEnemies[0], new Vector2Int(Random.Range(0, 5), Random.Range(0, 5))));
        if (Queue.HasActions)
        {
            ChangeState(BattleState.EnemyTakingActions);
        }
        else
        {
            ChangeState(BattleState.EnemyTurnEnd);
        }
    }

    private void EndEnemyTurn()
    {
        ChangeState(BattleState.PlayerTurnStart);
    }
    private void EndBattle()
    {

    }

    private bool CheckBattleEnd()
    {
        return CurrentBattleState.Player.CurrentHP <= 0 || CurrentBattleState.Enemies.Count <= 0;
    }

    private void ChangeState(BattleState nextState)
    {
        if (!CanChangeTo(nextState))
        {
            Debug.LogWarning($"Invalid state change: {CurrentBattleState.State} -> {nextState}");
            return;
        }

        CurrentBattleState.State = nextState;

        switch (nextState)
        {
            case BattleState.Initializing:
                DebugInfo("to Initializing");
                break;

            case BattleState.PlayerTurnStart:
                DebugInfo("to PlayerTurnStart");
                StartPlayerTurn();
                break;

            case BattleState.WaitingForPlayerInput:
                DebugInfo("to WaitingForPlayerInput");
                break;

            case BattleState.PlayerTakingActions:
                DebugInfo("to PlayerTakingActions");
                StartCoroutine(ExecuteActions());
                break;

            case BattleState.PlayerTurnEnd:
                DebugInfo("to PlayerTurnEnd");
                EndPlayerTurn();
                break;

            case BattleState.EnemyTurnStart:
                DebugInfo("to EnemyTurnStart");
                StartEnemyTurn();
                break;

            case BattleState.EnemyTakingActions:
                DebugInfo("to EnemyTakingActions");
                StartCoroutine(ExecuteActions());
                break;

            case BattleState.EnemyTurnEnd:
                DebugInfo("to EnemyTurnEnd");
                EndEnemyTurn();
                break;

            case BattleState.BattleEnd:
                DebugInfo("to BattleEnd");
                EndBattle();
                break;
        }
    }

    private void DebugInfo(string msg)
    {
        Debug.Log(msg);
    }


    private bool CanChangeTo(BattleState nextState)
    {
        BattleState currentState = CurrentBattleState.State;

        switch (currentState)
        {
            case BattleState.Initializing:
                return nextState == BattleState.PlayerTurnStart
                    || nextState == BattleState.BattleEnd;

            case BattleState.PlayerTurnStart:
                return nextState == BattleState.WaitingForPlayerInput
                    || nextState == BattleState.BattleEnd
                    || nextState == BattleState.PlayerTakingActions;

            case BattleState.WaitingForPlayerInput:
                return nextState == BattleState.PlayerTakingActions
                    || nextState == BattleState.PlayerTurnEnd
                    || nextState == BattleState.BattleEnd;

            case BattleState.PlayerTakingActions:
                return nextState == BattleState.WaitingForPlayerInput
                    || nextState == BattleState.BattleEnd;

            case BattleState.PlayerTurnEnd:
                return nextState == BattleState.EnemyTurnStart
                    || nextState == BattleState.BattleEnd;

            case BattleState.EnemyTurnStart:
                return nextState == BattleState.EnemyTakingActions
                    || nextState == BattleState.EnemyTurnEnd
                    || nextState == BattleState.BattleEnd;

            case BattleState.EnemyTakingActions:
                return nextState == BattleState.EnemyTurnEnd
                    || nextState == BattleState.BattleEnd;

            case BattleState.EnemyTurnEnd:
                return nextState == BattleState.PlayerTurnStart
                    || nextState == BattleState.BattleEnd;

            case BattleState.BattleEnd:
                return false;

            default:
                return false;
        }
    }
}


public enum BattleState
{
    Initializing,

    PlayerTurnStart,
    WaitingForPlayerInput,
    PlayerTakingActions,
    PlayerTurnEnd,

    EnemyTurnStart,
    EnemyTakingActions,
    EnemyTurnEnd,

    BattleEnd
}