using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1f;  //落下する時間
    private float previousTime;  //前の時間を記憶用
   

    //ミノを固定する関係のもの
    private bool isTouching = false;   //地面に触れているかチェック
    private float touciTime = 0f;      //触れてからの時間経過を計測用
    private float lockDelay = 1f;      //固定するまでの時間
    private Vector3 lastPosition;      //最後に確認した位置

    public enum TetrominoType
    {
        I, O, T, L, J, S, Z
    }

    public TetrominoType type;



    void Start()
    {
        
        
    }

    void FixedUpdate()
    {
        if (Time.time - previousTime > fallTime)
        {
            if (CanMove(Vector2.down))
                transform.position += Vector3.down;
            else
                CheckAndLock();

            previousTime = Time.time;
        }
    }


    void Update()
    {
        //落下処理
        /*if(Time.time - previousTime > fallTime) //現在の時間から前の時間を引いて
        {
            transform.position += Vector3.down;
            previousTime = Time.time;
        }*/

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

        //ミノを固定する処理
        if(IsTouchingGround())
        {
            if(transform.position == lastPosition)
            {
                touciTime += Time.deltaTime;

                if (touciTime > lockDelay)
                {
                    LockAndSpawnNew();
                }
            }
            else
            {
                touciTime = 0f;
                lastPosition = transform.position;
            }
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

    void TryRotate(float angle)  //回転時に回転出来るかどうか
    {

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        transform.Rotate(0, 0, angle);

        if(IsValidPosition())
        {
            return;
        }

        Vector3[] kicks = GetWallKickOffsets();

        foreach (Vector3 offset in kicks)
        {
            transform.position = originalPosition + offset;

            if (IsValidPosition())
                return; // OKならこの位置を採用
        }

        // どれもうまくいかなかったら元に戻す
        transform.position = originalPosition;
        transform.rotation = originalRotation;
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

    Vector3[] GetWallKickOffsets()
    {
        switch (type)
        {
            case TetrominoType.I:
                return new Vector3[]
                {
                    new Vector3(1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(2, 0, 0),
                    new Vector3(-2, 0, 0),
                    new Vector3(0, 0, 0),
                };

            case TetrominoType.L:
            case TetrominoType.J:
                return new Vector3[]
                {
                    new Vector3(1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 1, 0),
                };

            case TetrominoType.T:
            case TetrominoType.S:
            case TetrominoType.Z:
                return new Vector3[]
                {
                    new Vector3(1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 1, 0),
                };

            case TetrominoType.O:
            default:
                return new Vector3[0]; // Oミノなどはkick不要
        }
    }

    /*bool TryWallKick(float angle)　　//回転した時のキックバック
    {
        Vector3[] kicks = GetWallKickOffsets();
        

        foreach(Vector3 offset in kicks)
        {
            transform.position += offset;
            transform.Rotate(0, 0, angle);

            if(IsValidPosition())
                return true;

            transform.position -= offset;
            transform.Rotate(0, 0, -angle);
        }

        return false;
    }*/

    //地面に触れているかの判定
    bool IsTouchingGround()
    {
        foreach (Transform block in transform)
        {
            Vector2Int pos = GridManager.RoundVector(block.position + Vector3.down);

            //範囲外なら床に接触しているとみなす
            if (!GridManager.InsideGrid(pos) || GridManager.grid[pos.x, pos.y] != null)
                    return true;
        }

        return false;
    }

    void CheckAndLock()
    {
        if(transform.position == lastPosition)
        {
            touciTime += Time.fixedDeltaTime;

            if(touciTime > lockDelay)
            {
                LockAndSpawnNew();
            }
        }
        else
        {
            touciTime = 0f;
            lastPosition = transform.position;
        }
    }

    void LockAndSpawnNew()
    {
        //生成されたミノをグリッドに登録
        foreach(Transform block in transform)
        {
            Vector2Int pos = GridManager.RoundVector(block.position);
            
            if(GridManager.InsideGrid(pos))
            {
                GridManager.grid[pos.x, pos.y] = block;
            }
        Debug.Log($"グリッド登録: {pos.x}, {pos.y}");
        }

        //次のミノを出す処理(仮の処理）
        TetrominoSpawner spawner = FindObjectOfType<TetrominoSpawner>();
        if (spawner != null)
        {
            spawner.SpawnRandomTetromino();
        }
        else
        {
            Debug.LogError("Spawnerが見つかりません");
        }



        //現在のミノのスクリプトを破棄
        Destroy(this);

        GridManager.CheckForLines();
    }
}
