using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Gameplay.Objectives
{
    
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ObjectiveManager Objectives { get; private set; }
    public GameState State;
    
    public static event Action <GameState> OnGameStateChanged;
    void OnEnable()
    {
        EventBus.Instance.onGameplayPaused += GamePaused;
        EventBus.Instance.onGameplayResumed += GameResumed;
    }
    
    void OnDisable()
    {
        EventBus.Instance.onGameplayPaused -= GamePaused;
        EventBus.Instance.onGameplayResumed -= GameResumed;
    }
    
    private void GameResumed()
    {
        Time.timeScale = 1;
    }
    
    private void GamePaused()
    {
        Time.timeScale = 0;
    }

    void Awake()
    {
        Instance = this;
        Objectives = new ObjectiveManager();
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

public class ObjectiveManager
{
    public Action<Objective> OnObjectiveAdded;
    public List<Objective> Objectives { get; } = new();
    private readonly Dictionary<string, List<Objective>> _objectiveMap = new();
        
    // Adds an objective to the objective manager. 
    // If the objective has an EventTrigger, it's progress will be incremented by AddProgress when the event is triggered.
    // Multiple objectives can have the same EventTrigger (i.e. MobKilled, ItemCollected, etc)

    private void Start()
    {
        EventBus.Instance.OnObjectiveProgress += AddProgress;
        EventBus.Instance.OnObjectiveCreated += (eventName, text, max) => 
        {
            AddObjective(new Objective(eventName, text, max));
        };
    }
    
    private void OnDestroy()
    {
        // Clean up subscriptions
        EventBus.Instance.OnObjectiveProgress -= AddProgress;
    }
    
    public void AddObjective(Objective objective)
    {
        Objectives.Add(objective);
        if (!string.IsNullOrEmpty(objective.EventTrigger))
        {
            if (!_objectiveMap.ContainsKey(objective.EventTrigger))
            {
                _objectiveMap.Add(objective.EventTrigger, new List<Objective>());
            }

            _objectiveMap[objective.EventTrigger].Add(objective);
        }

        OnObjectiveAdded?.Invoke(objective);
    }
        
    public void AddProgress(string eventTrigger, int value)
    {
        if (!_objectiveMap.ContainsKey(eventTrigger)) return;
        foreach (var objective in _objectiveMap[eventTrigger])
        {
            objective.AddProgress(value);
        }
    }

}
}
