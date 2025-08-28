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

//やらなければいけないこと
//コルーチンがほしい
//てきの行動を記述する
//行動テキストを表示(ログも確認できると良し)
//コマの進捗状況をセーブできるようにする


public class ButtleSystem : MonoBehaviour
{
    public static ButtleSystem Instance { get; private set; }

    public GameObject Monster_Bison;        //バイソン
    public GameObject Monster_Zako1;        //モンスターズインク
    public GameObject Monster_Zako2;        //ヘビ
    public GameObject Monster_Boss;         //トロール
    public GameObject Monster_Rare;         //毛玉
    public GameObject Monster_Run;          //アルマジロ

    private GameObject Monster;
    private string save_monster;            //対戦しているモンスターの名前

    public GameObject playerStatus;         //プレイヤーステータスの参照
    private PlayerStatus playerST;
    private PlayerStatusManager playerST_TX;//表記上のプレイヤーステータス

    public GameObject aiStatus;
    private AIStatus AIST;
    private AIStatusManager AIST_TX;

    private AllyAI allyAI = new AllyAI();   //AI本体の定義

    public GameObject monsterStatus;        //モンスターステータスの参照
    private MonsterStatus monsterST;
    bool player_gurd = false;
    bool ai_gurd = false;
    bool monster_gurd = false;
    bool monster_charge = false;

    //AIの特殊攻撃のクールタイム
    private int AIguardBreakCooldown = 0;   // 0 の時は使用可能
    private const int AIguardBreakCooldownMax = 3; // 例: 3ターン待ち
    private string lastAction;              // 直前の行動を記録


    public GameObject AttackButton;         //各コマンドボタン
    public GameObject SPAttackButton;
    public GameObject GurdButton;
    public GameObject RunButton;

    public GameObject battleLogTextPrefab;  // 行動表示テキストのプレハブ
    public Transform battleLogContainer;    // テキスト表示先の親

    public GameObject GameManager;          //ゲームクリアscriptへ参照用
    private GameClearManager clear;

    private int maxLogCount = 30;
    public ScrollRect scrollRect;           //ScrollViewのScrollRect

    public Vector2 spawnPosition;

    //コマンドのディレイ
    private float commandDelay = 1.5f;

    private void OnEnable()
    {
        // シーンがロードされるたびに呼ばれる
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベントハンドラの解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //オブジェクトの取得
        playerStatus = GameObject.Find("PlayerStatus");
        aiStatus = GameObject.Find("AIStatus");
        monsterStatus = GameObject.Find("MonsterStatus");
        GameManager = GameObject.Find("GameClearManager");

        //プレイヤーステータス(hp,powerなど)の取得7766
        playerST = playerStatus.GetComponent<PlayerStatus>();
        playerST_TX = playerStatus.GetComponent<PlayerStatusManager>();
        monsterST = monsterStatus.GetComponent<MonsterStatus>();
        AIST = aiStatus.GetComponent<AIStatus>();
        AIST_TX = aiStatus.GetComponent<AIStatusManager>();
        clear = GameManager.GetComponent<GameClearManager>();

        //ステータスの反映
        playerST_TX.changeHP(playerST.hp);
        playerST_TX.changeLevel(playerST.Level);
        playerST_TX.changeEXP(playerST.exp);

        //別シーンで保存したモンスター名の取得(保存した変数名, 何も保存されていない場合)
        string monsterName = PlayerPrefs.GetString("monsterName", "Zako1");
        ButtleStart(monsterName);
    }

    //イベントマネージャーから起動する
    //ManageDiceUI(bool DicePanel, bool DiceButton)
    public void ButtleStart(string monster)
    {
        //コマンドボタンの表示
        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);

        //背景の表示やいらないテキストの非表示
        gameObject.SetActive(true);

        save_monster = monster;

        //即チャージして攻撃してこないようにする
        monster_charge = false;

