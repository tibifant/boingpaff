using System.Collections;
using UnityEngine;

public class NpcJumper : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector3 startingPosition;
    private float timeSinceStart;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;
    private bool hasPeaked = false;

    public GameObject scaleReference;
    public int scoreValue = 1;
    public float jumpSpeed;
    public float gravity;

    public AudioClip startClip;     // Audioclip für Start
    public AudioClip destroyClip;   // Audioclip für Destroy
    private AudioSource audioSource;

    public GameObject hitEffectPrefab; // Prefab für den Partikel-Effekt

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        timeSinceStart = 0f;

        handleStartAudio();
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if (transform.position.y < gameObject.transform.localScale.y/1.9 && hasPeaked)
        {
            Destroy(gameObject);
        }
        else
        {
            calcYPositon(timeSinceStart);
        }
    }

    void handleStartAudio()
    {
        // AudioSource-Komponente abrufen oder hinzufügen
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Audioclip für Start abspielen
        if (startClip != null)
        {
            audioSource.PlayOneShot(startClip);
        }
    }

    void handleDestroyAudio()
    {
        if (destroyClip != null)
        {
            audioSource.PlayOneShot(destroyClip);
        }
    }

    void calcYPositon(float timeSinceStart)
    {
        if(jumpSpeed * timeSinceStart - 0.5f * gravity * (timeSinceStart * timeSinceStart) < 0)
        {
            hasPeaked = true;
        }
        float newY = startingPosition.y + (jumpSpeed * timeSinceStart - 0.5f * gravity * (timeSinceStart * timeSinceStart));
        transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
    }

    private void OnDestroy()
    {
        spawnManager.setSpawnPositionsInUse(gameObject);
        if (gotHit && !gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        else if (gotHit && gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.ResetScore();
        }

        handleDestroyAudio();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gotHit = true;
            // Partikel-Effekt abspielen
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
