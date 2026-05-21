using UnityEngine;

public enum PlayerStateType
{
    Ghost,
    Possessed
}

public class PlayerState : MonoBehaviour
{
    public PlayerStateType CurrentState { get; private set; } = PlayerStateType.Ghost;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite ghostSprite;

    public bool IsGhost => CurrentState == PlayerStateType.Ghost;
    public bool IsPossessed => CurrentState == PlayerStateType.Possessed;

    private void Awake()
    {
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();

        SetGhost();
    }

    public void SetGhost()
    {
        CurrentState = PlayerStateType.Ghost;

        if (playerSpriteRenderer != null && ghostSprite != null)
            playerSpriteRenderer.sprite = ghostSprite;

        Debug.Log("[PlayerState] 유령 상태");
    }

    public void SetPossessed(Sprite possessedSprite)
    {
        CurrentState = PlayerStateType.Possessed;

        if (playerSpriteRenderer != null && possessedSprite != null)
            playerSpriteRenderer.sprite = possessedSprite;

        Debug.Log("[PlayerState] 빙의 상태");
    }
}