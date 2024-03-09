using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    [SerializeField] private Transform Container;
    [SerializeField] private Transform recipeTemplate;


    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryMangaer.Instance.OnRecipeSpwaned += DeliveryManager_OnRecipeSpwaned;
        DeliveryMangaer.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryMangaer.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpwaned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in Container)
        {

            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        

        foreach (RecipeSO recipeSO in DeliveryMangaer.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, Container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryMangerSingleUI>().SetRecipeSO(recipeSO);
        }
    }


}