        // プレハブを生成（MyScript付き）
        //モンスター名によって生成するモンスターを変える
        if (monster == "Bison") 
        {
            //ステータスのセット
            monsterST.SetStatus(monster);

            //モンスターの表示
            spawnPosition = new Vector2(0f, -1f);
            Monster = Instantiate(Monster_Bison);
            // 位置を設定（Z=0に固定）
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

    //通常攻撃
    public void Attack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //遅延
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        int Attackpower = 0;

        yield return new WaitForSeconds(commandDelay);

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0～99の乱数を生成

        //一定の確率で防御貫通＋2倍の攻撃力になる
        if (value < playerST.luck)
        {
            Attackpower = playerST.power * 2;
        }
        else
        {
            Attackpower = playerST.power - monsterST.monster_defence;
        }

        //もし0以下になった場合は1だけ減らすようにする
        if (Attackpower <= 0)
        {
            Attackpower = 1;
        }

        AddBattleLog("Player Give" + Attackpower + "Point!");

        //パワーの分だけ減らす
        monsterST.monster_hp -= Attackpower;

        yield return new WaitForSeconds(commandDelay);

        //相手の体力がなくなったら
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            //経験値が一定以上になったら
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");
                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
               {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //表記上にも経験値/HPを反映
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            if(AIST.hp >= 0)
            {
                Debug.Log("ターンが回る！");//ここまで動く
                                     //AIのターン
                StartCoroutine(AllyTurn());
            }
            else
            {
                Debug.Log("おれの番だ！");
                //モンスターのターン
                monsterTurn(save_monster);
            }
        }
    }

