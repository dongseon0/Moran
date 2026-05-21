using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public string GetPromptText(PlayerState playerState)
    {
        return "E: 테스트 상호작용";
    }

    public bool CanInteract(PlayerState playerState)
    {
        return true;
    }

    public void Interact(PlayerState playerState)
    {
        Debug.Log("상호작용 성공!");
    }
}