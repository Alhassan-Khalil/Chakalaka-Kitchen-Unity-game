using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO KitchenObjectSO;

    private IKitchenObjectParent KitchenObjParent;


    public void SetKitchenObjParent(IKitchenObjectParent KitchenObjParent)
    {
        //clear the old counter
        if(this.KitchenObjParent != null)
        {
            this.KitchenObjParent.clearKitchenObject();
        }

        // set the new counter
        this.KitchenObjParent = KitchenObjParent;
        if (KitchenObjParent.HasKitchenObject())
        {
            Debug.LogError("Counter already has a kitchenObject");
        }
        KitchenObjParent.SetKitchenObject(this);

        transform.parent = KitchenObjParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjParent() => KitchenObjParent;
    public KitchenObjectSO GetKitchenObjectSO() => KitchenObjectSO;


    public void DesterSelf()
    {
        KitchenObjParent.clearKitchenObject();
        Destroy(gameObject);
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent)
    {

        GameObject KitchenObjectGO = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = KitchenObjectGO.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjParent(kitchenObjectParent);

        return kitchenObject;
    }

}