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
