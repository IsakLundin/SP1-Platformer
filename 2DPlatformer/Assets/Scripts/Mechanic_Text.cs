using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mechanic_Text : MonoBehaviour
{
    [SerializeField] private GameObject mechanicText;
    [SerializeField] private Text text;
    [SerializeField] private string whatToSay;

    private void Start()
    {
        mechanicText.SetActive(false);
        text.text = whatToSay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mechanicText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            mechanicText.SetActive(false);
    }
}
