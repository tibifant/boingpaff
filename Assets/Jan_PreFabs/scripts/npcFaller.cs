using UnityEngine;

public class LimitSpeed : MonoBehaviour
{
    public float maxSpeed; // Maximale Geschwindigkeit
    public int scoreValue = 1;

    private Rigidbody rb;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;

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
        if(rb.position.y < -5.8)
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
            Destroy(gameObject);
        }
    }

}
