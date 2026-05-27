using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LoveNarrationManager : MonoBehaviour
{
    [Header("UI 컴포넌트 연결")]
    public GameObject narrationPanel;     // 검은색 배경 패널
    public TextMeshProUGUI narrationText; // 대사 텍스트

    [Header("나레이션 대사 입력")]
    [TextArea(3, 5)]
    public string[] sentences;            // 여기에 원하는 대사를 순서대로 적습니다.

    [Header("종료 후 실행할 이벤트 (씬 전환 등)")]
    public UnityEvent onNarrationEnd;     // 대사가 끝나면 실행할 이벤트

    [Header("엔딩 씬")]
    public string endingSceneName = "EndingScene";

    private int currentIndex = 0;
    private bool isNarrationActive = false;

    // 🔴 중요: 팀원의 상호작용 코드나 외부에서 이 함수를 실행시켜서 나레이션을 시작합니다.
    public void StartNarration()
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning("나레이션 대사가 비어있습니다!");
            return;
        }

        isNarrationActive = true;
        currentIndex = 0;

        // UI 켜기
        if (narrationPanel != null) narrationPanel.SetActive(true);

        // 첫 대사 보여주기
        ShowSentence();
    }

    void Update()
    {
        // 나레이션이 진행 중일 때만 스페이스바 입력을 받습니다.
        if (isNarrationActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }
    }

    void ShowSentence()
    {
        narrationText.text = sentences[currentIndex];
    }

    void NextSentence()
    {
        currentIndex++;

        // 모든 대사가 끝났다면?
        if (currentIndex >= sentences.Length)
        {
            EndNarration();
        }
        else
        {
            ShowSentence();
        }
    }

    void EndNarration()
    {
        isNarrationActive = false;

        if (narrationText != null)
            narrationText.text = "";

        if (narrationPanel != null)
            narrationPanel.SetActive(false);

        if (onNarrationEnd != null)
            onNarrationEnd.Invoke();

        if (string.IsNullOrEmpty(endingSceneName))
        {
            Debug.LogError("[LoveNarrationManager] Ending Scene Name이 비어 있습니다.");
            return;
        }

        Debug.Log("[LoveNarrationManager] 엔딩 씬으로 이동: " + endingSceneName);

        SceneManager.LoadScene(endingSceneName);
    }
}