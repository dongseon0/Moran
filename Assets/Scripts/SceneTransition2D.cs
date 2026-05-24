using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition2D : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string targetSceneName;

    [Header("Option")]
    [SerializeField] private string playerTag = "Player";

    private bool isLoading;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading) return;

        if (!other.CompareTag(playerTag)) return;

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("[SceneTransition2D] targetSceneName이 비어 있음");
            return;
        }

        isLoading = true;
        Debug.Log("[SceneTransition2D] 씬 이동: " + targetSceneName);

        SceneManager.LoadScene(targetSceneName);
    }
}