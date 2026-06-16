using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //public BattleStateSO BattleStateSO { get; private set; }

    [SerializeField] public CardDeck TestCardDeck;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if(!TestCardDeck)
        {
            Debug.LogError("BattleCard is empty!");
            return;
        }
    }

    void Start()
    {
        StartBattle();
    }

    void StartBattle()
    {
        BattleManager CurrentBattleManager = FindFirstObjectByType<BattleManager>();
        CardRenderer CurrentCardRenderer = FindFirstObjectByType<CardRenderer>();
        CardManager CurrentCardManager = FindFirstObjectByType<CardManager>();
        UIManager CurrentUIManager = FindFirstObjectByType<UIManager>();
        BattleInputController CurrentBattleInputController = FindFirstObjectByType<BattleInputController>();

        // temporal test
        RuntimeBattleState RTBS = new RuntimeBattleState
        {
            CurrentTurn = 0,
            State = BattleState.Initializing,
            Player = new UnitRuntime
            {
                Config = new UnitConfig { MaxHealth = 100f, MoveRange = 3 },

                CurrentHP = 100,

                GridPos = new Vector2Int(0, 2)
            },
            Enemies = new List<UnitRuntime> {
                new UnitRuntime
                {
                    Config = new UnitConfig{type = EnemyType.DEFAULT ,MaxHealth = 90f, MoveRange = 3 },

                    CurrentHP = 90,

                    GridPos = new Vector2Int(5, 2)
                }},
            MaxStamina = 4,
            CurrentStamina = 4,
            MaxHandCount = 6,
            CurrentCardDeck = TestCardDeck
        };

        Debug.Log("1 CardManager Init");
        CurrentCardManager.Init(RTBS);

        Debug.Log("2 CardRenderer Init");
        CurrentCardRenderer.Init(CurrentCardManager.instances);

        Debug.Log("3 UIManager Init");
        CurrentUIManager.Init(CurrentCardRenderer);

        Debug.Log("4 BattleManager Init");
        CurrentBattleManager.Init(RTBS, CurrentCardManager, CurrentUIManager);

        Debug.Log("5 Input Init");
        CurrentBattleInputController.Init(CurrentBattleManager);

        Debug.Log("6 GameStart");
        CurrentBattleManager.GameStart();
    }
}

// to be made as SO
public class RuntimeBattleState
{
    // battle
    public int CurrentTurn;
    public BattleState State;
    public UnitRuntime Player;
    public int MaxStamina;
    public int CurrentStamina;
    public List<UnitRuntime> Enemies;
    // card
    public int MaxHandCount;
    public CardDeck CurrentCardDeck;
}