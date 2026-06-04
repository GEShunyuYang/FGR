using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardInstance> instances { get; private set; }

    private List<CardInstance> HandCards;
    private List<CardInstance> DrawPile;
    private List<CardInstance> DiscardPile;

    private RuntimeBattleState CurrentBattleState;

    void Awake()
    {
        instances = new();
        HandCards = new();
        DrawPile = new();
        DiscardPile = new();
    }
    public void Init(RuntimeBattleState RTBattleState)
    {
        CurrentBattleState = RTBattleState;
        for (int i = 0; i < CurrentBattleState.CurrentCardDeck.Cards.Count; i++)
        {
            CardInstance instance = new();
            instance.Data = CurrentBattleState.CurrentCardDeck.Cards[i];
            instance.CurrentCost = instance.Data.BaseCost;
            instances.Add(instance);
            DrawPile.Add(instance);
        }
        
    }
    
    public void DrawRandomCards(int N, BattleActionQueue Queue)
    {
        List<CardInstance> DrawnCards = new();

        for (int i = 0; i < N; i++)
        {
            if(DrawPile.Count == 0)
            {
                ShuffleAndReplenishCards(Queue);
            }

            if (DrawPile.Count == 0)
            {
                break;
            }

            int index = Random.Range(0, DrawPile.Count);
            CardInstance card = DrawPile[index];

            DrawnCards.Add(card);
            // waiting to add animation
            //Queue.Enqueue(new DrawCardAction(card));
            DrawPile.RemoveAt(index);
        }
        EventsHandler.TriggerEvent(UIEvents.DRAW_CARD, DrawnCards);
        HandCards.AddRange(DrawnCards);
    }

    private void ShuffleAndReplenishCards(BattleActionQueue Queue)
    {
        DrawPile.AddRange(DiscardPile);
        DiscardPile.Clear();

        Shuffle(DrawPile);
        // waiting to add animation
        //Queue.Enqueue(new ReplenishCardsAction());
    }

    private void Shuffle(List<CardInstance> cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            CardInstance temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
}
