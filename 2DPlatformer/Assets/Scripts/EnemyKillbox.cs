using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyKillbox : MonoBehaviour
{

    GameObject gameObjectToKill;

    private void Start()
    {
        gameObjectToKill = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") == true)
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().isFalling() == true)
            {
                if (gameObject.transform.parent.CompareTag("EnemySlime"))
                    gameObject.GetComponentInParent<Enemy_SlimeMovement>().KillMe();
                else if (gameObject.transform.parent.CompareTag("EnemyFly"))
                    gameObject.GetComponentInParent<Enemy_FlyMovement>().KillMe();
                //Destroy(gameObjectToKill);
            }
        }
    }
}
