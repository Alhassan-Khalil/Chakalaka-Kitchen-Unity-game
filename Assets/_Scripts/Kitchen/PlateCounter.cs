using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{

    public event EventHandler OnplateSpwaned;
    public event EventHandler OnplateRemoved;


    private float spawnPlateTimer= 0f;
    private int plateSpawntAmount;
    private int plateSpawntAmountMax = 4;

    [SerializeField] private float SpawnCooldown = 4f;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > SpawnCooldown) {
            spawnPlateTimer = 0f;

            if(GameManager.Instance.IsGamePlaying() && plateSpawntAmount < plateSpawntAmountMax)
            {
                plateSpawntAmount++;

                OnplateSpwaned?.Invoke(this, EventArgs.Empty);

            }


        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empry handed
            if(plateSpawntAmount > 0)
            {//there is at least one plate here
                plateSpawntAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnplateRemoved?.Invoke(this, EventArgs.Empty);


            }
        }
    }


}
