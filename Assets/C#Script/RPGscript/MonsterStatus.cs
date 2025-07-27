using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public int monster_hp;
    public int monster_power;
    public int monster_defence;
    public int monster_EXP;

    public void SetStatus(string monster)
    {
        if (monster == "Bison")//‹­“G
        {
            monster_hp = 50;
            monster_power = 7;
            monster_defence = 3;
            monster_EXP = 20;
        }
        else if (monster == "Zako1")//•’Ê‚ÌG‹›
        {
            monster_hp = 10;
            monster_power = 2;
            monster_defence = 1;
            monster_EXP = 4;
        }
        else if (monster == "Zako2")//UŒ‚—Í‚ª‹­‚¢G‹›(ƒhƒ‰ƒL[)
        {
            monster_hp = 30;
            monster_power = 5;
            monster_defence = 1;
            monster_EXP = 10;
        }
        else if (monster == "Rare")//ƒhƒƒbƒv‚·‚é‚©‚à‚µ‚ê‚È‚¢“G
        {
            monster_hp = 20;
            monster_power = 2;
            monster_defence = 1;
            monster_EXP = 4;
        }
        else if (monster == "Run")//“¦‚°‚é“G
        {
            monster_hp = 5;
            monster_power = 1;
            monster_defence = 10;
            monster_EXP = 1000;
        }
        else//Boss
        {
            monster_hp = 100;
            monster_power = 10;
            monster_defence = 5;
            monster_EXP = 4;
        }
    }
}
