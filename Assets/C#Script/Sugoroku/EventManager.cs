using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject Player;      //現在止まっているマス参照用

    public GameObject playerStatus;//プレイヤーステータスの強化および回復用
    private PlayerStatus playerST;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        playerStatus = GameObject.Find("PlayerStatus");
        playerST = Player.GetComponent<PlayerStatus>();
    }

    //発火したイベントの処理
    public void IgnitionEvent(float x_pos, float y_pos)
    {
        //上のエリア
        //強敵ゾーン
        if (y_pos >= 0)
        {
            System.Random random = new System.Random();
            int value;

            if (x_pos == -2 && y_pos == 3)
            {
                value = random.Next(0, 2);//0～3の乱数を生成

                if (value == 0) { playerST.powerUp("power", 10, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 10, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 10, "none", 0); }

                Debug.Log("強化");

            }
            else if(x_pos == 2 && y_pos == 1) 
            {
                value = random.Next(0, 2);//0～3の乱数を生成

                if (value == 0) { playerST.powerUp("power", 10, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 10, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 10, "none", 0); }

                Debug.Log("強化");
            }
            else if (x_pos == 1 && y_pos == 3)
            {
                Debug.Log("回復だ！");
                playerST.hp += 30;
                if (playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }

            value = random.Next(0, 100);//0～99の乱数を生成
            if (value <= 50)
            {
                //確率でモンスターの種類が変わる
                if (value <= 10)
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Bison");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
                else if(value <= 40)
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Run");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
                else 
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako2");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
            }
        }
        //下のエリア
        else if(y_pos <= -2)
        {
            System.Random random = new System.Random();
            int value;

            if (y_pos == -4 && x_pos == 0)
            {
                value = random.Next(0, 2);//0～3の乱数を生成

                if(value == 0){ playerST.powerUp("power", 5, "none", 0); }
                else if(value == 1){ playerST.powerUp("hp", 5, "none", 0); }
                else if(value == 2){ playerST.powerUp("defence", 5, "none", 0); }

                Debug.Log("強化");
                
            }
            else if(y_pos == -2 && x_pos == 5)
            {
                value = random.Next(0, 2);//0～3の乱数を生成

                if (value == 0) { playerST.powerUp("power", 5, "none", 0); }
                else if (value == 1) { playerST.powerUp("hp", 5, "none", 0); }
                else if (value == 2) { playerST.powerUp("defence", 5, "none", 0); }

                Debug.Log("強化");
            }
            else if (y_pos == -5 && x_pos == 3)
            {
                Debug.Log("回復だ！");
                playerST.hp += 30;
                if (playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }

            value = random.Next(0, 100);//0～99の乱数を生成
            if (value <= 50)
            {
                //確率でモンスターの種類が変わる
                if (value <= 5)
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Rare");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
                else if (value <= 15)
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako2");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
                else
                {
                    //値を保存する
                    savePos();
                    PlayerPrefs.SetString("monsterName", "Zako1");
                    //RPGシーンに遷移する
                    SceneManager.LoadScene("RPGScene");
                }
            }
        }
        //ボスのエリア(マス)
        else if(x_pos <= -6)
        {
            //値を保存する
            savePos();
            PlayerPrefs.SetString("monsterName", "Boss");
            //RPGシーンに遷移する
            SceneManager.LoadScene("RPGScene");
        }
        //真っすぐのエリア
        else
        {
            if(x_pos == 0)
            {
                Debug.Log("回復だ！");
                playerST.hp += 30;
                if(playerST.hp > playerST.Max_hp)
                {
                    playerST.hp = playerST.Max_hp;
                }
            }
        }
    }

    public void testChange()
    {
        //値を保存する
        savePos();
        PlayerPrefs.SetString("monsterName", "Zako1");
        //RPGシーンに遷移する
        SceneManager.LoadScene("RPGScene");
    }

    public void savePos()
    {
        PlayerPrefs.SetFloat("x", Player.transform.position.x);
        PlayerPrefs.SetFloat("y", Player.transform.position.y);
        PlayerPrefs.SetFloat("z", Player.transform.position.z);
        PlayerPrefs.Save();
    }
}
