using UnityEngine;

public interface IKitchenObjectParent
{
    public void SetKitchenObject(KitchenObject kitchenObject);
    public Transform GetKitchenObjectFollowTransform();
    public KitchenObject GetKitchenObject();
    public void clearKitchenObject();
    public bool HasKitchenObject();


}
