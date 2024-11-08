using UnityEngine;

public class LimitSpeed : MonoBehaviour
{
    public GameObject scaleReference;
    public float maxSpeed; // Maximale Geschwindigkeit
    public int scoreValue = 1;

    private Rigidbody rb;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;

    public GameObject hitEffectPrefab; // Prefab für den Partikel-Effekt

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Geschwindigkeit begrenzen, wenn sie das Maximum überschreitet
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if(rb.position.y < scaleReference.transform.localScale.y/1.9)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        spawnManager.setSpawnPositionsInUse(gameObject);
        if(gotHit && !gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.AddScore(scoreValue);
        }else if (gotHit && gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.ResetScore();
        }
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
