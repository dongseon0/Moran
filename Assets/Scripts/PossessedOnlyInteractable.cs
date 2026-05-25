using UnityEngine;

public class PossessedOnlyInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popupPanel;

    public string GetPromptText(PlayerState playerState)
    {
        if (playerState.IsGhost)
            return "유령은 물건을 만질 수 없다.\n빙의 후 뒷문으로 들어와야 할 것 같다.";

        return "[E] 연서 열기";
    }

    public bool CanInteract(PlayerState playerState)
    {
        return playerState.IsPossessed;
    }

    public void Interact(PlayerState playerState)
    {
        if (popupPanel != null)
            popupPanel.SetActive(true);

        Debug.Log("[PossessedOnlyInteractable] Possessed state interaction successful");
    }
}