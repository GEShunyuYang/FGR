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
            Player = new UnitRuntime
            {
                Config = new UnitConfig { MaxHealth = 100f },

                CurrentHP = 100,

                GridPos = new Vector2Int(0, 2)
            },
            Enemies = new List<UnitRuntime> {
                new UnitRuntime
                {
                    Config = new UnitConfig{type = EnemyType.DEFAULT ,MaxHealth = 90f },

                    CurrentHP = 90,

                    GridPos = new Vector2Int(5, 3)
                }},
            CurrentCardDeck = TestCardDeck,
            State = BattleState.Initializing
        };

        CurrentCardManager.Init(RTBS);
        CurrentCardRenderer.Init(CurrentCardManager.instances);
        CurrentUIManager.Init(CurrentCardRenderer);
        CurrentBattleManager.Init(RTBS, CurrentCardManager);
        CurrentBattleInputController.Init(CurrentBattleManager);


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
    public List<UnitRuntime> Enemies;
    // card
    public CardDeck CurrentCardDeck;
}