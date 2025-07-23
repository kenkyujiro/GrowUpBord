using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public GameObject EventManager;    //�C�x���g�Ǘ��p
    private EventManager eventManager;

    public int GoPiece;                //�i�߂�o�ڂ̐�
    private bool How_First;            //�n�߂Ẵ_�C�X���[�����ǂ���
    public bool How_Branch;            //����ɐi�񂾂��ǂ���
    public bool canClick;              //�N���b�N�ł��邩�ǂ���

    private void Start()
    {
        clear = GameManager.GetComponent<GameClearManager>();
        rollSystem = DiceSystem.GetComponent<DiceRollSystem>();
        uiManage = uiManager.GetComponent<UIManager>();
        eventManager = EventManager.GetComponent<EventManager>();

        How_First = true;
        canClick = true;
        How_Branch = false;
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
        //�����I�����Ȃ�����A�������Ȃ��悤�ɂ���
        if (!canClick)
        {
            Debug.Log("�͂�̂����Ȃ���");
            return;
        }

        //�_�C�X�̖ڂ��c���Ă�����i��
        if(GoPiece != 0)
        {
            changePos();

            GoPiece -= 1;
            rollSystem.UpdateText(GoPiece);
            //�������C���ȏ�ł���΃N���A
            if(transform.position.x == -6) 
            {
                Debug.Log("�Q�[���N���A!!");
                clear.GameClear();
            }
            if(GoPiece == 0)//�o�ڕ������݂��������\������
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
                //�}�X��0�̎��ɃC�x���g�𔭉΂���
                eventManager.IgnitionEvent();
            }

            uiManage.ManageClickText(How_First);
        }
        else
        {
            Debug.Log("�_�C�X��U���Ă��Ȃ���");
        }
    }

    //�v���C���[�̃R�}��i�߂�֐�
    private void changePos() 
    {

        //����ɐi�ނƌ��߂��Ƃ�
        if (How_Branch == true)
        {
            transform.position += new Vector3(0, 1, 0);
            //����A����ɕ�����d�˂�\��ł��邽�߁A������false�ɂ��Ă���
            How_Branch = false;
        }
        //�����i��ł���Ƃ�
        else if (transform.position.y >= 0)
        {
            //����̊p��1�ڂɂ��Ă��Ȃ��Ƃ�
            if (transform.position.y < 3 && transform.position.x < 3)
            {
                transform.position += new Vector3(0, 1, 0);
            }
            //����̊p��2�ڂɂ��Ă��Ȃ��Ƃ�
            else if(transform.position.x < 3) 
            {
                transform.position += new Vector3(1, 0, 0);
            }
            //����̊p��2�ڂɂ����Ƃ�
            else
            {
                transform.position += new Vector3(0, -1, 0);
            }
        }
        //����ɐi�܂Ȃ��ƌ��߂��Ƃ��A�܂��͕���ɂ��ǂ蒅���Ă��Ȃ��Ƃ�
        else
        {
            //-1�����炷
            transform.position += new Vector3(-1, 0, 0);
            //����_�ɒ������Ƃ�
            if(transform.position.x == -2) 
            {
                canClick = false;
                howGO_Branch();
            }
        }
        
        
    }

    //����̑I�������֐�
    private void howGO_Branch()
    {
        //����̖��̕\��������OnMouseDown���t���Ă���
        uiManage.ManageBrunchArrow(true);
    }

    
}
