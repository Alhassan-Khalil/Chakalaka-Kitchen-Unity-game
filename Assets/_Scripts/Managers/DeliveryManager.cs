using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryMangaer : MonoBehaviour
{


    public event EventHandler OnRecipeSpwaned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryMangaer Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRescipeSOList;
    private float spwanRecipeTimer;
    private float spwanRescipeTimerMax = 8f;
    private int waitingRecipesMax = 5;
    private int successfulRecipesAmpunts;

    private void Awake()
    {
        Instance = this;
        waitingRescipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        spwanRecipeTimer -= Time.deltaTime;
        if (spwanRecipeTimer <= 0f)
        {
            spwanRecipeTimer = spwanRescipeTimerMax;

            if(GameManager.Instance.IsGamePlaying() && waitingRescipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.rescipeSOList[UnityEngine.Random.Range(0, recipeListSO.rescipeSOList.Count)];

                RecipeSO spawnedRecipe = Instantiate(waitingRecipeSO);

                spawnedRecipe.timeLimit = spawnedRecipe.timeLimitMax;
                //Debug.Log(waitingRecipeSO.recipeName);
                waitingRescipeSOList.Add(spawnedRecipe);

                OnRecipeSpwaned?.Invoke(this, EventArgs.Empty);

            }

        }
        // Update timers for each waiting recipe
        foreach (RecipeSO waitingRecipeSO in waitingRescipeSOList)
        {
            waitingRecipeSO.timeLimit -= Time.deltaTime;

            // Check if time limit has reached
            if (waitingRecipeSO.timeLimit <= 0f)
            {
                // Handle recipe failure (e.g., remove from the list, trigger events)
                waitingRescipeSOList.Remove(waitingRecipeSO);
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                break; // Exit the loop after handling one failed recipe
            }
        }
    }


    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRescipeSOList.Count; i++) 
        {
            RecipeSO waitingRecipeSO = waitingRescipeSOList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {//Has the same number of ingredients
                bool plateContentMatchesRecipe = true;

                foreach (KitchenObjectSO recipekitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {//cycling through all ingredients in the recipe
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateRecipekitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {//cycling through all ingredients in the plate
                        if(plateRecipekitchenObjectSO == recipekitchenObjectSO)
                        {//Ingredient match!
                            ingredientFound =true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {//this Recipe ingredient was not found on the Plate
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                {//player delevered the correct recipe!
                    successfulRecipesAmpunts++;

                    //Debug.Log("Player delevered the <color=green> correct </color> recipe!");
                    waitingRescipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);


                    return;
                }
            }
        }

        // no matches found!
        //The player didnt deliver a correct recipe
        //Debug.Log("player <color=red> didnt </color> deliver a correct recipe");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);


    }


    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRescipeSOList;

    public int GetSuccessfulRecipesAmpunts() => successfulRecipesAmpunts;
}
