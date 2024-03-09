using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBell : BaseCounter
{
    public static TableBell Instance {  get; private set; }

    public event EventHandler OnRingTheBell;

    private bool GameStarted = false;
    private AudioSource BellSound;




    private void Awake()
    {
        Instance = this;
        BellSound = GetComponent<AudioSource>();
    }


    public override void Interact(Player player)
    {
        BellSound.Play();

        if (!GameStarted) 
        {
            OnRingTheBell?.Invoke(this, EventArgs.Empty);
            GameStarted = true;
        }

    }

}
