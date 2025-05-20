using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] tetrominoPrefab;  //ƒ~ƒm‚ÌƒŠƒXƒg


    void Start()
    {
        SpawnRandomTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRandomTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoPrefab.Length);
        GameObject selected = tetrominoPrefab[randomIndex];
        Instantiate(selected, new Vector3(5, 20, 0), Quaternion.identity);
    }
}
