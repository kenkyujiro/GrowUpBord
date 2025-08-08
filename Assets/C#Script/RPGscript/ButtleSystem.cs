using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    bool player_gurd = false;
    bool monster_gurd = false;

    public GameObject AttackButton;         //�e�R�}���h�{�^��
    public GameObject SPAttackButton;
    public GameObject GurdButton;
    public GameObject RunButton;

    public GameObject battleLogTextPrefab;  // �s���\���e�L�X�g�̃v���n�u
    public Transform battleLogContainer;    // �e�L�X�g�\����̐e

    private int maxLogCount = 30;
    public ScrollRect scrollRect;           //ScrollView��ScrollRect

    public Vector2 spawnPosition;

    //�R�}���h�̃f�B���C
    private float commandDelay = 0.5f;

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

            //�X�e�[�^�X��\�L��ɂ����f
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

    //����U��(�K�[�h�ђʍU��)
    public void SpecialAttack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        StartCoroutine(SpecialAttackCoroutine());
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
            //�����X�^�[�̃^�[��
            monsterTurn(save_monster);
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

        //�����X�^�[�̃^�[��
        monsterTurn(save_monster);
    }

    //������
    public void Run()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        StartCoroutine(RunCoroutine());
    }

    private IEnumerator RunCoroutine()
    {
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Player Escaped!");

        yield return new WaitForSeconds(commandDelay);

        //if (monster = "Boss") { }
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
        //�V�[���`�F���W���s��
        SceneManager.LoadScene("SugorokuScene");
    }

    //�����X�^�[�ɂ���čs����ω�������
    //�������Ɣ��f�ł��鎞�Ԃ��ق������߁Atime.stop�݂����Ȃ��̂��~����
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
        //1=�U���A2=����U���A3=�K�[�h�A4=������
        yield return new WaitForSeconds(commandDelay);

        if (actionCommand == "Attack")
        {
            int Attackpower = monsterST.monster_power - playerST.defence;

            //�񕜂��Ȃ��悤�ɂ���
            if (Attackpower < 0)
            {
                Attackpower = 0;
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
        else if (actionCommand == "Special")
        {
            playerST.hp -= monsterST.monster_power;

            AddBattleLog("Player Damaged" + monsterST.monster_power + "Point!");

            //�\�L���HP�̔��f
            playerST_TX.changeHP(playerST.hp);

            if (playerST.hp <= 0)
            {
                AddBattleLog("You Losed...");

                yield return new WaitForSeconds(commandDelay);

                //�Q�[���I�[�o�[��ʂɑJ�ڂ���
                ButtleEnd();
            }
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

            ButtleEnd();
        }

        yield return new WaitForSeconds(commandDelay);

        if(player_gurd == true)
        {
            //�s�����I�������f�B�t�F���X��������
            playerST.defence -= 10;
            player_gurd = false;
        }

        //�R�}���h�{�^���̕\��
        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);

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
