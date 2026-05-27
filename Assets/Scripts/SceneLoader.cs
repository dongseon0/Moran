using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneWithSound());
    }

    IEnumerator LoadSceneWithSound()
    {
        audioSource.PlayOneShot(clickSound);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene("NarrationScene");
    }
}