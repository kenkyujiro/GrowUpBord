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

    public GameObject DiceSystem;      //出目テキスト参照用
    private DiceRollSystem rollSystem;

    public GameObject uiManager;       //UI表示管理用
    private UIManager uiManage;

    public GameObject EventManager;    //イベント管理用
    private EventManager eventManager;

    public GameObject playerStatus;    //プレイヤーステータスの参照(ゲームクリア確認用)
    private PlayerStatus playerST;

    public int GoPiece;                //進める出目の数
    private bool How_First;            //始めてのダイスロールかどうか
    public bool How_Branch1 = false;   //一つ目の分岐に進んだかどうか
    public bool How_Branch2 = false;   //二つ目の分岐に進んだかどうか
    public bool canClick;              //クリックできるかどうか

    private void Awake()
    {
        // Singletonパターン: すでにGameManagerが存在する場合は新しいインスタンスを作らない
        if (Instance != null)
        {
            Destroy(gameObject); // 既存のインスタンスがあれば、このオブジェクトを削除
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーン遷移後もこのオブジェクトを破棄しない

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
        //コマを表示にする
        this.gameObject.SetActive(true);

        DiceSystem = GameObject.Find("DiceSystem");
        uiManager = GameObject.Find("UIManager");
        EventManager = GameObject.Find("EventManager");

        rollSystem = DiceSystem?.GetComponent<DiceRollSystem>();
        uiManage = uiManager?.GetComponent<UIManager>();
        eventManager = EventManager?.GetComponent<EventManager>();

        //ゲームクリア確認用
        playerStatus = GameObject.Find("PlayerStatus");
        playerST = playerStatus.GetComponent<PlayerStatus>();
    }

    public void GetValue() 
    {
        //出た目分動いた場合のみ起動可能
        if (GoPiece == 0)
        {
            //サイコロを振る(ボタンにアタッチ)
            //ダイスロールscript内の関数を実行する
            GoPiece = FindObjectOfType<DiceRollSystem>().GetDice();
        }
        else 
        {
            Debug.Log("コマを動かしてね");
        }

        if(How_First == true)//始めてダイスロールするなら
        {
            //ManageClickText(bool How_first)
            uiManage.ManageClickText(How_First);
            How_First = false;
        }
    }

    private void OnMouseDown()
    {
        //分岐を選択しない限り、反応しないようにする
        if (!canClick)
        {
            Debug.Log("はんのうしないよ");
            return;
        }

        //ダイスの目が残っていたら進む
        if(GoPiece != 0)
        {
            changePos();

            GoPiece -= 1;
            rollSystem.UpdateText(GoPiece);
            //もしライン以上であればクリア
            if(transform.position.x == -6) 
            {
                GoPiece = 0;
                Debug.Log("ラストバトルだ！");
            }
            if(GoPiece == 0)//出目分すすみおわったら表示する
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
                //マスが0の時にイベントを発火する
                eventManager.IgnitionEvent(transform.position.x, transform.position.y);
            }

            uiManage.ManageClickText(How_First);
        }
        else
        {
            Debug.Log("ダイスを振っていないよ");
        }
    }

    //プレイヤーのコマを進める関数
    private void changePos() 
    {

        //分岐に進むと決めたとき
        if (How_Branch1 == true)
        {
            transform.position += new Vector3(0, -1, 0);
            //今後、分岐に分岐を重ねる予定であるため、ここでfalseにしておく
            How_Branch1 = false;
        }
        else if(How_Branch2 == true) 
        {
            transform.position += new Vector3(0, 1, 0);
            //今後、分岐に分岐を重ねる予定であるため、ここでfalseにしておく
            How_Branch2 = false;
        }
        //一つ目の分岐を進んでいるとき
        else if (transform.position.y <= -2)
        {
            //分岐の角の1個目についていないとき
            if (transform.position.y > -5 && transform.position.x < 6)
            {
                transform.position += new Vector3(0, -1, 0);
            }
            //分岐の角の2個目についていないとき
            else if (transform.position.x < 6)
            {
                transform.position += new Vector3(1, 0, 0);
            }
            //分岐の角の2個目についたとき
            else
            {
                transform.position += new Vector3(0, 1, 0);
            }
        }
        //二つ目の分岐を進んでいるとき
        else if (transform.position.y >= 0)
        {
            //分岐の角の1個目についていないとき
            if (transform.position.y < 3 && transform.position.x < 3)
            {
                transform.position += new Vector3(0, 1, 0);
            }
            //分岐の角の2個目についていないとき
            else if(transform.position.x < 3) 
            {
                transform.position += new Vector3(1, 0, 0);
            }
            //分岐の角の2個目についたとき
            else
            {
                transform.position += new Vector3(0, -1, 0);
            }
        }
        //分岐に進まないと決めたとき、または分岐にたどり着いていないとき
        else
        {
            //-1ずつずらす
            transform.position += new Vector3(-1, 0, 0);
            //一つ目の分岐に着いたとき
            if (transform.position.x == 1)
            {
                canClick = false;
                howGO_Branch(1);
            }
            //二つ目の分岐点に着いたとき
            if (transform.position.x == -2) 
            {
                canClick = false;
                howGO_Branch(2);
            }
        }
        
        
    }

    //分岐の選択処理関数
    private void howGO_Branch(int number)
    {
        //分岐の矢印の表示※矢印はOnMouseDownが付いている
        uiManage.ManageBrunchArrow(true, number);
    }

    
}
