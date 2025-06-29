using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;//�����X�R�A

    void Start()
    {
        //�X�R�A�̏�����
        UpdateScore();
    }

    private void Update()
    {
        //��b���ƂɃJ�E���g��60f��1�b
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
        //�X�R�A���e�L�X�g�Ƃ��Ĕ��f����
        scoreText.text = "Score: " + score.ToString();
    }
}
  