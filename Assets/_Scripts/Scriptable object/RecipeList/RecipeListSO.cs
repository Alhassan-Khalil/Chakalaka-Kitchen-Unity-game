using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RecipeList", fileName = "new RecipeList")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> rescipeSOList;

}
