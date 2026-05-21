using UnityEngine;

public class PossessedOnlyInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popupPanel;

    public string GetPromptText(PlayerState playerState)
    {
        if (playerState.IsGhost)
            return "Only human can interact with this.";

        return "E: Interact (Possessed Only)";
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