using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject Player;     //���ݎ~�܂��Ă���}�X�Q�Ɨp
    private MovePiece movePiece;

    void Awake()
    {
        movePiece = Player.GetComponent<MovePiece>();
    }

    //���΂����C�x���g�̏���
    public void IgnitionEvent()
    {
        //x (-5�`7)(y�F-1�̂�)�A y (-1�`3)(x�F-2�`3�̂�)
        //����ŏ�(-2 -1) ����Ō�(3 -1)
        if(movePiece.GoPiece == 0) 
        {
            if (Player.transform.position.x == -2)
            {
                if (Player.transform.position.y == 0)//(-2 0)
                {
                    Debug.Log("�����I");
                    //�l��ۑ�����
                    PlayerPrefs.SetString("monsterName", "Bison");
                    //RPG�V�[���ɑJ�ڂ���
                    SceneManager.LoadScene("RPGScene");
                }
                else if(Player.transform.position.y == 3)
                {
                    Debug.Log("�������I");
                }
            }
            else if (Player.transform.position.x == -5)//(-5 -1) 
            {
                Debug.Log("�ŏ����炾�I");
                Player.transform.position += new Vector3(13, 0, 0);
            }
        }
    }

    public void testChange()
    {
        //�l��ۑ�����
        PlayerPrefs.SetString("monsterName", "Zako1");
        //RPG�V�[���ɑJ�ڂ���
        SceneManager.LoadScene("RPGScene");
    }
}
