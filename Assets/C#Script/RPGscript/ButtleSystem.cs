using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtleSystem : MonoBehaviour
{
    public static ButtleSystem Instance { get; private set; }

    public TextMeshProUGUI GoalText;

    public GameObject uiManager;    //UI�\���Ǘ��p
    private UIManager uiManage;

    private void Awake()
    {
        // �V���O���g������������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // UIManager �̎擾
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
