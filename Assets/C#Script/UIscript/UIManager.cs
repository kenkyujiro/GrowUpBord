using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI DiceText;    //�o�ڂ̕\���e�L�X�g
    public GameObject DiceTextObj;      //�o�ڂ̕\���p�l��
    public GameObject DiceButtonObj;    //�_�C�X�{�^��

    public TextMeshProUGUI ClickText;   //Click!!�e�L�X�g

    public GameObject BranchArrow1;     //��ڂ̕���̖��
    public GameObject BranchArrow2;     //��ڂ̕���̖��

    void Start()
    {
        //�_�C�X�{�^���ȊO��\���ɂ���
        DiceTextObj.gameObject.SetActive(false);
        DiceButtonObj.gameObject.SetActive(true);
        ClickText.gameObject.SetActive(false);
        BranchArrow1.gameObject.SetActive(false);
        BranchArrow2.gameObject.SetActive(false);
    }

    public void ManageDiceUI(bool DicePanel, bool DiceButton)
    {
        DiceTextObj.gameObject.SetActive(DicePanel);
        DiceButtonObj.gameObject.SetActive(DiceButton);
    }

    public void ManageClickText(bool How_first)
    {
        ClickText.gameObject.SetActive(How_first);
    }

    public void ManageBrunchArrow(bool BrunchArrow, int number)
    {
        //���򂲂Ƃɕ\���������؂�ւ���
        if(number == 1)
        {
            BranchArrow1.gameObject.SetActive(BrunchArrow);
        }
        else
        {
            BranchArrow2.gameObject.SetActive(BrunchArrow);
        }
    }
}
