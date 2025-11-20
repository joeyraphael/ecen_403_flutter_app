using UnityEngine;
using UnityEngine.UI;  // For legacy Text

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;       // Changed to Text
    [SerializeField] private Text clueCountText;   // Changed to Text
    [SerializeField] private int totalClues = 5;   // Adjust based on your game

    private int score = 0;
    private int cluesFound = 0;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }
public void AddClueFound(bool firstTryCorrect)
{

    cluesFound++;
    score += 50;  // 50 points for finding a clue
    if (firstTryCorrect)
    {
        score += 50;  // Bonus 50 for first-try correct
    }

    UpdateUI();
        UpdateUI();

        // ✅ Check if all clues are found
        if (cluesFound >= totalClues)
        {
            Debug.Log("All clues found — loading ResultsScene");

            // Save data for the results screen
            PlayerPrefs.SetInt("FinalScore", score);
            PlayerPrefs.SetInt("TotalClues", totalClues);
            PlayerPrefs.SetInt("CluesFound", cluesFound);
            PlayerPrefs.Save();

            // Reset time and load the results scene
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsScene");
        }

    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
        }
        if (clueCountText != null)
        {
            clueCountText.text = $"Clues: {cluesFound}/{totalClues}";
        }
        else
        {
           
        }
    }

    public void ResetScores()
    {
        score = 0;
        cluesFound = 0;
        UpdateUI();
    }
}