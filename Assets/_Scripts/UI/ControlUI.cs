using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    public static ControlUI Instance {  get; private set; }


    [SerializeField] private TextMeshProUGUI moveForwardText;
    [SerializeField] private TextMeshProUGUI moveBackText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI IntractText;
    [SerializeField] private TextMeshProUGUI IntractAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [Space()]
    [SerializeField] private TextMeshProUGUI Gamepad_IntractText;
    [SerializeField] private TextMeshProUGUI Gamepad_IntractAltText;
    [SerializeField] private TextMeshProUGUI Gamepad_pauseText;
    [Space()]

    [SerializeField] private Button moveForwardBtn;
    [SerializeField] private Button moveBackBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button IntractBtn;
    [SerializeField] private Button IntractAltBtn;
    [SerializeField] private Button pauseBtn;
    [Space()]
    [SerializeField] private Button Gamepad_IntractBtn;
    [SerializeField] private Button Gamepad_IntractAltBtn;
    [SerializeField] private Button Gamepad_pauseBtn;
    [Space()]


    [SerializeField] private Button BackBtn;

    [SerializeField] private Transform pressToRebindTransform;

    private Action OnCloseButtonAction;


    private void Awake()
    {
        Instance = this;

        CallListener();
    }

    private void CallListener()
    {


        BackBtn.onClick.AddListener(() => {
            Hide();
            OnCloseButtonAction();
        });

        moveForwardBtn.onClick.AddListener(() => {RebindBinding(PlayerInputManager.Binding.Move_forward);});
        moveBackBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Move_back); });
        moveLeftBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Move_left); });
        moveRightBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Move_right); });
        IntractBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Interact); });
        IntractAltBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.InteractAlternet); });
        pauseBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Pause); });

        Gamepad_IntractBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Gamepad_Interact); });
        Gamepad_IntractAltBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Gamepad_InteractAlternet); });
        Gamepad_pauseBtn.onClick.AddListener(() => { RebindBinding(PlayerInputManager.Binding.Gamepad_Pause); });

    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManger_OnGamePaused;

        UpdateVisual();
        Hide();
        HidePressToRebindKey();

    }

    private void UpdateVisual()
    {
        moveForwardText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Move_forward);
        moveBackText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Move_back);
        moveLeftText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Move_left);
        moveRightText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Move_right);

        IntractText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Interact);
        IntractAltText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.InteractAlternet);
        pauseText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Pause);

        Gamepad_IntractText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Gamepad_Interact);
        Gamepad_IntractAltText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Gamepad_InteractAlternet);
        Gamepad_pauseText.text = PlayerInputManager.instance.GetbindingText(PlayerInputManager.Binding.Gamepad_Pause);

    }


    private void GameManger_OnGamePaused(object sender, EventArgs e)
    {
        Hide();
        //TODO: Fix the bug when esc in pause

    }
    private void RebindBinding(PlayerInputManager.Binding binding)
    {
        ShowPressToRebindKey();
        PlayerInputManager.instance.RebindBinding(binding,() =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }



    private void ShowPressToRebindKey() => pressToRebindTransform.gameObject.SetActive(true);

    private void HidePressToRebindKey() => pressToRebindTransform.gameObject.SetActive(false);

    private void Hide() => gameObject.SetActive(false);

    public void Show(Action OnCloseButtonAction)
    {
        this.OnCloseButtonAction = OnCloseButtonAction;
        gameObject.SetActive(true);
        moveForwardBtn.Select();
    }

}
