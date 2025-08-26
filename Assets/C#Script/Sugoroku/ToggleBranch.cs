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
        //一つ目の分岐で下に進む
        if(gameObject.CompareTag("Branch1_1"))
        {
            movePiece.How_Branch1 = true;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false, 1);
        }
        //一つ目の分岐で真っすぐ進む
        else if (gameObject.CompareTag("Branch1_2"))
        {
            movePiece.How_Branch1 = false;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false, 1);
        }
        else if(gameObject.CompareTag("Branch2_1"))
        {
            movePiece.How_Branch2 = true;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false, 2);
        }
        else if (gameObject.CompareTag("Branch2_2"))
        {
            movePiece.How_Branch2 = false;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false, 2);
        }
    }
}
