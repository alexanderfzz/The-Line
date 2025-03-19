using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    private Vector2 screenSize;
    private Vector2Int gridDimension;
    [SerializeField] private GameObject wallRow;
    private Queue<GameObject> wallRowPool = new Queue<GameObject>();
    private Dictionary<string, Queue<GameObject>> powerUpPool = new Dictionary<string, Queue<GameObject>>();
    private int rowsSinceLastPowerUp = 0;
    public int RowsSinceLastPowerUp {get {return rowsSinceLastPowerUp;} set{rowsSinceLastPowerUp = value;}}



    // Start is called before the first frame update
    void Start()
    {
        screenSize = GameManager.instance.ScreenSize;
        gridDimension = GameManager.instance.GridDimension;

        float wallHeight = screenSize.y / gridDimension.y;

        transform.position = new Vector3(0, (screenSize.y + wallHeight)/2, 0);
        transform.localScale = new Vector3(screenSize.x, screenSize.y, transform.localScale.z);


        for (int i=0; i<=gridDimension.y + 3; i++) {
            GameObject newWallRow = Instantiate(wallRow, transform);
            wallRowPool.Enqueue(newWallRow);
        }


        GameObject[] powerUps = GameManager.instance.PowerUps;
        for (int i=0; i<powerUps.Length; i++) {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            powerUpPool.Add(powerUps[i].name, newQueue);
            for (int j=0; j<3; j++) {
                newQueue.Enqueue(Instantiate(powerUps[i], transform));
            }
        }
        
        generateNextRow(Random.Range(0, gridDimension.x), (screenSize.y + wallHeight)/2, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void generateNextRow(int startIndex, float spawnYPos, bool previousRowHasHorizontalPath) {
        GameObject nextWallRow = wallRowPool.Dequeue();
        nextWallRow.GetComponent<WallRow>().ActivateRow(startIndex, spawnYPos, previousRowHasHorizontalPath);
    }

    public void RecycleWalls(GameObject wallRow) {
        wallRow.GetComponent<WallRow>().Activated = false;
        wallRowPool.Enqueue(wallRow);
    }

    public void RecyclePU(GameObject powerUp) {
        powerUp.GetComponent<PowerUp>().EffectApplied = false;
        powerUp.GetComponent<PowerUp>().Activated = false;
        powerUp.GetComponent<SpriteRenderer>().enabled = false;
        powerUpPool[powerUp.name.Split("(Clone)")[0]].Enqueue(powerUp);
    }

    public GameObject PopPowerUp() {
        return powerUpPool.ElementAt(Random.Range(0, powerUpPool.Count)).Value.Dequeue();
    }
}

