using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button diceButton;

    private void Start()
    {
        diceButton.onClick.RemoveAllListeners(); // �Â����X�i�[�������i�K�{�j
        diceButton.onClick.AddListener(() =>
        {
            MovePiece.Instance.GetValue();
        });
    }
}