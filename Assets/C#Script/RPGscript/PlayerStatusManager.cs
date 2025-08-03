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

    //本scriptが有効化されたとき
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //本scriptが無効化されたとき
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //シーンに遷移するたびに実行 OnSceneLoaded(読み込まれたシーン情報, シーンの読み込み方法)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーン内の Text を再検索
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
