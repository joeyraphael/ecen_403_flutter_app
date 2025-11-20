using UnityEngine;
using UnityEngine.SceneManagement; // Add this for SceneManager

public class Hiddenclues : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] string clueQuestion = "What color is the sky on a clear day?";
    [SerializeField] string[] clueAnswers = { "Blue", "Red", "Green" };
    [SerializeField] int correctAnswerIndex = 0;
    [SerializeField] string clueID = "SkyColor";  // Base ID

    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 3) <= 2)  // 20% chance
        {
            player.Character.Animator.IsMoving = false;
            // Append the scene name to the clueID to make it unique
            string uniqueClueID = $"{SceneManager.GetActiveScene().name}_{clueID}";
            GameController.Instance.StartClue(clueQuestion, clueAnswers, correctAnswerIndex, uniqueClueID);
        }
    }
}