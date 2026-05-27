using TMPro;
using UnityEngine;

public class SimpleMessageUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string message)
    {
        if (panel != null)
            panel.SetActive(true);

        if (messageText != null)
            messageText.text = message;
    }

    public void Hide()
    {
        if (messageText != null)
            messageText.text = "";

        if (panel != null)
            panel.SetActive(false);
    }
}