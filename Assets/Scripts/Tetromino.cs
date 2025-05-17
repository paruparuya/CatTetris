using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1f;  //落下する時間
    private float previousTime;  //前の時間を記憶用
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D がアタッチされていません！");
            return;
        }
    }

    
    void Update()
    {
        //落下処理
        if(Time.time - previousTime > fallTime) //現在の時間から前の時間を引いて
        {
            Vector2 newPos = rb.position + Vector2.down;
            rb.MovePosition(newPos);
            previousTime = Time.time;
        }

        //横移動の処理
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMove(Vector2.right))
                transform.position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMove(Vector2.left))
                transform.position += Vector3.left;
        }

        //回転の処理
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryRotate(90);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            TryRotate(-90);
        }
    }

    bool CanMove(Vector2 direction)　　//ミノのグリッド判定
    {
        foreach (Transform block in transform)
        {
            Vector2Int newPos = GridManager.RoundVector(block.position + (Vector3)direction);

            //グリッド外か判定
            if(!GridManager.InsideGrid(newPos))
                return false;

            //ブロックかあるか判定
            if (GridManager.grid[(int)newPos.x, (int)newPos.y] != null) 
                return false;
        }

        return true;
    }

    void TryRotate(float angle)
    {
        transform.Rotate(0, 0, angle);

        if(IsValidPosition())
        {
            return;
        }

        if(TryWallKick(angle))
        {
            return;
        }

        transform.Rotate(0, 0, -angle);
    }

    bool IsValidPosition()
    {
        foreach(Transform block in transform)
        {
            Vector2Int pos = GridManager.RoundVector(block.position);

            if(!GridManager.InsideGrid(pos))
                return false;

            if(GridManager.grid[pos.x, pos.y] != null)
                return false;

        }

        return true;
    }

    bool TryWallKick(float angle)
    {
        Vector3[] kicks = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),

        };

        foreach(Vector3 offset in kicks)
        {
            transform.position += offset;
            transform.Rotate(0, 0, angle);

            if(IsValidPosition())
                return false;

            transform.position -= offset;
            transform.Rotate(0, 0, -angle);
        }

        return true;
    }
}
