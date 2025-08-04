using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckStatus : MonoBehaviour
{
    public GameObject playerStatus;
    private PlayerStatus playerST;

    public TextMeshProUGUI Player_Level;
    public TextMeshProUGUI Player_HP;
    public TextMeshProUGUI Player_Power;
    public TextMeshProUGUI Player_Defence;
    public TextMeshProUGUI Player_Luck;
    public TextMeshProUGUI Player_EXP;

    void Start()
    {
        gameObject.SetActive(false);
        playerStatus = GameObject.Find("PlayerStatus");
        playerST = playerStatus.GetComponent<PlayerStatus>();
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
    }
}
