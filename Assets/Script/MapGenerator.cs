using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//public class PathNode : MonoBehaviour
//{
//    public Vector3 Position => transform.position;
//}

public class MapGenerator : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public GameObject tilePrefab;
    public GameObject endPrefab;
    public float tileSize = 1f;

    Vector2 lastPoint;
    [SerializeField] private Transform player;
    [SerializeField] private Transform pathParent;

    private void Start()
    {
        player.position = points[0].position;
    }

    public void GeneratePath()
    {
        if (tilePrefab == null || points.Count < 2)
        {
            Debug.LogWarning("Missing tile prefab or not enough points!");
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            CreatePathBetween(points[i].position, points[i + 1].position);
        }
        // Instantiate end tile at the last point
        CreateLastPoint();
    }
    private void CreateLastPoint()
    {
        Vector2 start = RoundToGrid(points[points.Count-2].position);
        Vector2 end  = RoundToGrid(points[points.Count - 1].position);
        Vector2 dir = GetStraightDirection2D(start, end);
        GameObject endTile = Instantiate(endPrefab, lastPoint, Quaternion.identity, this.transform);
        endTile.transform.SetParent(pathParent);
    }
    private void CreatePathBetween(Vector2 start, Vector2 end)
    {
        start = RoundToGrid(start);
        end = RoundToGrid(end);

        Vector2 direction = GetStraightDirection2D(start, end);

        float dist = Vector2.Distance(start, end);
        int tileCount = Mathf.RoundToInt(dist / tileSize);

        Vector2 current = start;

        for (int i = 0; i <= tileCount-1; i++)
        {
            
            GameObject Tile = Instantiate(tilePrefab, current, Quaternion.identity, this.transform);
            Tile.transform.SetParent(pathParent);
            current += direction * tileSize;
            lastPoint = current;
        }
    }
    private Vector2 GetStraightDirection2D(Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            return new Vector2(Mathf.Sign(diff.x), 0);
        else
            return new Vector2(0, Mathf.Sign(diff.y));
    }

    private Vector2 RoundToGrid(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // Only Up/Down/Left/Right (Manhattan)
    private Vector3 GetStepDirection(Vector3 from, Vector3 to)
    {
        Vector3 diff = to - from;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            // Move horizontally
            return new Vector3(Mathf.Sign(diff.x), 0, 0);
        }
        else
        {
            // Move vertically
            return new Vector3(0, 0, Mathf.Sign(diff.y));
        }
    }
    public void ClearPath()
    {
        if (pathParent == null)
        {
            Debug.LogWarning("Path parent is not assigned!");
            return;
        }
        for (int i = pathParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(pathParent.GetChild(i).gameObject);
        }
    }
}


