using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//���Ȃ���΂����Ȃ�����
//�R���[�`�����ق���
//�Ă��̍s�����L�q����
//�s���e�L�X�g��\��(���O���m�F�ł���Ɨǂ�)
//�R�}�̐i���󋵂��Z�[�u�ł���悤�ɂ���


public class ButtleSystem : MonoBehaviour
{
    public static ButtleSystem Instance { get; private set; }

    public GameObject Monster_Bison;        //�o�C�\��
    public GameObject Monster_Zako1;        //�����X�^�[�Y�C���N
    public GameObject Monster_Zako2;        //�w�r
    public GameObject Monster_Boss;         //�g���[��
    public GameObject Monster_Rare;         //�ы�
    public GameObject Monster_Run;          //�A���}�W��

    private GameObject Monster;
    private string save_monster;            //�ΐ킵�Ă��郂���X�^�[�̖��O

    public GameObject playerStatus;         //�v���C���[�X�e�[�^�X�̎Q��
    private PlayerStatus playerST;
    private PlayerStatusManager playerST_TX;//�\�L��̃v���C���[�X�e�[�^�X

    public GameObject monsterStatus;        //�����X�^�[�X�e�[�^�X�̎Q��
    private MonsterStatus monsterST;

    public GameObject AttackButton;         //�e�R�}���h�{�^��
    public GameObject SPAttackButton;
    public GameObject GurdButton;
    public GameObject RunButton;

    public Vector2 spawnPosition;

    private void OnEnable()
    {
        // �V�[�������[�h����邽�тɌĂ΂��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �C�x���g�n���h���̉���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //�I�u�W�F�N�g�̎擾
        playerStatus = GameObject.Find("PlayerStatus");

        //�v���C���[�X�e�[�^�X(hp,power�Ȃ�)�̎擾
        playerST = playerStatus.GetComponent<PlayerStatus>();
        playerST_TX = playerStatus.GetComponent<PlayerStatusManager>();
        monsterST = monsterStatus.GetComponent<MonsterStatus>();

        //�X�e�[�^�X�̔��f
        playerST_TX.changeHP(playerST.hp);
        playerST_TX.changeLevel(playerST.Level);
        playerST_TX.changeEXP(playerST.exp);

        //�ʃV�[���ŕۑ����������X�^�[���̎擾(�ۑ������ϐ���, �����ۑ�����Ă��Ȃ��ꍇ)
        string monsterName = PlayerPrefs.GetString("monsterName", "Zako1");
        ButtleStart(monsterName);
    }

    //�C�x���g�}�l�[�W���[����N������
    //ManageDiceUI(bool DicePanel, bool DiceButton)
    public void ButtleStart(string monster)
    {
        //�R�}���h�{�^���̕\��
        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);

        //�w�i�̕\���₢��Ȃ��e�L�X�g�̔�\��
        gameObject.SetActive(true);

        save_monster = monster;

