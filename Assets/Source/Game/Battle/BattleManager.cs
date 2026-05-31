using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;

    [SerializeField] List<GameObject> EnemyPrefabs;
    public Board Board { get; private set; }
    public int BoardWidth { get; private set; }
    public int BoardHeight { get; private set; }
    public float SplitLineWidth { get; private set; }
    public Unit CurrentPlayer { get; private set; }
    public List<Unit> CurrentEnemies { get; private set; }
    private BattleActionQueue Queue;

    private void Awake()
    {
        if (!PlayerPrefab) Debug.LogError("PlayerPrefab is empty");
        if (EnemyPrefabs.Count == 0) Debug.LogError("EnemyPrefabs is empty");
        CurrentEnemies = new List<Unit>();
        Board = FindFirstObjectByType<Board>();
        Queue = new BattleActionQueue();
    }

    void Start()
    {
        BoardWidth = GameManager.Instance.CurrentBattleState.BoardWidth;
        BoardHeight = GameManager.Instance.CurrentBattleState.BoardHeight;
        SplitLineWidth = GameManager.Instance.CurrentBattleState.SplitLineWidth;
        Board.Init(BoardWidth, BoardHeight, SplitLineWidth);

        CurrentPlayer = Instantiate(PlayerPrefab).GetComponent<Unit>();
        CurrentPlayer.Init(Board, GameManager.Instance.CurrentBattleState.Player);

        foreach (UnitRuntime enemy in GameManager.Instance.CurrentBattleState.Enemies)
        {
            Unit EnemyUnit;
            switch (enemy.Config.type) {
                default:
                    if (!EnemyPrefabs[0]) return;
                    EnemyUnit = Instantiate(EnemyPrefabs[0]).GetComponent<Unit>();
                    break;
            }
            EnemyUnit.Init(Board, enemy);
            CurrentEnemies.Add(EnemyUnit);
        }

        // temporal test
        //Queue.Enqueue(new MoveAction(Board, CurrentPlayer, new Vector2Int(3, 4)));
        //StartCoroutine(Queue.Execute());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
