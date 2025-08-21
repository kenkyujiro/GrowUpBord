using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartManager : MonoBehaviour
{
    public GameObject Player;     //コマ参照用

    public GameObject playerStatus;         //プレイヤーステータスの参照
    private PlayerStatus playerST;

    public GameObject aiStatus;
    private AIStatus AIST;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //オブジェクトの取得
        playerStatus = GameObject.Find("PlayerStatus");
        aiStatus = GameObject.Find("AIStatus");

        playerST = playerStatus.GetComponent<PlayerStatus>();
        AIST = aiStatus.GetComponent<AIStatus>();
    }

    public void ResetStaus()
    {
        //ステータスのリセット
        playerST.hp = 20;
        playerST.power = 1;
        playerST.defence = 1;
        playerST.luck = 1;
        playerST.Max_hp = 20;
        playerST.Level = 1;
        playerST.exp = 0;
        playerST.mp = 25;

        //ステータスのリセット
        AIST.maxHP = 30;
        AIST.hp = 30;
        AIST.power = 5;
        AIST.defence = 10;
        AIST.luck = 0;
        AIST.mp = 25;

        //座標のリセット
        PlayerPrefs.SetFloat("x", 8);
        PlayerPrefs.SetFloat("y", -1);
        PlayerPrefs.SetFloat("z", 0);
        PlayerPrefs.Save();
    }
}
