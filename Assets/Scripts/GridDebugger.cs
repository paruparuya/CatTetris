using UnityEngine;

public class GridDebugger : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // グリッドの全マスを描画
        for (int x = 0; x < GridManager.width; x++)
        {
            for (int y = 0; y < GridManager.height; y++)
            {
                Gizmos.color = GridManager.grid[x, y] == null ? Color.gray : Color.red;
                Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one);
            }
        }
    }
}
