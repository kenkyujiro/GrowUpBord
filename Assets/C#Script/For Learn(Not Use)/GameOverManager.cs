using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    //�Q�[���I�[�o�[���ɌĂяo���֐�
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Debug.Log("Game Over");
        //�V�[���������[�h���ă��X�^�[�g
        //������ŃX�R�A��0�ɖ߂�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 0;
    }
}
