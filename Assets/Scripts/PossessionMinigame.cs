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

    //BGM, 효과음용 코드
    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX")]
    [SerializeField] private AudioClip bellSound;
    [SerializeField] private AudioClip mashSound;

    [Header("BGM")]
    [SerializeField] private AudioClip minigameBGM;

    [Header("Audio Timing")]
    [SerializeField] private float startDelayAfterBell = 0.8f;
    //여기까지

    private int currentTapCount;
    private float remainTime;
    private bool isPlaying;

    private Action onSuccess;
    private Action onFail;

    private void Awake()
    {
        ClosePanelOnly();

        //BGM용 코드
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        //여기까지
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

        //효과음용 코드
        PlayBellSound();

        Invoke(nameof(ActuallyStartMinigame), startDelayAfterBell);
        //여기까지
    }

    private void ActuallyStartMinigame()
    {
        currentTapCount = 0;
        remainTime = timeLimit;
        isPlaying = true;

        if (panel != null)
            panel.SetActive(true);

        //BGM 재생용 코드
        PlayMinigameBGM();
        //여기까지

        UpdateUI();

        Debug.Log("[PossessionMinigame] 빙의 미니게임 시작");
    }

    public void OnMash(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!isPlaying) return;

        currentTapCount++;

        //효과음용 코드
        PlayMashSound();
        //여기까지

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

        //BGM, 효과음용 코드
        StopMinigameBGM();
        PlayBellSound();
        //여기까지

        Debug.Log("[PossessionMinigame] 빙의 성공");
        onSuccess?.Invoke();
    }

    private void Fail()
    {
        if (!isPlaying) return;

        isPlaying = false;
        ClosePanelOnly();

        //BGM, 효과음용 코드
        StopMinigameBGM();
        PlayBellSound();
        //여기까지

        Debug.Log("[PossessionMinigame] 빙의 실패");
        onFail?.Invoke();
    }

    //BGM, 효과음용 코드
    private void PlayBellSound()
    {
        if (sfxSource != null && bellSound != null)
            sfxSource.PlayOneShot(bellSound);
    }

    private void PlayMashSound()
    {
        if (sfxSource != null && mashSound != null)
            sfxSource.PlayOneShot(mashSound);
    }

    private void PlayMinigameBGM()
    {
        if (bgmSource != null && minigameBGM != null)
        {
            bgmSource.Stop();
            bgmSource.clip = minigameBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private void StopMinigameBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }
    //여기까지

    private void ClosePanelOnly()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public bool IsPlaying => isPlaying;
}