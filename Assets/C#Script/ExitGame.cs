using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // ゲーム終了の処理
    public void QuitGame()
    {
        //プリプロセッサディレクティブ…コンパイル時の環境の違いなどで利用される条件式
        // Unityエディタ内で実行されている場合
        #if UNITY_EDITOR
            //プレイモードを停止する
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // ビルドされたアプリケーションを終了
            Application.Quit();
        #endif
    }
}