using UnityEngine;
using System.Collections.Generic;

public class AIStatus : MonoBehaviour
{
    public static AIStatus Instance { get; private set; }

    public int maxHP = 30;
    public int hp = 30;
    public int power = 5;
    public int defence = 10;
    public int luck = 0;
    public int mp = 25;//�g�������l����������

    public void Awake()
    {
        // �V���O���g������������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //�������̕��@�ŃA�b�v�����Ƃ��p
    public void PowerUP(string UPName, int UPNumber)
    {
        if(UPName == "hp")
        {
            maxHP += UPNumber;
            hp += UPNumber;
        }
        else if(UPName == "power")
        {
            power += UPNumber;
        }
        else if (UPName == "defence")
        {
            defence += UPNumber;
        }
        else if (UPName == "luck")
        {
            luck += UPNumber;
        }
        else
        {
            mp += UPNumber;
        }
    }

    public void PowerDown(string DownName, int DownNumber)
    {
        if (DownName == "hp")
        {
            maxHP -= DownNumber;
            hp -= DownNumber;
        }
        else if (DownName == "power")
        {
            power -= DownNumber;
        }
        else if (DownName == "defence")
        {
            defence -= DownNumber;
        }
        else if (DownName == "luck")
        {
            luck -= DownNumber;
        }
        else
        {
            mp -= DownNumber;
        }
    }
}
