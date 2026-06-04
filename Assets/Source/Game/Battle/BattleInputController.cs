using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputController : MonoBehaviour
{
    private BattleManager CurrentBattleManager;

    public void Init(BattleManager battleManager)
    {
        CurrentBattleManager = battleManager;   
    }

    void OnTurnEnd()
    {
        CurrentBattleManager.TryEndTurn();
    }

    void OnEnable()
    {
        EventsHandler.RegisterEvent(TurnEvents.END_TURN, OnTurnEnd);
    }

     void OnDisable()
    {
        EventsHandler.UnregisterEvent(TurnEvents.END_TURN, OnTurnEnd);
    }
}
