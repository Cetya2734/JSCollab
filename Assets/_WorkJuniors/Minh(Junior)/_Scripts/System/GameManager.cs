using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState State;
    
    public static event Action <GameState> OnGameStateChanged;
    // void OnEnable()
    // {
    //     EventBus.Instance.onGameplayPaused += () => gamePaused();
    //     EventBus.Instance.onGameplayResumed += () => gameResumed();
    // }
    //
    // void OnDisable() {
    //     EventBus.Instance.onGameplayPaused -=  () => gamePaused();
    //     EventBus.Instance.onGameplayResumed -= () => gameResumed();
    // }
    //
    // private void gameResumed()
    // {
    //     Time.timeScale = 1;
    // }
    //
    // private void gamePaused()
    // {
    //     Time.timeScale = 0;
    // }

    void Awake()
    {
        Instance = this;
    }
    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {         
            case GameState.Gameplay:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                break;
        }
        
        OnGameStateChanged?.Invoke(newState);
    }
    
    private void HandleStarting() {
    }
}

public enum GameState
{
    Starting,
    Gameplay,
    Win,
    Lose
}
