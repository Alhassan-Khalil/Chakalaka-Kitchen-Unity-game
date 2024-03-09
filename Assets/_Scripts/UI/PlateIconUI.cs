using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject PlateKitchenObject;
    [SerializeField] private Transform IconTemplate;



    private void Awake()
    {
        IconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        PlateKitchenObject.OnIngredientAdded += KitchenObject_OnIngredientAdded;
    }

    private void KitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisula();
    }

    public void UpdateVisula()
    {
        foreach (Transform child in transform)
        {
           
            if (child == IconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in PlateKitchenObject.GetKitchenObjectSOList())
        {
            Transform IcontTransform = Instantiate(IconTemplate, transform);
            IcontTransform.gameObject.SetActive(true);
            IcontTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
