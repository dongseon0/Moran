using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LoveLetterInteractable : MonoBehaviour
{
    [Header("UI 안내창 연결")]
    public GameObject ghostWarningUI;   // "유령은 물건을 만질 수 없습니다" UI
    public GameObject promptEWithText;  // "[E] 연서 열기" UI

    [Header("새로운 나레이션 UI 연결")]
    public GameObject narrationPanel;     // 검은색 배경 패널
    public TextMeshProUGUI narrationText; // 대사 텍스트

    [Header("나레이션 대사 입력")]
    [TextArea(3, 5)]
    public string[] sentences;            

    [Header("종료 후 실행할 이벤트 (씬 전환)")]
    public UnityEvent onNarrationEnd;     

    private bool isPlayerNearby = false;
    private PlayerState cachedPlayerState; 

    // 나레이션 제어용 내부 변수
    private int currentIndex = 0;
    private bool isNarrationActive = false;

    // 🔴 [추가] 게임이 시작될 때 실행되는 함수입니다.
    void Start()
    {
        // 게임 시작 시 나레이션 패널을 확실하게 꺼둡니다.
        if (narrationPanel != null)
        {
            narrationPanel.SetActive(false);
        }
    }

    void Update()
    {
        // 1. 나레이션이 진행 중일 때는 오직 스페이스바 입력만 받아 대사를 넘깁니다.
        if (isNarrationActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextSentence();
            }
            return; 
        }

        // 2. 플레이어가 범위 안에 있고, [E] 키를 눌렀을 때 인터랙션 처리
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && cachedPlayerState != null)
        {
            HandleInteraction();
        }
    }

    void HandleInteraction()
    {
        if (cachedPlayerState.IsGhost)
        {
            if (ghostWarningUI != null) ghostWarningUI.SetActive(true);
            if (promptEWithText != null) promptEWithText.SetActive(false);
            Debug.Log("유령 상태라 연서를 열 수 없습니다.");
        }
        else if (cachedPlayerState.IsPossessed)
        {
            if (ghostWarningUI != null) ghostWarningUI.SetActive(false);
            if (promptEWithText != null) promptEWithText.SetActive(false);

            // 자체적으로 나레이션을 바로 시작합니다!
            StartNarration();
        }
    }

    void StartNarration()
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning("나레이션 대사가 비어있습니다!");
            return;
        }

        isNarrationActive = true;
        currentIndex = 0;

        if (narrationPanel != null) narrationPanel.SetActive(true); // E 누르면 여기서 켜짐!
        ShowSentence(); 
    }

    void ShowSentence()
    {
        narrationText.text = sentences[currentIndex];
    }

    void NextSentence()
    {
        currentIndex++;

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

        if (narrationPanel != null) narrationPanel.SetActive(false); // 끝나면 여기서 꺼짐!

        if (onNarrationEnd != null)
        {
            onNarrationEnd.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isNarrationActive) return;

        if (other.CompareTag("Player"))
        {
            cachedPlayerState = other.GetComponent<PlayerState>();

            if (cachedPlayerState != null)
            {
                isPlayerNearby = true;
                
                if (cachedPlayerState.IsGhost)
                {
                    if (ghostWarningUI != null) ghostWarningUI.SetActive(true);
                    if (promptEWithText != null) promptEWithText.SetActive(false);
                }
                else if (cachedPlayerState.IsPossessed)
                {
                    if (ghostWarningUI != null) ghostWarningUI.SetActive(false);
                    if (promptEWithText != null) promptEWithText.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            cachedPlayerState = null; 

            if (ghostWarningUI != null) ghostWarningUI.SetActive(false);
            if (promptEWithText != null) promptEWithText.SetActive(false);
        }
    }
}