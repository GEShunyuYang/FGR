using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardInstance UsedCardInstance { get; private set; }

    [SerializeField] private RawImage RawImage;
    public void Init(CardInstance instance, CardRenderer cardRenderer)
    {
        UsedCardInstance = instance;
        RawImage.texture = cardRenderer.CardsRenderTexture;
        RawImage.uvRect = cardRenderer.CardInstanceViewDictionary[UsedCardInstance].UVRect;
    }

    private Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        originalPosition = transform.position;
        RawImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData pointerEventData) {
        if (pointerEventData.dragging)
        {
            transform.position = pointerEventData.position;
            
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        transform.position = originalPosition;
        RawImage.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.transform.position += new Vector3(0, 170f, 0f);
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.position -= new Vector3(0, 170f, 0);
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
