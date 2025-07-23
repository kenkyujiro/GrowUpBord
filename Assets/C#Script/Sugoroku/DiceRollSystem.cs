using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DiceRollSystem : MonoBehaviour
{
    public TextMeshProUGUI DiceText;

    public GameObject uiManager;    //UI表示管理用
    private UIManager uiManage;

    int value_dice;

    private void Start()
    {
        uiManage = uiManager.GetComponent<UIManager>();
    }

    public int GetDice()
    {
        value_dice = Random.Range(1, 7);           //1～6の整数を返す
        //ManageDiceUI(bool DicePanel, bool DiceButton)
        uiManage.ManageDiceUI(true, false);
        //出た数値を表示する
        UpdateText(value_dice);
        return value_dice;
    }

    public  void UpdateText(int Now_value)//別scriptでも使うため、value_diceはそのまま使えない 
    {
        DiceText.text = Now_value.ToString();
    }
}
