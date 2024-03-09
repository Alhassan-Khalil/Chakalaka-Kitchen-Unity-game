using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{

    private static string PLAYER_PREFS_BINDING = "InputBidings";

    public static PlayerInputManager instance { get; private set; }



    private PlayerInputAction controls;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Jump { get; private set; }
    public bool Sprint { get; private set; }
    public bool Aim { get; private set; }
    public bool Shoot { get; private set; }
    public bool Crouch { get; private set; }
    public bool Interact { get; private set; }
    public bool InteractAlternet { get; private set; }
    public bool Pause { get; private set; }

    public bool Select { get; private set; }
    public bool Zoom { get; private set; }


    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternetAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebindAction;



    public enum Binding
    {
        Move_forward,
        Move_back,
        Move_left,
        Move_right,
        Interact,
        InteractAlternet,
        Pause,
        Sprint,
        Jump,
        Crouch,
        Gamepad_Interact,
        Gamepad_InteractAlternet,
        Gamepad_Pause,
        Gamepad_Sprint,
        Gamepad_Jump,
        Gamepad_Crouch,

    }

    private void Awake()
    {
        instance = this;
        controls = new PlayerInputAction();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDING)) { controls.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));  }
    }
    private void Start()
    {
        // Movement input
        controls.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => Move = Vector2.zero;

        // Mouse input
        controls.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => Look = Vector2.zero;

        // Jump input
        controls.Player.Jump.performed += ctx => Jump = true;
        controls.Player.Jump.canceled += ctx => Jump = false;

        // Sprint input
        controls.Player.Sprint.performed += ctx => Sprint = true;
        controls.Player.Sprint.canceled += ctx => Sprint = false;

        // Crouch input
        controls.Player.Crouch.started += ctx => Crouch = true;
        controls.Player.Crouch.canceled += ctx => Crouch = false;

        // zoom input
        controls.Player.Zoom.started += ctx => Zoom = true;
        controls.Player.Zoom.canceled += ctx => Zoom = false;

        // pause input
        controls.Player.Pause.performed += ctx => OnPauseAction?.Invoke(this, EventArgs.Empty);
        controls.Player.Pause.canceled += ctx => Zoom = false;

        // Interact input
        //controls.Player.Interact.started += ctx => Interact = true;
        //controls.Player.Interact.canceled += ctx => Interact = false;
        controls.Player.Interact.performed += ctx => OnInteractAction?.Invoke(this, EventArgs.Empty);
        controls.Player.InteractAlternet.performed += ctx => OnInteractAlternetAction?.Invoke(this, EventArgs.Empty);


    }

    public string GetbindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_forward:
                return controls.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_back:
                return controls.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_left:
                return controls.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_right:
                return controls.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return controls.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternet:
                return controls.Player.InteractAlternet.bindings[0].ToDisplayString();
            case Binding.Pause:
                return controls.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Sprint:
                return controls.Player.Sprint.bindings[0].ToDisplayString();
            case Binding.Jump:
                return controls.Player.Jump.bindings[0].ToDisplayString();
            case Binding.Crouch:
                return controls.Player.Crouch.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return controls.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternet:
                return controls.Player.InteractAlternet.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return controls.Player.Pause.bindings[1].ToDisplayString();
            case Binding.Gamepad_Sprint:
                return controls.Player.Sprint.bindings[1].ToDisplayString();
            case Binding.Gamepad_Jump:
                return controls.Player.Jump.bindings[1].ToDisplayString();
            case Binding.Gamepad_Crouch:
                return controls.Player.Crouch.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        controls.Player.Disable();

        InputAction inputAction;
        int BindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_forward:
                inputAction = controls.Player.Move;
                BindingIndex = 1;
                break;
            case Binding.Move_back:
                inputAction = controls.Player.Move;
                BindingIndex = 2;
                break;
            case Binding.Move_left:
                inputAction = controls.Player.Move;
                BindingIndex = 3;
                break;
            case Binding.Move_right:
                inputAction = controls.Player.Move;
                BindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = controls.Player.Interact;
                BindingIndex = 0;
                break;
            case Binding.InteractAlternet:
                inputAction = controls.Player.InteractAlternet;
                BindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = controls.Player.Pause;
                BindingIndex = 0;
                break;
            case Binding.Sprint:
                inputAction = controls.Player.Sprint;
                BindingIndex = 0;
                break;
            case Binding.Jump:
                inputAction = controls.Player.Jump;
                BindingIndex = 0;
                break;
            case Binding.Crouch:
                inputAction = controls.Player.Crouch;
                BindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = controls.Player.Interact;
                BindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternet:
                inputAction = controls.Player.InteractAlternet;
                BindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = controls.Player.Pause;
                BindingIndex = 1;
                break;
            case Binding.Gamepad_Sprint:
                inputAction = controls.Player.Sprint;
                BindingIndex = 1;
                break;
            case Binding.Gamepad_Jump:
                inputAction = controls.Player.Jump;
                BindingIndex = 1;
                break;
            case Binding.Gamepad_Crouch:
                inputAction = controls.Player.Crouch;
                BindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(BindingIndex).OnComplete(callback => {
            callback.Dispose();
            controls.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDING,controls.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebindAction?.Invoke(this,EventArgs.Empty);
        }).Start();
    }



    //if u get error when u load and unload the scean

    private void OnDestroy()
    {
        // unsubscribe from all  foe example  v 
        // controls.Player.Interact.performed -= ctx => OnInteractAction?.Invoke(this, EventArgs.Empty);

        controls.Dispose();

    }


    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }


}
