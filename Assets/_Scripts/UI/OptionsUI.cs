using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI instance { get; private set; }

    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button Controls;
    [SerializeField] private Button BackBtn;


    private Action OnCloseButtonAction;

    private void Awake()
    {
        instance = this;
        soundEffectSlider.onValueChanged.AddListener(OnSoundEffectChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        BackBtn.onClick.AddListener(()=> {
            Hide();
            OnCloseButtonAction();
        });
        Controls.onClick.AddListener(() => { 
            Hide();
            ControlUI.Instance.Show(Show);
        });

    }
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManger_OnGamePaused;
        soundEffectSlider.value = SoundManager.instance.GetVolume();
        musicSlider.value = MusicManager.instance.GetVolume();
        Hide();
    }

    private void GameManger_OnGamePaused(object sender, EventArgs e)
    {
        Hide();
        //TODO: Fix the bug when esc in pause

    }

    private void OnMusicSliderChanged(float volume)
    {
        MusicManager.instance.ChangeVolume(volume);
    }

    private void OnSoundEffectChanged(float volume)
    {
        SoundManager.instance.ChangeVolume(volume);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Action OnCloseButtonAction)
    {
        this.OnCloseButtonAction = OnCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectSlider.Select();
    }
    private void Show()
    {
        gameObject.SetActive(true);
        soundEffectSlider.Select();
    }

}
