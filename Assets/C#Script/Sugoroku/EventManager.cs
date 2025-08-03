using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject Player;     //現在止まっているマス参照用

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    //発火したイベントの処理
    public void IgnitionEvent(float x_pos, float y_pos)
    {
        //x (-5～7)(y：-1のみ)、 y (-1～3)(x：-2～3のみ)
        //分岐最初(-2 -1) 分岐最後(3 -1)
        if (x_pos == -2)
        {
            if (y_pos == 0)//(-2 0)
            {
                Debug.Log("強化！");
                //値を保存する
                savePos();
                PlayerPrefs.SetString("monsterName", "Bison");
                //RPGシーンに遷移する
                SceneManager.LoadScene("RPGScene");
            }
            else if(y_pos == 3)
            {
                Debug.Log("凶化だ！");
            }
        }
        else if (x_pos == -5)//(-5 -1) 
        {
            Debug.Log("最初からだ！");
            Player.transform.position += new Vector3(13, 0, 0);
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
