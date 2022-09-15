using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyOnWater : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") == true)
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }
}
