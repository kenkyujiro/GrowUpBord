using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatusManager : MonoBehaviour
{
    public TextMeshProUGUI Player_Level;
    public TextMeshProUGUI Player_HP;
    public TextMeshProUGUI Player_EXP;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //�V�[���ɑJ�ڂ��邽�тɎ��s
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�����V�[������ Text ���Č���
        Player_Level = GameObject.FindWithTag("LevelText")?.GetComponent<TextMeshProUGUI>();
        Player_HP = GameObject.FindWithTag("HPText")?.GetComponent<TextMeshProUGUI>();
        Player_EXP = GameObject.FindWithTag("EXPText")?.GetComponent<TextMeshProUGUI>();

    }

    public void changeLevel(int level)
    {
        Player_Level.text = "Lv:" + level.ToString();
    }

    public void changeHP(int hp)
    {
        Player_HP.text = "HP:" + hp.ToString();
    }

    public void changeEXP(int exp)
    {
        Player_EXP.text = "EXP:" + exp.ToString();
    }
}
