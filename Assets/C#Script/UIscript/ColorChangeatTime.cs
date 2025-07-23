using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;


public class ColorChangeatTime : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f; //���x�W��(�F�̕ω��X�s�[�h)
    public TextMeshProUGUI ClickText;     //Click!!�e�L�X�g

    void Update()
    {
        //�e�L�X�g����`����Ă��Ȃ��ꍇ�͉������Ȃ�
        if (ClickText == null) return;
        
        //Pingpong(x, 1.0f)�́Ax�̎��ԕ������āA0�`1.0�̐��l�ɕω�����
        float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1.0f);
        //.Lerp(a, b, t)�́Aa��b��t�ŁA���`���(Lerp)����
        Color color = Color.Lerp(Color.black, Color.white, t);
        ClickText.color = color;
    }
}