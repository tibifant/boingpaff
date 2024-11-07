using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentScore = 0;
    public TextMeshPro scoreText3D; // TextMeshPro für 3D-Textfeld
    private float gameDuration = 10f;
    private float timer;
    private bool isGameActive = false;

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
        StartGame();
    }

    public void StartGame()
    {
        currentScore = 0;
        timer = gameDuration;
        isGameActive = true;
    }

    private void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            // convert to string and remove decimal places
            scoreText3D.text = currentScore.ToString("F0");

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    private void EndGame()
    {
        isGameActive = false;
        SaveHighScore();
        //SceneManager.LoadScene("MainMenu");
    }

    private void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
