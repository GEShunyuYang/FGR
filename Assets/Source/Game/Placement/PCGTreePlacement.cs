using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGTreePlacement : MonoBehaviour
{
    public GameObject[] prefabs;

    public int count = 50;

    public LayerMask groundMask;

    [SerializeField] private BoxCollider[] exclusions;

    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);

    private List<Vector3> placedPositions = new List<Vector3>();

    public float minDistance = 3f;

    [ContextMenu("Generate Trees")]
    public void Generate()
    {
        placedPositions.Clear();

        BoxCollider box = GetComponent<BoxCollider>();

        Bounds bounds = box.bounds;

        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                bounds.max.y + 10f,
                Random.Range(bounds.min.z, bounds.max.z));

            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100f, groundMask))
            {
                if (IsInsideExclusionZone(hit.point))
                {
                    i--;
                    continue;
                }

                bool valid = true;

                foreach (Vector3 pos in placedPositions)
                {
                    Vector2 a = new Vector2(hit.point.x, hit.point.z);

                    Vector2 b = new Vector2(pos.x, pos.z);

                    if (Vector2.Distance(a, b) < minDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    i--;
                    continue;
                }

                placedPositions.Add(hit.point);

                if (prefabs == null || prefabs.Length == 0)
                {
                    Debug.LogError("No prefabs assigned!");
                    return;
                }

                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                GameObject tree = Instantiate(prefab, hit.point, rotation);

                tree.transform.SetParent(transform);
            }
        }
    }

    void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(box.center, box.size);
    }

    [ContextMenu("Clear Trees")]
    public void ClearTrees()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject obj in children)
        {
#if UNITY_EDITOR
            DestroyImmediate(obj);
#else
        Destroy(obj);
#endif
        }
    }

    private bool IsInsideExclusionZone(Vector3 position)
    {
        foreach (var zone in exclusions)
        {
            if (zone.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }
}
