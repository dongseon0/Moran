using UnityEngine;

public static class PlayerCarryData
{
    public static PlayerStateType CurrentState = PlayerStateType.Ghost;
    public static Sprite PossessedSprite;

    public static void SaveGhost()
    {
        CurrentState = PlayerStateType.Ghost;
        PossessedSprite = null;
    }

    public static void SavePossessed(Sprite sprite)
    {
        CurrentState = PlayerStateType.Possessed;
        PossessedSprite = sprite;
    }

    public static void Reset()
    {
        CurrentState = PlayerStateType.Ghost;
        PossessedSprite = null;
    }
}