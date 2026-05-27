using UnityEngine;
using UnityEngine.Events;
using TMPro;

//추가
using UnityEngine.SceneManagement;
//여기까지

public class LoveLetterInteractable : MonoBehaviour
{
    //BGM용 창 생성
    [Header("BGM")]
    public AudioSource bgmSource;      
    public AudioClip letterBGM;
    public AudioClip afterLetterBGM;
    //여기까지

    //상호작용 소리용 창 생성
    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip paperSound;
    //여기까지

    //효과음 창 생성
    [Header("Space SFX")]
    public AudioClip letterPageSound;
    public int letterSoundCount = 5;
    //여기까지

    [Header("UI 안내창 연결")]
    public GameObject ghostWarningUI;   // "유령은 물건을 만질 수 없습니다" UI
    public GameObject promptEWithText;  // "[E] 연서 열기" UI

    [Header("새로운 나레이션 UI 연결")]
    public GameObject narrationPanel;     // 검은색 배경 패널
    public TextMeshProUGUI narrationText; // 대사 텍스트

    [Header("엔딩 이미지 UI 연결")]
    // 🔴 [추가] 엔딩 이미지 오브젝트를 코드가 직접 제어할 수 있도록 칸을 만듭니다.
    public GameObject endingImageUI;     

    [Header("나레이션 대사 입력")]
    [TextArea(3, 5)]
    public string[] sentences;

    //엔딩 나레이션용 코드
    [Header("엔딩 나레이션")]
    public GameObject endingNarrationPanel;
    public TextMeshProUGUI endingNarrationText;

    [TextArea(3, 5)]
    public string[] endingSentences;

    private int endingIndex = 0;
    private bool isEndingNarrationActive = false;
    //여기까지

    [Header("종료 후 실행할 이벤트")]
    public UnityEvent onNarrationEnd;     

    private bool isPlayerNearby = false;
    private PlayerState cachedPlayerState; 

    private int currentIndex = 0;
    private bool isNarrationActive = false;

    void Start()
    {
        // 게임 시작 시 모든 UI를 확실하게 꺼두어 버그를 방지합니다.
        if (narrationPanel != null) narrationPanel.SetActive(false);
        if (endingImageUI != null) endingImageUI.SetActive(false); // 🔴 시작할 때 강제로 끄기!
    }

    void Update()
    {
        //엔딩 나레이션용 코드
        if (isEndingNarrationActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextEndingSentence();
            }
            return;
        }
        //여기까지

        if (isNarrationActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextSentence();
            }
            return; 
        }

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
        }
        else if (cachedPlayerState.IsPossessed)
        {
            //상호작용 소리용 코드
            PlayPaperSound();
            //여기까지

            if (ghostWarningUI != null) ghostWarningUI.SetActive(false);
            if (promptEWithText != null) promptEWithText.SetActive(false);

            //나레이션 재생 연장
            Invoke(nameof(StartNarration), 1.0f);
            //여기까지

            //StartNarration();
        }
    }

    void StartNarration()
    {
        if (sentences == null || sentences.Length == 0) return;

        isNarrationActive = true;
        currentIndex = 0;

        //연서 BGM 재생용 창
        if (bgmSource != null && letterBGM != null)
        {
            bgmSource.Stop();
            bgmSource.clip = letterBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        //여기까지

        if (narrationPanel != null) narrationPanel.SetActive(true); 
        ShowSentence(); 
    }

    void ShowSentence()
    {
        narrationText.text = sentences[currentIndex];
    }

    void NextSentence()
    {
        //효과음 생성용 코드
        PlaySpaceNextSound();
        //여기까지

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

    //효과음 생성용 코드
    void PlaySpaceNextSound()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        AudioClip targetClip = null;

        if (currentIndex == 4)
        {
            targetClip = letterPageSound;
        }

        if (targetClip != null)
        {
            sfxSource.PlayOneShot(targetClip);
        }
    }
    //여기까지

    void EndNarration()
    {
        isNarrationActive = false;
        narrationText.text = "";

        if (narrationPanel != null) narrationPanel.SetActive(false);

        // 엔딩 BGM 재생용 창
        if (bgmSource != null && afterLetterBGM != null)
        {
            bgmSource.Stop();
            bgmSource.clip = afterLetterBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        //여기까지

        // 🔴 대사가 끝났으므로 엔딩 이미지를 켜줍니다!
        if (endingImageUI != null) 
        {
            endingImageUI.SetActive(true); 
        }

        //엔딩 나레이션용 코드
        StartEndingNarration();
        //여기까지

        if (onNarrationEnd != null)
        {
            onNarrationEnd.Invoke();
        }
    }

    //엔딩 나레이션용 코드
    void StartEndingNarration()
    {
        if (endingSentences == null || endingSentences.Length == 0) return;

        isEndingNarrationActive = true;
        endingIndex = 0;

        if (endingNarrationPanel != null)
            endingNarrationPanel.SetActive(true);

        ShowEndingSentence();
    }

    void ShowEndingSentence()
    {
        if (endingNarrationText != null)
            endingNarrationText.text = endingSentences[endingIndex];
    }

    void NextEndingSentence()
    {
        endingIndex++;

        if (endingIndex >= endingSentences.Length)
        {
            isEndingNarrationActive = false;

            if (endingNarrationText != null)
                endingNarrationText.text = "";

            if (endingNarrationPanel != null)
                endingNarrationPanel.SetActive(false);

            return;
        }

        ShowEndingSentence();
    }
    //여기까지

    //상호작용 소리용 코드
    void PlayPaperSound()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (paperSound != null)
            sfxSource.PlayOneShot(paperSound);
    }
    //여기까지

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