using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
using static UnityEngine.EventSystems.EventTrigger;

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

    public GameObject aiStatus;
    private AIStatus AIST;
    private AIStatusManager AIST_TX;

    private AllyAI allyAI = new AllyAI();   //AI�{�̂̒�`

    public GameObject monsterStatus;        //�����X�^�[�X�e�[�^�X�̎Q��
    private MonsterStatus monsterST;
    bool player_gurd = false;
    bool ai_gurd = false;
    bool monster_gurd = false;
    bool monster_charge = false;

    //AI�̓���U���̃N�[���^�C��
    private int AIguardBreakCooldown = 0;   // 0 �̎��͎g�p�\
    private const int AIguardBreakCooldownMax = 3; // ��: 3�^�[���҂�
    private string lastAction;              // ���O�̍s�����L�^


    public GameObject AttackButton;         //�e�R�}���h�{�^��
    public GameObject SPAttackButton;
    public GameObject GurdButton;
    public GameObject RunButton;

    public GameObject battleLogTextPrefab;  // �s���\���e�L�X�g�̃v���n�u
    public Transform battleLogContainer;    // �e�L�X�g�\����̐e

    public GameObject GameManager;          //�Q�[���N���Ascript�֎Q�Ɨp
    private GameClearManager clear;

    private int maxLogCount = 30;
    public ScrollRect scrollRect;           //ScrollView��ScrollRect

    public Vector2 spawnPosition;

    //�R�}���h�̃f�B���C
    private float commandDelay = 1.5f;

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
        aiStatus = GameObject.Find("AIStatus");
        monsterStatus = GameObject.Find("MonsterStatus");
        GameManager = GameObject.Find("GameClearManager");

        //�v���C���[�X�e�[�^�X(hp,power�Ȃ�)�̎擾7766
        playerST = playerStatus.GetComponent<PlayerStatus>();
        playerST_TX = playerStatus.GetComponent<PlayerStatusManager>();
        monsterST = monsterStatus.GetComponent<MonsterStatus>();
        AIST = aiStatus.GetComponent<AIStatus>();
        AIST_TX = aiStatus.GetComponent<AIStatusManager>();
        clear = GameManager.GetComponent<GameClearManager>();

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

        //���`���[�W���čU�����Ă��Ȃ��悤�ɂ���
        monster_charge = false;

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

        AddBattleLog(monster + " popped out!");
    }

    //�ʏ�U��
    public void Attack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //�x��
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        int Attackpower = 0;

        yield return new WaitForSeconds(commandDelay);

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0�`99�̗����𐶐�

        //���̊m���Ŗh��ђʁ{2�{�̍U���͂ɂȂ�
        if (value < playerST.luck)
        {
            Attackpower = playerST.power * 2;
        }
        else
        {
            Attackpower = playerST.power - monsterST.monster_defence;
        }

        //����0�ȉ��ɂȂ����ꍇ��1�������炷�悤�ɂ���
        if (Attackpower <= 0)
        {
            Attackpower = 1;
        }

        AddBattleLog("Player Give" + Attackpower + "Point!");

        //�p���[�̕��������炷
        monsterST.monster_hp -= Attackpower;

        yield return new WaitForSeconds(commandDelay);

        //����̗̑͂��Ȃ��Ȃ�����
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            //�o���l�����ȏ�ɂȂ�����
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");
                yield return new WaitForSeconds(commandDelay);

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
               {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //�\�L��ɂ��o���l/HP�𔽉f
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            if(AIST.hp >= 0)
            {
                Debug.Log("�^�[�������I");//�����܂œ���
                                     //AI�̃^�[��
                StartCoroutine(AllyTurn());
            }
            else
            {
                Debug.Log("����̔Ԃ��I");
                //�����X�^�[�̃^�[��
                monsterTurn(save_monster);
            }
        }
    }

    //����U��(�K�[�h�ђʍU��)
    public void SpecialAttack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //mp���Ȃ��ƌ��ĂȂ��悤�ɂ���
        if(playerST.mp >= 5)
        {
            playerST.mp -= 5;
            StartCoroutine(SpecialAttackCoroutine());
        }
        else
        {
            AddBattleLog("You none MP!");

            //�R�}���h�{�^���̕\��
            AttackButton.gameObject.SetActive(true);
            SPAttackButton.gameObject.SetActive(true);
            GurdButton.gameObject.SetActive(true);
            RunButton.gameObject.SetActive(true);
        }

    }

    private IEnumerator SpecialAttackCoroutine()
    {
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Player Give" + playerST.power + "Point!");

        //�p���[�̕��������炷
        monsterST.monster_hp -= playerST.power;

        yield return new WaitForSeconds(commandDelay);

        //����̗̑͂��Ȃ��Ȃ�����
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //�o���l�����ȏ�ɂȂ�����
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //�\�L��ɂ��o���l/HP�𔽉f
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            if (AIST.hp >= 0)
            {
                //AI�̃^�[��
                StartCoroutine(AllyTurn());
            }
            else
            {
                //�����X�^�[�̃^�[��
                monsterTurn(save_monster);
            }
        }
    }

    //�K�[�h
    public void Gurd()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        StartCoroutine(GurdCoroutine());
    }

    private IEnumerator GurdCoroutine()
    {
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Player Gurd!");

        yield return new WaitForSeconds(commandDelay);

        //�ꎞ�I�Ƀf�B�t�F���X���グ��
        playerST.defence += 10;
        player_gurd = true;

        if (AIST.hp >= 0)
        {
            //AI�̃^�[��
            StartCoroutine(AllyTurn());
        }
        else
        {
            //�����X�^�[�̃^�[��
            monsterTurn(save_monster);
        }
    }

    //������
    public void Run()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        if (save_monster == "Boss")
        {
            AddBattleLog("�������Ȃ��I");
        }
        else
        {
            StartCoroutine(RunCoroutine());
        }
    }

    private IEnumerator RunCoroutine()
    {
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Player Escaped!");

        yield return new WaitForSeconds(commandDelay);

        ButtleEnd();
    }

    private void ButtleEnd()
    {
        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0�`99�̗����𐶐�

        int heal_point = 0;

        if(value < 30)
        {
            heal_point = 1;
        }
        else if(value < 60) 
        {
            heal_point = 20;
        }
        else if(value < 90) 
        {
            heal_point = playerST.Max_hp;
        }
        else
        {
            heal_point = 0; 
        }

        //������������o�Ȃ�����
        AddBattleLog("You Healed" + heal_point + "HP!");

        playerST.hp += heal_point;

        //�񕜂͒��߂��Ȃ��悤�ɂ���
        if(playerST.Max_hp <= playerST.hp)
        {
            playerST.hp = playerST.Max_hp;
        }

        Destroy(Monster);

        //�{�X�����j���Ă���ꍇ�̓Q�[���N���A�ɂȂ�
        if (save_monster == "Boss")
        {
            Debug.Log("�Q�[���N���A!!");
            clear.GameClear();
        }
        else
        {
            //�V�[���`�F���W���s��
            SceneManager.LoadScene("SugorokuScene");
        }
    }

    private void monsterTurn(string monster)
    {
        //�����K�[�h���Ă���Ȃ��������
        if(monster_gurd == true) 
        {
            monsterST.monster_defence -= 10;
            monster_gurd = false;
        }

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0�`99�̗����𐶐�

        if (monster == "Bison")         //�U���A����U��
        {
            if (value < 60)             //�U��
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else if (value < 90)        //����U��
            {
                StartCoroutine(MonsterAction("Special"));
            }
            else                        //�K�[�h
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Zako1")    //�U���A�K�[�h
        {
            if (value < 70)             //�U��
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //�K�[�h 
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Zako2")    //�U���A�K�[�h
        {
            if (value < 80)             //�U�� 
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //�K�[�h
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Rare")     //�U���A�K�[�h
        {
            if (value < 80)             //�U��
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //�K�[�h
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Run")      //�U���A������
        {
            if (value < 70)             //�U�� 
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //������
            {
                StartCoroutine(MonsterAction("Run"));
            }
        }
        else                            //�U���A�K�[�h�A����U��
        {
            if (value < 60)             //�U��
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else if (value < 90)        //����U��
            {
                StartCoroutine(MonsterAction("Special"));
            }
            else                        //�K�[�h
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
    }

    private IEnumerator MonsterAction(string actionCommand)
    {
        //�����_���ōU���Ώۂ����߂�
        System.Random random = new System.Random();
        int value = random.Next(0, 1);

        //1=�U���A2=����U���A3=�K�[�h�A4=������
        yield return new WaitForSeconds(commandDelay);

        Debug.Log("����̍U�����I");

        if(monster_charge == true)
        {
            int Attackpower;

            if (value == 0)
            {
                Attackpower = monsterST.monster_power*3 - playerST.defence;
                playerST.hp -= Attackpower;

                AddBattleLog("Player Damaged" + Attackpower + "Point!");

                //�\�L���HP�̔��f
                playerST_TX.changeHP(playerST.hp);

                if (playerST.hp <= 0)
                {
                    AddBattleLog("You Losed...");

                    yield return new WaitForSeconds(commandDelay);

                    //�Q�[���I�[�o�[��ʂɑJ�ڂ���
                    SceneManager.LoadScene("GameOverScene");
                }
            }
            else
            {
                Attackpower = monsterST.monster_power * 3 - AIST.defence;
                AIST.hp -= Attackpower;

                AddBattleLog("Friend Damaged" + Attackpower + "Point!");

                //�s�����̎Z�o
                List<string> actions = GetAllyActions();

                //�󋵂̍ĕ���
                BattleContext ctx = new BattleContext(
                (float)AIST.hp / AIST.maxHP,
                    (float)monsterST.monster_hp / monsterST.monster_maxHp,
                    actions
                );

                //AI�̍Ċw�K
                allyAI.Learn(ctx, lastAction, -5f);
                AddBattleLog("Fri:I was Reinforced!");

                //�\�L���HP�̔��f
                AIST_TX.changeHP(AIST.hp);

                if (AIST.hp <= 0)
                {
                    AddBattleLog("Friend Losed...");

                    yield return new WaitForSeconds(commandDelay);
                }
            }
        }
        else if (actionCommand == "Attack")
        {
            int Attackpower;

            //�ΏہF�v���C���[
            if(value == 0)
            {
                Attackpower = monsterST.monster_power - playerST.defence;

                //�񕜂��Ȃ��悤�ɂ���
                if (Attackpower < 0)
                {
                    Attackpower = 1;
                }

                AddBattleLog("Player Damaged" + Attackpower + "Point!");

                //�p���[�̕��������炷
                playerST.hp -= Attackpower;
                //�\�L���HP���f
                playerST_TX.changeHP(playerST.hp);

                if (playerST.hp <= 0)
                {
                    AddBattleLog("You Losed...");

                    yield return new WaitForSeconds(commandDelay);

                    //�Q�[���I�[�o�[��ʂɑJ�ڂ���
                    ButtleEnd();
                }
            }
            else
            {
                Attackpower = monsterST.monster_power - AIST.defence;

                if(Attackpower < 0)
                {
                    Attackpower = 1;
                }

                AddBattleLog("Friend Damaged" + Attackpower + "Point!");

                //�p���[�̕��������炷
                AIST.hp -= Attackpower;

                //�s�����̎Z�o
                List<string> actions = GetAllyActions();

                //�󋵂̍ĕ���
                BattleContext ctx = new BattleContext(
                (float)AIST.hp / AIST.maxHP,
                    (float)monsterST.monster_hp / monsterST.monster_maxHp,
                    actions
                );

                //AI�̍Ċw�K
                allyAI.Learn(ctx, lastAction, -5f);
                AddBattleLog("Fri:I was Reinforced!");

                //�\�L���HP���f
                AIST_TX.changeHP(AIST.hp);

                if (AIST.hp <= 0)
                {
                    AddBattleLog("Friend Losed...");

                    yield return new WaitForSeconds(commandDelay);
                }
            }
            
        }
        else if (actionCommand == "Special")
        {
            //����U���̃`���[�W
            monster_charge = true;
            AddBattleLog("Monster Power Charge!");
        }
        else if (actionCommand == "Gurd") 
        {
            monsterST.monster_defence += 10;
            monster_gurd = true;

            AddBattleLog("Monster Gurded!");
        }
        else
        {
            AddBattleLog("Monster RunAway!");

            //�s�����̎Z�o
            List<string> actions = GetAllyActions();

            //�󋵂̍ĕ���
            BattleContext ctx = new BattleContext(
            (float)AIST.hp / AIST.maxHP,
                (float)monsterST.monster_hp / monsterST.monster_maxHp,
                actions
            );

            //AI�̍Ċw�K
            allyAI.Learn(ctx, lastAction, -10f);

            ButtleEnd();
        }

        yield return new WaitForSeconds(commandDelay);

        if(player_gurd == true)
        {
            //�s�����I�������f�B�t�F���X��������
            playerST.defence -= 10;
            player_gurd = false;
        }
        if (ai_gurd == true)
        {
            //�s�����I�������f�B�t�F���X��������
            AIST.defence -= 10;
            ai_gurd = false;
        }

        //�N�[���^�[���̌���
        if(AIguardBreakCooldown > 0)
        {
            AIguardBreakCooldown -= 1;
        }

        //�R�}���h�{�^���̕\��
        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);

    }

    IEnumerator AllyTurn()
    {
        Debug.Log("���͊J�n�I");

        List<string> actions = GetAllyActions();

        //���݂̃X�e�[�^�X�̏󋵂��Ԃ񂹂�����
        BattleContext ctx = new BattleContext(
        (float)AIST.hp / AIST.maxHP,
            (float)monsterST.monster_hp / monsterST.monster_maxHp,
            actions
        );

        int index = allyAI.ChooseAction(ctx);
        string action = actions[index];
        lastAction = action;

        Debug.Log($"�����̍s��: {action}");

        switch (action)
        {
            case "NormalAttack":
                yield return StartCoroutine(NormalAttack());
                allyAI.Learn(ctx, action, 10f);
                AddBattleLog("Fri:I was Reinforced!");
                break;
            case "Guard":
                yield return StartCoroutine(Guard());
                allyAI.Learn(ctx, action, 5f);
                AddBattleLog("Fri:I was Reinforced!");
                break;
            case "Heal":
                yield return StartCoroutine(Heal());
                allyAI.Learn(ctx, action, (AIST.hp < AIST.maxHP / 2) ? 12f : -5f);
                AddBattleLog("Fri:I was Reinforced!");
                break;
            case "GuardBreak":
                yield return StartCoroutine(GuardBreakAttack());
                allyAI.Learn(ctx, action, 15f);
                AddBattleLog("Fri:I was Reinforced!");
                AIguardBreakCooldown = AIguardBreakCooldownMax;
                break;
        }
    }

    private IEnumerator NormalAttack()
    {
        Debug.Log("�������U���I");
        // �U������
        int Attackpower = 0;

        yield return new WaitForSeconds(commandDelay);

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0�`99�̗����𐶐�

        //���̊m���Ŗh��ђʁ{2�{�̍U���͂ɂȂ�
        if (value < playerST.luck)
        {
            Attackpower = AIST.power * 2;
        }
        else
        {
            Attackpower = AIST.power - monsterST.monster_defence;
        }

        //����0�ȉ��ɂȂ����ꍇ��1�������炷�悤�ɂ���
        if (Attackpower <= 0)
        {
            Attackpower = 1;
        }

        AddBattleLog("Friend Give" + Attackpower + "Point!");

        //�p���[�̕��������炷
        monsterST.monster_hp -= Attackpower;

        //����̗̑͂��Ȃ��Ȃ�����
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //�o���l�����ȏ�ɂȂ�����
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
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
        }
    }

    private IEnumerator Guard()
    {
        Debug.Log("�������K�[�h�I");
        // �K�[�h����
        AddBattleLog("Friend Gurd!");

        yield return new WaitForSeconds(commandDelay);

        //�ꎞ�I�Ƀf�B�t�F���X���グ��
        AIST.defence += 10;
        ai_gurd = true;

        //�����X�^�[�̃^�[��
        monsterTurn(save_monster);
    }

    private IEnumerator GuardBreakAttack()
    {
        Debug.Log("�������K�[�h�ђʍU���I");
        // �K�[�h�ђʏ���
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Friend Give" + playerST.power + "Point!");

        //�p���[�̕��������炷
        monsterST.monster_hp -= AIST.power;

        yield return new WaitForSeconds(commandDelay);

        //����̗̑͂��Ȃ��Ȃ�����
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //�o���l�����ȏ�ɂȂ�����
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //��x��40�ȏ�̌o���l�������ꍇ�̑Ώ�
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //�\�L��ɂ����x���𔽉f
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
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
        }

    }

    private IEnumerator Heal()
    {
        Debug.Log("�������񕜁I");
        AIST.hp += 30; // �񕜗�
        if (AIST.hp > AIST.maxHP)
        { 
            AIST.hp = AIST.maxHP;
        }

        yield return new WaitForSeconds(commandDelay);

        //�����X�^�[�̃^�[��
        monsterTurn(save_monster);
    }

    //�^�[�����ɍs���ł��郊�X�g�̎Z�o
    public List<string> GetAllyActions()
    {
        // ��{�s��
        List<string> actions = new List<string> { "NormalAttack", "Guard", "Heal" };

        // �K�[�h�ђʂ��N�[���^�C�����łȂ���Βǉ�
        if (AIguardBreakCooldown == 0)
        {
            actions.Add("GuardBreak");
        }

        return actions;
    }


    //�o�g�����O�e�L�X�g�̒ǉ�
    private void AddBattleLog(string message)
    {
        if (battleLogContainer.childCount >= maxLogCount)
        {
            // �ŌẪ��O���폜
            Destroy(battleLogContainer.GetChild(0).gameObject);
        }

        GameObject logEntry = Instantiate(battleLogTextPrefab, battleLogContainer);
        TextMeshProUGUI logText = logEntry.GetComponent<TextMeshProUGUI>();
        logText.text = message;

        // �ǉ���ɃX�N���[������ԉ���
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

}