using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;//内部スコア

    void Start()
    {
        //スコアの初期化
        UpdateScore();
    }

    private void Update()
    {
        //一秒ごとにカウント※60fで1秒
        if(Time.frameCount % 60 == 0) 
        {
            AddScore(1);
        }
    }

    public void AddScore(int points) 
    {
        score += points;
        UpdateScore();
    }
    void UpdateScore()
    {
        //スコアをテキストとして反映する
        scoreText.text = "Score: " + score.ToString();
    }
}
  