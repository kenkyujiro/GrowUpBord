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
    public GameObject GameManager;     //ゲームクリアscriptへ参照用
    private GameClearManager clear;

    public GameObject DiceSystem;      //出目テキスト参照用
    private DiceRollSystem rollSystem;

    public GameObject uiManager;       //UI表示管理用
    private UIManager uiManage;

    public GameObject EventManager;    //イベント管理用
    private EventManager eventManager;

    public int GoPiece;                //進める出目の数
    private bool How_First;            //始めてのダイスロールかどうか
    public bool How_Branch;            //分岐に進んだかどうか
    public bool canClick;              //クリックできるかどうか

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
        //出た目分動いた場合のみ起動可能
        if (GoPiece == 0)
        {
            //サイコロを振る(ボタンにアタッチ)
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
                Debug.Log("ゲームクリア!!");
                clear.GameClear();
            }
            if(GoPiece == 0)//出目分すすみおわったら表示する
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
                //マスが0の時にイベントを発火する
                eventManager.IgnitionEvent();
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
        if (How_Branch == true)
        {
            transform.position += new Vector3(0, 1, 0);
            //今後、分岐に分岐を重ねる予定であるため、ここでfalseにしておく
            How_Branch = false;
        }
        //分岐を進んでいるとき
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
            //分岐点に着いたとき
            if(transform.position.x == -2) 
            {
                canClick = false;
                howGO_Branch();
            }
        }
        
        
    }

    //分岐の選択処理関数
    private void howGO_Branch()
    {
        //分岐の矢印の表示※矢印はOnMouseDownが付いている
        uiManage.ManageBrunchArrow(true);
    }

    
}
