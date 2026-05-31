using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BattleState CurrentBattleState { get; private set; }

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

        // temporal test
        CurrentBattleState = new BattleState
        {
            BoardWidth = 6,
            BoardHeight = 6,
            SplitLineWidth = 0.01f,
            Player = new UnitRuntime
            {
                Config = new UnitConfig { MaxHealth = 100f },

                CurrentHP = 100,

                GridPos = new Vector2Int(0, 2)
            },
            Enemies = new List<UnitRuntime> {
                new UnitRuntime
                {
                    Config = new UnitConfig{ MaxHealth = 90f },

                    CurrentHP = 100,

                    GridPos = new Vector2Int(5, 3)
                }},
            CurrentCardDeck = TestCardDeck
        };
    }
}

public class BattleState
{
    // battle
    public int BoardWidth;
    public int BoardHeight;
    public float SplitLineWidth;
    public UnitRuntime Player;
    public List<UnitRuntime> Enemies;
    // card
    public CardDeck CurrentCardDeck;
}