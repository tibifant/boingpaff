using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallSoundController : MonoBehaviour
{
    public AudioClip flightSound;            // Der Sound während des Fluges
    public AudioClip collisionSound;         // Der Sound bei einer Kollision (Allgemein)
    public AudioClip triggerCollisionSound;  // Der Sound bei einer Trigger-Kollision (z.B. Tube)
    public AudioClip explosionSound;         // Der Sound für die Explosion
    public float minVelocityForFlight = 0.5f; // Mindestgeschwindigkeit, damit der Flug-Sound abgespielt wird

    [Range(0f, 1f)] public float flightSoundVolume = 0.5f;   // Lautstärke für den Flug-Sound
    [Range(0f, 1f)] public float collisionSoundVolume = 1f;  // Lautstärke für den Kollisions-Sound
    [Range(0f, 1f)] public float triggerCollisionSoundVolume = 1f; // Lautstärke für den Trigger-Kollisions-Sound
    [Range(0f, 1f)] public float explosionSoundVolume = 1f;  // Lautstärke für den Explosionssound
    [Range(0f, 1f)] public float dampenedVolume = 0.2f; // Lautstärke im Dämpfungsbereich
    [Range(0f, 1f)] public float maxTubeCollisionSoundVolume = 2.0f; // Maximal Lautstärke für Tube-Kollision

    public GameObject explosionEffect; // Partikel-Explosion (Prefab), die bei Kollision abgespielt wird

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isPlayingFlightSound = false;
    private bool isInDampingZone = false; // Flag, um festzustellen, ob sich die Kugel in der Dämpfungszone befindet

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // Flug-Sound looped abspielen, aber initial stoppen
        audioSource.loop = true;
        audioSource.clip = flightSound;
        audioSource.volume = flightSoundVolume;
        audioSource.Stop();
    }

    void Update()
    {
        // Überprüfe, ob die Geschwindigkeit hoch genug ist, um den Flug-Sound abzuspielen
        if (rb.velocity.magnitude > minVelocityForFlight)
        {
            if (!isPlayingFlightSound)
            {
                audioSource.clip = flightSound;
                audioSource.volume = isInDampingZone ? dampenedVolume : flightSoundVolume; // Volume anpassen
                audioSource.Play();
                isPlayingFlightSound = true;
            }
        }
        else if (isPlayingFlightSound)
        {
            // Stoppe den Flug-Sound, wenn die Geschwindigkeit zu niedrig ist
            audioSource.Stop();
            isPlayingFlightSound = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Bei Kollision, Flug-Sound stoppen
        if (isPlayingFlightSound)
        {
            audioSource.Stop();
            isPlayingFlightSound = false;
        }

        // Kollisions-Sound abspielen
        //AudioSource.PlayClipAtPoint(collisionSound, transform.position, collisionSoundVolume);

        // Partikel-Explosion an der Kollisionsposition erzeugen und Explosionssound abspielen
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Explosionssound abspielen
            //AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        // Ball zerstören
        //Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Überprüfe, ob das kollidierte Objekt der Tube ist
        if (other.CompareTag("Tube")) // Stelle sicher, dass der Tube den Tag "Tube" hat
        {
            float adjustedCollisionVolume = Mathf.Clamp(collisionSoundVolume * 100.0f, 0f, maxTubeCollisionSoundVolume);
            AudioSource.PlayClipAtPoint(triggerCollisionSound, transform.position, adjustedCollisionVolume);
            // Setze die Flag für die Dämpfung auf true, um die Lautstärke zu verringern
            isInDampingZone = true;
            if (isPlayingFlightSound)
            {
                audioSource.volume = dampenedVolume; // Lautstärke reduzieren
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Wenn die Kugel den Dämpfungsbereich verlässt, setze die Flag zurück
        if (other.CompareTag("Tube"))
        {
            isInDampingZone = false; // Setze die Flag für die Dämpfung auf false
            if (isPlayingFlightSound)
            {
                audioSource.volume = flightSoundVolume; // Lautstärke zurücksetzen
            }

            // Kollisions-Sound abspielen, wenn die Tube verlassen wird, und die Lautstärke verdoppeln
            
        }
    }
}
