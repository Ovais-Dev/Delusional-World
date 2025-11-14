using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject menu;
    public GameObject gameCompletionMenu;

    [Header("Texts")]
    [SerializeField] private TMP_Text levelCompletionTxt;
    [SerializeField] private Color successColor, loseColor;
    private void Awake()
    {
        Instance = this;
        
        gameCompletionMenu.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DelayEventSubscribe), 0.1f);
    }
    void DelayEventSubscribe()
    {
        Player.GameOverEvent += OnGameOver;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void MenuEnable(bool enable = false)
    {
        menu.SetActive(enable);
    }
    public void OnGameOver(bool won)
    {
        gameCompletionMenu.SetActive(true);
        levelCompletionTxt.text = won ? "Completed" : "Defeated";
        levelCompletionTxt.color = won ? successColor : loseColor;
        Player.GameOverEvent -= OnGameOver;
    }
}
