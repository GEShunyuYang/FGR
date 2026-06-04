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

    private void OnDrawCard(List<CardInstance> cards)
    {
        //draw card
        CurrentHandView.DrawCards(cards);
    }



    void OnEnable()
    {
        EventsHandler.RegisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
    }

    void OnDisable()
    {
        EventsHandler.UnregisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
    }
}
