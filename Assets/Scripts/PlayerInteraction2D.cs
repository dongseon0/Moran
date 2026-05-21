using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction2D : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private InteractionPromptUI promptUI;

    private IInteractable currentInteractable;

    private void Awake()
    {
        if (playerState == null)
            playerState = GetComponent<PlayerState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[PlayerInteraction2D] Trigger Enter: " + other.name);
        TrySetInteractable(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (currentInteractable != null) return;

        Debug.Log("[PlayerInteraction2D] Trigger Stay: " + other.name);
        TrySetInteractable(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("[PlayerInteraction2D] Trigger Exit: " + other.name);

        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable = null;

                if (promptUI != null)
                    promptUI.Hide();
            }
        }
    }

    private void TrySetInteractable(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            Debug.Log("[PlayerInteraction2D] IInteractable 감지 성공: " + other.name);

            currentInteractable = interactable;
            UpdatePrompt();
        }
        else
        {
            Debug.Log("[PlayerInteraction2D] IInteractable 없음: " + other.name);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log("[PlayerInteraction2D] Interact 입력 감지");

        if (currentInteractable == null)
        {
            Debug.Log("[PlayerInteraction2D] currentInteractable 없음");
            return;
        }

        if (!currentInteractable.CanInteract(playerState))
        {
            if (promptUI != null)
                promptUI.Show(currentInteractable.GetPromptText(playerState));

            return;
        }

        currentInteractable.Interact(playerState);
        UpdatePrompt();
    }

    private void UpdatePrompt()
    {
        if (promptUI == null)
        {
            Debug.LogError("[PlayerInteraction2D] Prompt UI 연결 안 됨");
            return;
        }

        if (currentInteractable == null)
        {
            promptUI.Hide();
            return;
        }

        string prompt = currentInteractable.GetPromptText(playerState);
        Debug.Log("[PlayerInteraction2D] Prompt 표시: " + prompt);

        promptUI.Show(prompt);
    }
}