using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text livesText;
    public TMP_Text comboText;
    public GameObject gameOverPanel;
    public Button restartButton;

    [Header("Lives")]
    public int lives = 3;

    [Header("Combo")]
    public float comboWindow = 2f;

    private int score = 0;
    private int highScore = 0;
    private int combo = 0;
    private float comboTimer = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        gameOverPanel.SetActive(false);
        UpdateScoreUI();
        UpdateLivesUI();
    }

    void Update()
    {
        if (combo > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                combo = 0;
                if (comboText != null)
                    comboText.gameObject.SetActive(false);
            }
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        combo++;
        comboTimer = comboWindow;
        int multiplier = Mathf.Min(combo, 5);
        score += points * multiplier;

        // Show combo text
        if (comboText != null && combo > 1)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = "x" + multiplier + " COMBO!";
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (highScoreText != null)
            highScoreText.text = "Best: " + Mathf.Max(score, highScore);
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    public void LoseLife()
    {
        if (isGameOver) return;
        lives--;
        UpdateLivesUI();
        if (lives <= 0)
            GameOver();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Save high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null) spawner.StopSpawning();

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator ShakeCamera()
    {
        Vector3 original = Camera.main.transform.position;
        float duration = 0.3f;
        float magnitude = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.position = new Vector3(
                original.x + x,
                original.y + y,
                original.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = original;
    }
}