        // �v���n�u�𐶐��iMyScript�t���j
        //�����X�^�[���ɂ���Đ������郂���X�^�[��ς���
        if (monster == "Bison") 
        {
            //�X�e�[�^�X�̃Z�b�g
            monsterST.SetStatus(monster);

            //�����X�^�[�̕\��
            spawnPosition = new Vector2(0f, -1f);
            Monster = Instantiate(Monster_Bison);
            // �ʒu��ݒ�iZ=0�ɌŒ�j
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
        else if (monster == "Zako1")
        {
            monsterST.SetStatus(monster);

            spawnPosition = new Vector2(0f, -0.5f);
            Monster = Instantiate(Monster_Zako1);
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
        else if (monster == "Zako2") 
        {
            monsterST.SetStatus(monster);

            spawnPosition = new Vector2(0f, -1f);
            Monster = Instantiate(Monster_Zako2);
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
        else if (monster == "Rare") 
        {
            monsterST.SetStatus(monster);

            spawnPosition = new Vector2(0f, -0.5f);
            Monster = Instantiate(Monster_Rare);
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
        else if (monster == "Run") 
        {
            monsterST.SetStatus(monster);

            spawnPosition = new Vector2(0f, -1.5f);
            Monster = Instantiate(Monster_Run);
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
        else
        {
            monsterST.SetStatus(monster);

            spawnPosition = new Vector2(0f, 1f);
            Monster = Instantiate(Monster_Boss);
            Monster.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        }
    }

    //�ʏ�U��
    public void Attack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        int Attackpower = playerST.power - monsterST.monster_defence;

        //����0�ȉ��ɂȂ����ꍇ��1�������炷�悤�ɂ���
        if (Attackpower <= 0)
        {
            Attackpower = 1;
        }
        //�p���[�̕��������炷
        monsterST.monster_hp -= Attackpower;


        //����̗̑͂��Ȃ��Ȃ�����
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            //�o���l�����ȏ�ɂȂ�����
            if (playerST.exp >= 40)
            {
                playerST.exp -= 40;
                playerST.Levelup();

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
                {
                    if (playerST.exp < 40)
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    playerST.exp -= 40;
                    playerST.Levelup();
                }
            }

            //�X�e�[�^�X��\�L��ɂ����f
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            //�����X�^�[�̃^�[��
            monsterTurn(save_monster);

            //�R�}���h�{�^���̕\��
            AttackButton.gameObject.SetActive(true);
            SPAttackButton.gameObject.SetActive(true);
            GurdButton.gameObject.SetActive(true);
            RunButton.gameObject.SetActive(true);
        }
    }

    //����U��(�K�[�h�ђʍU��)
    public void SpecialAttack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //�p���[�̕��������炷
        monsterST.monster_hp -= playerST.power;

        //����̗̑͂��Ȃ��Ȃ�����
        if(monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            //�o���l�����ȏ�ɂȂ�����
            if(playerST.exp >= 40)
            {
                playerST.exp -= 40;
                playerST.Levelup();

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
                {
                    if(playerST.exp < 40) 
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    playerST.exp -= 40;
                    playerST.Levelup();
                }
            }

            //�\�L��ɂ��o���l/HP�𔽉f
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else 
        {
            //�����X�^�[�̃^�[��
            monsterTurn(save_monster);

            //�R�}���h�{�^���̕\��
            AttackButton.gameObject.SetActive(true);
            SPAttackButton.gameObject.SetActive(true);
            GurdButton.gameObject.SetActive(true);
            RunButton.gameObject.SetActive(true);
        }
    }

    //�K�[�h
    public void Gurd()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //�ꎞ�I�Ƀf�B�t�F���X���グ��
        playerST.defence += 10;

        //�����X�^�[�̃^�[��
        monsterTurn(save_monster);

        //�s�����I�������f�B�t�F���X��������
        playerST.defence -= 10;

        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);
    }

    //������
    public void Run()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //if (monster = "Boss") { }
        ButtleEnd();
    }

    private void ButtleEnd()
    {
        Destroy(Monster);
        gameObject.SetActive(false);
        //�V�[���`�F���W���s��
        SceneManager.LoadScene("SugorokuScene");
    }

    //�����X�^�[�ɂ���čs����ω�������
    //�������Ɣ��f�ł��鎞�Ԃ��ق������߁Atime.stop�݂����Ȃ��̂��~����
    private void monsterTurn(string monster)
    {
        if (monster == "Bison")//�U���A�K�[�h�A����U��
        {}
        else if (monster == "Zako1")//�U���A�K�[�h
        {}
        else if (monster == "Zako2")//�U���A�K�[�h
        {}
        else if (monster == "Rare")//�U���A�K�[�h
        {}
        else if (monster == "Run")//�U���A������
        {}
        else//�U���A�K�[�h�A����U��
        {}
    }
}
