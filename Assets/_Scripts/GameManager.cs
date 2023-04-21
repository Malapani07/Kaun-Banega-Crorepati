using UnityEngine;

public enum GameState
{
    Wait,
    PlayGame,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioManager audioManager;
    public UIManager UiManager;
    public APIcontroller apicontroller;
    public Timer timer;

    public GameState gamestate;
    public int NoOfAnsweredQuestion=0;
   // public event System.Action gameStarted, gameLost, QuestionLoad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartLoadingQuestion()
    {
        apicontroller.loadquestion();
    }
    
 
}
