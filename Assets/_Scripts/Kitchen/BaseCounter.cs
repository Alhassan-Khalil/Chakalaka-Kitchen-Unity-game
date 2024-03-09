using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaceHere;

    new public static void ResetStaticData()
    {
        OnAnyObjectPlaceHere = null;
    }
    [SerializeField] private Transform ItemSpawnPoint;



    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternet(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternet();");
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyObjectPlaceHere?.Invoke(this,EventArgs.Empty);
        }
    }
    public Transform GetKitchenObjectFollowTransform() => ItemSpawnPoint;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void clearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}
