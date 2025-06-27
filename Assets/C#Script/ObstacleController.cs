using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        //障害物を現在位置から、speedの速度で、左に移動する
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //if(transform.position.x < -10f) //x座標が-10より小さくなった場合
        //{
            //Destroy(gameObject);
        //}
    }

    //カメラから見えなくなったときに発動する関数
    void OnBecameInvisible()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(gm != null)
        {
            gm.AddScore(1);
        }
        else
        {
            Debug.LogWarning("GMが見つかりませんでした。");
        }
        
        Destroy(gameObject);
    }
}
