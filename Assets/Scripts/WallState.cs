using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Tilemaps;

public class WallState : MonoBehaviour
{
    public GameObject player;
    PlayerState playerState;
    Collider2D cd;

    private void Awake()
    {
        cd = GetComponent<Collider2D>();
        playerState = player.GetComponent<PlayerState>();
    }
    private void Update()
    {
        if (IsPassable())
        {
            cd.enabled = false;
            //Debug.Log("통과가능");
        }
        else
        {
            cd.enabled = true;
            //Debug.Log("막혔음");
        }
            
    }

    private bool IsPassable()
    {
        return playerState.CurrentState == PlayerStateType.Ghost ? true : false;
    }
}
