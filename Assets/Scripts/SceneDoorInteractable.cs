using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Scene")]
    [SerializeField] private string targetSceneName;

    [Header("Spawn")]
    [SerializeField] private string targetSpawnId;

    [Header("Prompt")]
    [SerializeField] private string promptText = "E: 이동하기";

    private bool isLoading;

    public string GetPromptText(PlayerState playerState)
    {
        return promptText;
    }

    public bool CanInteract(PlayerState playerState)
    {
        return !isLoading;
    }

    public void Interact(PlayerState playerState)
    {
        if (isLoading) return;

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("[SceneDoorInteractable] Target Scene Name이 비어 있음");
            return;
        }

        SceneTransitionData.NextSpawnId = targetSpawnId;

        isLoading = true;

        Debug.Log($"[SceneDoorInteractable] 씬 이동: {targetSceneName}, SpawnId: {targetSpawnId}");

        SceneManager.LoadScene(targetSceneName);
    }
}