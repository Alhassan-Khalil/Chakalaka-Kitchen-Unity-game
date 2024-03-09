using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get;private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public event EventHandler OnPickSomething;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }



    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private Transform KicthenObjHoldPoint;
    [SerializeField] private Transform InteractionBeamPostion;


    private PlayerInputManager inputHander;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;





    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;

        inputHander = GetComponent<PlayerInputManager>();
    }
    private void Start()
    {
        inputHander.OnInteractAction += InputHander_OnInteractAction;
        inputHander.OnInteractAlternetAction += InputHander_OnInteractAlternetAction;

    }

    private void InputHander_OnInteractAlternetAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternet(this);
        }
    }

    private void InputHander_OnInteractAction(object sender, System.EventArgs e)
    {
        //if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        //if (!inputHander.Interact) return;
        RaycastHit hit;
        if (Physics.Raycast(InteractionBeamPostion.position, Camera.main.transform.forward, out hit, interactDistance))
        {
            //Debug.Log(hit.transform);
            //var interactable = hit.collider.GetComponent<Iinteractable>();
            //interactable?.Interact();
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
            //Debug.Log("Nothing was hit.");
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnPickSomething?.Invoke(this,EventArgs.Empty);
        }
    }

    public Transform GetKitchenObjectFollowTransform() => KicthenObjHoldPoint;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void clearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}
