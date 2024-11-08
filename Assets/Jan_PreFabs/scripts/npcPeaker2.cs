using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcPeaker2 : MonoBehaviour
{
    public GameObject scaleReference;
    public float restingPointY = 5f;
    public float restingTime = 1f;
    public float riseSpeed = 0.5f;
    public int scoreValue = 1;

    private Rigidbody rb;
    private float timeResting;
    private bool hasRisen;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;

    public AudioClip startClip;     // Audioclip für Start
    public AudioClip destroyClip;   // Audioclip für Destroy
    private AudioSource audioSource;

    public GameObject hitEffectPrefab; // Prefab für den Partikel-Effekt

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        timeResting = 0;
        hasRisen = false;

        handleStartAudio();
    }

    void FixedUpdate()
    {
        // Berechne die neue Position unter Verwendung des Rigidbody
        if (rb.position.y < restingPointY && !hasRisen)
        {
            rb.useGravity = false;
            rise();

        }
        else if (rb.position.y >= restingPointY && timeResting < restingTime)
        {
            hasRisen = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            timeResting += Time.fixedDeltaTime;
        }
        else if (timeResting >= restingTime)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            Debug.Log("Gravity is on");
            if (rb.position.y < gameObject.transform.localScale.y/1.9)
            {
                Destroy(gameObject);
            }
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

    void rise()
    {
        Vector3 upwardMovement = Vector3.up * riseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + upwardMovement);
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
