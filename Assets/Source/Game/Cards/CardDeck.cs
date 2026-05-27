using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour
{
    public static Dictionary<string, GameObject> GOdic = new();

    [SerializeField] public List<GameObject> cards;

    [SerializeField] public Camera cardCamera;

    private RenderTexture cardsTexture;

    void Start()
    {
        //cards = Resources.Load<BattleSceneData>(BattleConfig.BattleName).cards;
        if (cards == null)
        {
            Debug.LogError("No cards from scene loading");
        }

        transform.position = new Vector3(-100, 0, 0);

        cardsTexture = new RenderTexture(343 * cards.Count, 512, 16);
        cardsTexture.Create();

        //cardCamera.targetTexture = cardsTexture;
        
        GOdic.Clear();

        float cardWorldWidth = 0;
        float cardWorldHeight = 0;
        float totalWidth = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            if (GOdic.ContainsKey(cards[i].name)) continue;
            GameObject cardGO = Instantiate(cards[i]);
            if (i == 0)
            {
                Vector3 cardSize = cardGO.GetComponent<MeshRenderer>().bounds.size;
                cardWorldWidth = cardSize.x;
                cardWorldHeight = cardSize.y;
            }
            cardGO.transform.position = new(0 - i * cardWorldWidth, 0, 10);
            totalWidth += (0 - i * cardWorldWidth);
            cardGO.transform.SetParent(transform, false);
            BaseCard card = cardGO.GetComponent<BaseCard>();
            GOdic.TryAdd(card.cardName, cardGO);
        }
        /*
        if (cards.Count < 15) {
            GameObject cardGO = Instantiate(Resources.Load<Clueless>("Clueless").gameObject);
            cardGO.transform.position = new(0 - GOdic.Count * cardWorldWidth, 0, 10);
            totalWidth -= cardWorldWidth;
            cardGO.transform.SetParent(transform, false);
            BaseCard card = cardGO.GetComponent<BaseCard>();
            GOdic.TryAdd(card.cardName, cardGO);
        }*/
        /*
        cardCamera.orthographic = true;
        cardCamera.orthographicSize = cardWorldHeight / 2f;
        cardCamera.aspect = cardWorldWidth / cardWorldHeight * GOdic.Count;
        cardCamera.transform.position = transform.position + new Vector3(totalWidth / GOdic.Count, 0, 10) + new Vector3(0, 0, -10);

        cardCamera.Render();
        RenderTexture.active = cardsTexture;
        foreach (KeyValuePair<string, GameObject> item in GOdic)
        {
            GameObject cardGO = item.Value;
            float cardXMin = cardGO.transform.position.x - cardWorldWidth / 2;
            float cardYMin = cardGO.transform.position.y - cardWorldHeight / 2;
            Vector3 worldPos = new(cardXMin, cardYMin, 0);
            Vector3 screenPos = cardCamera.WorldToScreenPoint(worldPos);
            Texture2D tex = new(343, 512, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(screenPos.x, screenPos.y, 343, 512), 0, 0);
            tex.Apply();
            cardGO.transform.Find("Profile").GetComponent<RawImage>().texture = tex;
            cardGO.transform.Find("Profile").GetComponent<RawImage>().SetNativeSize();
        }*/
    }


    public static float ApplyCard(float baseAttack, List<BaseCard> cards)
    {
        float damage = baseAttack;
        for (int i = 0; i < cards.Count; i++)
        {
            switch (cards[i].cardName) {
                default:
                    damage = baseAttack;
                    break;
            }
        }
        return damage;
    }
}

public static class CardsName
{
    public const string ONE_MORE_MOVE = "OneMoreStep";
    public const string CLUELESS = "Clueless";
}