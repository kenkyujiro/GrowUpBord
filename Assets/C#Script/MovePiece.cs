using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovePiece : MonoBehaviour
{
    public GameObject GameManager;
    private GameClearManager clear;

    int GoPiece;

    private void Start()
    {
        clear = GameManager.GetComponent<GameClearManager>();
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
    }

    private void OnMouseDown()
    {
        Debug.Log("動いている");

        if(GoPiece != 0)
        {
            //-1ずつずらす
            transform.position += new Vector3(-1, 0, 0);
            GoPiece -= 1;
            //もしライン以上であればクリア
            if(transform.position.x <= -7) 
            {
                Debug.Log("ゲームクリア!!");
                clear.GameClear();
            }
        }
        else
        {
            Debug.Log("ダイスを振っていないよ");
        }
    }
}
