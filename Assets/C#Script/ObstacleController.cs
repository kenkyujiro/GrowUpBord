using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        //��Q�������݈ʒu����Aspeed�̑��x�ŁA���Ɉړ�����
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //if(transform.position.x < -10f) //x���W��-10��菬�����Ȃ����ꍇ
        //{
            //Destroy(gameObject);
        //}
    }

    //�J�������猩���Ȃ��Ȃ����Ƃ��ɔ�������֐�
    void OnBecameInvisible()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(gm != null)
        {
            gm.AddScore(1);
        }
        else
        {
            Debug.LogWarning("GM��������܂���ł����B");
        }
        
        Destroy(gameObject);
    }
}
