using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 22;
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
    public static bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos .y >= 0;
    }

    public static void CheckForLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                MoveAllLinesDown(y);
                y--;
            }
        }
    }

    static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if(grid[x, y] == null)
                return false;
        }
        return true;
    }

    static void DeleteLine(int y)
    {
        for(int x = 0;x < width; x++)
        {
            if (grid[x, y] != null)
            {
                GameObject.Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    static void MoveAllLinesDown(int fromY)
    {
        for (int y = fromY + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
}


