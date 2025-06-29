using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DiceRollSystem : MonoBehaviour
{
    public TextMeshProUGUI DiceText;
    int value_dice;

    public int GetDice()
    {
        value_dice = Random.Range(1, 7);//1`6‚Ì®”‚ğ•Ô‚·
        //o‚½”’l‚ğ•\¦‚·‚é
        UpdateText();
        return value_dice;
    }

    private  void UpdateText() 
    {
        DiceText.text = value_dice.ToString();
    }
}
