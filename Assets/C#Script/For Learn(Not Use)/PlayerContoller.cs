using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        //水平方向の座標取得
        float horizontal = Input.GetAxis("Horizontal");
        //垂直方向の座標取得
        float vertical = Input.GetAxis("Vertical");

        //更新される移動後の座標取得
        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        //現在位置の変更
        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ぶつかったよ");
       //ぶつかった相手が特定のタグを持っている場合、GameOver関数を呼び出す
       if(collision.gameObject.CompareTag("Obstacle"))
        {
            FindObjectOfType<GameOverManager>().GameOver();
        }
    }
}
