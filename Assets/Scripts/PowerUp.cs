using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected GameObject wallGenerator;
    protected GameObject player;
    protected Vector2 screenSize;
    protected float powerUpWidth;
    protected float duration = 5f;
    protected bool effectApplied = false;
    public bool EffectApplied {get {return effectApplied;} set {effectApplied = value;}}
    private bool activated;
    public bool Activated {get {return activated;} set {activated = value;}}
    private float speed;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.Find("Player");
        wallGenerator = GameObject.Find("WallGenerator");
        Vector2Int gridDimension = GameManager.instance.GridDimension;
        screenSize = GameManager.instance.ScreenSize;
        
        float wallPUScaleRatio = GameManager.instance.WallPUScaleRatio;
        powerUpWidth = screenSize.x/gridDimension.x/wallPUScaleRatio;
        
        float powerUpWidthScale = (float) 1/gridDimension.x/wallPUScaleRatio;
        transform.localScale = new Vector3(powerUpWidthScale, powerUpWidthScale, transform.localScale.z);

        speed = GameManager.instance.MapSpeed;
        gameObject.tag = "PowerUp";
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (activated) {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (transform.position.y < -(screenSize.y + powerUpWidth)/2) {
            transform.localPosition = wallGenerator.transform.position;
            wallGenerator.GetComponent<WallGenerator>().RecyclePU(gameObject);
        }

        CheckPlayerCollision();
    }

    public abstract void ApplyEffect();

    private void CheckPlayerCollision() {
        if (!activated) {
            return;
        }

        float playerRadius = player.GetComponent<Player>().PlayerRadius;
        if (player.transform.position.y - playerRadius >  transform.position.y + powerUpWidth/2) {
            return;
        } 

        if (GameManager.instance.RectObjPlayerCollision(gameObject, powerUpWidth, powerUpWidth)) {
            player.GetComponent<Player>().OnPlayerCollision(gameObject);
        }
    }
}
