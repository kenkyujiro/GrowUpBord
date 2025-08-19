using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject Player;     //���ݎ~�܂��Ă���}�X�Q�Ɨp

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    //���΂����C�x���g�̏���
    public void IgnitionEvent(float x_pos, float y_pos)
    {
        //x (-5�`7)(y�F-1�̂�)�A y (-1�`3)(x�F-2�`3�̂�)
        //����ŏ�(-2 -1) ����Ō�(3 -1) -1�`2
        if (y_pos == 3)//���G�]�[��
        {
            System.Random random = new System.Random();
            int value = random.Next(0, 100);//0�`99�̗����𐶐�
            if (value <= 30)
            {
                if(value <= 5)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Bison");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else if(value <= 15)
                {
                    //�l��ۑ�����
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Rare");
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
        else if (x_pos == -2)
        {
            if (y_pos == 0)//(-2 0)
            {
                //�l��ۑ�����
                savePos();
                PlayerPrefs.SetString("monsterName", "Run");
                //RPG�V�[���ɑJ�ڂ���
                SceneManager.LoadScene("RPGScene");
            }
            else if(y_pos == 3)
            {
                Debug.Log("�������I");
            }
        }
        else if (x_pos >= -1 && x_pos <= 2)
        {
            System.Random random = new System.Random();
            int value = random.Next(0, 100);//0�`99�̗����𐶐�
            if(value == 30)
            {
                //�l��ۑ�����
                savePos();
                PlayerPrefs.SetString("monsterName", "Zako1");
                //RPG�V�[���ɑJ�ڂ���
                SceneManager.LoadScene("RPGScene");
            }
        }
        else if (x_pos == -5)//(-5 -1) 
        {
            Debug.Log("�ŏ����炾�I");
            Player.transform.position += new Vector3(13, 0, 0);
        }
        else if(x_pos <= -6)
        {
            //�l��ۑ�����
            savePos();
            PlayerPrefs.SetString("monsterName", "Boss");
            //RPG�V�[���ɑJ�ڂ���
            SceneManager.LoadScene("RPGScene");
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
