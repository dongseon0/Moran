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
        if (!MainRoomProgress.HasCheckedDeskAsGhost)
            return "...(당신이 보이지 않는 듯하다.)";

        if (isPossessed)
            return "이미 빙의한 몸이다.";

        if (playerState.IsPossessed)
            return "이미 다른 몸을 빌리고 있다.";

        if (isCooldown)
            return "시녀의 영혼이 불안정하다. 잠시 후 다시 시도하자.";

        return "E: 시녀에게 빙의 시도";
    }

    public bool CanInteract(PlayerState playerState)
    {
        if (!MainRoomProgress.HasCheckedDeskAsGhost)
            return false;

        return playerState.IsGhost && !isCooldown && !isPossessed;
    }

    public void Interact(PlayerState playerState)
    {
        if (!MainRoomProgress.HasCheckedDeskAsGhost)
        {
            Debug.Log("[MaidPossessable] 아직 책상을 조사하지 않음. 빙의 불가");
            return;
        }

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