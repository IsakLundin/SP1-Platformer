using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlyTargetRange : MonoBehaviour
{
    [SerializeField] private Transform startPos;

    private void Start()
    {
        transform.parent.GetComponent<Enemy_FlyMovement>().target = startPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<Enemy_FlyMovement>().target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<Enemy_FlyMovement>().target = startPos;
        }
    }
}
