using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentScore = 0;
    public TextMeshPro scoreText3D; // TextMeshPro für 3D-Textfeld
    public int motivationSoundInterval = 20;
    private float gameDuration = 10f;
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

        // Prüfen, ob der Score ein Vielfaches von 20 ist
        if (currentScore % motivationSoundInterval == 0)
        {
            PlayNextScoreSound();
        }
    }

    private void PlayNextScoreSound()
    {
        // Falls keine Sounds in der Queue sind, die Queue neu mischen
        if (soundQueue.Count == 0)
        {
            RefreshSoundQueue();
        }

        // Den nächsten Sound aus der Queue nehmen und abspielen
        if (soundQueue.Count > 0)
        {
            AudioClip clip = soundQueue[0];
            soundQueue.RemoveAt(0);
            audioSource.PlayOneShot(clip);
        }
    }

    private void RefreshSoundQueue()
    {
        // Sounds in zufälliger Reihenfolge zur Queue hinzufügen
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
        SceneManager.LoadScene("MainMenu");
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
    public void ResetScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }
}