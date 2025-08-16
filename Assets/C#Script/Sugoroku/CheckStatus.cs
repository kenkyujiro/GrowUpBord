using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckStatus : MonoBehaviour
{
    public GameObject playerStatus;
    private PlayerStatus playerST;

    public GameObject AIStatus;
    private AIStatus AIST;

    public TextMeshProUGUI Player_Level;
    public TextMeshProUGUI Player_HP;
    public TextMeshProUGUI Player_Power;
    public TextMeshProUGUI Player_Defence;
    public TextMeshProUGUI Player_Luck;
    public TextMeshProUGUI Player_EXP;

    //AIのステータスも追加する
    public TextMeshProUGUI AI_HP;
    public TextMeshProUGUI AI_Power;
    public TextMeshProUGUI AI_Defence;
    public TextMeshProUGUI AI_Luck;

    void Start()
    {
        gameObject.SetActive(false);
        playerStatus = GameObject.Find("PlayerStatus");
        playerST = playerStatus.GetComponent<PlayerStatus>();
        AIStatus = GameObject.Find("AIStatus");
        AIST = AIStatus.GetComponent<AIStatus>();
    }

    //UIの表示を切り替える
    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        //ステータスの反映
        Player_Level.text = "Level:" + playerST.Level.ToString();
        Player_HP.text = "HP:" + playerST.hp.ToString();
        Player_Power.text = "Power:" + playerST.power.ToString();
        Player_Defence.text = "Defence:" + playerST.defence.ToString();
        Player_Luck.text = "Luck:" + playerST.luck.ToString();
        Player_EXP.text = "EXP:" + playerST.exp.ToString();

        AI_HP.text = "HP:" + AIST.hp.ToString();
        AI_Power.text = "Power:" + AIST.power.ToString();
        AI_Defence.text = "Defence:" + AIST.defence.ToString();
        AI_Luck.text = "Luck:" + AIST.luck.ToString();
    }
}
