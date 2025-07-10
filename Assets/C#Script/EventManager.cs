using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject Player;     //現在止まっているマス参照用
    private MovePiece movePiece;

    void Awake()
    {
        movePiece = Player.GetComponent<MovePiece>();
    }

    public void IgnitionEvent()
    {
        //x (-5～7)(y：-1のみ)、 y (-1～3)(x：-2～3のみ)
        //分岐最初(-2 -1) 分岐最後(3 -1)
        if(movePiece.GoPiece == 0) 
        {
            if (Player.transform.position.x == -2)
            {
                if (Player.transform.position.y == 0)//(-2 0)
                {
                    Debug.Log("強化！");
                }
                else if(Player.transform.position.y == 3)
                {
                    Debug.Log("凶化だ！");
                }
            }
            else if (Player.transform.position.x == -5)//(-5 -1) 
            {
                Debug.Log("最初からだ！");
                Player.transform.position += new Vector3(13, 0, 0);
            }
        }
    }
}
