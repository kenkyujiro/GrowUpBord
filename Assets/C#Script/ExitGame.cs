using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // �Q�[���I���̏���
    public void QuitGame()
    {
        //�v���v���Z�b�T�f�B���N�e�B�u�c�R���p�C�����̊��̈Ⴂ�Ȃǂŗ��p����������
        // Unity�G�f�B�^���Ŏ��s����Ă���ꍇ
        #if UNITY_EDITOR
            //�v���C���[�h���~����
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // �r���h���ꂽ�A�v���P�[�V�������I��
            Application.Quit();
        #endif
    }
}