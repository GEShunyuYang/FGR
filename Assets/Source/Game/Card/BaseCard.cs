using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardData Data { get; private set; }

    [Header("UI")]
    [SerializeField] public TMP_Text nameText;
    [SerializeField] public TMP_Text costText;
    [SerializeField] public TMP_Text descriptionText;


    public void Initialize(CardData data)
    {
        Data = data;
        nameText.text = Data.CardName;
        costText.text = Data.BaseCost.ToString();
        descriptionText.text = Data.Description;
    }

    private Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData pointerEventData)
    {

    }

    public void OnDrag(PointerEventData pointerEventData) {

    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}
