using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPU : PowerUp
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
    override public void ApplyEffect() {
        if (effectApplied) {
            return;
        }
        effectApplied = true;
        
        player.GetComponent<Player>().ActivePowerUp = gameObject.name.Split("(Clone)")[0];
        player.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(RemoveEffect());
    }

    IEnumerator RemoveEffect() {
        yield return new WaitForSeconds(duration);
        player.GetComponent<SpriteRenderer>().color = Color.white;
        player.GetComponent<Player>().ActivePowerUp = null;
    }
}
