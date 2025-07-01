using System.Collections;
using System.Collections.Generic;
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

    int GoPiece;                       //進める出目の数
    bool How_First;                    //始めてのダイスロールかどうか

    private void Start()
    {
        clear = GameManager.GetComponent<GameClearManager>();
        rollSystem = DiceSystem.GetComponent<DiceRollSystem>();
        uiManage = uiManager.GetComponent<UIManager>();

        How_First = true;
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

        if(GoPiece != 0)
        {
            //-1ずつずらす
            transform.position += new Vector3(-1, 0, 0);
            GoPiece -= 1;
            rollSystem.UpdateText(GoPiece);
            //もしライン以上であればクリア
            if(transform.position.x <= -7) 
            {
                Debug.Log("ゲームクリア!!");
                clear.GameClear();
            }
            if(GoPiece == 0)//出目分すすみおわったら表示する
            {
                //ManageDiceUI(bool DicePanel, bool DiceButton)
                uiManage.ManageDiceUI(false, true);
            }

            uiManage.ManageClickText(How_First);
        }
        else
        {
            Debug.Log("ダイスを振っていないよ");
        }
    }
}
