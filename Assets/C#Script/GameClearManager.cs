using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    public TextMeshProUGUI gameClearText;

    private void Start()
    {
        gameClearText.gameObject.SetActive(false);
    }

    public void GameClear()
    {
        gameClearText.gameObject.SetActive(true);
        Debug.Log("Game Over");
        //シーンをリロードしてリスタート
        //※これでスコアが0に戻る
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 0;
    }
}
