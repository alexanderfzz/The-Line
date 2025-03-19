using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector2 screenSize;

    // Start is called before the first frame update
    void Start()
    {
        screenSize = GameManager.instance.ScreenSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        updatePlayerPos();
    }
    void OnMouseDrag()
    {
        updatePlayerPos();
    }
    
    private void updatePlayerPos() {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3 newPlayerPos = new Vector3(mouseWorldPos.x, player.transform.position.y, player.transform.position.z);

        float playerRadius = player.GetComponent<Player>().PlayerRadius;
        newPlayerPos.x = Mathf.Min(newPlayerPos.x, screenSize.x/2 - playerRadius);
        newPlayerPos.x = Mathf.Max(newPlayerPos.x, -screenSize.x/2 + playerRadius);
        
        player.transform.position = newPlayerPos;
    }
}
