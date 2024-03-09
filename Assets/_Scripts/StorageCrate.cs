using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCrate : MonoBehaviour
{
    [SerializeField] public KitchenObjectSO kitchenObjectSO;
    [SerializeField] private int ammountofkitchenobj = 0;

    [SerializeField] private Transform[] PointsIntheCrate;

    private GameObject[] KitchenObjectsInCrate;

    private void Start()
    {
        KitchenObjectsInCrate = new GameObject[PointsIntheCrate.Length]; // Initialize the array

        for (int i = 0; i < PointsIntheCrate.Length; i++)
        {
            Transform obj = Instantiate(kitchenObjectSO.prefab).transform;
            KitchenObjectsInCrate[i] = obj.gameObject;

            obj.parent = PointsIntheCrate[i];
            obj.localPosition = Vector3.zero;

            ammountofkitchenobj++;
        }

    }


    public void RemoveKitchenobjfromCrate()
    {
        if (ammountofkitchenobj > 0)
        {
            Destroy(KitchenObjectsInCrate[ammountofkitchenobj - 1], 0.1f);
            ammountofkitchenobj--;
        }
    }


}
