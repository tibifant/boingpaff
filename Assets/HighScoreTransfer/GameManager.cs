using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentScore = 0;
    public TextMeshPro scoreText3D;     // Textfeld für den aktuellen Score
    public TextMeshPro highScoreText3D; // Neues Textfeld für den Highscore
    public int motivationSoundInterval = 20;
    private float gameDuration = 120f;
    private float timer;
    private bool isGameActive = false;

    // Array für 5 verschiedene AudioClips
    public AudioClip[] scoreSounds;
    private AudioSource audioSource;
    private List<AudioClip> soundQueue = new List<AudioClip>();

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
        audioSource = gameObject.AddComponent<AudioSource>();
        StartGame();
        UpdateHighScoreDisplay(); // Highscore-Anzeige initialisieren
    }

    public void StartGame()
    {
        currentScore = 0;
        timer = gameDuration;
        isGameActive = true;
        RefreshSoundQueue(); // Sound-Warteschlange initialisieren
    }

    private void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText3D.text = currentScore.ToString("F0");

        // Prüfen, ob der Score ein Vielfaches von 20 ist
        if (currentScore % motivationSoundInterval == 0)
        {
            PlayNextScoreSound();
        }
    }

    private void PlayNextScoreSound()
    {
        if (soundQueue.Count == 0)
        {
            RefreshSoundQueue();
        }

        if (soundQueue.Count > 0)
        {
            AudioClip clip = soundQueue[0];
            soundQueue.RemoveAt(0);
            audioSource.PlayOneShot(clip);
        }
    }

    private void RefreshSoundQueue()
    {
        soundQueue = new List<AudioClip>(scoreSounds);
        for (int i = 0; i < soundQueue.Count; i++)
        {
            AudioClip temp = soundQueue[i];
            int randomIndex = Random.Range(i, soundQueue.Count);
            soundQueue[i] = soundQueue[randomIndex];
            soundQueue[randomIndex] = temp;
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        SaveHighScore();
        ResetGame();
    }

    private void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            UpdateHighScoreDisplay(); // Anzeige aktualisieren
        }
    }

    private void UpdateHighScoreDisplay()
    {
        highScoreText3D.text = GetHighScore().ToString("F0");
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetGame()
    {
        currentScore = 0;
        timer = gameDuration;
        isGameActive = true;
        RefreshSoundQueue();
        scoreText3D.text = "Score: 0";
        UpdateHighScoreDisplay();
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        UpdateHighScoreDisplay(); // Highscore-Anzeige auf 0 setzen
    }
}
