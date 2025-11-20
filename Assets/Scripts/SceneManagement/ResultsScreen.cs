using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private Text resultsText;
    [SerializeField] private Button returnButton;

    void Start()
    {
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        int total = PlayerPrefs.GetInt("TotalClues", 0);
        int found = PlayerPrefs.GetInt("CluesFound", 0);

        resultsText.text = $"You found {found}/{total} clues!\nFinal Score: {score}";

        returnButton.onClick.AddListener(() =>
        {
            //  Always resume time
            Time.timeScale = 1f;

            //  Reset scores
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ResetScores();

            //  Destroy persistent controllers that could pause the game
            var controllers = GameObject.FindObjectsOfType<GameController>();
            foreach (var c in controllers)
                Destroy(c.gameObject);

            var essentials = GameObject.Find("EssentialObjects");
            if (essentials != null)
                Destroy(essentials);

            //  Load WelcomeScene
            UnityEngine.SceneManagement.SceneManager.LoadScene("WelcomeScene");
        });


    }
}
