using UnityEngine;

public class PlayerSceneSpawnHandler : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        string targetSpawnId = SceneTransitionData.NextSpawnId;

        if (string.IsNullOrEmpty(targetSpawnId))
        {
            Debug.Log("[PlayerSceneSpawnHandler] 지정된 SpawnId 없음. 기본 위치 사용");
            return;
        }

        SceneSpawnPoint[] spawnPoints = FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None);

        SceneSpawnPoint targetSpawnPoint = null;

        foreach (SceneSpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.SpawnId == targetSpawnId)
            {
                targetSpawnPoint = spawnPoint;
                break;
            }
        }

        if (targetSpawnPoint == null)
        {
            Debug.LogWarning("[PlayerSceneSpawnHandler] SpawnId를 찾지 못함: " + targetSpawnId);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
        {
            Debug.LogError("[PlayerSceneSpawnHandler] Player 태그를 가진 오브젝트를 찾지 못함");
            return;
        }

        player.transform.position = targetSpawnPoint.transform.position;

        Debug.Log("[PlayerSceneSpawnHandler] Player Spawn 완료: " + targetSpawnId);

        SceneTransitionData.NextSpawnId = null;
    }
}