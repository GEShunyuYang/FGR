using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private BaseCard CardPrefab;
    public List<BaseCard> BattleCardDictionary { get; private set; }

    [SerializeField] private Camera CardsCamera;

    private RenderTexture CardsRenderTexture;

    private Vector3 CardsPosition = new Vector3(-100, 0, 0);
    void Awake()
    {
        if(!CardsCamera)
        {
            Debug.LogError("Card Camera is not set!");
            return;
        }

        BattleCardDictionary = new List<BaseCard>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!CardsCamera)
        {
            Debug.LogError("Card Camera is not set!");
            return;
        }

        List<CardData> CardDatas = GameManager.Instance.CurrentBattleState.CurrentCardDeck.Cards;
        int columns = Mathf.CeilToInt(Mathf.Sqrt(CardDatas.Count));
        int rows = Mathf.CeilToInt(CardDatas.Count / (float)columns);

        Bounds bounds = CardPrefab.GetComponentInChildren<Renderer>().bounds;
        float cardWidth = bounds.size.x;
        float cardHeight = bounds.size.y;

        float totalCardsHeight = cardHeight * rows;
        float totalCardsWidth = cardWidth * columns;

        const int CardPixelWidth = 343;
        const int CardPixelHeight = 512;

        CardsRenderTexture = new RenderTexture(CardPixelWidth * columns, CardPixelHeight * rows, 16);
        CardsRenderTexture.Create();
        CardsCamera.targetTexture = CardsRenderTexture;

        for (int i = 0; i < CardDatas.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Vector3 pos = CardsPosition + new Vector3(col * cardWidth, -row * cardHeight, 0);

            BaseCard CardComponent = Instantiate(CardPrefab, pos, CardPrefab.transform.rotation);
            CardComponent.Initialize(CardDatas[i]);
            BattleCardDictionary.Add(CardComponent);
        }

        CardsCamera.orthographic = true;
        CardsCamera.orthographicSize = totalCardsHeight / 2f;
        CardsCamera.aspect = totalCardsWidth / totalCardsHeight;
        CardsCamera.transform.position = CardsPosition + new Vector3(totalCardsWidth * 0.5f - cardWidth * 0.5f, -(totalCardsHeight * 0.5f - cardHeight * 0.5f), -10f);

        CardsCamera.Render();
        RenderTexture.active = CardsRenderTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
