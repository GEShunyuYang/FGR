using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardInstance> instances { get; private set; }

    private RuntimeBattleState CurrentBattleState;

    bool once = true;
    void Awake()
    {
        instances = new List<CardInstance>();
    }
    public void Init(RuntimeBattleState RTBattleState)
    {
        CurrentBattleState = RTBattleState;
        for (int i = 0; i < CurrentBattleState.CurrentCardDeck.Cards.Count; i++)
        {
            CardInstance instance = new();
            instance.Data = CurrentBattleState.CurrentCardDeck.Cards[i];
            instances.Add(instance);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (once)
        {
            once = !once;
            EventsHandler.TriggerEvent(UIEvents.DRAW_CARD, instances);
        }
    }
}
