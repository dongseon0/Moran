using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 반드시 필요합니다!

public class AutoSceneChanger : MonoBehaviour
{
    // 이 함수를 원하는 타이밍(영상이 끝났을 때, 애니메이션이 끝났을 때 등)에 호출하세요.
    public void LoadNextScene()
    {
        // 현재 활성화된 씬의 인덱스(번호)를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // 다음 씬의 인덱스를 계산합니다.
        int nextSceneIndex = currentSceneIndex + 1;

        // 다음 씬의 인덱스가 총 씬 개수보다 작을 때만 넘어갑니다. (에러 방지)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("다음 씬이 빌드 세팅에 없습니다! 마지막 씬입니다.");
        }
    }
}