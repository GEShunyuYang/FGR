using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public BattleConfig[] BattleConfigs;

    private string[] BattleNames = { "TeachingScene", "BattleScene1", "BattleScene2", "BattleScene3" };

    private int CurrentSceneNum;

    private RuntimeBattleState RTBS;
    public TransitionContext CurrentTransitionContext { get; private set; }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if(!BattleConfigs[0].PlayerDeck)
        {
            Debug.LogError("player card deck is empty!");
            return;
        }

        if (BattleConfigs.Length < BattleNames.Length)
        {
            Debug.LogError("battle configs' count < scene count");
        }
    }
    private void OnLoadingNextScene()
    {
        CurrentSceneNum++;

        if (CurrentSceneNum >= BattleNames.Length)
        {
            SceneManager.LoadScene("GameStart");
            return;
        }

        SceneManager.LoadScene(BattleNames[CurrentSceneNum]);
    }

    private void OnPlayAgain()
    {
        if (CurrentSceneNum >= BattleNames.Length)
        {
            SceneManager.LoadScene("GameStart");
            return;
        }

        SceneManager.LoadScene(BattleNames[CurrentSceneNum]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TeachingScene" || scene.name == "BattleScene1" ||
            scene.name == "BattleScene2" || scene.name == "BattleScene3")
        {
            StartBattle();
        }
    }

    private void OnBattleEnd()
    {
        BattleConfig config = BattleConfigs[CurrentSceneNum];

        if (!config.UseTransitionAfterBattle)
        {
            OnLoadingNextScene();
            return;
        }

        CurrentTransitionContext = new TransitionContext
        {
            ShowLevelUpPage = config.ShowLevelUpPage,
            ShowRewardCardPage = config.ShowRewardCardPage,
            RewardCard = config.ShowRewardCardPage ? config.RewardCard : null
        };

        SceneManager.LoadScene("Transition");
    }

    void StartBattle()
    {
        BattleManager CurrentBattleManager = FindFirstObjectByType<BattleManager>();
        CardRenderer CurrentCardRenderer = FindFirstObjectByType<CardRenderer>();
        CardManager CurrentCardManager = FindFirstObjectByType<CardManager>();
        UIManager CurrentUIManager = FindFirstObjectByType<UIManager>();
        BattleInputController CurrentBattleInputController = FindFirstObjectByType<BattleInputController>();

        RTBS = CreateRuntimeState(BattleConfigs[CurrentSceneNum]);

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

    public RuntimeBattleState CreateRuntimeState(BattleConfig config)
    {
        RuntimeBattleState state = new RuntimeBattleState
        {
            CurrentTurn = 0,
            State = BattleState.Initializing,

            Player = new UnitRuntime
            {
                Prefab = config.PlayerPrefab,
                Config = new UnitConfig
                {
                    MaxHealth = config.PlayerMaxHealth,
                    MoveRange = config.PlayerMoveRange,
                    MoveSpeed = config.PlayerMoveSpeed
                },
                CurrentHP = config.PlayerMaxHealth,
                GridPos = config.PlayerStartPos
            },

            Enemies = new List<UnitRuntime>(),

            MaxStamina = config.MaxStamina,
            CurrentStamina = config.MaxStamina,
            MaxHandCount = config.MaxHandCount,
            CurrentCardDeck = config.PlayerDeck
        };

        foreach (EnemySpawnConfig enemy in config.Enemies)
        {
            state.Enemies.Add(new UnitRuntime
            {
                Prefab = enemy.Config.EnemyPrefab,
                Config = new UnitConfig
                {
                    MaxHealth = enemy.Config.MaxHealth,
                    MoveRange = enemy.Config.MoveRange,
                    MoveSpeed = enemy.Config.MoveSpeed
                },
                CurrentHP = enemy.Config.MaxHealth,
                GridPos = enemy.GridPos
            });
        }

        return state;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventsHandler.RegisterEvent(SceneEvents.NEXT_SCENE, OnLoadingNextScene);
        EventsHandler.RegisterEvent(BattleEvents.END_BATTLE, OnBattleEnd);
        EventsHandler.RegisterEvent(SceneEvents.TRY_AGAIN, OnPlayAgain);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventsHandler.UnregisterEvent(SceneEvents.NEXT_SCENE, OnLoadingNextScene);
        EventsHandler.UnregisterEvent(BattleEvents.END_BATTLE, OnBattleEnd);
        EventsHandler.UnregisterEvent(SceneEvents.TRY_AGAIN, OnPlayAgain);
    }
}

public class TransitionContext
{
    public bool ShowLevelUpPage;
    public bool ShowRewardCardPage;
    public CardData RewardCard;
}

public class RuntimeBattleState
{
    public int CurrentTurn;
    public BattleState State;

    public UnitRuntime Player;
    public List<UnitRuntime> Enemies;

    public int MaxStamina;
    public int CurrentStamina;
    public int MaxHandCount;

    public CardDeck CurrentCardDeck;
}