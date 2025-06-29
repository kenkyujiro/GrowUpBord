using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    //ゲームオーバー時に呼び出す関数
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Debug.Log("Game Over");
        //シーンをリロードしてリスタート
        //※これでスコアが0に戻る
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 0;
    }
}
