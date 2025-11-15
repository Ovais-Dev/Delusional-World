using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.Rendering;

using UnityEngine.Rendering.Universal;
[System.Serializable]
public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState = GameState.Paused;



    private Volume volume;
    private DepthOfField dof;
    Vector2 lastPoint;
    Player player;
    bool firstTime = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    player = FindFirstObjectByType<Player>();
    //    lastPoint = GameObject.FindWithTag("EndPoint").transform.position;
    //    player.SetEndPoint(lastPoint);
    //}
    private void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Player.GameOverEvent += OnGameOver;
        volume = FindFirstObjectByType<Volume>();
        if (volume.profile.TryGet(out dof))
        {
            dof.active = true;
        }
        UIManager.Instance.MenuEnable(true);
        Time.timeScale = 0f;
    }

    public void OnSwitchScene()
    {
        player = FindFirstObjectByType<Player>();
        lastPoint = GameObject.FindWithTag("EndPoint").transform.position;
        //player.SetEndPoint(lastPoint);
        volume = FindFirstObjectByType<Volume>();
        Player.GameOverEvent += OnGameOver;
        Player.GameOverEvent += UIManager.Instance.OnGameOver;
        if (!firstTime)
        {
            if (volume.profile.TryGet(out dof)) dof.active = false;
            UIManager.Instance.MenuEnable(false);
            Time.timeScale = 1f;
        }
           
        currentState = GameState.Playing;
    }
    public void StartGame()
    {
        firstTime = false;
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        if (dof != null)
        {
            dof.active = false;
        }
        UIManager.Instance.MenuEnable(false);
    }
    void OnGameOver(bool won)
    {
        dof.active = true;
        Time.timeScale = 0f;
        Player.GameOverEvent -= OnGameOver;
        currentState = GameState.GameOver;
    }
}
