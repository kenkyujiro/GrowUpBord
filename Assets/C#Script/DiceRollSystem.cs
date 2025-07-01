using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DiceRollSystem : MonoBehaviour
{
    public TextMeshProUGUI DiceText;

    public GameObject uiManager;    //UI�\���Ǘ��p
    private UIManager uiManage;

    int value_dice;

    private void Start()
    {
        uiManage = uiManager.GetComponent<UIManager>();
    }

    public int GetDice()
    {
        value_dice = Random.Range(1, 7);           //1�`6�̐�����Ԃ�
        //ManageDiceUI(bool DicePanel, bool DiceButton)
        uiManage.ManageDiceUI(true, false);
        //�o�����l��\������
        UpdateText(value_dice);
        return value_dice;
    }

    public  void UpdateText(int Now_value)//��script�ł��g�����߁Avalue_dice�͂��̂܂܎g���Ȃ� 
    {
        DiceText.text = Now_value.ToString();
    }
}
