using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int currentLevel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void Play()
    {
        GameManager.Instance.StartGame();
    }
    public void LoadNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene("World" + currentLevel);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        OnSwitchScene();
    }
    void OnSwitchScene()
    {
        GameManager.Instance.OnSwitchScene();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
