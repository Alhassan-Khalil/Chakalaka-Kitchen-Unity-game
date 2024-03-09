using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player)
    {
        //TODO: Trash Counter;
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DesterSelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

}
