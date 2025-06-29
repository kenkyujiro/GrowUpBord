using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
