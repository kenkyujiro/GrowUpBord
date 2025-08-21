using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartManager : MonoBehaviour
{
    public GameObject Player;     //�R�}�Q�Ɨp

    public GameObject playerStatus;         //�v���C���[�X�e�[�^�X�̎Q��
    private PlayerStatus playerST;

    public GameObject aiStatus;
    private AIStatus AIST;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //�I�u�W�F�N�g�̎擾
        playerStatus = GameObject.Find("PlayerStatus");
        aiStatus = GameObject.Find("AIStatus");

        playerST = playerStatus.GetComponent<PlayerStatus>();
        AIST = aiStatus.GetComponent<AIStatus>();
    }

    public void ResetStaus()
    {
        //�X�e�[�^�X�̃��Z�b�g
        playerST.hp = 20;
        playerST.power = 1;
        playerST.defence = 1;
        playerST.luck = 1;
        playerST.Max_hp = 20;
        playerST.Level = 1;
        playerST.exp = 0;
        playerST.mp = 25;

        //�X�e�[�^�X�̃��Z�b�g
        AIST.maxHP = 30;
        AIST.hp = 30;
        AIST.power = 5;
        AIST.defence = 10;
        AIST.luck = 0;
        AIST.mp = 25;

        //���W�̃��Z�b�g
        PlayerPrefs.SetFloat("x", 8);
        PlayerPrefs.SetFloat("y", -1);
        PlayerPrefs.SetFloat("z", 0);
        PlayerPrefs.Save();
    }
}
