using UnityEngine;

public class WallRow : MonoBehaviour
{
    private Vector2 screenSize;
    private Vector2Int gridDimension;
    [SerializeField] private GameObject wall;
    private float speed;
    private float wallHeight;
    private float wallWidth;
    private bool activated = false;
    public bool Activated {get {return activated;} set {activated = value;}}
    private bool isTopRow = false;
    private int nextRowStartIndex;
    private bool nextRowCanHaveHorizontalPath;


    // Start is called before the first frame update
    void Start()
    {
        gridDimension = GameManager.instance.GridDimension;
        screenSize = GameManager.instance.ScreenSize;
        speed = GameManager.instance.MapSpeed;

        float wallHeightScale = (float) 1 / gridDimension.y;
        float wallWidthScale = (float) 1 / gridDimension.x;
        wallHeight = screenSize.y / gridDimension.y;
        wallWidth = screenSize.x / gridDimension.x;

        for (int i=0; i<gridDimension.x; i++) {
            GameObject newWall = Instantiate(wall, transform);
            newWall.transform.localScale = new Vector3(wallWidthScale, wallHeightScale, transform.localScale.z);
            newWall.transform.position = transform.position + new Vector3((i-(gridDimension.x-1)*0.5f) * wallWidth, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        // when the top row fully falls into screen
        if (transform.position.y <= (screenSize.y - wallHeight)/2 && isTopRow) {
            isTopRow = false;
            transform.parent.GetComponent<WallGenerator>().generateNextRow(nextRowStartIndex, transform.position.y, nextRowCanHaveHorizontalPath);
        }

        // when the walls fall out of the viewport
        if (transform.position.y < -(screenSize.y + wallHeight)/2) {
            transform.localPosition = Vector3.zero;
            transform.parent.GetComponent<WallGenerator>().RecycleWalls(gameObject);
        }
    }

    public void ActivateRow(int startIndex, float spawnYPos, bool previousRowHasHorizontalPath) {
        transform.parent.GetComponent<WallGenerator>().RowsSinceLastPowerUp++;
        int powerUpGenerationCoolDown = GameManager.instance.PowerUpGenerationCoolDown;

        bool containsPowerUp = false;
        if (transform.parent.GetComponent<WallGenerator>().RowsSinceLastPowerUp >= powerUpGenerationCoolDown) {
            containsPowerUp = Random.Range(1, 5) == 1;
        }

        bool hasHorizontalPath = previousRowHasHorizontalPath ? false : Random.Range(1,3) == 1;
        nextRowCanHaveHorizontalPath = hasHorizontalPath;
        
        // false is left, true is right
        bool horizontalPathDir = false;
        int horizontalPathWidth = 0; 
        if (hasHorizontalPath) {
            int horizontalPathMaxWidth;
            if (startIndex == 0)
            {
                horizontalPathDir = true;
                horizontalPathMaxWidth = gridDimension.x - 1;
            }
            else if (startIndex == gridDimension.x - 1)
            {
                horizontalPathDir = false;
                horizontalPathMaxWidth = gridDimension.x - 1;
            }
            else
            {
                horizontalPathDir = Random.Range(0, 1) % 2 == 0;
                horizontalPathMaxWidth = horizontalPathDir ? gridDimension.x - startIndex - 1 : startIndex;
            }
            horizontalPathWidth = Random.Range(1, horizontalPathMaxWidth);
        }

        nextRowStartIndex = horizontalPathDir ? startIndex + horizontalPathWidth : startIndex - horizontalPathWidth;

        for (int i=0; i<gridDimension.x; i++) {
            GameObject currentWall = transform.GetChild(i).gameObject;
            currentWall.SetActive(true);
            if (containsPowerUp && i == startIndex) {
                GameObject newPowerUp = transform.parent.GetComponent<WallGenerator>().PopPowerUp();
                newPowerUp.transform.position = transform.position + new Vector3((i-(gridDimension.x-1)*0.5f) * wallWidth, 0, 0);
                newPowerUp.GetComponent<PowerUp>().Activated = true;
                transform.parent.GetComponent<WallGenerator>().RowsSinceLastPowerUp = 0;
            }
            if (!horizontalPathDir && i >= startIndex - horizontalPathWidth && i <= startIndex) {
                currentWall.SetActive(false);
                continue;
            }
            if (horizontalPathDir && i >= startIndex && i <= startIndex + horizontalPathWidth) {
                currentWall.SetActive(false);
                continue;
            }
            currentWall.transform.position = new Vector3((i-(gridDimension.x-1)*0.5f) * wallWidth, spawnYPos + wallHeight, 0);
        }

        isTopRow = true;
        activated = true;
    }
}