    //特殊攻撃(ガード貫通攻撃)
    public void SpecialAttack()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        //mpがないと撃てないようにする
        if(playerST.mp >= 5)
        {
            playerST.mp -= 5;
            StartCoroutine(SpecialAttackCoroutine());
        }
        else
        {
            AddBattleLog("You none MP!");

            //コマンドボタンの表示
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

        //パワーの分だけ減らす
        monsterST.monster_hp -= playerST.power;

        yield return new WaitForSeconds(commandDelay);

        //相手の体力がなくなったら
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //経験値が一定以上になったら
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //表記上にも経験値/HPを反映
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            if (AIST.hp >= 0)
            {
                //AIのターン
                StartCoroutine(AllyTurn());
            }
            else
            {
                //モンスターのターン
                monsterTurn(save_monster);
            }
        }
    }

    //ガード
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

        //一時的にディフェンスを上げる
        playerST.defence += 10;
        player_gurd = true;

        if (AIST.hp >= 0)
        {
            //AIのターン
            StartCoroutine(AllyTurn());
        }
        else
        {
            //モンスターのターン
            monsterTurn(save_monster);
        }
    }

    //逃げる
    public void Run()
    {
        AttackButton.gameObject.SetActive(false);
        SPAttackButton.gameObject.SetActive(false);
        GurdButton.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

        if (save_monster == "Boss")
        {
            AddBattleLog("逃げられない！");
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
        int value = random.Next(0, 100);//0～99の乱数を生成

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

        //もしかしたら出ないかも
        AddBattleLog("You Healed" + heal_point + "HP!");

        playerST.hp += heal_point;

        //回復は超過しないようにする
        if(playerST.Max_hp <= playerST.hp)
        {
            playerST.hp = playerST.Max_hp;
        }

        Destroy(Monster);

        //ボスを撃破している場合はゲームクリアになる
        if (save_monster == "Boss")
        {
            Debug.Log("ゲームクリア!!");
            clear.GameClear();
        }
        else
        {
            //シーンチェンジを行う
            SceneManager.LoadScene("SugorokuScene");
        }
    }

    private void monsterTurn(string monster)
    {
        //もしガードしているなら解除する
        if(monster_gurd == true) 
        {
            monsterST.monster_defence -= 10;
            monster_gurd = false;
        }

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0～99の乱数を生成

        if (monster == "Bison")         //攻撃、特殊攻撃
        {
            if (value < 60)             //攻撃
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else if (value < 90)        //特殊攻撃
            {
                StartCoroutine(MonsterAction("Special"));
            }
            else                        //ガード
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Zako1")    //攻撃、ガード
        {
            if (value < 70)             //攻撃
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //ガード 
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Zako2")    //攻撃、ガード
        {
            if (value < 80)             //攻撃 
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //ガード
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Rare")     //攻撃、ガード
        {
            if (value < 80)             //攻撃
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //ガード
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
        else if (monster == "Run")      //攻撃、逃げる
        {
            if (value < 70)             //攻撃 
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else                        //逃げる
            {
                StartCoroutine(MonsterAction("Run"));
            }
        }
        else                            //攻撃、ガード、特殊攻撃
        {
            if (value < 60)             //攻撃
            {
                StartCoroutine(MonsterAction("Attack"));
            }
            else if (value < 90)        //特殊攻撃
            {
                StartCoroutine(MonsterAction("Special"));
            }
            else                        //ガード
            {
                StartCoroutine(MonsterAction("Gurd"));
            }
        }
    }

    private IEnumerator MonsterAction(string actionCommand)
    {
        //ランダムで攻撃対象を決める
        System.Random random = new System.Random();
        int value = random.Next(0, 1);

        //1=攻撃、2=特殊攻撃、3=ガード、4=逃げる
        yield return new WaitForSeconds(commandDelay);

        Debug.Log("おれの攻撃だ！");

        if(monster_charge == true)
        {
            int Attackpower;

            if (value == 0)
            {
                Attackpower = monsterST.monster_power*3 - playerST.defence;
                playerST.hp -= Attackpower;

                AddBattleLog("Player Damaged" + Attackpower + "Point!");

                //表記上のHPの反映
                playerST_TX.changeHP(playerST.hp);

                if (playerST.hp <= 0)
                {
                    AddBattleLog("You Losed...");

                    yield return new WaitForSeconds(commandDelay);

                    //ゲームオーバー画面に遷移する
                    SceneManager.LoadScene("GameOverScene");
                }
            }
            else
            {
                Attackpower = monsterST.monster_power * 3 - AIST.defence;
                AIST.hp -= Attackpower;

                AddBattleLog("Friend Damaged" + Attackpower + "Point!");

                //行動候補の算出
                List<string> actions = GetAllyActions();

                //状況の再分析
                BattleContext ctx = new BattleContext(
                (float)AIST.hp / AIST.maxHP,
                    (float)monsterST.monster_hp / monsterST.monster_maxHp,
                    actions
                );

                //AIの再学習
                allyAI.Learn(ctx, lastAction, -5f);
                AddBattleLog("Fri:I was Reinforced!");

                //表記上のHPの反映
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

            //対象：プレイヤー
            if(value == 0)
            {
                Attackpower = monsterST.monster_power - playerST.defence;

                //回復しないようにする
                if (Attackpower < 0)
                {
                    Attackpower = 1;
                }

                AddBattleLog("Player Damaged" + Attackpower + "Point!");

                //パワーの分だけ減らす
                playerST.hp -= Attackpower;
                //表記上のHP反映
                playerST_TX.changeHP(playerST.hp);

                if (playerST.hp <= 0)
                {
                    AddBattleLog("You Losed...");

                    yield return new WaitForSeconds(commandDelay);

                    //ゲームオーバー画面に遷移する
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

                //パワーの分だけ減らす
                AIST.hp -= Attackpower;

                //行動候補の算出
                List<string> actions = GetAllyActions();

                //状況の再分析
                BattleContext ctx = new BattleContext(
                (float)AIST.hp / AIST.maxHP,
                    (float)monsterST.monster_hp / monsterST.monster_maxHp,
                    actions
                );

                //AIの再学習
                allyAI.Learn(ctx, lastAction, -5f);
                AddBattleLog("Fri:I was Reinforced!");

                //表記上のHP反映
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
            //特殊攻撃のチャージ
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

            //行動候補の算出
            List<string> actions = GetAllyActions();

            //状況の再分析
            BattleContext ctx = new BattleContext(
            (float)AIST.hp / AIST.maxHP,
                (float)monsterST.monster_hp / monsterST.monster_maxHp,
                actions
            );

            //AIの再学習
            allyAI.Learn(ctx, lastAction, -10f);

            ButtleEnd();
        }

        yield return new WaitForSeconds(commandDelay);

        if(player_gurd == true)
        {
            //行動が終わったらディフェンスを下げる
            playerST.defence -= 10;
            player_gurd = false;
        }
        if (ai_gurd == true)
        {
            //行動が終わったらディフェンスを下げる
            AIST.defence -= 10;
            ai_gurd = false;
        }

        //クールターンの減少
        if(AIguardBreakCooldown > 0)
        {
            AIguardBreakCooldown -= 1;
        }

        //コマンドボタンの表示
        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);

    }

    IEnumerator AllyTurn()
    {
        Debug.Log("分析開始！");

        List<string> actions = GetAllyActions();

        //現在のステータスの状況をぶんせきする
        BattleContext ctx = new BattleContext(
        (float)AIST.hp / AIST.maxHP,
            (float)monsterST.monster_hp / monsterST.monster_maxHp,
            actions
        );

        int index = allyAI.ChooseAction(ctx);
        string action = actions[index];
        lastAction = action;

        Debug.Log($"味方の行動: {action}");

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
        Debug.Log("味方が攻撃！");
        // 攻撃処理
        int Attackpower = 0;

        yield return new WaitForSeconds(commandDelay);

        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0～99の乱数を生成

        //一定の確率で防御貫通＋2倍の攻撃力になる
        if (value < playerST.luck)
        {
            Attackpower = AIST.power * 2;
        }
        else
        {
            Attackpower = AIST.power - monsterST.monster_defence;
        }

        //もし0以下になった場合は1だけ減らすようにする
        if (Attackpower <= 0)
        {
            Attackpower = 1;
        }

        AddBattleLog("Friend Give" + Attackpower + "Point!");

        //パワーの分だけ減らす
        monsterST.monster_hp -= Attackpower;

        //相手の体力がなくなったら
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //経験値が一定以上になったら
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //表記上にも経験値/HPを反映
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            //モンスターのターン
            monsterTurn(save_monster);
        }
    }

    private IEnumerator Guard()
    {
        Debug.Log("味方がガード！");
        // ガード処理
        AddBattleLog("Friend Gurd!");

        yield return new WaitForSeconds(commandDelay);

        //一時的にディフェンスを上げる
        AIST.defence += 10;
        ai_gurd = true;

        //モンスターのターン
        monsterTurn(save_monster);
    }

    private IEnumerator GuardBreakAttack()
    {
        Debug.Log("味方がガード貫通攻撃！");
        // ガード貫通処理
        yield return new WaitForSeconds(commandDelay);

        AddBattleLog("Friend Give" + playerST.power + "Point!");

        //パワーの分だけ減らす
        monsterST.monster_hp -= AIST.power;

        yield return new WaitForSeconds(commandDelay);

        //相手の体力がなくなったら
        if (monsterST.monster_hp <= 0)
        {
            playerST.exp += monsterST.monster_EXP;

            AddBattleLog("You Get" + monsterST.monster_EXP + "EXP!");

            yield return new WaitForSeconds(commandDelay);

            //経験値が一定以上になったら
            if (playerST.exp >= 4 * playerST.Level)
            {
                playerST.exp -= 4 * playerST.Level;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
                {
                    if (playerST.exp < 4 * playerST.Level)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 4 * playerST.Level;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //表記上にも経験値/HPを反映
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            //モンスターのターン
            monsterTurn(save_monster);
        }

    }

    private IEnumerator Heal()
    {
        Debug.Log("味方が回復！");
        AIST.hp += 30; // 回復量
        if (AIST.hp > AIST.maxHP)
        { 
            AIST.hp = AIST.maxHP;
        }

        yield return new WaitForSeconds(commandDelay);

        //モンスターのターン
        monsterTurn(save_monster);
    }

    //ターン中に行動できるリストの算出
    public List<string> GetAllyActions()
    {
        // 基本行動
        List<string> actions = new List<string> { "NormalAttack", "Guard", "Heal" };

        // ガード貫通がクールタイム中でなければ追加
        if (AIguardBreakCooldown == 0)
        {
            actions.Add("GuardBreak");
        }

        return actions;
    }


    //バトルログテキストの追加
    private void AddBattleLog(string message)
    {
        if (battleLogContainer.childCount >= maxLogCount)
        {
            // 最古のログを削除
            Destroy(battleLogContainer.GetChild(0).gameObject);
        }

        GameObject logEntry = Instantiate(battleLogTextPrefab, battleLogContainer);
        TextMeshProUGUI logText = logEntry.GetComponent<TextMeshProUGUI>();
        logText.text = message;

        // 追加後にスクロールを一番下に
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

}