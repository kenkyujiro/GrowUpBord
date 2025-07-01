using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;


public class ColorChangeatTime : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f; //速度係数(色の変化スピード)
    public TextMeshProUGUI ClickText;     //Click!!テキスト

    void Update()
    {
        //テキストが定義されていない場合は何もしない
        if (ClickText == null) return;
        
        //Pingpong(x, 1.0f)は、xの時間分かけて、0～1.0の数値に変化する
        float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1.0f);
        //.Lerp(a, b, t)は、aとbをtで、線形補間(Lerp)する
        Color color = Color.Lerp(Color.black, Color.white, t);
        ClickText.color = color;
    }
}