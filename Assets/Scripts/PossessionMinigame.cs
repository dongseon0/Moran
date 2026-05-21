using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PossessionMinigame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider gaugeSlider;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countText;

    [Header("Settings")]
    [SerializeField] private int requiredTapCount = 15;
    [SerializeField] private float timeLimit = 3f;

    private int currentTapCount;
    private float remainTime;
    private bool isPlaying;

    private Action onSuccess;
    private Action onFail;

    private void Awake()
    {
        ClosePanelOnly();
    }

    private void Update()
    {
        if (!isPlaying) return;

        remainTime -= Time.deltaTime;

        if (timerText != null)
            timerText.text = remainTime.ToString("F1");

        if (remainTime <= 0f)
        {
            Fail();
        }
    }

    public void StartMinigame(Action successCallback, Action failCallback)
    {
        onSuccess = successCallback;
        onFail = failCallback;

        currentTapCount = 0;
        remainTime = timeLimit;
        isPlaying = true;

        if (panel != null)
            panel.SetActive(true);

        UpdateUI();

        Debug.Log("[PossessionMinigame] 빙의 미니게임 시작");
    }

    public void OnMash(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!isPlaying) return;

        currentTapCount++;
        UpdateUI();

        Debug.Log("[PossessionMinigame] 연타: " + currentTapCount);

        if (currentTapCount >= requiredTapCount)
        {
            Success();
        }
    }

    private void UpdateUI()
    {
        if (gaugeSlider != null)
        {
            gaugeSlider.maxValue = requiredTapCount;
            gaugeSlider.value = currentTapCount;
        }

        if (countText != null)
            countText.text = $"{currentTapCount} / {requiredTapCount}";

        if (timerText != null)
            timerText.text = remainTime.ToString("F1");
    }

    private void Success()
    {
        if (!isPlaying) return;

        isPlaying = false;
        ClosePanelOnly();

        Debug.Log("[PossessionMinigame] 빙의 성공");
        onSuccess?.Invoke();
    }

    private void Fail()
    {
        if (!isPlaying) return;

        isPlaying = false;
        ClosePanelOnly();

        Debug.Log("[PossessionMinigame] 빙의 실패");
        onFail?.Invoke();
    }

    private void ClosePanelOnly()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public bool IsPlaying => isPlaying;
}