using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private PlayerMovement playerMovment;


    private float footstepTimer;
    private float footstepTimeMax =0.3f;
    [SerializeField]private float footstepVolume = 1f;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovment = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f)
        {
            footstepTimer = footstepTimeMax;

            if(playerMovment.isWalking) 
            {

                SoundManager.instance.PlayFootstepsSound(player.transform.position, footstepVolume);
            }
        }
    }



}
