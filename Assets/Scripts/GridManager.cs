using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    public static Vector2Int RoundVector(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    public static bool InsideGrid(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.x < width && pos.y >= 0);
    }

    public static void AddToGrid(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            Vector2Int pos = RoundVector(block.position);
            if (InsideGrid(pos))
            {
                grid[pos.x, pos.y] = block;
            }
        }
    }
}
