using TMPro;
using UnityEngine;

public class QuitImg : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void TextUpdate()
    {
        text.text = $"버틴 턴 수 : {EventManager.Instance.GameTurn}";
    }
}
