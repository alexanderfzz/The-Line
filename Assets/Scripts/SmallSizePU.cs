using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SmallSizePu : PowerUp
{
    private float smallSizeRatio = 2f;

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

    public override void ApplyEffect()
    {
        if (effectApplied) {
            return;
        }
        effectApplied = true;

        player.GetComponent<Player>().ActivePowerUp = gameObject.name.Split("(Clone)")[0];
        player.transform.localScale /= smallSizeRatio;
        player.GetComponent<Player>().PlayerRadius /= smallSizeRatio;
        GameManager.instance.CollisionGraceDist /= smallSizeRatio;
        player.GetComponent<SpriteRenderer>().color = Color.yellow;
        
        StartCoroutine(RemoveEffect());
    }

    IEnumerator RemoveEffect() {
        yield return new WaitForSeconds(duration);
        player.transform.localScale *= smallSizeRatio;
        player.GetComponent<Player>().PlayerRadius *= smallSizeRatio;
        GameManager.instance.CollisionGraceDist *= smallSizeRatio;
        player.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
