using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardRenderView : MonoBehaviour
{
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardCost;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Transform AnimatedObject; // temporal use
    public Renderer CardRenderer => CRenderer;
    public Transform CardTransform => AnimatedObject;

    [SerializeField] private Renderer CRenderer;

    public void Bind(CardInstance card)
    {
        cardName.text = card.Data.CardName;
        cardCost.text = card.CurrentCost.ToString();
        description.text = card.Data.Description;
    }
}
