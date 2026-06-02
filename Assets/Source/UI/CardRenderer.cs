using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
    [SerializeField] private Renderer RTPrefab;

    public Dictionary<CardInstance, CardRenderInfo> CardInstanceViewDictionary { get; private set; }

    [SerializeField] private Camera CardsCamera;

    public RenderTexture CardsRenderTexture { get; private set; }

    private Vector3 CardsPosition = new Vector3(-100, 0, 0);
    void Awake()
    {
        if (!CardsCamera || !RTPrefab)
        {
            Debug.LogError("Card Renderer is not set!");
            return;
        }

        CardInstanceViewDictionary = new();
    }

    public void Init(List<CardInstance> instances)
    {
        int columns = Mathf.CeilToInt(Mathf.Sqrt(instances.Count));
        int rows = Mathf.CeilToInt(instances.Count / (float)columns);

        Bounds bounds = RTPrefab.bounds;
        float cardWidth = bounds.size.x;
        float cardHeight = bounds.size.y;

        float totalCardsHeight = cardHeight * rows;
        float totalCardsWidth = cardWidth * columns;

        const int CardPixelWidth = 343;
        const int CardPixelHeight = 512;

        CardsRenderTexture = new RenderTexture(CardPixelWidth * columns, CardPixelHeight * rows, 16);
        CardsRenderTexture.Create();
        CardsCamera.targetTexture = CardsRenderTexture;

        for (int i = 0; i < instances.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Vector3 pos = CardsPosition + new Vector3(col * cardWidth, -row * cardHeight, 0);

            float cellWidth = 1f / columns;

            float cellHeight = 1f / rows;
            float u = col * cellWidth;
            float v = 1f - (row + 1) * cellHeight;

            CardRenderInfo viewinfo = new();
            Rect rect = new(u, v, cellWidth, cellHeight);
            viewinfo.UVRect = rect;
            viewinfo.AtlasIndex = i;

            Renderer render = Instantiate(RTPrefab, pos, RTPrefab.transform.rotation);
            render.transform.SetParent(transform, false);

            CardInstanceViewDictionary.Add(instances[i], viewinfo);
        }

        CardsCamera.orthographic = true;
        CardsCamera.orthographicSize = totalCardsHeight / 2f;
        CardsCamera.aspect = totalCardsWidth / totalCardsHeight;
        CardsCamera.transform.position = CardsPosition + new Vector3(totalCardsWidth * 0.5f - cardWidth * 0.5f, -(totalCardsHeight * 0.5f - cardHeight * 0.5f), -10f);

        CardsCamera.Render();
    }

    public void UpdateTexture()
    {
        CardsCamera.Render();
    }
}
