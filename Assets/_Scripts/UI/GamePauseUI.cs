using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button OptionsButton;



    private void Awake()
    {
        ResumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene._MainMenuScene);
        });
        OptionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.instance.Show(Show);
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGamePaused += (sender, e) => Show();
        GameManager.Instance.OnGameUnPaused += (sender, e) => Hide();

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        ResumeButton.Select();
    }
}
