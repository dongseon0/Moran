using System.Collections;
using UnityEngine;

public class MaidPossessable : MonoBehaviour, IInteractable
{
    [SerializeField] private PossessionMinigame possessionMinigame;

    [Header("Maid Visual")]
    [SerializeField] private SpriteRenderer maidSpriteRenderer;

    [Header("Cooldown")]
    [SerializeField] private float failCooldown = 5f;

    private bool isCooldown;
    private bool isPossessed;

    public string GetPromptText(PlayerState playerState)
    {
        if (isPossessed)
            return "Already possessed.";

        if (playerState.IsPossessed)
            return "Already possessing another body.";

        if (isCooldown)
            return "The maid's spirit is shaking. Try again later.";

        return "E: Attempt to possess the maid";
    }

    public bool CanInteract(PlayerState playerState)
    {
        return playerState.IsGhost && !isCooldown && !isPossessed;
    }

    public void Interact(PlayerState playerState)
    {
        if (possessionMinigame == null)
        {
            Debug.LogError("[MaidPossessable] PossessionMinigame 연결 안 됨");
            return;
        }

        possessionMinigame.StartMinigame(
            successCallback: () =>
            {
                isPossessed = true;

                Sprite maidSprite = null;

                if (maidSpriteRenderer != null)
                    maidSprite = maidSpriteRenderer.sprite;

                playerState.SetPossessed(maidSprite);

                PlayerInteraction2D interaction = playerState.GetComponent<PlayerInteraction2D>();
                if (interaction != null)
                    interaction.ClearCurrentInteractable();

                Debug.Log("[MaidPossessable] 시녀 빙의 성공. 시녀 NPC 제거");

                Destroy(gameObject);
            },
            failCallback: () =>
            {
                StartCoroutine(CooldownRoutine());
                Debug.Log("[MaidPossessable] 시녀 빙의 실패. 쿨타임 시작");
            }
        );
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(failCooldown);
        isCooldown = false;

        Debug.Log("[MaidPossessable] 시녀 빙의 쿨타임 종료");
    }
}