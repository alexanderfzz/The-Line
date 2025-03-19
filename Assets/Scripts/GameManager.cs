using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Vector2Int gridDimension;
    public Vector2Int GridDimension {get {return gridDimension;}}
    [SerializeField] private float mapSpeed;
    public float MapSpeed {get {return mapSpeed;}}
    private Vector2 screenSize;
    public Vector2 ScreenSize {get {return screenSize;}}
    private float wallPlayerScaleRatio = 2.5f;
    public float WallPlayerScaleRatio {get {return wallPlayerScaleRatio;}}
    private float wallPUScaleRatio = 4;
    public float WallPUScaleRatio {get {return wallPUScaleRatio;}}

    private float collisionGraceDist = 0.15f;
    public float CollisionGraceDist {get {return collisionGraceDist;} set {collisionGraceDist = value;}}
    private float score = 0;
    private float timeScoreMultiplier = 3;
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool gameOverState = false;
    public bool GameOverState {get {return gameOverState;} set{ gameOverState = value;}}
    [SerializeField] private GameObject PausedPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverPanelScoreText;
    [SerializeField] private GameObject[] powerUps;
    public GameObject[] PowerUps {get {return powerUps;}}
    [SerializeField] private int powerUpGenerationCoolDown;
    public int PowerUpGenerationCoolDown {get {return powerUpGenerationCoolDown;}}
    [SerializeField] private GameObject player;


    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        float screenHeight = 2f * Camera.main.orthographicSize;
        float screenWidth = screenHeight * Camera.main.aspect;
        screenSize = new Vector2(screenWidth, screenHeight);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * timeScoreMultiplier;
        scoreText.text = ((int) score).ToString("0000");
    }


    public void gameOver() {
        if (gameOverState) {
            return;
        }
        gameOverState = true;
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
        gameOverPanelScoreText.text = "Score: "+((int) score).ToString("0000");
    }

    public void PauseLevel() {
        Time.timeScale = 0;
        PausedPanel.SetActive(true);
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueLevel() {
        Time.timeScale = 1;
        PausedPanel.SetActive(false);
    }


    public bool RectObjPlayerCollision(GameObject gameObject, float height, float width) {
        float verticalEdgeX = player.transform.position.x;
        float horizontalEdgeY = player.transform.position.y;

        if (player.transform.position.x < gameObject.transform.position.x - width/2) {
            verticalEdgeX = gameObject.transform.position.x - width/2;
        } else if (player.transform.position.x > gameObject.transform.position.x + width/2){
            verticalEdgeX = gameObject.transform.position.x + width/2;
        } 

        if (player.transform.position.y < gameObject.transform.position.y - height/2) {
            horizontalEdgeY = gameObject.transform.position.y - height/2;
        } else if (player.transform.position.y > gameObject.transform.position.y + height/2){
            horizontalEdgeY = gameObject.transform.position.y + height/2;
        }

        float deltaX = player.transform.position.x - verticalEdgeX;
        float deltaY = player.transform.position.y - horizontalEdgeY; 
        float deltaDist = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        float playerRadius = player.GetComponent<Player>().PlayerRadius;
        return deltaDist + collisionGraceDist < playerRadius;
    }
}


