using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovePiece : MonoBehaviour
{
    public GameObject GameManager;
    private GameClearManager clear;

    int GoPiece;

    private void Start()
    {
        clear = GameManager.GetComponent<GameClearManager>();
    }

    public void GetValue() 
    {
        //�o���ڕ��������ꍇ�̂݋N���\
        if (GoPiece == 0)
        {
            //�T�C�R����U��(�{�^���ɃA�^�b�`)
            GoPiece = FindObjectOfType<DiceRollSystem>().GetDice();
        }
        else 
        {
            Debug.Log("�R�}�𓮂����Ă�");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("�����Ă���");

        if(GoPiece != 0)
        {
            //-1�����炷
            transform.position += new Vector3(-1, 0, 0);
            GoPiece -= 1;
            //�������C���ȏ�ł���΃N���A
            if(transform.position.x <= -7) 
            {
                Debug.Log("�Q�[���N���A!!");
                clear.GameClear();
            }
        }
        else
        {
            Debug.Log("�_�C�X��U���Ă��Ȃ���");
        }
    }
}
