using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private RectTransform CardsRoot;

    [SerializeField] private CardView CardPrefab;

    private List<CardView> HandCards;

    private CardRenderer CurrentCardRenderer;
    public void Init(CardRenderer cardRenderer)
    {
        CurrentCardRenderer = cardRenderer;
        HandCards = new();
    }

    public void DrawCards(List<CardInstance> instances)
    {
        for (int i = 0; i < instances.Count; i++) {
            CardPrefab.Init(instances[i], CurrentCardRenderer);
            HandCards.Add(Instantiate(CardPrefab, CardsRoot, false));
        }
        LayoutHandCards();
    }

    void LayoutHandCards()
    {
        float spacing = 200f;

        float start = -(HandCards.Count - 1) * spacing * 0.5f;

        for (int i = 0; i < HandCards.Count; i++)
        {
            RectTransform rect = HandCards[i].GetComponent<RectTransform>();

            rect.anchoredPosition =  new Vector2(start + i * spacing, 0);
        }
    }
}
