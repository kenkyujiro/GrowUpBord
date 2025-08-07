using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject monsterStatus;        //モンスターステータスの参照
    private MonsterStatus monsterST;

    public GameObject AttackButton;         //各コマンドボタン
    public GameObject SPAttackButton;
    public GameObject GurdButton;
    public GameObject RunButton;

    public GameObject battleLogTextPrefab;  // 行動表示テキストのプレハブ
    public Transform battleLogContainer;    // テキスト表示先の親

    private int maxLogCount = 30;
    public ScrollRect scrollRect;           //ScrollViewのScrollRect

    public Vector2 spawnPosition;

    //コマンドのディレイ
    private float commandDelay = 0.5f;

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

        //プレイヤーステータス(hp,powerなど)の取得
        playerST = playerStatus.GetComponent<PlayerStatus>();
        playerST_TX = playerStatus.GetComponent<PlayerStatusManager>();
        monsterST = monsterStatus.GetComponent<MonsterStatus>();

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

        yield return new WaitForSeconds(commandDelay);

        if (playerST == null)
        {
            Debug.LogError("playerST is null");
        }
        if (monsterST == null)
        {
            Debug.LogError("monsterST is null");
        }

        int Attackpower = playerST.power - monsterST.monster_defence;

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
            if (playerST.exp >= 40)
            {
                playerST.exp -= 40;
                playerST.Levelup();

                AddBattleLog("Level UP!");
                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
               {
                    if (playerST.exp < 40)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 40;
                    playerST.Levelup();
                    yield return new WaitForSeconds(commandDelay);
                }
            }

            //ステータスを表記上にも反映
            playerST_TX.changeEXP(playerST.exp);
            playerST_TX.changeHP(playerST.hp);
            ButtleEnd();
        }
        else
        {
            //モンスターのターン
            monsterTurn(save_monster);

            //コマンドボタンの表示
            AttackButton.gameObject.SetActive(true);
            SPAttackButton.gameObject.SetActive(true);
            GurdButton.gameObject.SetActive(true);
            RunButton.gameObject.SetActive(true);
        }
    }

    //特殊攻撃(ガード貫通攻撃)
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
            if (playerST.exp >= 40)
            {
                playerST.exp -= 40;
                playerST.Levelup();

                AddBattleLog("Level UP!");

                yield return new WaitForSeconds(commandDelay);

                //一度に40以上の経験値を貰った場合の対処
                while (true)
                {
                    if (playerST.exp < 40)
                    {
                        //表記上にもレベルを反映
                        playerST_TX.changeLevel(playerST.Level);
                        break;
                    }
                    AddBattleLog("Level UP!");
                    playerST.exp -= 40;
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

            //コマンドボタンの表示
            AttackButton.gameObject.SetActive(true);
            SPAttackButton.gameObject.SetActive(true);
            GurdButton.gameObject.SetActive(true);
            RunButton.gameObject.SetActive(true);
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

        //モンスターのターン
        monsterTurn(save_monster);

        //行動が終わったらディフェンスを下げる
        playerST.defence -= 10;

        AttackButton.gameObject.SetActive(true);
        SPAttackButton.gameObject.SetActive(true);
        GurdButton.gameObject.SetActive(true);
        RunButton.gameObject.SetActive(true);
    }

    //逃げる
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
        Destroy(Monster);
        //シーンチェンジを行う
        SceneManager.LoadScene("SugorokuScene");
    }

    //モンスターによって行動を変化させる
    //逃げたと判断できる時間がほしいため、time.stopみたいなものが欲しい
    private void monsterTurn(string monster)
    {
        System.Random random = new System.Random();
        int value = random.Next(0, 100);//0～99の乱数を生成

        if (monster == "Bison")         //攻撃、ガード、特殊攻撃
        {
            if (value < 60)             //攻撃
            {
                MonsterAction("Attack");
            }
            else if (value < 90)        //特殊攻撃
            {
                MonsterAction("Special");
            }
            else                        //ガード
            {
                MonsterAction("Gurd");
            }
        }
        else if (monster == "Zako1")    //攻撃、ガード
        {
            if (value < 70)             //攻撃
            {
                MonsterAction("Attack");
            }
            else                        //ガード 
            {
                MonsterAction("Gurd");
            }
        }
        else if (monster == "Zako2")    //攻撃、ガード
        {
            if (value < 80)             //攻撃 
            {
                MonsterAction("Attack");
            }
            else                        //ガード
            {
                MonsterAction("Gurd");
            }
        }
        else if (monster == "Rare")     //攻撃、ガード
        {
            if (value < 80)             //攻撃
            {
                MonsterAction("Attack");
            }
            else                        //ガード
            {
                MonsterAction("Gurd");
            }
        }
        else if (monster == "Run")      //攻撃、逃げる
        {
            if (value < 70)             //攻撃 
            {
                MonsterAction("Attack");
            }
            else                        //逃げる
            {
                MonsterAction("Run");
            }
        }
        else                            //攻撃、ガード、特殊攻撃
        {
            if (value < 60)             //攻撃
            {
                MonsterAction("Attack");
            }
            else if (value < 90)        //特殊攻撃
            {
                MonsterAction("Special");
            }
            else                        //ガード
            {
                MonsterAction("Gurd");
            }
        }
    }

    private IEnumerator MonsterAction(string actionCommand)
    {
        //1=攻撃、2=特殊攻撃、3=ガード、4=逃げる
        yield return new WaitForSeconds(commandDelay);

        if (actionCommand == "Attack")
        {}
        else if (actionCommand == "Special")
        {}
        else if (actionCommand == "Gurd") 
        {}
        else 
        {}

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
