using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // chess board
    private Board Board;

    // player
    [SerializeField] private Unit PlayerPrefab;
    private Unit CurrentPlayer;
    private List<CardInstance> HandCards;
    private List<CardInstance> DrawPile;
    private List<CardInstance> DiscardPile;

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

    public void Init(RuntimeBattleState RTBattleState)
    {
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

        // temporal test
        Queue.Enqueue(new MoveAction(Board, CurrentPlayer, new Vector2Int(3, 4)));
        StartCoroutine(Queue.Execute());
    }

    public void GameStart()
    {
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        CurrentBattleState.State = BattleState.PlayerTurn;
    }

    private void EnemyTurn()
    {
        CurrentBattleState.State = BattleState.EnemyTurn;
    }

    private void PlayingAnimations()
    {
        CurrentBattleState.State = BattleState.TakingActions;
    }

    private void BattleEnd()
    {
        CurrentBattleState.State = BattleState.BattleEnd;
    }

}

public enum BattleState
{
    Initializing,
    PlayerTurn,
    TakingActions,
    EnemyTurn,
    BattleEnd
}