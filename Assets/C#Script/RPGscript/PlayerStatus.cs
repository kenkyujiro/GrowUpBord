using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //defenceはluckで貫通できるもの
    public int power, hp, defence, luck;
    public int Max_hp;
    public int Level;

    public void Awake()
    {
        Level = 1;
        power = 1;
        hp = 20;
        Max_hp = 20;
        defence = 0;
        //疑似乱数で作る
        luck = 1;
    }

    public void Levelup()
    {
        Level += 1;

        //レベルごとに能力上昇
        if(Level % 2 == 0)
        {
            powerUp("power", 2, "0", 0);
        }
        if(Level % 5 == 0)
        {
            powerUp("defenece", 2, "0", 0);
        }
        else if (Level % 10 == 0)
        {
            powerUp("luck", 1, "0", 0);
        }

        //必ず最大HPが増えるようにする
        powerUp("hp", 3, "0", 0);
    }

    //二つ要素があるのは装備用
    public void powerUp(string element1, int element1_number, string element2, int element2_number)
    {
        if (element1 == "power")
        {
            power += element1_number;
        }
        else if (element1 == "hp")
        {
            Max_hp += element1_number;
        }
        else if (element1 == "defence")
        {
            defence += element1_number;
        }
        else
        {
            luck += element1_number;
        }


        if (element2 == "power")
        {
            power += element2_number;
        }
        else if (element2 == "hp")
        {
            Max_hp += element2_number;
        }
        else if (element2 == "defence")
        {
            defence += element2_number;
        }
        //2つ目の要素は空の場合がある
        else if (element2 == "luck")
        {
            luck += element2_number;
        }
    }

    //二つ要素があるのは装備用
    public void powerDown(string element1, int element1_number, string element2, int element2_number)
    {
        if (element1 == "power")
        {
            power -= element1_number;
        }
        else if (element1 == "hp")
        {
            Max_hp -= element1_number;
        }
        else if (element1 == "defence")
        {
            defence -= element1_number;
        }
        else
        {
            luck -= element1_number;
        }


        if (element2 == "power")
        {
            power -= element2_number;
        }
        else if (element2 == "hp")
        {
            Max_hp -= element2_number;
        }
        else if (element2 == "defence")
        {
            defence -= element2_number;
        }
        //2つ目の要素は空の場合がある
        else if (element2 == "luck")
        {
            luck -= element2_number;
        }
    }
}
