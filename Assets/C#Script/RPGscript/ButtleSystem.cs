using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtleSystem : MonoBehaviour
{
    public static ButtleSystem Instance { get; private set; }

    public TextMeshProUGUI GoalText;

    public GameObject uiManager;    //UI表示管理用
    private UIManager uiManage;

    private void Awake()
    {
        // シングルトン初期化処理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // UIManager の取得
        uiManage = uiManager.GetComponent<UIManager>();
    }

    //ManageDiceUI(bool DicePanel, bool DiceButton)
    public void ButtleStart()
    {
        GoalText.gameObject.SetActive(false);
        gameObject.SetActive(true);
        uiManage.ManageDiceUI(false, false);
    }

    public void ButtleEnd()
    {
        GoalText.gameObject.SetActive(true);
        gameObject.SetActive(false);
        uiManage.ManageDiceUI(false, true);
    }
}
