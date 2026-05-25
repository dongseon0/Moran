using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 쓰기 위해 꼭 필요한 네임스페이스입니다.
using UnityEngine.Events; // ⭐ [추가] 유니티 이벤트를 쓰기 위해 반드시 필요합니다!

public class DialogueManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject dialoguePanel;     // 배경이 될 검은색 대사 박스(Panel)
    public TextMeshProUGUI dialogueText; // 화면에 표시될 텍스트 컴포넌트

    [Header("대사 내용 입력")]
    [TextArea(3, 5)] // 인스펙터 창에서 줄바꿈을 편하게 할 수 있게 해줍니다.
    public string[] sentences; // 대사들을 순서대로 담을 배열

    [Header("이벤트 설정")]
    // ⭐ [추가] 대사가 모두 끝났을 때 실행될 이벤트입니다. 인스펙터 창에 버튼 형태의 칸이 생깁니다.
    public UnityEvent onDialogueComplete; 

    private int currentIndex = 0; // 현재 몇 번째 대사를 보고 있는지 기억하는 변수

    void Start()
    {
        // 게임이 시작될 때 대사 박스가 켜진 상태로 시작하게 합니다.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // 첫 번째 대사를 보여줍니다.
        if (sentences.Length > 0)
        {
            ShowDialogue();
        }
        else
        {
            Debug.LogWarning("대사 데이터(Sentences)가 비어있습니다!");
        }
    }

    void Update()
    {
        // 사용자가 스페이스바를 누르면 다음 대사로 넘어갑니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    // 현재 순서의 대사를 화면에 출력하는 함수
    void ShowDialogue()
    {
        dialogueText.text = sentences[currentIndex];
    }

    // 다음 대사로 진행하는 함수
    void NextDialogue()
    {
        currentIndex++; // 다음 순서로 번호를 1 올립니다.

        // 만약 준비된 대사가 끝났다면?
        if (currentIndex >= sentences.Length)
        {
            EndDialogue();
        }
        else
        {
            // 아직 대사가 남았다면 다음 대사를 출력합니다.
            ShowDialogue();
        }
    }

    // 모든 독백이 끝났을 때 호출될 함수
    void EndDialogue()
    {
        dialogueText.text = ""; // 텍스트 창을 비웁니다.
        
        Debug.Log("주인공의 독백이 끝났습니다! 이제 유서를 클릭하거나 다음 씬으로 넘어갑니다."); 
        
        // 글자뿐만 아니라 검은색 대사 박스(Panel)를 통째로 화면에서 사라지게 만듭니다.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); 
        }

        // ⭐ [추가] 대사가 끝났음을 알리고 등록된 이벤트(씬 전환 등)를 실행합니다!
        if (onDialogueComplete != null)
        {
            onDialogueComplete.Invoke();
        }

        // 매니저 오브젝트를 끕니다. (이벤트를 먼저 실행한 뒤 안전하게 꺼지도록 순서를 변경했습니다.)
        gameObject.SetActive(false); 
    }
}