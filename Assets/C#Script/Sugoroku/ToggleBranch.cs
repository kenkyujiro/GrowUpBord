using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//矢印決定システム
public class ToggleBranch : MonoBehaviour
{
    public GameObject Player;     //ゲームクリアscriptへ参照用
    private MovePiece movePiece;

    public GameObject uiManager;       //UI表示管理用
    private UIManager uiManage;

    private void Awake()
    {
        movePiece = Player.GetComponent<MovePiece>();
        uiManage = uiManager.GetComponent<UIManager>();
    }
    private void OnMouseDown()
    {
        if(gameObject.CompareTag("Branch"))
        {
            movePiece.How_Branch = true;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false);
        }
        else
        {
            movePiece.How_Branch = false;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false);
        }
    }
}
