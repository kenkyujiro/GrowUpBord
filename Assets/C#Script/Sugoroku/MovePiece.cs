using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovePiece : MonoBehaviour
{
    public static MovePiece Instance { get; private set; }

    public GameObject DiceSystem;      //�o�ڃe�L�X�g�Q�Ɨp
    private DiceRollSystem rollSystem;

    public GameObject uiManager;       //UI�\���Ǘ��p
    private UIManager uiManage;

    public GameObject EventManager;    //�C�x���g�Ǘ��p
    private EventManager eventManager;

    public GameObject playerStatus;    //�v���C���[�X�e�[�^�X�̎Q��(�Q�[���N���A�m�F�p)
    private PlayerStatus playerST;

    public int GoPiece;                //�i�߂�o�ڂ̐�
    private bool How_First;            //�n�߂Ẵ_�C�X���[�����ǂ���
    public bool How_Branch1 = false;   //��ڂ̕���ɐi�񂾂��ǂ���
    public bool How_Branch2 = false;   //��ڂ̕���ɐi�񂾂��ǂ���
    public bool canClick;              //�N���b�N�ł��邩�ǂ���

    private void Awake()
    {
        // Singleton�p�^�[��: ���ł�GameManager�����݂���ꍇ�͐V�����C���X�^���X�����Ȃ�
        if (Instance != null)
        {
            Destroy(gameObject); // �����̃C���X�^���X������΁A���̃I�u�W�F�N�g���폜
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �V�[���J�ڌ�����̃I�u�W�F�N�g��j�����Ȃ�

        How_First = true;
        canClick = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //�R�}��\���ɂ���
        this.gameObject.SetActive(true);

        DiceSystem = GameObject.Find("DiceSystem");
        uiManager = GameObject.Find("UIManager");
        EventManager = GameObject.Find("EventManager");

        rollSystem = DiceSystem?.GetComponent<DiceRollSystem>();
        uiManage = uiManager?.GetComponent<UIManager>();
        eventManager = EventManager?.GetComponent<EventManager>();

        //�Q�[���N���A�m�F�p
        playerStatus = GameObject.Find("PlayerStatus");
        playerST = playerStatus.GetComponent<PlayerStatus>();
    }

    public void GetValue() 
    {
        //�o���ڕ��������ꍇ�̂݋N���\
        if (GoPiece == 0)
        {
            //�T�C�R����U��(�{�^���ɃA�^�b�`)
            //�_�C�X���[��script���̊֐������s����
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
                GoPiece = 0;
                Debug.Log("���X�g�o�g�����I");
            }
            if(GoPiece == 0)//�o�ڕ������݂��������\������
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
                //�}�X��0�̎��ɃC�x���g�𔭉΂���
                eventManager.IgnitionEvent(transform.position.x, transform.position.y);
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
        if (How_Branch1 == true)
        {
            transform.position += new Vector3(0, -1, 0);
            //����A����ɕ�����d�˂�\��ł��邽�߁A������false�ɂ��Ă���
            How_Branch1 = false;
        }
        else if(How_Branch2 == true) 
        {
            transform.position += new Vector3(0, 1, 0);
            //����A����ɕ�����d�˂�\��ł��邽�߁A������false�ɂ��Ă���
            How_Branch2 = false;
        }
        //��ڂ̕����i��ł���Ƃ�
        else if (transform.position.y <= -2)
        {
            //����̊p��1�ڂɂ��Ă��Ȃ��Ƃ�
            if (transform.position.y > -5 && transform.position.x < 6)
            {
                transform.position += new Vector3(0, -1, 0);
            }
            //����̊p��2�ڂɂ��Ă��Ȃ��Ƃ�
            else if (transform.position.x < 6)
            {
                transform.position += new Vector3(1, 0, 0);
            }
            //����̊p��2�ڂɂ����Ƃ�
            else
            {
                transform.position += new Vector3(0, 1, 0);
            }
        }
        //��ڂ̕����i��ł���Ƃ�
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
            //��ڂ̕���ɒ������Ƃ�
            if (transform.position.x == 1)
            {
                canClick = false;
                howGO_Branch(1);
            }
            //��ڂ̕���_�ɒ������Ƃ�
            if (transform.position.x == -2) 
            {
                canClick = false;
                howGO_Branch(2);
            }
        }
        
        
    }

    //����̑I�������֐�
    private void howGO_Branch(int number)
    {
        //����̖��̕\��������OnMouseDown���t���Ă���
        uiManage.ManageBrunchArrow(true, number);
    }

    
}
