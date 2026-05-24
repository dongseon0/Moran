using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text promptText;

    [Header("Follow Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Camera worldCamera;

    [Header("Position Offset")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private Vector2 screenOffset = new Vector2(0f, 20f);

    private RectTransform panelRect;
    private bool isShowing;

    private void Awake()
    {
        if (panel == null)
            panel = gameObject;

        panelRect = panel.GetComponent<RectTransform>();

        if (worldCamera == null)
            worldCamera = Camera.main;

        Hide();
    }

    private void LateUpdate()
    {
        if (!isShowing) return;
        if (target == null) return;
        if (panelRect == null) return;
        if (worldCamera == null) return;

        Vector3 worldPosition = target.position + worldOffset;
        Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);

        panelRect.position = screenPosition + new Vector3(screenOffset.x, screenOffset.y, 0f);
    }

    public void Show(string text)
    {
        if (panel != null)
            panel.SetActive(true);

        if (promptText != null)
            promptText.text = text;

        isShowing = true;
    }

    public void Hide()
    {
        isShowing = false;

        if (panel != null)
            panel.SetActive(false);
    }
}