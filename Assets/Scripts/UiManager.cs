using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUi;

    public void OnEnable()
    {
        SetScoreText(0);
        SetActiveGameOver(false);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    
    public void SetActiveGameOver(bool active)
    {
        gameOverUi.SetActive(active);
    }

    public void OnClickRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
