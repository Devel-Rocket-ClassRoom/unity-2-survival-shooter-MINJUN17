using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UiManager uimanager;
    public ZombieSpawner zombieSpawner;

    public int score = 0;
    public bool isGameOver { get; private set; }

    private void Start()
    {
        uimanager.SetScoreText(score);
    }
    public void AddScore(int add)
    {
        if (isGameOver)
        {
            return;
        }
        score += add;
        uimanager.SetScoreText(score);
    }
    public void EndGame()
    {
        isGameOver = true;
        zombieSpawner.enabled = false;
        uimanager.SetActiveGameOver(true);
        StartCoroutine(uimanager.GameOverSequence());

    }
}
