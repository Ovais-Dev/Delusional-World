using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SmallEventManagers : MonoBehaviour
{
    [Header("Next Level Button Setup")]
    public Button nextLevelButton;
    public TMP_Text nextLevelButtonTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.GameOverEvent += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGameOver(bool won)
    {
        if (won)
        {
           // nextLevelButton.gameObject.SetActive(true);
            nextLevelButtonTxt.text = "Next";
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() => LevelManager.Instance.LoadNextLevel());
        }
        else
        {
           // nextLevelButton.gameObject.SetActive(true);
            nextLevelButtonTxt.text = "Restart";
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() => LevelManager.Instance.RestartLevel());
        }
    }
}
