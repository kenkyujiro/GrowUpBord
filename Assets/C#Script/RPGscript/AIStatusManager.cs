using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIStatusManager : MonoBehaviour
{
    public TextMeshProUGUI AI_HP;

    //�{script���L�������ꂽ�Ƃ�
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //�{script�����������ꂽ�Ƃ�
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //�V�[���ɑJ�ڂ��邽�тɎ��s OnSceneLoaded(�ǂݍ��܂ꂽ�V�[�����, �V�[���̓ǂݍ��ݕ��@)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�����V�[������ Text ���Č���
        AI_HP = GameObject.FindWithTag("HPText")?.GetComponent<TextMeshProUGUI>();

    }

    public void changeHP(int hp)
    {
        AI_HP.text = "HP:" + hp.ToString();
    }
}
