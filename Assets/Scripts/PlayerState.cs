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

    [Header("Animation")]
    [SerializeField] private Animator animator;

    public bool IsGhost => CurrentState == PlayerStateType.Ghost;
    public bool IsPossessed => CurrentState == PlayerStateType.Possessed;

    private void Awake()
    {
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ApplySavedState();
    }

    private void ApplySavedState()
    {
        if (PlayerCarryData.CurrentState == PlayerStateType.Possessed &&
            PlayerCarryData.PossessedSprite != null)
        {
            SetPossessed(PlayerCarryData.PossessedSprite, false);
        }
        else
        {
            SetGhost(false);
        }
    }

    public void SetGhost(bool save = true)
    {
        CurrentState = PlayerStateType.Ghost;

        if (animator != null)
            animator.enabled = true;

        if (playerSpriteRenderer != null && ghostSprite != null)
            playerSpriteRenderer.sprite = ghostSprite;

        if (save)
            PlayerCarryData.SaveGhost();

        Debug.Log("[PlayerState] 유령 상태");
    }

    public void SetPossessed(Sprite possessedSprite, bool save = true)
    {
        CurrentState = PlayerStateType.Possessed;

        if (animator != null)
            animator.enabled = false;

        if (playerSpriteRenderer != null && possessedSprite != null)
            playerSpriteRenderer.sprite = possessedSprite;

        if (save)
            PlayerCarryData.SavePossessed(possessedSprite);

        Debug.Log("[PlayerState] 빙의 상태 유지");
    }
}