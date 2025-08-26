using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject Player;      //���ݎ~�܂��Ă���}�X�Q�Ɨp

    public GameObject playerStatus;//�v���C���[�X�e�[�^�X�̋�������щ񕜗p
    private PlayerStatus playerST;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        playerStatus = GameObject.Find("PlayerStatus");
        playerST = Player.GetComponent<PlayerStatus>();
    }

    //���΂����C�x���g�̏���
    public void IgnitionEvent(float x_pos, float y_pos)
    {
        //��̃G���A
        //���G�]�[��
        if (y_pos >= 0)
        {
            System.Random random = new System.Random();
            int value;

            if (x_pos == -2 && y_pos == 3)
            {
                value = random.Next(0, 2);//0�`3�̗����𐶐�

                if (value == 0) { playerST.powerUp("power", 10, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 10, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 10, "none", 0); }

                Debug.Log("����");

            }
            else if(x_pos == 2 && y_pos == 1) 
            {
                value = random.Next(0, 2);//0�`3�̗����𐶐�

                if (value == 0) { playerST.powerUp("power", 10, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 10, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 10, "none", 0); }

                Debug.Log("����");
            }
            else if (x_pos == 1 && y_pos == 3)
            {
                Debug.Log("�񕜂��I");
                playerST.hp += 30;
                if (playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }

            value = random.Next(0, 100);//0�`99�̗����𐶐�
            if (value <= 50)
            {
                //�m���Ń����X�^�[�̎�ނ��ς��
                if (value <= 10)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Bison");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else if(value <= 40)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Run");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else 
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako2");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
            }
        }
        //���̃G���A
        else if(y_pos <= -2)
        {
            System.Random random = new System.Random();
            int value;

            if (y_pos == -4 && x_pos == 0)
            {
                value = random.Next(0, 2);//0�`3�̗����𐶐�

                if(value == 0){ playerST.powerUp("power", 5, "none", 0); }
                else if(value == 1){ playerST.powerUp("hp", 5, "none", 0); }
                else if(value == 2){ playerST.powerUp("defence", 5, "none", 0); }

                Debug.Log("����");
                
            }
            else if(y_pos == -2 && x_pos == 5)
            {
                value = random.Next(0, 2);//0�`3�̗����𐶐�

                if (value == 0) { playerST.powerUp("power", 5, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 5, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 5, "none", 0); }

                Debug.Log("����");
            }
            else if (y_pos == -5 && x_pos == 3)
            {
                Debug.Log("�񕜂��I");
                playerST.hp += 30;
                if (playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }

            value = random.Next(0, 100);//0�`99�̗����𐶐�
            if (value <= 50)
            {
                //�m���Ń����X�^�[�̎�ނ��ς��
                if (value <= 5)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Rare");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else if (value <= 15)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako2");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako1");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
            }
        }
        //�{�X�̃G���A(�}�X)
        else if(x_pos <= -6)
        {
            //�l��ۑ�����
            savePos();
            PlayerPrefs.SetString("monsterName", "Boss");
            //RPG�V�[���ɑJ�ڂ���
            SceneManager.LoadScene("RPGScene");
        }
        //�^�������̃G���A
        else
        {
            if(x_pos == 0)
            {
                Debug.Log("�񕜂��I");
                playerST.hp += 30;
                if(playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }
        }
    }

    public void testChange()
    {
        //�l��ۑ�����
        savePos();
        PlayerPrefs.SetString("monsterName", "Zako1");
        //RPG�V�[���ɑJ�ڂ���
        SceneManager.LoadScene("RPGScene");
    }

    public void savePos()
    {
        PlayerPrefs.SetFloat("x", Player.transform.position.x);
        PlayerPrefs.SetFloat("y", Player.transform.position.y);
        PlayerPrefs.SetFloat("z", Player.transform.position.z);
        PlayerPrefs.Save();
    }
}
