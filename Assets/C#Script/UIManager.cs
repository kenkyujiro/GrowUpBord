using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI DiceText;//出目の表示テキスト
    public GameObject DiceTextObj;  //出目の表示パネル
    public GameObject DiceButtonObj;//ダイスボタン

    public TextMeshProUGUI ClickText;//Click!!テキスト

    public GameObject BranchArrow;//分岐の矢印

    void Start()
    {
        //ダイスボタン以外非表示にする
        DiceTextObj.gameObject.SetActive(false);
        DiceButtonObj.gameObject.SetActive(true);
        ClickText.gameObject.SetActive(false);
        BranchArrow.gameObject.SetActive(false);
    }

    public void ManageDiceUI(bool DicePanel, bool DiceButton)
    {
        DiceTextObj.gameObject.SetActive(DicePanel);
        DiceButtonObj.gameObject.SetActive(DiceButton);
    }

    public void ManageClickText(bool How_first)
    {
        ClickText.gameObject.SetActive(How_first);
    }

    public void ManageBrunchArrow(bool BrunchArrow)
    {
        BranchArrow.gameObject.SetActive(BrunchArrow);
    }
}
