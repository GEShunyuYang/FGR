using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public string cardName;
    [HideInInspector] public string description;
    [HideInInspector] public RawImage image;
    [HideInInspector] public TextMeshPro text;

    private Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData pointerEventData) {
        if (pointerEventData.dragging)
        {
            transform.position = pointerEventData.position;
            image.raycastTarget = false;
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        GameObject GO = pointerEventData.pointerCurrentRaycast.gameObject;

        if (GO && GO.name.Equals("MoveArea")) {           
            transform.position = GO.transform.position;
            transform.rotation = Quaternion.Euler(GO.transform.rotation.x, GO.transform.rotation.y, Random.rotation.z * 100);
        } else if (GO && GO.name.Equals("EnemyResCanvas")) {
            Debug.Log($"play card {this.cardName}");
            Destroy(this.gameObject);
        } else {
            transform.position = originalPosition;
            image.raycastTarget = true;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.transform.position += new Vector3(0, 1f, 0f);
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.position -= new Vector3(0, 1f, 0);
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
