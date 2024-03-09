using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCounter : BaseCounter,IHasProgress
{


    public event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArg> OnProgressChanged;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }


    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }


    [SerializeField] private FryingRecipeSO[] FryingRecipeSOArray;

    private State state;
    private float Timer = 0f;
    private FryingRecipeSO fryingRecipeSO;


    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:

                    break;
                case State.Frying:

                    Timer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNormalized = Timer / fryingRecipeSO.FryingTimerMax
                    });


                    if (Timer >= fryingRecipeSO.FryingTimerMax)
                    {
                        GetKitchenObject().DesterSelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        
                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });

                        Timer = 0f;
                    }
                    break;
                case State.Fried:
                    Timer += Time.deltaTime;
                    fryingRecipeSO = GerFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNormalized = Timer / fryingRecipeSO.FryingTimerMax
                    });

                    if (Timer >= fryingRecipeSO.FryingTimerMax)
                    {
                        GetKitchenObject().DesterSelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });


                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    //TODO: Fire State 
                    break;
                default:
                    break;
            }
        }
    }




    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {// there is no KitchenObject here
            if (player.HasKitchenObject())
            {// player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {// Player carrz somthing that can be fried
                    player.GetKitchenObject().SetKitchenObjParent(this);

                    fryingRecipeSO = GerFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    if (fryingRecipeSO.Cooked)
                    {
                        state = State.Fried;

                    }
                    else
                    {
                        state = State.Frying;

                    }

                    OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                    {
                        state = state
                    });

                    Timer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNormalized = Timer/ fryingRecipeSO.FryingTimerMax
                    });

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

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                        {
                            progressNormalized = 0f
                        });
                    }
                }

            }
            else
            {// player not carrying something
                GetKitchenObject().SetKitchenObjParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                {
                    progressNormalized = 0f
                });
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        if (GerFryingRecipeSOWithInput(kitchenObjectSO))
            return true;

        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GerFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GerFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in FryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public bool isFried()
    {
        return state == State.Fried;
    }
}
