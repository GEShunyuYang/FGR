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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CurrentBattleManager.TryUndoMove();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryClickBoardCell();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            LocalizationManager.Instance.SetLanguage(Language.En);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            LocalizationManager.Instance.SetLanguage(Language.Zh);
        }
    }

    private void TryClickBoardCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log($"{hit.collider.name}");
            Board board = hit.collider.GetComponentInParent<Board>();

            if (board == null)
            {
                return;
            }

            Vector2Int cell = board.WorldToGrid(hit.point);
            CurrentBattleManager.TryMovePlayer(cell);
        }
    }

    void OnTurnEnd()
    {
        CurrentBattleManager.TryEndTurn();
    }

    void OnCardPlayRequested(PlayCardRequest request)
    {
        CurrentBattleManager.TryPlayCard(request.Card, request.Target);
    }

    void OnCardPreviewRequested(CardInstance card)
    {
        CurrentBattleManager.ShowCardPreview(card);
    }

    void OnCardPreviewClear()
    {
        CurrentBattleManager.ClearCardPreview();
    }

    void OnEnable()
    {
        EventsHandler.RegisterEvent(TurnEvents.END_TURN, OnTurnEnd);
        EventsHandler.RegisterEvent<PlayCardRequest>(CardEvents.PLAY_CARD_REQUEST, OnCardPlayRequested);
        EventsHandler.RegisterEvent<CardInstance>(CardEvents.SHOW_CARD_RANGE, OnCardPreviewRequested);
        EventsHandler.RegisterEvent(CardEvents.CLEAR_CARD_RANGE, OnCardPreviewClear);
    }

     void OnDisable()
    {
        EventsHandler.UnregisterEvent(TurnEvents.END_TURN, OnTurnEnd);
        EventsHandler.UnregisterEvent<PlayCardRequest>(CardEvents.PLAY_CARD_REQUEST, OnCardPlayRequested);
        EventsHandler.UnregisterEvent<CardInstance>(CardEvents.SHOW_CARD_RANGE, OnCardPreviewRequested);
        EventsHandler.UnregisterEvent(CardEvents.CLEAR_CARD_RANGE, OnCardPreviewClear);
    }
}


public class PlayCardRequest
{
    public CardInstance Card { get; private set; }
    public Unit Target { get; private set; }
    public PlayCardRequest(CardInstance card, Unit unit)
    {
        Card = card;
        Target = unit;
    }
}