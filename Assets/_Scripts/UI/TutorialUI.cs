using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI instance { get; private set; }

    [Header("Only for UI")]
    [SerializeField] private bool isInGUI;
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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerInputManager.instance.OnBindingRebindAction += GameInput_OnBindingRebindAction;
        GameManager.Instance.OnStateChanged += GameManger_OnStateChanged;

        UpdateVisual();
        Show();
    }

    private void GameManger_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebindAction(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    private void UpdateVisual()
    {
        if (!isInGUI) return; 
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


}
