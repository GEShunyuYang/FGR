using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private GameObject canvas;

    [SerializeField] List<BaseCard> cards;

    private GameObject root;
    private GameObject cardGO;

    private List<GameObject> handCards = new();

    private bool start = false;

    void Start()
    {
        root = Instantiate(canvas);
        root.transform.SetParent(this.transform, false);
        cardGO = root.transform.Find("CardCanvas").gameObject;
    }

    void Update()
    {
        OnDrawCard(cards);
    }

    private void OnDrawCard(List<BaseCard> cards)
    {
        float logWidth = Mathf.Log(1920f / Screen.width, 2f);
        float logHeight = Mathf.Log(1080f / Screen.height, 2f);
        float scaleFactor = Mathf.Pow(2f, Mathf.Lerp(logWidth, logHeight, 0.5f));
        float width = Screen.width * scaleFactor;
        float height = Screen.height * scaleFactor;
        //draw card
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject handCard = new($"handCard{i + 1}", typeof(RectTransform), typeof(GraphicRaycaster), typeof(BaseCard), typeof(RawImage));
            handCard.layer = 5;
            RectTransform position = handCard.GetComponent<RectTransform>();
            position.SetParent(cardGO.transform, false);
            position.anchorMin = Vector3.zero;
            position.anchorMax = Vector3.one;
            float spacing = 5f;

            position.offsetMin =
                new Vector2(width / 50 * (14 + i * spacing), 0);

            position.offsetMax =
                new Vector2(-width / 50 * (32 - i * spacing),
                            -height / 50 * 39);
            Canvas cardsCanvas = handCard.GetComponent<Canvas>();
            cardsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cardsCanvas.overrideSorting = true;
            cardsCanvas.sortingOrder = cards.Count + 2 - i;
            BaseCard card = handCard.GetComponent<BaseCard>();
            //card.image = handCard.GetComponent<RawImage>();
            //card.image.texture = CardDeck.GOdic["OneMoreStep"].transform.Find("Profile").GetComponent<RawImage>().texture;

            handCards.Add(handCard);
        }
    }
}
