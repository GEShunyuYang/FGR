using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private HandView CurrentHandView;

    [SerializeField] private Image fillImage;

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

    private void OnPreviewCardDescription(CardDescriptionPreview preview)
    {
        CurrentCardRenderer.SetDescriptionPreview(preview);
    }

    private void OnClearCardDescriptionPreview(CardInstance card)
    {
        CurrentCardRenderer.ClearDescriptionPreview(card);
    }

    public void SetStaminaProgress(float percentage)
    {
        if (percentage <= 0)
        {
            fillImage.fillAmount = 0;
            return;
        }

        fillImage.fillAmount = Mathf.Clamp01(percentage);
    }

    void OnEnable()
    {
        EventsHandler.RegisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
        EventsHandler.RegisterEvent<CardInstance>(CardEvents.PLAY_CARD, OnPlayCard);
        EventsHandler.RegisterEvent<CardDescriptionPreview>(CardEvents.PREVIEW_CARD_DESCRIPTION, OnPreviewCardDescription);
        EventsHandler.RegisterEvent<CardInstance>(CardEvents.CLEAR_CARD_DESCRIPTION_PREVIEW, OnClearCardDescriptionPreview);
        EventsHandler.RegisterEvent<float>(UIEvents.STAMINA_CHANGE, SetStaminaProgress);
    }

    void OnDisable()
    {
        EventsHandler.UnregisterEvent<List<CardInstance>>(UIEvents.DRAW_CARD, OnDrawCard);
        EventsHandler.UnregisterEvent<CardInstance>(CardEvents.PLAY_CARD, OnPlayCard);
        EventsHandler.UnregisterEvent<CardDescriptionPreview>(CardEvents.PREVIEW_CARD_DESCRIPTION, OnPreviewCardDescription);
        EventsHandler.UnregisterEvent<CardInstance>(CardEvents.CLEAR_CARD_DESCRIPTION_PREVIEW, OnClearCardDescriptionPreview);
        EventsHandler.UnregisterEvent<float>(UIEvents.STAMINA_CHANGE, SetStaminaProgress);
    }
}
