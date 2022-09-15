using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
    public static void SaveCoinsToMemory(int amount)
    {
        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount") + amount);
    }
}
