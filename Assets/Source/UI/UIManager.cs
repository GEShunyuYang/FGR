using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private HandView CurrentHandView;

    private CardRenderer CurrentCardRenderer;
    void Awake()
    {
        
    }

    public void Init(CardRenderer cardRenderer)
    {
        CurrentCardRenderer = cardRenderer;
        CurrentHandView.Init(CurrentCardRenderer);
    }


    void Update()
    {
        //OnDrawCard(cards);
    }

    private void OnDrawCard(List<CardInstance> instances)
    {
        //draw card
        CurrentHandView.DrawCards(instances);
    }

    private void OnPlayCard(CardInstance instances) {
        CurrentHandView.RemoveCard(instances);
    }

    void OnEnable()
    {
        EventsHandler.RegisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
        EventsHandler.RegisterEvent<CardInstance>(CardEvents.PLAY_CARD, OnPlayCard);
    }

    void OnDisable()
    {
        EventsHandler.UnregisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
        EventsHandler.UnregisterEvent<CardInstance>(CardEvents.PLAY_CARD, OnPlayCard);
    }
}
