using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {// there is no KitchenObject here
            if (player.HasKitchenObject())
            {// player is carrying something

                player.GetKitchenObject().SetKitchenObjParent(this);
            }
            else
            {// player not carrying something

            }
        }
        else
        {// there is a KitchenObject here
            if (player.HasKitchenObject())
            {// player is carrying something

                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player holding Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DesterSelf();
                    }
                }
                else
                {//Player Not Holding  Plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {// there is Plate on the Counter
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                           player.GetKitchenObject().DesterSelf();
                        }
                    }
                }
            }
            else
            {// player not carrying something
                GetKitchenObject().SetKitchenObjParent(player);
            }
        }
    
    }





}
