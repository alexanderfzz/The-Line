using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject wallGenerator;
    private float playerRadius;
    public float PlayerRadius {get {return playerRadius;} set{playerRadius = value;}}
    private string activePowerUp;
    public string ActivePowerUp {get {return activePowerUp;} set{activePowerUp = value;}}



    // Start is called before the first frame update
    void Start()
    {
        Vector2Int gridDimension = GameManager.instance.GridDimension;
        Vector2 screenSize = GameManager.instance.ScreenSize;

        float wallWidth = screenSize.x / gridDimension.x;
        float wallPlayerScaleRatio = GameManager.instance.WallPlayerScaleRatio;
        playerRadius = wallWidth / wallPlayerScaleRatio;

        transform.localScale = new Vector3(playerRadius, playerRadius, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerCollision(GameObject gameObject)
    {
        if (gameObject.CompareTag("Wall")) {
            if (activePowerUp != "InvincibilityPU") {
                GameManager.instance.gameOver();
                return;
            } else {
                gameObject.SetActive(false);
            }
            return;
        }
        if (gameObject.CompareTag("PowerUp")) {
            gameObject.GetComponent<PowerUp>().ApplyEffect();
            wallGenerator.GetComponent<WallGenerator>().RecyclePU(gameObject);
        }
    }
}

