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
        narrationText.text = "";

        // UI 끄기
        if (narrationPanel != null) narrationPanel.SetActive(false);

        // 대사가 끝났으니 등록된 이벤트(씬 전환 등)를 터트립니다!
        if (onNarrationEnd != null)
        {
            onNarrationEnd.Invoke();
        }

        // 스크립트가 붙은 오브젝트도 안전하게 꺼줍니다.
        gameObject.SetActive(false);
    }
}