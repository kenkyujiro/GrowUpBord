using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovePiece : MonoBehaviour
{
    public GameObject GameManager;     //�Q�[���N���Ascript�֎Q�Ɨp
    private GameClearManager clear;

    public GameObject DiceSystem;      //�o�ڃe�L�X�g�Q�Ɨp
    private DiceRollSystem rollSystem;

    public GameObject uiManager;       //UI�\���Ǘ��p
    private UIManager uiManage;

    int GoPiece;                       //�i�߂�o�ڂ̐�
    bool How_First;                    //�n�߂Ẵ_�C�X���[�����ǂ���

    private void Start()
    {
        clear = GameManager.GetComponent<GameClearManager>();
        rollSystem = DiceSystem.GetComponent<DiceRollSystem>();
        uiManage = uiManager.GetComponent<UIManager>();

        How_First = true;
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

        if(How_First == true)//�n�߂ă_�C�X���[������Ȃ�
        {
            //ManageClickText(bool How_first)
            uiManage.ManageClickText(How_First);
            How_First = false;
        }
    }

    private void OnMouseDown()
    {

        if(GoPiece != 0)
        {
            //-1�����炷
            transform.position += new Vector3(-1, 0, 0);
            GoPiece -= 1;
            rollSystem.UpdateText(GoPiece);
            //�������C���ȏ�ł���΃N���A
            if(transform.position.x <= -7) 
            {
                Debug.Log("�Q�[���N���A!!");
                clear.GameClear();
            }
            if(GoPiece == 0)//�o�ڕ������݂��������\������
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
            }

            uiManage.ManageClickText(How_First);
        }
        else
        {
            Debug.Log("�_�C�X��U���Ă��Ȃ���");
        }
    }
}
