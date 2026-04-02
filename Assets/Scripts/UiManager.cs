using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUi;
    public Image damageEffect;
    public Image fadePanel;  
    public TextMeshProUGUI gameOverText;
    public IEnumerator FlashDamage()
    {
        damageEffect.color = new Color(1f, 0f, 0f, 0.4f);
        yield return new WaitForSeconds(0.1f);
        while (damageEffect.color.a > 0)
        {
            var color = damageEffect.color;
            color.a -= Time.deltaTime * 2f;
            damageEffect.color = color;
            yield return null;
        }
    }
    public IEnumerator GameOverSequence()
    {
        float elapsed = 0f;
        float duration = 1f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        gameOverText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
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
