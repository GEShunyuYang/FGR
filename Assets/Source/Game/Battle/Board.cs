using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Renderer BoardRenderer;

    [SerializeField] private int Width;
    [SerializeField] private int Height;

    [SerializeField] private float LineWidth;

    public TileData[,] Tiles { get; private set; }

    private float CellWidth;
    private float CellHeight;

    private Bounds BoardBounds;

    private void Awake()
    {
        if (!BoardRenderer) BoardRenderer = GetComponent<Renderer>();
    }

    public Vector3 GridToWorld(Vector2Int cell)
    {
        float x = BoardBounds.min.x + (cell.x + 0.5f) * CellWidth;

        float z = BoardBounds.min.z + (cell.y + 0.5f) * CellHeight;

        return new Vector3(x, transform.position.y, z);
    }

    public Vector2Int WorldToGrid(Vector3 pos)
    {
        float localX = pos.x - BoardBounds.min.x;
        float localZ = pos.z - BoardBounds.min.z;

        int x = Mathf.FloorToInt(localX / CellWidth);
        int y = Mathf.FloorToInt(localZ / CellHeight);

        x = Mathf.Clamp(x, 0, Width - 1);
        y = Mathf.Clamp(y, 0, Height - 1);

        return new Vector2Int(x, y);
    }

    public void Init()
    {
        Material material = BoardRenderer.material;
        material.SetFloat("_LineWidth", LineWidth);
        material.SetFloat("_GridX", Width);
        material.SetFloat("_GridY", Height);

        BoardBounds = BoardRenderer.bounds;
        CellWidth = BoardRenderer.bounds.size.x / Width;
        CellHeight = BoardRenderer.bounds.size.z / Height;

        Tiles = new TileData[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y] = new TileData { Coord = new Vector2Int(x, y) };
            }
        }
    }
}

public class TileData
{
    public Vector2Int Coord;

    public Unit Occupant;
}

