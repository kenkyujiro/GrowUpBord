using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStatus : MonoBehaviour
{

    public GameObject UIStatus;
    
    void Start()
    {
        UIStatus = GameObject.Find("PlayerStatus");
        UIStatus.SetActive(false);
    }

    //UI�̕\����؂�ւ���
    public void ToggleUI()
    {
        UIStatus.SetActive(!UIStatus.activeSelf);
    }
}
