using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��󌈒�V�X�e��
public class ToggleBranch : MonoBehaviour
{
    public GameObject Player;     //�Q�[���N���Ascript�֎Q�Ɨp
    private MovePiece movePiece;

    public GameObject uiManager;       //UI�\���Ǘ��p
    private UIManager uiManage;

    private void Awake()
    {
        movePiece = Player.GetComponent<MovePiece>();
        uiManage = uiManager.GetComponent<UIManager>();
    }
    private void OnMouseDown()
    {
        //��ڂ̕���ŉ��ɐi��
        if(gameObject.CompareTag("Branch1_1"))
        {
            movePiece.How_Branch1 = true;
            movePiece.canClick = true;
            uiManage.ManageBrunchArrow(false, 1);
        }
        //��ڂ̕���Ő^�������i��
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
