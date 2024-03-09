using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }


    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;


    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    //private float waitingToStartTimer = 1f;
    private float CountdownToStartTimer = 5f;
    private float GamePlayingTimerMax = 240f;
    private float GamePlayingTimer;

    private bool isGamePause = false;



    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        PlayerInputManager.instance.OnPauseAction += InputManager_OnPauseAction;
        TableBell.Instance.OnRingTheBell += TableBell_OnRingTheBell;
    }

    private void TableBell_OnRingTheBell(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputManager_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }



    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
            break;

            case State.CountdownToStart:
                CountdownToStartTimer -= Time.deltaTime;
                if (CountdownToStartTimer <= 0f)
                {
                    state = State.GamePlaying;
                    GamePlayingTimer = GamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;

            case State.GamePlaying:
                GamePlayingTimer -= Time.deltaTime;
                if (GamePlayingTimer <= 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:
                break;
        }
    }


    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetCountdownToStartTimer()
    {
        return CountdownToStartTimer;
    }

    public float GetGamePlayingTimer()
    {
        return 1 - (GamePlayingTimer / GamePlayingTimerMax);
    }


    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;
        if (isGamePause)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible =true;
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
}
