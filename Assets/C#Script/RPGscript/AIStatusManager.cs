using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIStatusManager : MonoBehaviour
{
    public TextMeshProUGUI AI_HP;

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
        AI_HP = GameObject.FindWithTag("HPText")?.GetComponent<TextMeshProUGUI>();

    }

    public void changeHP(int hp)
    {
        AI_HP.text = "HP:" + hp.ToString();
    }
}
