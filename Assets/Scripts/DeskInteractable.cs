using UnityEngine;

public class DeskInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private SimpleMessageUI messageUI;

    [Header("Messages")]
    [TextArea(2, 4)]
    [SerializeField] private string ghostMessage = "유령은 물건을 만질 수 없다. ";

    [TextArea(3, 6)]
    [SerializeField] private string possessedMessage =
        "쓰다 만 일기가 보인다.\n[그 사람이 보고 싶을 때마다 그 방에 간다.]\n라고 쓰여있다.";

    public string GetPromptText(PlayerState playerState)
    {
        return "E: 책상 조사하기";
    }

    public bool CanInteract(PlayerState playerState)
    {
        // 유령 상태에서도 E 입력을 받아야 플래그를 켤 수 있으므로 true
        return true;
    }

    public void Interact(PlayerState playerState)
    {
        if (playerState.IsGhost)
        {
            MainRoomProgress.HasCheckedDeskAsGhost = true;

            if (messageUI != null)
                messageUI.Show(ghostMessage);

            Debug.Log("[DeskInteractable] 유령 상태로 책상 조사. 시녀 빙의 가능 플래그 ON");
            return;
        }

        if (playerState.IsPossessed)
        {
            if (messageUI != null)
                messageUI.Show(possessedMessage);

            Debug.Log("[DeskInteractable] 빙의 상태로 책상 조사");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (messageUI != null)
            messageUI.Hide();

        Debug.Log("[DeskInteractable] 책상 범위 이탈. 메시지 숨김");
    }
}