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

        //�V�[���������[�h���ă��X�^�[�g����֐�
        //������ŃX�R�A��0�ɖ߂�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //�Q�[�����~�߂Ă���
        Time.timeScale = 0;
    }
}
