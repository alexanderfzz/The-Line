using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private GameObject player;
    private Vector2Int gridDimension;
    private Vector2 screenSize;
    private float wallHeight;
    private float wallWidth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        screenSize = GameManager.instance.ScreenSize;
        gridDimension = GameManager.instance.GridDimension;
        wallHeight = screenSize.y / gridDimension.y;
        wallWidth = screenSize.x / gridDimension.x;
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerCollision();
    }

    private void checkPlayerCollision() {
        // The wall cannot collide with the player if it is not current in use
        if (!transform.parent.GetComponent<WallRow>().Activated) {
            return;
        }
        float playerRadius = player.GetComponent<Player>().PlayerRadius;
        // The wall cannot collide with the player if it is beneath the player
        if (player.transform.position.y - playerRadius >  transform.position.y + wallHeight/2) {
            return;
        } 

        if (GameManager.instance.RectObjPlayerCollision(gameObject, wallHeight, wallWidth)) {
            player.GetComponent<Player>().OnPlayerCollision(gameObject);
        }
    }
}


