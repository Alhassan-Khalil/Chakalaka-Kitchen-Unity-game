using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCountter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO; 



    [Space(10)]
    [SerializeField] private SpriteRenderer[] SR;



    [SerializeField] private Transform CrateType;
    [SerializeField] private StorageCrate[] CratesHere;

    [SerializeField] private Transform[] cratesPlace;
    [SerializeField] private int amountOfCrates = 2;


    private void Awake()
    {
        if(SR != null) { 
            foreach (SpriteRenderer sr in SR) {
            sr.sprite = kitchenObjectSO.sprite;
            }
        }




        if (!CrateType) return;
        CratesHere = new StorageCrate[amountOfCrates]; // Initialize the array

        for (int i = 0; i < cratesPlace.Length; i++)
        {
            if (i <= amountOfCrates - 1)
            {
                Transform obj = Instantiate(CrateType).transform;

                obj.GetComponent<StorageCrate>().kitchenObjectSO = this.kitchenObjectSO;
                CratesHere[i] = obj.GetComponent<StorageCrate>();

                obj.transform.parent = cratesPlace[i];
                obj.transform.localPosition = Vector3.zero;

            }

        }
    }

    public override void Interact(Player player)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (!player.HasKitchenObject())
        {// player not carrying something


            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            if (CrateType)
            {
                CratesHere[0].RemoveKitchenobjfromCrate();
            }
        }
        else
        {
            // player is carrying something
        }

    }


}
