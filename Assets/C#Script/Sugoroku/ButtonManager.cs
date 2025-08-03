using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button diceButton;

    private void Start()
    {
        diceButton.onClick.RemoveAllListeners(); // 古いリスナーを除去（必須）
        diceButton.onClick.AddListener(() =>
        {
            MovePiece.Instance.GetValue();
        });
    }
}