using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public static event EventHandler OnAnyCut;

    [SerializeField] private Transform PressFToCutUI;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArg> OnProgressChanged;

    public event EventHandler OnCut;

    private int CuttingProgress;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {// there is no KitchenObject here
            if (player.HasKitchenObject())
            {// player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {// Player carrz somthing that can be cutt
                    player.GetKitchenObject().SetKitchenObjParent(this);
                    CuttingProgress = 0;


                    CuttingRecipeSO cuttingRecipeSO = GerCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());


                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNormalized = (float)CuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                    PressFToCutUI.gameObject.SetActive(true);
                }
            }
            else
            {// player not carrying something

            }
        }
        else
        {// there is a KitchenObject here
            if (player.HasKitchenObject())
            {// player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player holding Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DesterSelf();
                        PressFToCutUI.gameObject.SetActive(false);

                    }
                }
            }
            else
            {// player not carrying something
                GetKitchenObject().SetKitchenObjParent(player);
                PressFToCutUI.gameObject.SetActive(false);

            }
        }
    }

    public override void InteractAlternet(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {   // There a kitchen obj here

            CuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);


            CuttingRecipeSO cuttingRecipeSO = GerCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());


            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
            {
                progressNormalized = (float)CuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });


            if (CuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outPutKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DesterSelf();

                KitchenObject.SpawnKitchenObject(outPutKitchenObjectSO, this);
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        if (GerCuttingRecipeSOWithInput(kitchenObjectSO))
            return true;

        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GerCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GerCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

